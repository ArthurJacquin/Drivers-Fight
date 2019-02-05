using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : CarStatsScript
{
    void Start()
    {
        EquipmentManagerScript.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged (EquipmentScript newItem, EquipmentScript oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
        }
    }
}