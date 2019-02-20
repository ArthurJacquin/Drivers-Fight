﻿using System.Collections;
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

        private void OnTriggerEnter(Collider whosThat)
        {
            if (whosThat.CompareTag("Player"))
            {
                Debug.LogWarning("Dans l'secteur !");
                whosThat.gameObject.GetComponent<PlayerMovementScript>().carEnterInSector();
            }
        }
    }
}