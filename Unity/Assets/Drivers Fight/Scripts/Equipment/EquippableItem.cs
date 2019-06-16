using UnityEngine;
using Drivers.CharacterStats;
using System.ComponentModel;

public enum EquipmentType
{
    [Description("Front Armor")]
    FrontArmor,
    [Description("Rear Armor")]
    RearArmor,
    [Description("Right Armor")]
    RightArmor,
    [Description("Left Armor")]
    LeftArmor,
    Wheel,
    Tires,
    Engine
}

[CreateAssetMenu(menuName ="Items/Equippable Item")]
public class EquippableItem : Item
{
    public int FrontBumperArmorBonus;
    public int RearBumperArmorBonus;
    public int RightFlankArmorBonus;
    public int LeftFlankArmorBonus;
    public int WheelArmorBonus;
    public int TiresArmorBonus;
    public int MaximumSpeedBonus;
    public float AccelerationSpeedBonus;
    public float DecelerationSpeedBonus;
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

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

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

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        // Not percent stat
        AddStat(FrontBumperArmorBonus, "Front armor");
        AddStat(RearBumperArmorBonus, "Rear armor");
        AddStat(RightFlankArmorBonus, "Right armor");
        AddStat(LeftFlankArmorBonus, "Left armor");
        AddStat(TiresArmorBonus, "Tires armor");
        AddStat(WheelArmorBonus, "Wheel armor");
        AddStat(MaximumSpeedBonus, "Max. speed");
        AddStat(AccelerationSpeedBonus, "Acceleration");
        AddStat(DecelerationSpeedBonus, "Deceleration");
        AddStat(ManeuverabilityBonus, "Maneuverability");
        AddStat(DamageBonus, "Damage");

        // Percent stat
        AddStat(FrontBumperArmorPercentBonus, "Front armor", isPercent: true);
        AddStat(RearBumperArmorPercentBonus, "Rear armor", isPercent: true);
        AddStat(RightFlankArmorPercentBonus, "Right armor", isPercent: true);
        AddStat(LeftFlankArmorPercentBonus, "Left armor", isPercent: true);
        AddStat(TiresArmorPercentBonus, "Tires armor", isPercent: true);
        AddStat(WheelArmorPercentBonus, "Wheel armor", isPercent: true);
        AddStat(MaximumSpeedPercentBonus, "Max. speed", isPercent: true);
        AddStat(AccelerationSpeedPercentBonus, "Acceleration", isPercent: true);
        AddStat(DecelerationSpeedPercentBonus, "Deceleration", isPercent: true);
        AddStat(ManeuverabilityPercentBonus, "Maneuverability", isPercent: true);
        AddStat(DamagePercentBonus, "Damage", isPercent: true);

        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (value > 0)
            {
                sb.Append("+");
            }

            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }

            sb.Append(statName);
        }
    }
}