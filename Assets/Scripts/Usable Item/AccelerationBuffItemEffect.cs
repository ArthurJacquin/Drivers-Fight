using System.Collections;
using UnityEngine;
using Drivers.CharacterStats;

[CreateAssetMenu(menuName = "Item Effects/Acceleration Buff")]
public class AccelerationBuffItemEffect : UsableItemEffect
{
    public float Acceleration;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        StatModifier statModifier = new StatModifier(Acceleration, StatModType.Flat, parentItem);
        character.AccelerationSpeed.AddModifier(statModifier);
        character.UpdateStatValues();
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
    }

    public override string GetDescription()
    {
        return "Grants " + Acceleration + " acceleration for " + Duration + " seconds.";
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.AccelerationSpeed.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}