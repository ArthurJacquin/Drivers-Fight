using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace DriversFight.Scripts
{
    public class SectorSpawnManagement : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] sectors;
        
        [SerializeField]
        private int firstSectorSpawn = 60;

        [SerializeField]
        private int timeNextSectorSpawn = 30;

        [SerializeField]
        private int timeToShowWarning = 15;

        [SerializeField]
        private PhotonView photonView;

        private System.Random rng = new System.Random();

        /*[SerializeField]
        private TextMesh warningText;*/

        private float timeToSpawnTheSector = 0f;

        private List<int> sectorsAlreadyPop;

        private int numberGeneratedSector = 0;

        private int sectorsFinalNumber = 0;

        private List<int> sectorNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

        private void OnEnable()
        {
            Debug.Log("Lancement du script de spawn de secteur");

            //Reset
            sectorsAlreadyPop = new List<int>();
            foreach(var sector in sectors)
            {
                if (sector.activeSelf)
                    sector.SetActive(false);
            }

            timeToSpawnTheSector = firstSectorSpawn + Time.time;
            Shuffle(sectorNumbers);
            sectorsFinalNumber = sectors.Length - 1;

            if (sectorsAlreadyPop.Count != 0)
            {
                foreach (var sector in sectorsAlreadyPop)
                {
                    sectorsAlreadyPop.Remove(sector);
                }
            }
        }

        private void OnDisable()
        {
            //Reset
            sectorsAlreadyPop = new List<int>();
            foreach (var sector in sectors)
            {
                if (sector.activeSelf)
                    sector.SetActive(false);
            }

            timeToSpawnTheSector = firstSectorSpawn + Time.time;
            Shuffle(sectorNumbers);
            sectorsFinalNumber = sectors.Length - 1;

            if (sectorsAlreadyPop.Count != 0)
            {
                foreach (var sector in sectorsAlreadyPop)
                {
                    sectorsAlreadyPop.Remove(sector);
                }
            }
        }

        void Update()
        {
            if (numberGeneratedSector <= sectorsFinalNumber)
            {
                if (PhotonNetwork.IsMasterClient && Time.time > timeToSpawnTheSector)
                {
                    StartCoroutine(RandomSpawnSector());
                }
            }
        }

        public void Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private IEnumerator RandomSpawnSector()
        {
            sectors[sectorNumbers[numberGeneratedSector]].SetActive(true);
            sectorsAlreadyPop.Add(sectorNumbers[numberGeneratedSector]);
            photonView.RPC("SpawnSector", RpcTarget.OthersBuffered, sectorNumbers[numberGeneratedSector]);

            numberGeneratedSector++;
            timeToSpawnTheSector += timeNextSectorSpawn;

            yield return new WaitForSeconds(1);
        }

        /*private IEnumerator CountDownSector()
        {
            while (true)
            {
                int seconds = Mathf.FloorToInt(timeToSpawnTheSector - Time.time);
                string niceTime = string.Format("{00}", seconds);
                warningText.text = "Sector " + sectorNumber + " will be close in " + niceTime;
                yield return new WaitForSeconds(1);
            }
        }*/

        [PunRPC]
        private void SpawnSector(int sectorNumber)
        {
            sectors[sectorNumber].SetActive(true);
        }
    }
}
