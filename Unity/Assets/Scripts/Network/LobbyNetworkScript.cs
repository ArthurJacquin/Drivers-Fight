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
        [Header("UI")]
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button joinRoomButton;

        [SerializeField]
        private Button createRoomButton;

        [SerializeField]
        private TMPro.TextMeshProUGUI welcomeMessageText;

        [Header("Instantiation")]
        private AvatarExposerScript[] avatars;

        [SerializeField]
        private Transform[] startPositions;

        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private GameObject sectorSpawnerPrefab;

        public event Action OnlinePlayReady;

        public event Action<int> PlayerJoined;

        public event Action<int> PlayerLeft;

        public event Action Disconnected;

        public event Action MasterClientSwitched;

        private void Awake()
        {
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(InformPlayerJoinedEndOfFrame(newPlayer.ActorNumber));
            }
        }

        private IEnumerator InformPlayerJoinedEndOfFrame(int actorNumber)
        {
            yield return new WaitForSeconds(0.1f);
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
            yield return new WaitForSeconds(0.1f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            welcomeMessageText.text = $"You are Actor : {PhotonNetwork.LocalPlayer.ActorNumber}\n "
                                      + $"You are controlling Avatar {i}, Let's Play !";

            PhotonNetwork.Instantiate(playerPrefab.name, startPositions[i].position, startPositions[i].rotation);

            //Lancement de la génération des secteurs si master client
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Sectors/" + sectorSpawnerPrefab.name, sectorSpawnerPrefab.transform.position, Quaternion.identity);
            }
        }
    }
}