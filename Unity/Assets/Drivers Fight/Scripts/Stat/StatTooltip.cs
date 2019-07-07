using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Drivers.CharacterStats;
using Drivers.Localization;

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

    private string CheckStatNameLanguage(string statName)
    {
        if (Locale.CurrentLanguage == "French")
        {
            if (statName == "Front armor")
            {
                statName = "Armure frontale";
            }
            else if (statName == "Rear armor")
            {
                statName = "Armure arrière";
            }
            else if (statName == "Left armor")
            {
                statName = "Armure gauche";
            }
            else if (statName == "Right armor")
            {
                statName = "Armure droite";
            }
            else if (statName == "Tires armor")
            {
                statName = "Armure pneus";
            }
            else if (statName == "Wheel armor")
            {
                statName = "Armure volant";
            }
            else if (statName == "Max. speed")
            {
                statName = "Vitesse max.";
            }
            else if (statName == "Acceleration")
            {
                statName = "Accélération";
            }
            else if (statName == "Deceleration")
            {
                statName = "Décélération";
            }
            else if (statName == "Maneuverability")
            {
                statName = "Maniabilité";
            }
            else if (statName == "Damage")
            {
                statName = "Dommage";
            }
        }

        return statName;
    }

    private string CheckItemNameLanguage(string itemName)
    {
        if (Locale.CurrentLanguage == "French")
        {
            itemName = itemName.Replace("Engine", "Moteur");
            itemName = itemName.Replace("Front bumper", "Pare-choc avant");
            itemName = itemName.Replace("Rear bumper", "Pare-choc arrière");
            itemName = itemName.Replace("Left protection", "Portière gauche");
            itemName = itemName.Replace("Right protection", "Portière droite");
            itemName = itemName.Replace("Steering wheel", "Volant");
            itemName = itemName.Replace("Tires", "Pneus");
        }

        return itemName;
    }

    private string GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        statName = CheckStatNameLanguage(statName);
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if (stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);

            if (stat.Value > stat.BaseValue)
            {
                sb.Append("+");
            }

            sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");
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
}