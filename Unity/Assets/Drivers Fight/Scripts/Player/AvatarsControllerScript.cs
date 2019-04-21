using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System.Collections.Generic;

namespace DriversFight.Scripts
{
    public class AvatarsControllerScript : MonoBehaviour
    {
        [SerializeField]
        private AvatarExposerScript[] avatars; 

        [SerializeField]
        private Transform[] startPositions; //Spawn position

        [SerializeField]
        private AIntentReceiver[] onlineIntentReceivers;

        [SerializeField]
        private AIntentReceiver[] offlineIntentReceivers;

        [SerializeField]
        private StartGameControllerScript startGameControllerScript;

        [SerializeField]
        private PhotonView photonView;

        private float carSpeed = 0f; //Current speed of the car

        private float carMaximumSpeed = 20f; //Max speed of the car

        private float carAcceleration = 0.2f; //Acceleration of the car

        private float carDeceleration = 0.3f; //Deceleration of the car

        private AIntentReceiver[] activatedIntentReceivers;

        [SerializeField]
        private PlayerUIScript playerUIScript;

        [SerializeField]
        private GameObject playerUI;

        private bool collisionSubscriptionDone = false;

        private bool gameStarted = false;

        private readonly Dictionary<CollisionEnterDispatcherScript, AvatarExposerScript>
            dispatcherToAvatar = new Dictionary<CollisionEnterDispatcherScript, AvatarExposerScript>();

        private readonly Dictionary<Collider, AvatarExposerScript>
            colliderToAvatar = new Dictionary<Collider, AvatarExposerScript>();

        private bool GameStarted
        {
            get { return gameStarted; }
            set
            {

                if (value && !gameStarted && (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient))
                {
                    SubscribeCollisionsEffect();
                }
                gameStarted = value;
            }
        }

        private void SubscribeCollisionsEffect()
        {
            foreach (var avatar in avatars)
            {
                avatar.CollisionDispatcher.CollisionEvent += HandleCollision;
            }
        }

        private void HandleCollision(CollisionEnterDispatcherScript collisionDispatcher,
            Collider col)
        {
            if (!dispatcherToAvatar.TryGetValue(collisionDispatcher, out var sourceAvatar)
                || !colliderToAvatar.TryGetValue(col, out var targetAvatar))
            {
                return;
            }

            targetAvatar.Stats.TakeFrontDamage((int)sourceAvatar.Stats.currentSpeed * 2);
            Debug.Log(targetAvatar.gameObject.name + " lost 10 hp !");
        }

        private void Awake()
        {
            startGameControllerScript.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameControllerScript.OfflinePlayReady += ChooseAndSubscribeToOfflineIntentReceivers;
            startGameControllerScript.PlayerJoined += ActivateAvatar;
            startGameControllerScript.PlayerLeft += DeactivateAvatar;
            startGameControllerScript.Disconnected += EndGame;
            startGameControllerScript.MasterClientSwitched += EndGame;
            startGameControllerScript.PlayerSetup += SetupPlayer;

            foreach (var avatar in avatars)
            {
                dispatcherToAvatar.Add(avatar.CollisionDispatcher, avatar);
                colliderToAvatar.Add(avatar.MainCollider, avatar);
            }
        }

        private void SetupPlayer(int id)
        {
            //Camera setup for client
            PlayerCameraScript cam = Camera.main.GetComponent<PlayerCameraScript>();
            if (cam.enabled == false)
            {
                cam.enabled = true;
                cam.target = avatars[id].AvatarRootTransform;
            }


            //UI setup for client
            if (!playerUI.activeSelf)
            {
                playerUIScript.avatar = avatars[id];
                playerUI.gameObject.SetActive(true);
                playerUIScript.enabled = true;
            }
        }

        private void ActivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("ActivateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                ActivateAvatarRPC(id);
            }
        }

        private void DeactivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                DeactivateAvatarRPC(id);
            }
        }

        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            activatedIntentReceivers = onlineIntentReceivers;
            ResetGame();
        }

        private void ChooseAndSubscribeToOfflineIntentReceivers()
        {
            activatedIntentReceivers = offlineIntentReceivers;
            ResetGame();
        }

        private void DisableIntentReceivers()
        {
            if (activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                activatedIntentReceivers[i].enabled = false;
            }
        }

        private void EnableIntentReceivers()
        {
            if (activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                activatedIntentReceivers[i].enabled = true;
                activatedIntentReceivers[i].WantToMoveLeft = false;
                activatedIntentReceivers[i].WantToMoveBackward = false;
                activatedIntentReceivers[i].WantToMoveRight = false;
                activatedIntentReceivers[i].WantToMoveForward = false;
            }
        }

        private void ResetGame()
        {
            for (var i = 0; i < avatars.Length; i++)
            {
                var avatar = avatars[i];
                avatar.AvatarRigidBody.velocity = Vector3.zero;
                avatar.AvatarRigidBody.angularVelocity = Vector3.zero;
                avatar.AvatarRootTransform.position = startPositions[i].position;
                avatar.AvatarRootTransform.rotation = startPositions[i].rotation;
                avatar.AvatarTransformView.enabled = activatedIntentReceivers == onlineIntentReceivers;
            }

            EnableIntentReceivers();
            GameStarted = true;
            
        }

        private void FixedUpdate()
        {
            // If on network, only the master client can move objects
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            // Do nothing if the game is not started
            if (!GameStarted)
            {
                return;
            }

            // If intents and avatars are not setup properly
            if (activatedIntentReceivers == null
                || avatars == null
                || avatars.Length != activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }

            var deadAvatarsCount = 0;
            var activatedAvatarsCount = 0;

            //Execute the requested actions
            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                //Movements

                var intentReceiver = activatedIntentReceivers[i];
                var avatar = avatars[i];

                CarStatsScript mystats = avatar.Stats;

                activatedAvatarsCount += avatar.AvatarRootGameObject.activeSelf ? 1 : 0;

                //Backward
                if (intentReceiver.WantToMoveBackward)
                {
                    //Update speed
                    if (mystats.currentSpeed < mystats.currentMaximumSpeed.GetValue() && !intentReceiver.WantToStopTheCar)
                    {
                        mystats.currentSpeed += mystats.currentAccelerationSpeed.GetValue();
                    }

                    //Turn Left
                    if (intentReceiver.WantToMoveLeft)
                    {
                        avatar.AvatarRootTransform.position += -avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, mystats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                    }

                    //Turn Right
                    else if (intentReceiver.WantToMoveRight)
                    {
                        avatar.AvatarRootTransform.position += -avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, -mystats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                    }

                    //Dont turn
                    else
                    {
                        avatar.AvatarRootTransform.position += -avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                    }
                }

                //Forward
                if (intentReceiver.WantToMoveForward)
                {
                    //Update speed
                    if (mystats.currentSpeed < mystats.currentMaximumSpeed.GetValue() && !intentReceiver.WantToStopTheCar)
                    {
                        mystats.currentSpeed += mystats.currentAccelerationSpeed.GetValue();
                    }

                    //Turn Left
                    if (intentReceiver.WantToMoveLeft)
                    {
                        avatar.AvatarRootTransform.position += avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, -mystats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                    }

                    //Turn Right
                    else if (intentReceiver.WantToMoveRight)
                    {
                        avatar.AvatarRootTransform.position += avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, mystats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                    }

                    //Dont turn
                    else
                    {
                        avatar.AvatarRootTransform.position += avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                    }
                }

                //Deceleration
                if(intentReceiver.WantToStopTheCar)
                {
                    if (mystats.currentSpeed > 0f)
                    {
                        mystats.currentSpeed -= mystats.currentDecelerationSpeed.GetValue();

                        if (mystats.currentSpeed < 0f)
                        {
                            mystats.currentSpeed = 0f;
                        }
                    }

                    if (mystats.currentSpeed <= 0f)
                    {
                        mystats.currentSpeed = 0f;

                        intentReceiver.WantToMoveForward = false;
                        intentReceiver.WantToMoveBackward = false;

                        intentReceiver.WantToStopTheCar = false;
                    }
                }

                photonView.RPC("UpdateClientsUIRPC", RpcTarget.OthersBuffered, i, mystats.currentSpeed, mystats.currentEngineHealth);
            }

            //If 1 player remaining then end the game
            if (activatedAvatarsCount - deadAvatarsCount <= 1 && activatedAvatarsCount > 1)
            {
                EndGame();
            }
        }

        public void EndGame()
        {
            GameStarted = false;
            activatedIntentReceivers = null;

            for (var i = 0; i < avatars.Length; i++)
            {
                avatars[i].AvatarRootGameObject.SetActive(false);
            }

            startGameControllerScript.ShowMainMenu();

            DisableIntentReceivers();
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(false);
        }

        [PunRPC]
        private void UpdateClientsUIRPC(int id, float speed, int hp)
        {
            avatars[id].Stats.currentSpeed = speed;
            avatars[id].Stats.currentEngineHealth = hp;
        }
    }
}