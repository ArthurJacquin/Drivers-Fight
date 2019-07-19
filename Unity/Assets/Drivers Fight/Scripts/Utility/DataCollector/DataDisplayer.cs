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
    private KillDataVault KillData;

    [SerializeField]
    private FactoryKillDataVault FactoryKillData;

    [SerializeField]
    private GameDurationDataVault GameDurationData;

    [SerializeField]
    private AnimationCurve Killchart;

    [SerializeField]
    private AnimationCurve FactoryKillchart;

    [SerializeField]
    private AnimationCurve GameDurationchart;

    public void ClearData()
    {
       foreach(Transform child in dataCollectorGO)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateData()
    {
        Killchart = new AnimationCurve();
        int playerKilled = 0;

        foreach (var entity in KillData.KillDatas)
        {
            Instantiate(KillIndicator, entity.pos, Quaternion.identity, dataCollectorGO);

            playerKilled++;
            Killchart.AddKey(entity.GameTime, playerKilled);
        }

        FactoryKillchart = new AnimationCurve();
        playerKilled = 0;
        foreach(var data in FactoryKillData.KillDatas)
        {
            playerKilled++;
            FactoryKillchart.AddKey(data.GameTime, playerKilled);
        }

        GameDurationchart = new AnimationCurve();

        foreach (var data in GameDurationData.datas)
        {
            GameDurationchart.AddKey(data.GameTime, data.TotalPlayers);
        }
    }
}
