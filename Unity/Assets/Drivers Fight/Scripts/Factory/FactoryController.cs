using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class FactoryController : MonoBehaviour
    {
        public GameObject factoryPannel;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Car"))
            {
                OpenFactory();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Car"))
            {
                CloseFactory();
            }
        }

        void OpenFactory()
        {
            factoryPannel.SetActive(true);
        }

        void CloseFactory()
        {
            factoryPannel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

