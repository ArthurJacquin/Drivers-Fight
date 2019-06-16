using UnityEngine;

[CreateAssetMenu(menuName ="Item Effects/Engine Repair")]
public class EngineRepairItemEffect : UsableItemEffect
{
    public int HealthAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.EngineHealth += HealthAmount;
        if (character.EngineHealth > 500)
        {
            character.EngineHealth = 500;
        }
    }

    public override string GetDescription()
    {
        if (Application.systemLanguage == SystemLanguage.French)
        {
            return "Répare le moteur pour " + HealthAmount + " santé.";
        }

        return "Repair engine for " + HealthAmount + " health.";
    }
}
