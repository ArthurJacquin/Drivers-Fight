using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
            if(PhotonNetwork.IsMasterClient)
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
        }

        private void ResetGame()
        {

        }

        public void EndGame()
        {
            var rank = PlayerNumbering.SortedPlayers.Length;
            PhotonNetwork.Disconnect();
            LobbyNetworkScript.instance.ShowEndGamePanel(rank);
        }
    }
}
