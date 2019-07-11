﻿using UnityEngine;
using System.Text;
using Drivers.LocalizationSettings;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName ="Items/Item")]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    public string ItemName;
    public Sprite Icon;
    [Range(1,999)]
    public int MaximumStacks = 1;

    [Space]
    [Header("Description of the item")]
    [SerializeField] string ItemDescription;

    protected static readonly StringBuilder sb = new StringBuilder();

    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
    #endif

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }

    public virtual string GetItemType()
    {
        return "";
    }

    public virtual string GetDescription()
    {
        if (ItemDescription == "COIN_DESCRIPTION")
        {
            return LocalizationManager.Instance.GetText("COIN_DESCRIPTION");
        }
        else if (ItemDescription == "REPAIR_KIT_DESCRIPTION")
        {
            return LocalizationManager.Instance.GetText("REPAIR_KIT_DESCRIPTION");
        }
        return "";
    }
}