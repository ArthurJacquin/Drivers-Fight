using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Drivers.CharacterStats;
using Drivers.LocalizationSettings;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] Text StatNameText;
    [SerializeField] Text StatModifiersLabelText;
    [SerializeField] Text StatModifiersText;

    private readonly StringBuilder sb = new StringBuilder();

    public void ShowTooltip(CharacterStat stat, string statName)
    {
        StatNameText.text = GetStatTopText(stat, statName);

        StatModifiersText.text = GetStatModifiersText(stat);

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private string GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        statName = CheckStatNameLanguage(statName);
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        StatModifiersLabelText.enabled = false;

        if (stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);

            if (stat.Value > stat.BaseValue)
            {
                sb.Append("+");
            }
            
            sb.Append("<color=lime>" + System.Math.Round(stat.Value - stat.BaseValue, 4) + "</color>");
            sb.Append(")");
            StatModifiersLabelText.enabled = true;
        }

        return sb.ToString();
    }

    private string GetStatModifiersText(CharacterStat stat)
    {
        sb.Length = 0;

        foreach (StatModifier mod in stat.StatModifiers)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (mod.Value > 0)
            {
                sb.Append("+");
            }

            if (mod.Type == StatModType.Flat)
            {
                sb.Append(mod.Value);
            }
            else
            {
                sb.Append(mod.Value * 100);
                sb.Append("%");
            }

            Item item = mod.Source as Item;

            if (item != null)
            {
                item.ItemName = CheckItemNameLanguage(item.ItemName);
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.LogError("Modifiers is not an Item");
            }
        }

        return sb.ToString();
    }

    private string CheckItemNameLanguage(string itemName)
    {
        itemName = itemName.Replace("Engine repair kit", LocalizationManager.Instance.GetText("ENGINE_REPAIR_KIT"));
        itemName = itemName.Replace("Engine", LocalizationManager.Instance.GetText("ENGINE"));
        itemName = itemName.Replace("Front bumper", LocalizationManager.Instance.GetText("FRONT_BUMPER"));
        itemName = itemName.Replace("Rear bumper", LocalizationManager.Instance.GetText("REAR_BUMPER"));
        itemName = itemName.Replace("Left protection", LocalizationManager.Instance.GetText("LEFT_PROTECTION"));
        itemName = itemName.Replace("Right protection", LocalizationManager.Instance.GetText("RIGHT_PROTECTION"));
        itemName = itemName.Replace("Steering wheel", LocalizationManager.Instance.GetText("STEERING_WHEEL"));
        itemName = itemName.Replace("Tires", LocalizationManager.Instance.GetText("TIRES"));

        return itemName;
    }

    private string CheckStatNameLanguage(string statName)
    {
        if (statName == "Front armor")
        {
            statName = LocalizationManager.Instance.GetText("FRONT_ARMOR");
        }
        else if (statName == "Rear armor")
        {
            statName = LocalizationManager.Instance.GetText("REAR_ARMOR");
        }
        else if (statName == "Left armor")
        {
            statName = LocalizationManager.Instance.GetText("LEFT_ARMOR");
        }
        else if (statName == "Right armor")
        {
            statName = LocalizationManager.Instance.GetText("RIGHT_ARMOR");
        }
        else if (statName == "Tires armor")
        {
            statName = LocalizationManager.Instance.GetText("TIRES_ARMOR");
        }
        else if (statName == "Wheel armor")
        {
            statName = LocalizationManager.Instance.GetText("WHEEL_ARMOR");
        }
        else if (statName == "Max. speed")
        {
            statName = LocalizationManager.Instance.GetText("MAX_SPEED");
        }
        else if (statName == "Acceleration")
        {
            statName = LocalizationManager.Instance.GetText("ACCELERATION");
        }
        else if (statName == "Deceleration")
        {
            statName = LocalizationManager.Instance.GetText("DECELERATION");
        }
        else if (statName == "Maneuverability")
        {
            statName = LocalizationManager.Instance.GetText("MANEUVERABILITY");
        }
        else if (statName == "Damage")
        {
            statName = LocalizationManager.Instance.GetText("DAMAGE");
        }

        return statName;
    }
}