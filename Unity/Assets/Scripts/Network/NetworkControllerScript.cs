using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class NetworkControllerScript : MonoBehaviour
    {
        /*[SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private LobbyNetworkScript LobbyNetworkScript;*/

        public static NetworkControllerScript instance = null;

        private bool GameStarted { get; set; }

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

        }

        private void ResetGame()
        {

        }

        private void EndGame()
        {

        }

        public void endBadDriverGame()
        {
            PhotonNetwork.Disconnect();
            LobbyNetworkScript.instance.ShowMainMenu();
        }

        public void endGoodDriverGame()
        {
            System.Threading.Thread.Sleep(5000);
            PhotonNetwork.Disconnect();
            LobbyNetworkScript.instance.ShowMainMenu();
        }

        public static implicit operator NetworkControllerScript(PlayerMovementScript v)
        {
            throw new NotImplementedException();
        }
    }
}
