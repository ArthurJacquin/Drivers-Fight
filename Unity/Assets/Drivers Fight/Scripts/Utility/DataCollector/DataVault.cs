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

[CreateAssetMenu(fileName = "Factory Kill Vault", menuName = "Heat/New FactoryKill Vault")]
public class FactoryKillDataVault : ScriptableObject
{
    [SerializeField] private List<FactoryEntityKill> entities;
    public List<FactoryEntityKill> KillDatas => this.entities;

    public void ResetData()
    {
        this.entities.Clear();
    }

    public void AddFactoryKillEntry(float timeStamp)
    {
        this.entities.Add(new FactoryEntityKill() { GameTime = timeStamp });
    }

    public void AddPlayerKill(FactoryEntityKill data)
    {
        this.entities.Add(data);
    }
}

[CreateAssetMenu(fileName = "Game Duration Vault", menuName = "Heat/Game Duration Vault")]
public class GameDurationDataVault : ScriptableObject
{
    [SerializeField] private List<GameDuration> entities;
    public List<GameDuration> datas => this.entities;

    public void ResetData()
    {
        this.entities.Clear();
    }

    public void AddGameDurationEntry(int players, float timeStamp)
    {
        this.entities.Add(new GameDuration() { TotalPlayers = players, GameTime = timeStamp });
    }
}

[Serializable]
public class EntityKill
{
    public Vector3 pos;
    public float GameTime;
}

[Serializable]
public class FactoryEntityKill
{
    public float GameTime;
}

[Serializable]
public class GameDuration
{
    public int TotalPlayers;
    public float GameTime;
}
