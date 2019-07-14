using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Drivers.LocalizationSettings;

namespace DriversFight.Scripts
{
    public class AvatarsControllerScript : MonoBehaviour
    {
        [SerializeField]
        private AvatarExposerScript[] avatars; //AvatarExposer des joueurs

        [SerializeField]
        private Transform[] startPositions; //Spawn position

        [SerializeField]
        private BotExposerScript[] bots;

        [SerializeField]
        private Transform[] botStartPosition; 

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
        private int totalPlayer = 0;

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
        private GameObject dropItemArea;

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

        private readonly Dictionary<Collider, BotExposerScript>
            colliderToBot = new Dictionary<Collider, BotExposerScript>();

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
                avatar.CollisionDispatcher.BotCollisionEvent += HandleBotCollision;
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
                    if (hit.normal == targetAvatar.transform.forward)
                        targetAvatar.Stats.TakeDamage((int)sourceAvatar.Stats.currentSpeed * 5, EquipmentType.FrontArmor);
                    if (hit.normal == -targetAvatar.transform.forward)
                        targetAvatar.Stats.TakeDamage((int)sourceAvatar.Stats.currentSpeed * 5, EquipmentType.RearArmor);
                    if (hit.normal == targetAvatar.transform.right)
                        targetAvatar.Stats.TakeDamage((int)sourceAvatar.Stats.currentSpeed * 5, EquipmentType.RightArmor);
                    if (hit.normal == -targetAvatar.transform.right)
                        targetAvatar.Stats.TakeDamage((int)sourceAvatar.Stats.currentSpeed * 5, EquipmentType.LeftArmor);
                }
            }
        }

        private void HandleBotCollision(CollisionEnterDispatcherScript collisionDispatcher,
           Collider col)
        {
            if (!dispatcherToAvatar.TryGetValue(collisionDispatcher, out var targetAvatar)
                || !colliderToBot.TryGetValue(col, out var sourceBot))
            {
                return;
            }
            Debug.Log("hit bot");
            RaycastHit hit;
            if (Physics.Raycast(sourceBot.transform.position, targetAvatar.transform.position - sourceBot.transform.position, out hit, 3, 1 << 8))
            {
                if (hit.normal == targetAvatar.transform.forward)
                    targetAvatar.Stats.TakeDamage((int)sourceBot.NavMeshAgent.speed * 5, EquipmentType.FrontArmor);
                if (hit.normal == -targetAvatar.transform.forward)
                    targetAvatar.Stats.TakeDamage((int)sourceBot.NavMeshAgent.speed * 5, EquipmentType.RearArmor);
                if (hit.normal == targetAvatar.transform.right)
                    targetAvatar.Stats.TakeDamage((int)sourceBot.NavMeshAgent.speed * 5, EquipmentType.RightArmor);
                if (hit.normal == -targetAvatar.transform.right)
                    targetAvatar.Stats.TakeDamage((int)sourceBot.NavMeshAgent.speed * 5, EquipmentType.LeftArmor);
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
            startGameControllerScript.PlayerSetup += SetupPlayer;

            foreach (var avatar in avatars)
            {
                dispatcherToAvatar.Add(avatar.CollisionDispatcher, avatar);
                colliderToAvatar.Add(avatar.MainCollider, avatar);
            }

            foreach(var bot in bots)
            {
                colliderToBot.Add(bot.MainCollider, bot);
            }
        }

        //Mise en place de la camera et du HUD du joueur
        //For master and clients
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

            dropItemArea.SetActive(true);

            //Sector reset
            sectorSpawnManagementScript.enabled = true;
            sectorSpawnManagementScript.enabled = false;

            //Hide mouse cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

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
        //Only for master
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
                avatar.Stats.ResetStats();
            }

            StartCoroutine("WaitForPlayersAndStartTheGame");
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

            //Execute the requested actions
            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                
                var intentReceiver = activatedIntentReceivers[i];
                var avatar = avatars[i];

                Character mystats = avatar.Stats;

                //Movements

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
            if (totalPlayer - deadAvatarsCount <= 1 && !waitingForPlayers)
            {
                for(var i = 0; i < avatars.Length; i++)
                {
                    if(avatars[i].transform.gameObject.activeSelf)
                        photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, i, true);
                }
            }
        }

        public void EndGame(string com, string rank)
        {
            endGamePanelRankText.text = rank;
            endGamePanelCommentaryText.text = com;

            //Reset 
            //Camera
            PlayerCameraScript camScript = Camera.main.GetComponent<PlayerCameraScript>();
            camScript.enabled = false;

            //UI
            playerUI.SetActive(false);
            PauseMenu.inGame = false;

            //Sectors
            sectorSpawnManagementScript.enabled = false;

            //Bots
            for (int i = 0; i < bots.Length; i++)
            {
                bots[i].BotRootGameObject.SetActive(false);
            }

            dropItemArea.SetActive(false);

            //Activate mouse cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            GameStarted = false;
            waitingForPlayers = false;
            activatedIntentReceivers = null;

            for (var i = 0; i < avatars.Length; i++)
            {
                avatars[i].AvatarRootGameObject.SetActive(false);
            }

            DisableIntentReceivers();
        }

        //Wait for players to connect to the game and disconnect if nobody in the room
        IEnumerator WaitForPlayersAndStartTheGame()
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
        
                if (endGamePanel.activeSelf)
                   yield break;

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

            //Start the game or not ?
            totalPlayer = PlayerNumbering.SortedPlayers.Length;

            //if alone, leave the game
            if (totalPlayer <= 1)
            {
                LeaveRoom();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                mainText.text = "DRIVER'S FIGHT !";
                mainText.gameObject.SetActive(false);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    timeWaited = 0;
                    photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
                }

                //Wait for game start
                while (timeWaited < 10)
                {
                    if (endGamePanel.activeSelf)
                        yield break;

                    mainText.text = "Game begin in " + (10 - timeWaited) + " !";
                    yield return new WaitForSeconds(1);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        timeWaited++;
                        photonView.RPC("UpdateTimeWaited", RpcTarget.OthersBuffered, timeWaited);
                    }
                }

                //Game start
                waitingForPlayers = false;

                EnableIntentReceivers();
                GameStarted = true;
                //items.SetActive(true);

                //Bots
                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i].BotRootTransform.position = botStartPosition[i].position;
                    bots[i].BotRootTransform.rotation = botStartPosition[i].rotation;
                    bots[i].BotRootGameObject.SetActive(true);
                    Debug.Log(botStartPosition[i].position);
                }

                //Sector reset
                if (PhotonNetwork.IsMasterClient)
                    sectorSpawnManagementScript.enabled = true;
                else
                {
                    sectorSpawnManagementScript.enabled = true;
                    sectorSpawnManagementScript.enabled = false;
                }

                mainText.text = "FIGHT !";

                yield return new WaitForSeconds(3);
                mainText.text = "DRIVER'S FIGHT !";
                mainText.gameObject.SetActive(false);
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

            if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[avatarId].ActorNumber)
            {
                string commentary = "";
                string rank = "";

                //Good or bad end
                if ((totalPlayer - deadAvatarsCount <= 1 && avatars[avatarId].IsAlive) || totalPlayer == 1)
                {
                    rank = LocalizationManager.Instance.GetText("ULTIMATE_DRIVER");
                    commentary = LocalizationManager.Instance.GetText("CONGRATULATION");
                }
                else
                {
                    rank = LocalizationManager.Instance.GetText("YOU_FINISH_IN") + (totalPlayer - deadAvatarsCount) + LocalizationManager.Instance.GetText("POSITION");
                    if (totalPlayer - deadAvatarsCount == 3)
                    {
                        rank = rank.Replace("nd", "rd");
                    }
                    commentary = LocalizationManager.Instance.GetText("DEFEAT_MESSAGE");
                }

                EndGame(commentary, rank);

                //Stats
                avatars[avatarId].Stats.ResetStats();

                //Leave room
                if (!PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LeaveRoom();
                }
                else
                {
                    if (totalPlayer - deadAvatarsCount <= 1)
                    {
                        StartCoroutine(WaitBeforeDeconnectMaster());
                    } 
                }
            }

            deadAvatarsCount++;
        }

        IEnumerator WaitBeforeDeconnectMaster()
        {
            yield return new WaitForSeconds(2);

            deadAvatarsCount = 0;
            gameStarted = false;

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
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