using UnityEngine;
using Drivers.CharacterStats;

public enum EquipmentType
{
    ParechocAvant,
    ParechocArrière,
    FlancDroit,
    FlancGauche,
    Volant,
    Pneus,
    Moteur
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    public int FrontBumperArmorBonus;
    public int RearBumperArmorBonus;
    public int RightFlankArmorBonus;
    public int LeftFlankArmorBonus;
    public int WheelArmorBonus;
    public int TiresArmorBonus;
    public int MaximumSpeedBonus;
    public int AccelerationSpeedBonus;
    public int DecelerationSpeedBonus;
    public int ManeuverabilityBonus;
    public int DamageBonus;

    [Space]
    public float FrontBumperArmorPercentBonus;
    public float RearBumperArmorPercentBonus;
    public float RightFlankArmorPercentBonus;
    public float LeftFlankArmorPercentBonus;
    public float WheelArmorPercentBonus;
    public float TiresArmorPercentBonus;
    public float MaximumSpeedPercentBonus;
    public float AccelerationSpeedPercentBonus;
    public float DecelerationSpeedPercentBonus;
    public float ManeuverabilityPercentBonus;
    public float DamagePercentBonus;

    [Space]
    public EquipmentType EquipmentType;

    public void Equip(Character c)
    {
        // Flat stats
        if (FrontBumperArmorBonus != 0)
        {
            c.FrontBumperArmor.AddModifier(new StatModifier(FrontBumperArmorBonus, StatModType.Flat, this));
        }
        if (RearBumperArmorBonus != 0)
        {
            c.RearBumperArmor.AddModifier(new StatModifier(RearBumperArmorBonus, StatModType.Flat, this));
        }
        if (RightFlankArmorBonus != 0)
        {
            c.RightFlankArmor.AddModifier(new StatModifier(RightFlankArmorBonus, StatModType.Flat, this));
        }
        if (LeftFlankArmorBonus != 0)
        {
            c.LeftFlankArmor.AddModifier(new StatModifier(LeftFlankArmorBonus, StatModType.Flat, this));
        }
        if (WheelArmorBonus != 0)
        {
            c.WheelArmor.AddModifier(new StatModifier(WheelArmorBonus, StatModType.Flat, this));
        }
        if (TiresArmorBonus != 0)
        {
            c.TiresArmor.AddModifier(new StatModifier(TiresArmorBonus, StatModType.Flat, this));
        }
        if (MaximumSpeedBonus != 0)
        {
            c.MaximumSpeed.AddModifier(new StatModifier(MaximumSpeedBonus, StatModType.Flat, this));
        }
        if (AccelerationSpeedBonus != 0)
        {
            c.AccelerationSpeed.AddModifier(new StatModifier(AccelerationSpeedBonus, StatModType.Flat, this));
        }
        if (DecelerationSpeedBonus != 0)
        {
            c.DecelerationSpeed.AddModifier(new StatModifier(DecelerationSpeedBonus, StatModType.Flat, this));
        }
        if (ManeuverabilityBonus != 0)
        {
            c.Maneuverability.AddModifier(new StatModifier(ManeuverabilityBonus, StatModType.Flat, this));
        }
        if (DamageBonus != 0)
        {
            c.Damage.AddModifier(new StatModifier(DamageBonus, StatModType.Flat, this));
        }

        // Percent stats
        if (FrontBumperArmorPercentBonus != 0)
        {
            c.FrontBumperArmor.AddModifier(new StatModifier(FrontBumperArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (RearBumperArmorPercentBonus != 0)
        {
            c.RearBumperArmor.AddModifier(new StatModifier(RearBumperArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (RightFlankArmorPercentBonus != 0)
        {
            c.RightFlankArmor.AddModifier(new StatModifier(RightFlankArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (LeftFlankArmorPercentBonus != 0)
        {
            c.LeftFlankArmor.AddModifier(new StatModifier(LeftFlankArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (WheelArmorPercentBonus != 0)
        {
            c.WheelArmor.AddModifier(new StatModifier(WheelArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (TiresArmorPercentBonus != 0)
        {
            c.TiresArmor.AddModifier(new StatModifier(TiresArmorPercentBonus, StatModType.PercentMult, this));
        }
        if (MaximumSpeedPercentBonus != 0)
        {
            c.MaximumSpeed.AddModifier(new StatModifier(MaximumSpeedPercentBonus, StatModType.PercentMult, this));
        }
        if (AccelerationSpeedPercentBonus != 0)
        {
            c.AccelerationSpeed.AddModifier(new StatModifier(AccelerationSpeedPercentBonus, StatModType.PercentMult, this));
        }
        if (DecelerationSpeedPercentBonus != 0)
        {
            c.DecelerationSpeed.AddModifier(new StatModifier(DecelerationSpeedPercentBonus, StatModType.PercentMult, this));
        }
        if (ManeuverabilityPercentBonus != 0)
        {
            c.Maneuverability.AddModifier(new StatModifier(ManeuverabilityPercentBonus, StatModType.PercentMult, this));
        }
        if (DamagePercentBonus != 0)
        {
            c.Damage.AddModifier(new StatModifier(DamagePercentBonus, StatModType.PercentMult, this));
        }
    }

    public void Unequip(Character c)
    {
        c.FrontBumperArmor.RemoveAllModifiersFromSource(this);
        c.RearBumperArmor.RemoveAllModifiersFromSource(this);
        c.RightFlankArmor.RemoveAllModifiersFromSource(this);
        c.LeftFlankArmor.RemoveAllModifiersFromSource(this);
        c.TiresArmor.RemoveAllModifiersFromSource(this);
        c.WheelArmor.RemoveAllModifiersFromSource(this);
        c.MaximumSpeed.RemoveAllModifiersFromSource(this);
        c.AccelerationSpeed.RemoveAllModifiersFromSource(this);
        c.DecelerationSpeed.RemoveAllModifiersFromSource(this);
        c.Maneuverability.RemoveAllModifiersFromSource(this);
        c.Damage.RemoveAllModifiersFromSource(this);
    }
}
