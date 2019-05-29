using UnityEngine;
using Drivers.CharacterStats;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // Player life
    public int EngineHealth = 500;

    // Stat declaration
    public CharacterStat FrontBumperArmor;
    public CharacterStat RearBumperArmor;
    public CharacterStat RightFlankArmor;
    public CharacterStat LeftFlankArmor;
    public CharacterStat WheelArmor;
    public CharacterStat TiresArmor;

    public CharacterStat MaximumSpeed;
    public CharacterStat AccelerationSpeed;
    public CharacterStat DecelerationSpeed;

    public CharacterStat Maneuverability;

    public float currentSpeed;

    public CharacterStat Damage;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;

    public IEnumerable<StatModifier> StatModifiers { get; internal set; }

    private void Awake()
    {
        statPanel.SetStats(FrontBumperArmor, RearBumperArmor, RightFlankArmor, LeftFlankArmor, WheelArmor, TiresArmor, MaximumSpeed, AccelerationSpeed, DecelerationSpeed, Maneuverability, Damage);
        statPanel.UpdateStatValues();

        currentSpeed = 0f;

        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
    }

    private void EquipFromInventory(Item item)
    {
        if (item is EquippableItem)
        {
            Equip((EquippableItem)item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquippableItem)
        {
            Unequip((EquippableItem)item);
        }
    }

    /*private void Drop(ItemSlot dropItemSlot)
    {
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Amount = draggedItemAmount;
    }*/

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }

    public void TakeFrontDamage(int damage)
    {
        damage -= (int)FrontBumperArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;

    }

    public void TakeRearDamage(int damage)
    {
        damage -= (int)RearBumperArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;
    }

    public void TakeRightDamage(int damage)
    {
        damage -= (int)RightFlankArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;
    }

    public void TakeLeftDamage(int damage)
    {
        damage -= (int)LeftFlankArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;
    }

    public void TakeWheelDamage(int damage)
    {
        damage -= (int)WheelArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;
    }

    public void TakeTiresDamage(int damage)
    {
        damage -= (int)TiresArmor.Value;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        EngineHealth -= damage;
    }
}
