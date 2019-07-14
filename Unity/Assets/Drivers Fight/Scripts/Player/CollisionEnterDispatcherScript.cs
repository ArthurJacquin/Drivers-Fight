using System;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class CollisionEnterDispatcherScript : MonoBehaviour
    {
        public event Action<CollisionEnterDispatcherScript, Collider> CollisionEvent;
        public event Action<CollisionEnterDispatcherScript> SectorTriggerEvent;
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
            if(col.gameObject.tag == "Sector")
                SectorTriggerEvent?.Invoke(this);
        }
    }
}