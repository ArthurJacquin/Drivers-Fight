using System;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class CollisionEnterDispatcherScript : MonoBehaviour
    {
        public event Action<CollisionEnterDispatcherScript, Collider> CollisionEvent;
        public event Action<CollisionEnterDispatcherScript> SectorTriggerEvent;

        public void OnCollisionEnter(Collision col)
        {
            CollisionEvent?.Invoke(this, col.collider);
        }

        private void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.tag == "Sector")
                SectorTriggerEvent?.Invoke(this);
        }

    }
}