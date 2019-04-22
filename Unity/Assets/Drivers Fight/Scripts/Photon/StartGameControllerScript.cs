using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace DriversFight.Scripts
{
    public class StartGameControllerScript : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject gameModeSelection;

        [SerializeField]
        private Button localPlayButton;

        [SerializeField]
        private Button onlinePlayButton;

        [SerializeField]
        private Button createRoomButton;

        [SerializeField]
        private Button joinRoomButton;

        [SerializeField]
        private Text welcomeMessageText;

        [SerializeField]
        private GameObject endGamePanel;

        public event Action OnlinePlayReady;

        public event Action OfflinePlayReady;

        public event Action<int> PlayerJoined;

        public event Action<int> PlayerLeft;

        public event Action<int> PlayerSetup;

        public event Action Disconnected;

        public event Action MasterClientSwitched;

        private void Awake()
        {
            localPlayButton.onClick.AddListener(LocalPlaySetup);
            onlinePlayButton.onClick.AddListener(OnlinePlaySetup);
            createRoomButton.onClick.AddListener(AskForRoomCreation);
            joinRoomButton.onClick.AddListener(AskForRoomJoin);
        }

        private void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            mainMenu.SetActive(true);

            localPlayButton.gameObject.SetActive(true);
            onlinePlayButton.gameObject.SetActive(true);
            localPlayButton.interactable = true;
            onlinePlayButton.interactable = true;
            gameModeSelection.SetActive(false);

            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            welcomeMessageText.text = "Choose Game Mode !";
        }

        private void LocalPlaySetup()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);

            welcomeMessageText.text = "Let's Play !";

            OfflinePlayReady?.Invoke();

            PlayerJoined?.Invoke(0);
            PlayerJoined?.Invoke(1);
            PlayerJoined?.Invoke(2);
            PlayerJoined?.Invoke(3);
        }

        private void OnlinePlaySetup()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(true);
            joinRoomButton.gameObject.SetActive(true);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            welcomeMessageText.text = "Create or Join an existing Game ?";

            PhotonNetwork.ConnectUsingSettings();
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
            PhotonNetwork.CreateRoom("Drivers Fight", new RoomOptions
            {
                MaxPlayers = 4,
                PlayerTtl = 10000
            });
        }

        public void AskForRoomJoin()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);

            StartCoroutine(SetWelcomeMessageAndSetReadyAtTheEndOfFrame());
        }

        public override void OnLeftRoom()
        {
            endGamePanel.SetActive(true);

            PhotonNetwork.Disconnect();
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(InformPlayerJoinedEndOfFrame(newPlayer.ActorNumber));
            }
        }

        private IEnumerator InformPlayerJoinedEndOfFrame(int actorNumber)
        {
            yield return new WaitForSeconds(2f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (actorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            PlayerJoined?.Invoke(i);
        }

        private IEnumerator SetWelcomeMessageAndSetReadyAtTheEndOfFrame()
        {
            yield return new WaitForSeconds(2f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            StartCoroutine(WelcomeMessage(i));

            OnlinePlayReady?.Invoke();

            if (PhotonNetwork.IsMasterClient)
            {
                PlayerJoined?.Invoke(i);
            }

            PlayerSetup?.Invoke(i);
        }

        IEnumerator WelcomeMessage(int i)
        {
            welcomeMessageText.text = $"You are Actor : {PhotonNetwork.LocalPlayer.ActorNumber}\n "
                                      + $"You are controlling Avatar {i}, Let's Play !";

            yield return new WaitForSeconds(5f);

            welcomeMessageText.enabled = false;
        }
    }
}
