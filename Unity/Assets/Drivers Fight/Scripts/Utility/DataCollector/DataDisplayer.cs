using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject KillIndicator;

    [SerializeField]
    private Transform dataCollectorGO;

    [SerializeField]
    private KillDataVault data;

    [SerializeField]
    private AnimationCurve chart;
    
    private void OnEnable()
    {
        chart = new AnimationCurve();
        int playerKilled = 0;

        foreach (var entity in data.KillDatas)
        {
            Instantiate(KillIndicator, entity.pos, Quaternion.identity, dataCollectorGO);

            playerKilled++;
            chart.AddKey(entity.GameTime, playerKilled);
        }
    }

    private void OnDisable()
    {
       foreach(Transform child in dataCollectorGO)
        {
            Destroy(child.gameObject);
        }
    }
}
