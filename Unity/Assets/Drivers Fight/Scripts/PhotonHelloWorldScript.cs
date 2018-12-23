using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PhotonHelloWorldScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button createRoomButton;

    [SerializeField]
    private Button joinRoomButton;

    [SerializeField]
    private Text welcomeMessageText;

    [SerializeField]
    private PhotonView targetPhotonView;

    private void Start()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            targetPhotonView.RPC("SayHello", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;

        createRoomButton.onClick.AddListener(AskForRoomCreation);
        joinRoomButton.onClick.AddListener(AskForRoomJoin);
    }

    public void AskForRoomCreation()
    {
        PhotonNetwork.CreateRoom("Toto");
    }

    public void AskForRoomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        welcomeMessageText.text = PhotonNetwork.IsMasterClient ? "Room Created !" : "Room Joined";
    }

    [PunRPC]
    public void SayHello(int playerNum)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            welcomeMessageText.text = $"Client {playerNum} said Hello !";
            targetPhotonView.RPC("SayHello", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            welcomeMessageText.text = "Server said Hello !";
        }
    }
}
