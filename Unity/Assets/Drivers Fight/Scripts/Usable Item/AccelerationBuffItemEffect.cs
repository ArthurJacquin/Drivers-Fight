using System.Collections;
using UnityEngine;
using Drivers.CharacterStats;
using Drivers.LocalizationSettings;

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
        FindObjectOfType<AudioManager>().Play("Nitro");
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
    }

    public override string GetDescription()
    {
        return LocalizationManager.Instance.GetText("GRANTS") + " " + Acceleration + " " + LocalizationManager.Instance.GetText("ACCELERATION_FOR") + " " + Duration + " " + LocalizationManager.Instance.GetText("SECONDS") + ".";
    }

    private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.AccelerationSpeed.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}