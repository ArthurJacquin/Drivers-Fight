using Photon.Pun;
using UnityEngine;

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

        [SerializeField]
        private PlayerUI playerUIScript;

        [SerializeField]
        private GameObject playerUI;

        private float carSpeed = 0f; //Current speed of the car

        private float carMaximumSpeed = 20f; //Max speed of the car

        private float carAcceleration = 0.2f; //Acceleration of the car

        private float carDeceleration = 0.3f; //Deceleration of the car

        private AIntentReceiver[] activatedIntentReceivers;

        private bool GameStarted { get; set; }

        private void Awake()
        {
            startGameControllerScript.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameControllerScript.OfflinePlayReady += ChooseAndSubscribeToOfflineIntentReceivers;
            startGameControllerScript.PlayerJoined += ActivateAvatar;
            startGameControllerScript.PlayerLeft += DeactivateAvatar;
            startGameControllerScript.Disconnected += EndGame;
            startGameControllerScript.MasterClientSwitched += EndGame;
        }

        private void ActivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("ActivateAvatarRPC", RpcTarget.AllBuffered, id);

                //Camera steup
                /*PlayerCamera cam = Camera.main.GetComponent<PlayerCamera>();
                cam.enabled = true;
                cam.target = avatars[id].AvatarRootTransform;

                //UI setup
                playerUIScript.avatar = avatars[id];
                playerUI.gameObject.SetActive(true);
                playerUIScript.enabled = true;*/
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

                //UI
                /*playerUIScript.avatar = null;
                playerUI.gameObject.SetActive(false);
                playerUIScript.enabled = false;*/
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
                var moveIntent = Vector3.zero;

                var intentReceiver = activatedIntentReceivers[i];
                var avatar = avatars[i];

                CarStatsScript mystats = avatar.Stats;

                activatedAvatarsCount += avatar.AvatarRootGameObject.activeSelf ? 1 : 0;

                //Backward
                if (intentReceiver.WantToMoveBackward)
                {
                    //Update speed
                    if (mystats.currentSpeed < carMaximumSpeed && !intentReceiver.WantToStopTheCar)
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
                    if (mystats.currentSpeed < carMaximumSpeed && !intentReceiver.WantToStopTheCar)
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
                
                //Refresh dead players count
                //TODO: Décommenter une fois isDead() implémenté
                /*if (!avatar.isDead()))
                {
                    continue;
                }

                fallenAvatarsCount++;*/
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
    }
}