﻿using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace DriversFight.Scripts
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        [FormerlySerializedAs("PlayerActorId")]
        [SerializeField]
        private int PlayerIndex;

        [SerializeField]
        private PhotonView photonView;

        public void Update()
        {
            if (PlayerNumbering.SortedPlayers.Length <= PlayerIndex || 
                PlayerNumbering.SortedPlayers[PlayerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                photonView.RPC("WantToMoveLeftRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                photonView.RPC("WantToMoveBackwardRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                photonView.RPC("WantToMoveRightRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKey(KeyCode.Z))
            {
                photonView.RPC("WantToMoveForwardRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                photonView.RPC("WantToMoveLeftRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                photonView.RPC("WantToStopRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("WantToMoveRightRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                photonView.RPC("WantToStopRPC", RpcTarget.MasterClient, true);
            }
        }

        [PunRPC]
        void WantToMoveLeftRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                WantToMoveLeft = intent;
            }
        }

        [PunRPC]
        void WantToMoveBackwardRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (WantToMoveForward == false)
                {
                    WantToMoveBackward = intent;
                    WantToStopTheCar = false;
                }
            }
        }

        [PunRPC]
        void WantToMoveRightRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                WantToMoveRight = intent;
            }
        }

        [PunRPC]
        void WantToMoveForwardRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (WantToMoveBackward == false)
                {
                    WantToMoveForward = intent;
                    WantToStopTheCar = false;
                }
            }
        }

        [PunRPC]
        void WantToStopRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                WantToStopTheCar = intent;
            }
        }
    }
}