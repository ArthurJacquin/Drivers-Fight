using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Kill Vault", menuName ="Heat/New Kill Vault")]
public class KillDataVault : ScriptableObject
{
    [SerializeField] private List<EntityKill> entities;
    public List<EntityKill> KillDatas => this.entities;

    public void ResetData()
    {
        this.entities.Clear();
    }

    public void AddKillPosEntry(Vector3 pos)
    {
        this.entities.Add(new EntityKill() { pos = pos });
    }
    public void AddKillPosEntry(Vector3 pos, float timeStamp)
    {
        this.entities.Add(new EntityKill() { pos = pos, GameTime = timeStamp });
    }

    public void AddPlayerKill(EntityKill data)
    {
        this.entities.Add(data);
    }
}

[Serializable]
public class EntityKill
{
    public Vector3 pos;
    public float GameTime;
}
