using Drivers.CharacterStats;

public class OldItem
{
    public void Equip(Character c)
    {
        // Create the modifiers and set the Source to "this"
        // Note that we don't need to store the modifiers in variables anymore
        //c.Strength.AddModifier(new StatModifier(10, StatModType.Flat, this));
        //c.Strength.AddModifier(new StatModifier(0.1f, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        // Remove all modifiers applied by "this" Item
        //c.Strength.RemoveAllModifiersFromSource(this);
    }
}