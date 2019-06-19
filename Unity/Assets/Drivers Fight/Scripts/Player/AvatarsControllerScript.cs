using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

namespace DriversFight.Scripts
{
    public class AvatarsControllerScript : MonoBehaviour
    {
        [SerializeField]
        private AvatarExposerScript[] avatars; //AvatarExposer des joueurs

        [SerializeField]
        private Transform[] startPositions; //Spawn position

        [SerializeField]
        private AIntentReceiver[] onlineIntentReceivers;

        [SerializeField]
        private AIntentReceiver[] offlineIntentReceivers;

        [SerializeField]
        private StartGameControllerScript startGameControllerScript;

        [SerializeField]
        private SectorSpawnManagement sectorSpawnManagementScript;

        [SerializeField]
        private PhotonView photonView;

        private float carSpeed = 0f; //Current speed of the car

        private float carMaximumSpeed = 20f; //Max speed of the car

        private float carAcceleration = 0.2f; //Acceleration of the car

        private float carDeceleration = 0.3f; //Deceleration of the car

        private int deadAvatarsCount = 0;

        private AIntentReceiver[] activatedIntentReceivers;

        [SerializeField]
        private PlayerUIScript playerUIScript; //Script de mise à jour de l'UI

        [SerializeField]
        private GameObject playerUI; // Gameobject contenant l'UI

        [SerializeField]
        private MiniMapCameraScript miniCam;

        [SerializeField]
        private GameObject items;

        [SerializeField]
        private TextMeshProUGUI endGamePanelRankText;

        [SerializeField]
        private GameObject endGamePanel;

        [SerializeField]
        private TextMeshProUGUI mainText;

        [SerializeField]
        private TextMeshProUGUI endGamePanelCommentaryText;

        private bool collisionSubscriptionDone = false;

        private int timeWaited;

        private bool gameStarted = false;

        private bool waitingForPlayers = false;

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
                avatar.CollisionDispatcher.SectorTriggerEvent += HandleTrigger;
            }
        }

        //Choses à faire en cas de collision
        private void HandleCollision(CollisionEnterDispatcherScript collisionDispatcher,
            Collider col)
        {
            if (!dispatcherToAvatar.TryGetValue(collisionDispatcher, out var sourceAvatar)
                || !colliderToAvatar.TryGetValue(col, out var targetAvatar))
            {
                return;
            }

            if (sourceAvatar.Stats.currentSpeed > 1)
            {
                RaycastHit hit;
                if (Physics.Raycast(sourceAvatar.transform.position, targetAvatar.transform.position - sourceAvatar.transform.position, out hit, 3, 1 << 8))
                {
                    Debug.DrawLine(this.transform.position, targetAvatar.transform.position - sourceAvatar.transform.position, Color.blue, 999f, false);
                    Debug.Log("Hit : " + hit.rigidbody.gameObject.name);
                    if (hit.normal == targetAvatar.transform.forward)
                        targetAvatar.Stats.TakeFrontDamage((int)sourceAvatar.Stats.currentSpeed * 5);
                    if (hit.normal == -targetAvatar.transform.forward)
                        targetAvatar.Stats.TakeRearDamage((int)sourceAvatar.Stats.currentSpeed * 5);
                    if (hit.normal == targetAvatar.transform.right)
                        targetAvatar.Stats.TakeRightDamage((int)sourceAvatar.Stats.currentSpeed * 5);
                    if (hit.normal == -targetAvatar.transform.right)
                        targetAvatar.Stats.TakeLeftDamage((int)sourceAvatar.Stats.currentSpeed * 5);
                }
            }
        }

        private void HandleTrigger(CollisionEnterDispatcherScript collisionDispatcher)
        {
            if (!dispatcherToAvatar.TryGetValue(collisionDispatcher, out var sourceAvatar))
            {
                return;
            }

            for(var i = 0; i < avatars.Length; i++)
            {
                if(sourceAvatar == avatars[i])
                {
                    photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, i, false);
                    break;
                }
            }
        }

        private void Awake()
        {
            startGameControllerScript.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameControllerScript.OfflinePlayReady += ChooseAndSubscribeToOfflineIntentReceivers;
            startGameControllerScript.PlayerJoined += ActivateAvatar;
            startGameControllerScript.PlayerLeft += DeactivateAvatar;
            startGameControllerScript.Disconnected += EndGame;
            //startGameControllerScript.MasterClientSwitched += EndGame;
            startGameControllerScript.PlayerSetup += SetupPlayer;

            foreach (var avatar in avatars)
            {
                dispatcherToAvatar.Add(avatar.CollisionDispatcher, avatar);
                colliderToAvatar.Add(avatar.MainCollider, avatar);
            }
        }

        //Mise en place de la camera et du HUD du joueur
        private void SetupPlayer(int id)
        {
            //Camera setup for client
            PlayerCameraScript cam = Camera.main.GetComponent<PlayerCameraScript>();
            if (cam.enabled == false)
            {
                cam.enabled = true;
                cam.target = avatars[id].AvatarRootTransform;
            }

            //Camera for minimap
            if (miniCam.enabled == false)
            {
                miniCam.enabled = true;
                miniCam.player = avatars[id].AvatarRootTransform;
            }


            //UI setup for client
            if (!playerUI.activeSelf)
            {
                playerUIScript.avatar = avatars[id];
                playerUI.gameObject.SetActive(true);
                playerUIScript.enabled = true;
            }

            gameStarted = false;
            PauseMenu.inGame = true;
            deadAvatarsCount = 0;
        }

        //Active le gameobject du joueur
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

        //Desactive le gameobject du joueur
        private void DeactivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, id, false);
            }
            else
            {
                DeactivateAvatarRPC(id, false);
            }
        }

        //Récupère les IntentReceivers actifs online
        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            activatedIntentReceivers = onlineIntentReceivers;
            ResetGame();
        }

        //Récupère les IntentReceivers actifs offline
        private void ChooseAndSubscribeToOfflineIntentReceivers()
        {
            activatedIntentReceivers = offlineIntentReceivers;
            ResetGame();
        }

        //Desactive les IntentReceivers des joueurs
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

        //Active les IntentReceivers des joueurs
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

        //Réinitialise les paramètres de partie
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
                avatar.IsAlive = true;

                //Stats/Inventory
                //TODO : Fonction pour full reset des stats et inventaire
                avatar.Stats.EngineHealth = 500;
                avatar.Stats.currentSpeed = 0f;
            }

            StartCoroutine("WaitForPlayers");
            StartCoroutine("StartTheGame");
            
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

            var activatedAvatarsCount = 0;

            //Execute the requested actions
            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                //Movements

                var intentReceiver = activatedIntentReceivers[i];
                var avatar = avatars[i];

                Character mystats = avatar.Stats;

                activatedAvatarsCount += avatar.AvatarRootGameObject.activeSelf ? 1 : 0;

                //Backward
                if (intentReceiver.WantToMoveBackward)
                {
                    //Update speed
                    if (mystats.currentSpeed < mystats.MaximumSpeed.Value && !intentReceiver.WantToStopTheCar)
                    {
                        mystats.currentSpeed += mystats.AccelerationSpeed.Value;
                    }

                    //Turn Left
                    if (intentReceiver.WantToMoveLeft)
                    {
                        avatar.AvatarRootTransform.position += -avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, mystats.Maneuverability.Value  * Time.deltaTime, 0.0f);
                    }

                    //Turn Right
                    else if (intentReceiver.WantToMoveRight)
                    {
                        avatar.AvatarRootTransform.position += -avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, -mystats.Maneuverability.Value * Time.deltaTime, 0.0f);
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
                    if (mystats.currentSpeed < mystats.MaximumSpeed.Value  && !intentReceiver.WantToStopTheCar)
                    {
                        mystats.currentSpeed += mystats.AccelerationSpeed.Value ;
                    }

                    //Turn Left
                    if (intentReceiver.WantToMoveLeft)
                    {
                        avatar.AvatarRootTransform.position += avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, -mystats.Maneuverability.Value * Time.deltaTime, 0.0f);
                    }

                    //Turn Right
                    else if (intentReceiver.WantToMoveRight)
                    {
                        avatar.AvatarRootTransform.position += avatar.AvatarRootTransform.forward * mystats.currentSpeed * Time.deltaTime;
                        avatar.AvatarRootTransform.Rotate(0.0f, mystats.Maneuverability.Value * Time.deltaTime, 0.0f);
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
                        mystats.currentSpeed -= mystats.DecelerationSpeed.Value ;

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

                photonView.RPC("UpdateClientsUIRPC", RpcTarget.OthersBuffered, i, mystats.currentSpeed , mystats.EngineHealth);

                if(mystats.EngineHealth <= 0 && avatar.gameObject.activeSelf)
                {
                    photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, i, false);
                }
            }

            //If 1 player remaining then end the game
            if (activatedAvatarsCount - deadAvatarsCount <= 1 && !waitingForPlayers)
            {
                for(var i = 0; i < avatars.Length; i++)
                {
                    if(avatars[i].transform.gameObject.activeSelf)
                        photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, i, true);
                }
            }
        }

        public void EndGame()
        {
            GameStarted = false;
            waitingForPlayers = false;
            activatedIntentReceivers = null;

            for (var i = 0; i < avatars.Length; i++)
            {
                avatars[i].AvatarRootGameObject.SetActive(false);
            }

            DisableIntentReceivers();
            if (PhotonNetwork.IsConnected)
            {
                LeaveRoom();
            }
        }

        //Wait for players to connect to the game and disconnect if nobody in the room
        IEnumerator WaitForPlayers()
        {
            waitingForPlayers = true;

            if (PhotonNetwork.IsMasterClient)
            {
                timeWaited = 0;
                photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
            }
           
            //Wait for players
            while(timeWaited < 30)
            {
                mainText.text = "Waiting for players... " + (30 - timeWaited);
                yield return new WaitForSeconds(1);

                if (PhotonNetwork.IsMasterClient)
                {
                    timeWaited++;
                    photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
                }

                if (PlayerNumbering.SortedPlayers.Length != 1)
                    break;
            }

            //if alone, leave the game
            if (timeWaited == 30)
            {
                EndGame();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
                

            if (PhotonNetwork.IsMasterClient)
            {
                timeWaited = 0;
                photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
            }

            //Wait for game start
            while (timeWaited < 10)
            {
                mainText.text = "Game begin in " + (10 - timeWaited) + " !";
                yield return new WaitForSeconds(1);

                if (PhotonNetwork.IsMasterClient)
                {
                    timeWaited++;
                    photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
                }
            }
            waitingForPlayers = false;

            mainText.text = "FIGHT !";

            yield return new WaitForSeconds(3);
            mainText.text = "DRIVER'S FIGHT !";
            mainText.gameObject.SetActive(false);
        }

        IEnumerator StartTheGame()
        {
            while (waitingForPlayers)
            {
                yield return new WaitForSeconds(1);
            }

            EnableIntentReceivers();
            GameStarted = true;
            //items.SetActive(true);

            //Sector reset
            if(PhotonNetwork.IsMasterClient)
                sectorSpawnManagementScript.enabled = true;
            else
            {
                sectorSpawnManagementScript.enabled = true;
                sectorSpawnManagementScript.enabled = false;
            }
            
        }

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId, bool isAlive)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(false);
            avatars[avatarId].IsAlive = isAlive;
            deadAvatarsCount++;

            if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[avatarId].ActorNumber)
            {
                //Good or bad end
                if (PlayerNumbering.SortedPlayers.Length <= 2 && avatars[avatarId].IsAlive)
                {
                    endGamePanelRankText.text = "Vous êtes l'ULTIME DRIVER !";
                    endGamePanelCommentaryText.text = "Félicitation !";
                }
                else
                {
                    endGamePanelRankText.text = "Tu termines en " + PlayerNumbering.SortedPlayers.Length + "eme position.";
                    endGamePanelCommentaryText.text = "Tu conduis moins bien que ma \ngrand - mère !";
                }

                //Reset 
                //Camera
                PlayerCameraScript camScript = Camera.main.GetComponent<PlayerCameraScript>();
                camScript.enabled = false;

                //UI
                playerUI.SetActive(false);
                PauseMenu.inGame = false;

                //Sectors
                sectorSpawnManagementScript.enabled = false;

                //Leave room
                if (!PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LeaveRoom();
                }
                else
                {
                    if (PlayerNumbering.SortedPlayers.Length <= 2)
                    {
                        PhotonNetwork.LeaveRoom();
                        PhotonNetwork.Disconnect();
                    } 
                }
            }
        }

        //Update client player stats
        [PunRPC]
        private void UpdateClientsUIRPC(int id, float speed, int hp)
        {
            avatars[id].Stats.currentSpeed = speed;
            avatars[id].Stats.EngineHealth = hp;
        }

        //Update timer for all players
        [PunRPC]
        private void UpdateTimeWaited(int time)
        {
            timeWaited = time;
        }

        //Deconnect the player from a room
        public void LeaveRoom()
        {
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, i, false);
        }
    }
}