using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class NetworkControllerScript : MonoBehaviour
    {
        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private LobbyNetworkScript LobbyNetworkScript;

        private bool GameStarted { get; set; }

        private void Awake()
        {

        }



        private void ResetGame()
        {

        }

        private void EndGame()
        {

        }
    }
}
