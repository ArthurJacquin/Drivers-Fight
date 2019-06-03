using UnityEngine;

[CreateAssetMenu(menuName ="Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public int HealthAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.EngineHealth += HealthAmount;
    }

    public override string GetDescription()
    {
        return "Heals for " + HealthAmount + " health.";
    }
}
