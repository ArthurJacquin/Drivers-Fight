using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;

    [SerializeField] private KillDataVault DataVault;
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
            instance.DataVault.ResetData();
    }

    public static void RegisterEntityKill(MonoBehaviour m)
    {
        if (instance != null && instance.DataVault != null)
            instance.DataVault.AddKillPosEntry(m.transform.position);
    }

    public static void RegisterEntityKillWithTime(MonoBehaviour m)
    {
        if (instance != null && instance.DataVault != null)
            instance.DataVault.AddKillPosEntry(m.transform.position, Time.time);
    }
}
