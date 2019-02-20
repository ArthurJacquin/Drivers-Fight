using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DriversFight.Scripts
{ 
    public class SectorPlayerColliderManagement : MonoBehaviour
    {
        private BoxCollider box;

        // Start is called before the first frame update
        void Start()
        {
            box = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("v,," + other.gameObject.tag);
            if (other.gameObject.tag.Equals("Car"))
            {
                Debug.LogWarning("Dans l'secteur !");
                other.gameObject.GetComponent<PlayerMovementScript>().carEnterInSector();
            }
        }
    }
}