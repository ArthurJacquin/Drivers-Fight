using System;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace DriversFight.Scripts
{
    public class CollisionEnterDispatcherScript : MonoBehaviour
    {
        public event Action<CollisionEnterDispatcherScript, Collider> CollisionEvent;
        public event Action<CollisionEnterDispatcherScript, int> SectorTriggerEvent;
        public event Action<CollisionEnterDispatcherScript, Collider> BotCollisionEvent;

        public void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "Car")
            {
                CollisionEvent?.Invoke(this, col.collider);
            } 
            else if (col.gameObject.tag == "Bot")
            {
                BotCollisionEvent?.Invoke(this, col.collider);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            if (col.gameObject.tag == "Sector")
                SectorTriggerEvent?.Invoke(this, i);
        }
    }
}