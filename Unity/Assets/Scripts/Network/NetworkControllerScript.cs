using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class NetworkControllerScript : MonoBehaviour
    {
        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private LobbyNetworkScript lobbyNetworkScript;

        public int playerCount

        private bool GameStarted { get; set; }

        private void Awake()
        {

        }

        //Réinitialise tout pour le début d'une nouvelle partie
        private void ResetGame()
        {
            
        }

        [PunRPC]
        private void EndGame()
        {
            //TODO : Ecran de fin de partie
            lobbyNetworkScript.ShowMainMenu();
        }
    }
}
