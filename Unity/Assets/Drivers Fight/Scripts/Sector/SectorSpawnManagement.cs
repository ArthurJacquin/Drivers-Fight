﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

namespace DriversFight.Scripts
{
    public class SectorSpawnManagement : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] sectors;

        [SerializeField]
        private int firstSectorSpawn = 0;

        [SerializeField]
        private int timeNextSectorSpawn = 2;

        [SerializeField]
        private int timeToShowWarning = 15;

        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private Color colorBefore;

        [SerializeField]
        private Color colorAfter;

        private Collider colliderino;
        private Renderer rendererino;

        private System.Random rng = new System.Random();

        /*[SerializeField]
        private TextMesh warningText;*/

        private float timeToSpawnTheSector = 0f;

        private List<int> sectorsAlreadyPop;

        private int numberGeneratedSector = 0;
        private int numberGeneratedSectorSave = 0;

        private int sectorsFinalNumber = 0;

        private bool warningSector = true;
        private bool endGameScale = false;
        private float scaleSpeed = 0.03f;

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

            numberGeneratedSector = 0;
            endGameScale = false;
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

            numberGeneratedSector = 0;
            endGameScale = false;
        }

        void Update()
        {
            if (numberGeneratedSector <= sectorsFinalNumber)
            {
                if (PhotonNetwork.IsMasterClient && Time.time > (timeToSpawnTheSector-10) && warningSector)
                {
                    StartCoroutine(RandomSpawnSector());
                }
                if (PhotonNetwork.IsMasterClient && Time.time > timeToSpawnTheSector)
                {
                    StartCoroutine(RandomSpawnSector());
                }
            }
            else
            {
                if (!endGameScale)
                {
                    StartCoroutine(WaitBeforeEndSectorScale());
                }
                else
                {
                    sectors[1].transform.localScale = Vector3.Lerp(sectors[1].transform.localScale, new Vector3(320, 350, 550), scaleSpeed * Time.deltaTime);
                    sectors[5].transform.localScale = Vector3.Lerp(sectors[5].transform.localScale, new Vector3(320, 350, 550), scaleSpeed * Time.deltaTime);
                    sectors[3].transform.localScale = Vector3.Lerp(sectors[3].transform.localScale, new Vector3(550, 350, 320), scaleSpeed * Time.deltaTime);
                    sectors[7].transform.localScale = Vector3.Lerp(sectors[7].transform.localScale, new Vector3(550, 350, 320), scaleSpeed * Time.deltaTime);
                }
            }
        }

        IEnumerator WaitBeforeEndSectorScale()
        {
            yield return new WaitForSeconds(2);
            endGameScale = true;
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
            if (warningSector)
            {
                numberGeneratedSectorSave = numberGeneratedSector;
                sectors[sectorNumbers[numberGeneratedSector]].SetActive(true);
                colliderino = sectors[sectorNumbers[numberGeneratedSector]].GetComponent<Collider>();
                colliderino.enabled = false;
                rendererino = sectors[sectorNumbers[numberGeneratedSector]].GetComponent<Renderer>();
                rendererino.sharedMaterial.color = colorBefore;
                sectorsAlreadyPop.Add(sectorNumbers[numberGeneratedSector]);
                photonView.RPC("SpawnSector", RpcTarget.OthersBuffered, sectorNumbers[numberGeneratedSector]);
                warningSector = false;
            }
            else
            {
                rendererino = sectors[sectorNumbers[numberGeneratedSector]].GetComponent<Renderer>();
                rendererino.sharedMaterial.color = colorAfter;
                colliderino = sectors[sectorNumbers[numberGeneratedSector]].GetComponent<Collider>();
                colliderino.enabled = true;
                photonView.RPC("SpawnSector", RpcTarget.OthersBuffered, sectorNumbers[numberGeneratedSector]);
                numberGeneratedSector++;
                timeToSpawnTheSector += timeNextSectorSpawn;
                warningSector = true;
            }

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
