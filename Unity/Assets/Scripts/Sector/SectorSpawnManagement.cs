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
        private GameObject sector0;

        [SerializeField]
        private GameObject sector1;

        [SerializeField]
        private GameObject sector2;

        [SerializeField]
        private GameObject sector3;

        [SerializeField]
        private GameObject sector4;

        [SerializeField]
        private GameObject sector5;

        [SerializeField]
        private GameObject sector6;

        [SerializeField]
        private GameObject sector7;
        
        [SerializeField]
        private int firstSectorSpawn = 0;

        [SerializeField]
        private int timeNextSectorSpawn = 0;

        [SerializeField]
        private int timeToShowWarningDude = 0;

        /*[SerializeField]
        private TMPro.TextMeshProUGUI warningText;*/

        private float timeToSpawnTheSectorMaBoy = 0f;

        private List<GameObject> sectors = new List<GameObject>();

        private List<int> sectorsAlreadyPop = new List<int>();

        private int numberGeneratedSector = 0;

        private int sectorsFinalNumber = 0;

        private int sectorNumber;

        private void Start()
        {
            Debug.Log("Lancement du script de spawn de secteur");
            
            sectorsAlreadyPop.Add(-1);

            timeToSpawnTheSectorMaBoy = firstSectorSpawn + Time.time;

            sectors.Add(sector0);
            sectors.Add(sector1);
            sectors.Add(sector2);
            sectors.Add(sector3);
            sectors.Add(sector4);
            sectors.Add(sector5);
            sectors.Add(sector6);
            sectors.Add(sector7);

            sectorsFinalNumber = sectors.Count;
        }

        void Update()
        {
            if (numberGeneratedSector < sectorsFinalNumber)
            {
                if (Time.time > timeToSpawnTheSectorMaBoy)
                {
                    StartCoroutine(RandomSpawnSector());
                }

                /*if (timeToSpawnTheSectorMaBoy - Time.time <= 11)
                {
                    warningText.enabled = true;
                    StartCoroutine(CountDownSector());
                }*/

                if (timeToSpawnTheSectorMaBoy - Time.time > timeToShowWarningDude && timeToSpawnTheSectorMaBoy - Time.time < timeToShowWarningDude + 1)
                {
                    if (sectorsAlreadyPop.Count <= sectorsFinalNumber)
                    {
                        sectorNumber = Random.Range(0, sectors.Count);
                        int i = 1;
                        while (i > 0)
                        {
                            i = 0;
                            foreach (int pop in sectorsAlreadyPop)
                            {
                                if (sectorNumber == pop)
                                {
                                    sectorNumber = Random.Range(0, sectors.Count);
                                    i++;
                                }
                            }
                        }
                    }
                    /*StopCoroutine(CountDownSector());
                    warningText.enabled = false;*/
                }
            }

        }

        private IEnumerator RandomSpawnSector()
        {
            PhotonNetwork.Instantiate("Sectors/" + sectors[sectorNumber].name, sectors[sectorNumber].transform.position, Quaternion.identity);
            sectorsAlreadyPop.Add(sectorNumber);
            numberGeneratedSector++;
            timeToSpawnTheSectorMaBoy += timeNextSectorSpawn;

            Debug.LogWarning("secteur : " + sectorNumber + "    " + Time.time);

            yield return new WaitForSeconds(1);
        }

        /*private IEnumerator CountDownSector()
        {
            while (true)
            {
                int seconds = Mathf.FloorToInt(timeToSpawnTheSectorMaBoy - Time.time);
                string niceTime = string.Format("{00}", seconds);
                warningText.text = "Sector " + sectorNumber + " will be close in " + niceTime;
                yield return new WaitForSeconds(1);
            }
        }*/
    }
}
