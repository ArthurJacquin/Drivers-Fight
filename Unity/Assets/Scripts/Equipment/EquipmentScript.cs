using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName ="Inventory/Equipment")]
public class EquipmentScript : ItemScript
{
    public EquipmentSlot equipSlot;

    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();
        EquipmentManagerScript.instance.Equip(this);
        RemoveFromInventory();
    }

}

public enum EquipmentSlot { ParechocAvant, ParechocArrière, FlancDroit, FlancGauche, Volant, Pneus, Moteur }