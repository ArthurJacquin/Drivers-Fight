using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManagerScript : MonoBehaviour
{
    public static EquipmentManagerScript instance;

    void Awake()
    {
        instance = this;
    }

    EquipmentScript[] currentEquipment;

    public delegate void OnEquipmentChanged(EquipmentScript newItem, EquipmentScript oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    InventoryScript inventory;

    void Start()
    {
        inventory = InventoryScript.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new EquipmentScript[numSlots];
    }

    public void Equip(EquipmentScript newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        EquipmentScript oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            EquipmentScript oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}
