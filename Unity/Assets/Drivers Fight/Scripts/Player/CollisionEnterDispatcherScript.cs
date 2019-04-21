using System;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class CollisionEnterDispatcherScript : MonoBehaviour
    {
        public event Action<CollisionEnterDispatcherScript, Collider> CollisionEvent;

        public void OnCollisionEnter(Collision col)
        {
            CollisionEvent?.Invoke(this, col.collider);
        }
    }
}