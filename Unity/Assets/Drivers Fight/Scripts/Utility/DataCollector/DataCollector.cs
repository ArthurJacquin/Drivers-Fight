using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;

    [SerializeField] private KillDataVault killData;
    [SerializeField] private FactoryKillDataVault FactoryKillData;
    [SerializeField] private GameDurationDataVault gameDurationData;
    [SerializeField] private bool resetData = true;

    public static DataCollector Instance()
    {
        return instance;
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        if (resetData)
            instance.killData.ResetData();
    }

    public static void RegisterEntityKillWithTime(Vector3 pos, float time)
    {
        if (instance != null && instance.killData != null)
            instance.killData.AddKillPosEntry(pos, time);
    }

    public static void RegisterEntityKillNearFactoryWithTime(float time)
    {
        if (instance != null && instance.FactoryKillData != null)
            instance.FactoryKillData.AddFactoryKillEntry(time);
    }

    public static void RegisterGameDuration(int players, float time)
    {
        if (instance != null && instance.gameDurationData != null)
            instance.gameDurationData.AddGameDurationEntry(players, time);
    }


    public void GenerateData()
    {
        for (int i = 0; i < 50; i++)
        {
            DataCollector.RegisterEntityKillWithTime(new Vector3(Random.Range(-500, 500), 0.5f, Random.Range(-500, 500)), Random.Range(0, 15));
            DataCollector.RegisterEntityKillNearFactoryWithTime(Random.Range(0, 15));
            DataCollector.RegisterGameDuration(Random.Range(0, 8), Random.Range(0, 15));
            Debug.Log("Generation... " + i + "/50");
        }
    }
}
