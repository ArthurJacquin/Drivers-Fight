using System.Collections;
using UnityEngine;
using Drivers.CharacterStats;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
    public int MaxSpeedBuff;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        StatModifier statModifier = new StatModifier(MaxSpeedBuff, StatModType.Flat, parentItem);
        character.MaximumSpeed.AddModifier(statModifier);
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
        character.UpdateStatValues();
    }

    public override string GetDescription()
    {
        return "Grants " + MaxSpeedBuff + " max. speed for " + Duration + " seconds.";
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.MaximumSpeed.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}