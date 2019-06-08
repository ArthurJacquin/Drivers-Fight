using UnityEngine;

[CreateAssetMenu(menuName ="Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
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
        return "Repair engine for " + HealthAmount + " health.";
    }
}
