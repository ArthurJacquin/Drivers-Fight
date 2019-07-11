using System.Collections;
using UnityEngine;
using Drivers.CharacterStats;
using Drivers.LocalizationSettings;

[CreateAssetMenu(menuName = "Item Effects/Max Speed Buff")]
public class MaxSpeedBuffItemEffect : UsableItemEffect
{
    public int MaxSpeed;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        StatModifier statModifier = new StatModifier(MaxSpeed, StatModType.Flat, parentItem);
        character.MaximumSpeed.AddModifier(statModifier);
        character.UpdateStatValues();
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
    }

    public override string GetDescription()
    {
        return LocalizationManager.Instance.GetText("GRANTS") + " " + MaxSpeed + " " + LocalizationManager.Instance.GetText("MAX_SPEED_FOR") + " " + Duration + " " + LocalizationManager.Instance.GetText("SECONDS") + ".";
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.MaximumSpeed.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}