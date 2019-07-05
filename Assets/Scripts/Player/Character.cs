using UnityEngine;
using UnityEngine.UI;
using Drivers.CharacterStats;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // Player life
    public int EngineHealth = 500;
    // For know if the player have a invincible buff or not. False by default.
    public bool Invincibility = false;

    // Stat declaration
    [Header("Stats")]
    public CharacterStat FrontBumperArmor;
    public CharacterStat RearBumperArmor;
    public CharacterStat LeftFlankArmor;
    public CharacterStat RightFlankArmor;
    public CharacterStat WheelArmor;
    public CharacterStat TiresArmor;

    public CharacterStat MaximumSpeed;
    public CharacterStat AccelerationSpeed;
    public CharacterStat DecelerationSpeed;

    public CharacterStat Maneuverability;

    public float currentSpeed;

    public CharacterStat Damage;

    // Durability of items
    [Header("Durability of items")]
    [SerializeField] EquipmentDurability equipmentDurability;

    [Header("Public")]
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    [Header("Serialize Field")]
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] QuestionDialog questionDialog;

    private BaseItemSlot dragItemSlot;

    public IEnumerable<StatModifier> StatModifiers { get; internal set; }

    private void OnValidate()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Awake()
    {
        statPanel.SetStats(FrontBumperArmor, RearBumperArmor, LeftFlankArmor, RightFlankArmor, WheelArmor, TiresArmor, MaximumSpeed, AccelerationSpeed, DecelerationSpeed, Maneuverability, Damage);
        statPanel.UpdateStatValues();

        currentSpeed = 0f;

        // Setup Events:
        // Right Click
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
        // Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;
        // Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        craftingWindow.OnPointerExitEvent += HideTooltip;
        // Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        // End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        // Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        // Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
        dropItemArea.OnDropEvent += DropItemOutsideUI;
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if (itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(this);

            if (usableItem.IsConsumable)
            {
                inventory.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        if (itemTooltip.gameObject.activeSelf)
        {
            itemTooltip.HideTooltip();
        }
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.gameObject.SetActive(true);
        }
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        draggableItem.transform.position = Input.mousePosition;
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (dragItemSlot == null)
        {
            return;
        }

        if (dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }
        else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddablesStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddablesStacks, dragItemSlot.Amount);

        // Add stacks until drop slot is full
        // Remove the same number of stacks from drag item
        dropItemSlot.Amount += stacksToAdd;
        dragItemSlot.Amount -= stacksToAdd;
    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItem dragItem = dragItemSlot.Item as EquippableItem;
        EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

        if (dropItemSlot is EquipmentSlot)
        {
            if (dropItem != null)
            {
                dropItem.Unequip(this);
                UnequipArmorDurability(dropItem);
            }
            if (dragItem != null)
            {
                dragItem.Equip(this);
                EquipArmorDurability(dragItem);
            }
        }

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragItem != null)
            {
                dragItem.Unequip(this);
                UnequipArmorDurability(dragItem);
            }
            if (dropItem != null)
            {
                dropItem.Equip(this);
                EquipArmorDurability(dropItem);
            }
        }

        statPanel.UpdateStatValues();

        Item draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    private void DropItemOutsideUI()
    {
        if (dragItemSlot == null)
        {
            return;
        }

        questionDialog.Show();
        BaseItemSlot baseItemSlot = dragItemSlot;
        questionDialog.OnYesEvent += () => DestroyItemInSlot(baseItemSlot);
    }

    private void DestroyItemInSlot(BaseItemSlot baseItemSlot)
    {
        baseItemSlot.Item.Destroy();
        baseItemSlot.Item = null;
    }

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
                EquipArmorDurability(item);
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
        if (inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
            UnequipArmorDurability(item);
        }
    }

    private void EquipArmorDurability(EquippableItem item)
    {
        if (item.EquipmentType == EquipmentType.FrontArmor)
        {
            equipmentDurability.CurrentFrontArmorDurability = item.ArmorDurability;
            equipmentDurability.FrontArmorDurability = item.ArmorDurability;
        }
        else if (item.EquipmentType == EquipmentType.RearArmor)
        {
            equipmentDurability.CurrentRearArmorDurability = item.ArmorDurability;
            equipmentDurability.RearArmorDurability = item.ArmorDurability;
        }
        else if (item.EquipmentType == EquipmentType.LeftArmor)
        {
            equipmentDurability.CurrentLeftArmorDurability = item.ArmorDurability;
            equipmentDurability.LeftArmorDurability = item.ArmorDurability;
        }
        else if (item.EquipmentType == EquipmentType.RightArmor)
        {
            equipmentDurability.CurrentRightArmorDurability = item.ArmorDurability;
            equipmentDurability.RightArmorDurability = item.ArmorDurability;
        }
    }

    private void UnequipArmorDurability(EquippableItem item)
    {
        if (item.EquipmentType == EquipmentType.FrontArmor)
        {
            equipmentDurability.CurrentFrontArmorDurability = 0;
            equipmentDurability.FrontArmorDurability = 0;
        }
        else if (item.EquipmentType == EquipmentType.RearArmor)
        {
            equipmentDurability.CurrentRearArmorDurability = 0;
            equipmentDurability.RearArmorDurability = 0;
        }
        else if (item.EquipmentType == EquipmentType.LeftArmor)
        {
            equipmentDurability.CurrentLeftArmorDurability = 0;
            equipmentDurability.LeftArmorDurability = 0;
        }
        else if (item.EquipmentType == EquipmentType.RightArmor)
        {
            equipmentDurability.CurrentRightArmorDurability = 0;
            equipmentDurability.RightArmorDurability = 0;
        }
    }

    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeFrontDamage(10);
            //TakeRearDamage(10);
            //TakeRightDamage(10);
            //TakeLeftDamage(10);
            Debug.Log(EngineHealth);
        }
    }

    public void TakeFrontDamage(int damage)
    {
        if (!Invincibility)
        {
            // Reduce damage taken by armor value
            damage -= (int)FrontBumperArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // if player have a front armor
            if (equipmentDurability.CurrentFrontArmorDurability > 0)
            {
                // Reduce the front armor durability first from damage
                equipmentDurability.CurrentFrontArmorDurability -= damage;

                // If armor is destroyed, remaining damage hit the engine
                if (equipmentDurability.CurrentFrontArmorDurability < 0)
                {
                    EngineHealth -= -equipmentDurability.CurrentFrontArmorDurability;
                    equipmentDurability.CurrentFrontArmorDurability = 0;
                }
            }
            else
            {
                EngineHealth -= damage;
            }
        }
    }

    public void TakeRearDamage(int damage)
    {
        if (!Invincibility)
        {
            damage -= (int)RearBumperArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // if player have a rear armor
            if (equipmentDurability.CurrentRearArmorDurability > 0)
            {
                // Reduce the front armor durability first from damage
                equipmentDurability.CurrentRearArmorDurability -= damage;

                // If armor is destroyed, remaining damage hit the engine
                if (equipmentDurability.CurrentRearArmorDurability < 0)
                {
                    EngineHealth -= -equipmentDurability.CurrentRearArmorDurability;
                    equipmentDurability.CurrentRearArmorDurability = 0;
                }
            }
            else
            {
                EngineHealth -= damage;
            }
        }
    }

    public void TakeRightDamage(int damage)
    {
        if (!Invincibility)
        {
            damage -= (int)RightFlankArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // if player have a rear armor
            if (equipmentDurability.CurrentRightArmorDurability > 0)
            {
                // Reduce the front armor durability first from damage
                equipmentDurability.CurrentRightArmorDurability -= damage;

                // If armor is destroyed, remaining damage hit the engine
                if (equipmentDurability.CurrentRightArmorDurability < 0)
                {
                    EngineHealth -= -equipmentDurability.CurrentRightArmorDurability;
                    equipmentDurability.CurrentRightArmorDurability = 0;
                }
            }
            else
            {
                EngineHealth -= damage;
            }
        }
    }

    public void TakeLeftDamage(int damage)
    {
        if (!Invincibility)
        {
            damage -= (int)LeftFlankArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // if player have a rear armor
            if (equipmentDurability.CurrentLeftArmorDurability > 0)
            {
                // Reduce the front armor durability first from damage
                equipmentDurability.CurrentLeftArmorDurability -= damage;

                // If armor is destroyed, remaining damage hit the engine
                if (equipmentDurability.CurrentLeftArmorDurability < 0)
                {
                    EngineHealth -= -equipmentDurability.CurrentLeftArmorDurability;
                    equipmentDurability.CurrentLeftArmorDurability = 0;
                }
            }
            else
            {
                EngineHealth -= damage;
            }
        }
    }

    public void TakeWheelDamage(int damage)
    {
        if (!Invincibility)
        {
            damage -= (int)WheelArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            EngineHealth -= damage;
        }
    }

    public void TakeTiresDamage(int damage)
    {
        if (!Invincibility)
        {
            damage -= (int)TiresArmor.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            EngineHealth -= damage;
        }
    }
}