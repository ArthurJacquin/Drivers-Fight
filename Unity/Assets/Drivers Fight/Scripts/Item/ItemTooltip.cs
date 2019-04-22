using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemSlotText;
    [SerializeField] Text ItemStatsText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(EquippableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.EquipmentType.ToString();

        sb.Length = 0;
        // Not percent stat
        AddStat(item.FrontBumperArmorBonus, "Front armor");
        AddStat(item.RearBumperArmorBonus, "Rear armor");
        AddStat(item.RightFlankArmorBonus, "Right armor");
        AddStat(item.LeftFlankArmorBonus, "Left armor");
        AddStat(item.TiresArmorBonus, "Tires armor");
        AddStat(item.WheelArmorBonus, "Wheel armor");
        AddStat(item.MaximumSpeedBonus, "Max. speed");
        AddStat(item.AccelerationSpeedBonus, "Acceleration");
        AddStat(item.DecelerationSpeedBonus, "Deceleration");
        AddStat(item.ManeuverabilityBonus, "Maneuverability");
        AddStat(item.DamageBonus, "Damage");

        // Percent stat
        AddStat(item.FrontBumperArmorPercentBonus, "Front armor", isPercent: true);
        AddStat(item.RearBumperArmorPercentBonus, "Rear armor", isPercent: true);
        AddStat(item.RightFlankArmorPercentBonus, "Right armor", isPercent: true);
        AddStat(item.LeftFlankArmorPercentBonus, "Left armor", isPercent: true);
        AddStat(item.TiresArmorPercentBonus, "Tires armor", isPercent: true);
        AddStat(item.WheelArmorPercentBonus, "Wheel armor", isPercent: true);
        AddStat(item.MaximumSpeedPercentBonus, "Max. speed", isPercent: true);
        AddStat(item.AccelerationSpeedPercentBonus, "Acceleration", isPercent: true);
        AddStat(item.DecelerationSpeedPercentBonus, "Deceleration", isPercent: true);
        AddStat(item.ManeuverabilityPercentBonus, "Maneuverability", isPercent: true);
        AddStat(item.DamagePercentBonus, "Damage", isPercent: true);

        ItemStatsText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
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
