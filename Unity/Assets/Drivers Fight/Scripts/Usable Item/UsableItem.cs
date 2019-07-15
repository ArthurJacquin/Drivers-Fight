using System.Collections.Generic;
using UnityEngine;

public enum UsableType
{
    Usable
}

[CreateAssetMenu(menuName ="Items/Usable Item")]
public class UsableItem : Item
{
    [Space]
    public bool IsConsumable;

    [Space]
    [Header("Usable effect")]
    public List<UsableItemEffect> Effects;

    [Space]
    [Header("Usable type")]
    public UsableType UsableType;

    public virtual void Use(Character character)
    {
        foreach (UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, character);
        }
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        foreach (UsableItemEffect effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        }

        return sb.ToString();
    }
}
