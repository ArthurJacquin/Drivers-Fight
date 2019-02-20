using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using System;
namespace DriversFight.Scripts
{
    public class LobbyNetworkScript : MonoBehaviourPunCallbacks
    {
        public static LobbyNetworkScript instance = null;

        [Header("Menu")]
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button joinRoomButton;

        [SerializeField]
        private Button createRoomButton;

        [SerializeField]
        private TMPro.TextMeshProUGUI welcomeMessageText;

        [Header("End game panel")]
        [SerializeField]
        private GameObject endGamePanel;

        [SerializeField]
        private TMPro.TextMeshProUGUI rankingText;

        [SerializeField]
        private TMPro.TextMeshProUGUI commentaryText;

        [SerializeField]
        private Button backButton;

        [Header("Instantiation")]
        private AvatarExposerScript[] avatars;

        [SerializeField]
        private Transform[] startPositions;

        [SerializeField]
        NetworkControllerScript networkController;

        [Header("UI")]
        [SerializeField]
        private GameObject playerUI;

        [SerializeField]
        private PlayerUI playerUIScript;


        public event Action OnlinePlayReady;

        public event Action<int> PlayerJoined;

        public event Action<int> PlayerLeft;

        public event Action Disconnected;

        public event Action MasterClientSwitched;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.Log("oui");
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                Debug.Log("non");
            }

            playButton.onClick.AddListener(OnlinePlaySetup);
            createRoomButton.onClick.AddListener(AskForRoomCreation);
            joinRoomButton.onClick.AddListener(AskForRoomJoin);
        }

        private void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            playButton.gameObject.SetActive(true);
            playButton.interactable = true;

            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            welcomeMessageText.text = "Drivers Fight";

            endGamePanel.SetActive(false);
            playerUI.gameObject.SetActive(false);
            playerUIScript.enabled = false;
        }

        private void OnlinePlaySetup()
        {
            playButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(true);
            joinRoomButton.gameObject.SetActive(true);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            PhotonNetwork.ConnectUsingSettings();

            welcomeMessageText.text = "Join or Create a room ?";
        }

        public override void OnConnectedToMaster()
        {
            createRoomButton.interactable = true;
            joinRoomButton.interactable = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Disconnected?.Invoke();
        }

        public override void OnMasterClientSwitched(Player player)
        {
            MasterClientSwitched?.Invoke();
        }

        public void AskForRoomCreation()
        {
            PhotonNetwork.CreateRoom("Toto", new RoomOptions
            {
                MaxPlayers = 8,
                PlayerTtl = 10000
            });
        }

        public void AskForRoomJoin()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Room Joined");

            playButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);

            StartCoroutine(SetWelcomeMessageAndSetReadyAtTheEndOfFrame());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (otherPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            PlayerLeft?.Invoke(i);
        }

        private IEnumerator SetWelcomeMessageAndSetReadyAtTheEndOfFrame()
        {
            yield return new WaitForSeconds(0.1f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            welcomeMessageText.enabled = false;

            //Setup game
            var newPlayer = PhotonNetwork.Instantiate("PlayerRoot", startPositions[i].position, startPositions[i].rotation);

            PlayerCamera cam = Camera.main.GetComponent<PlayerCamera>();
            cam.enabled = true;
            cam.target = newPlayer.transform;

            PhotonNetwork.Instantiate("scriptsWhenGameLaunchPrefab", Vector3.zero, Quaternion.identity);

            playerUIScript.avatar = newPlayer.GetComponent<AvatarExposerScript>();
            playerUI.gameObject.SetActive(true);
            playerUIScript.enabled = true;
        }

        public void ShowEndGamePanel(int rank)
        {
            endGamePanel.SetActive(true);

            //Good or bad end
            if(rank == 1)
            {
                rankingText.text = "Vous êtes l'ULTIME DRIVER !";
                commentaryText.text = "";
            }
            else
            {
                rankingText.text = "Tu termines en " + rank + "eme position.";
                commentaryText.text = "Tu conduis moins bien que ma \ngrand - mère !"; 
            }

            backButton.onClick.AddListener(ShowMainMenu);
        }
    }
}