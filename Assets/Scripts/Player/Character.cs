using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] QuestionDialog questionDialog;

    private BaseItemSlot dragItemSlot;

    public IEnumerable<StatModifier> StatModifiers { get; internal set; }

    private void Start()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }

        statPanel.SetStats(FrontBumperArmor, RearBumperArmor, RightFlankArmor, LeftFlankArmor, WheelArmor, TiresArmor, MaximumSpeed, AccelerationSpeed, DecelerationSpeed, Maneuverability, Damage);
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
                itemTooltip.HideTooltip();
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
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }
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

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquippableItem dragItem = dragItemSlot.Item as EquippableItem;
        EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragItem != null)
            {
                dragItem.Unequip(this);
            }
            if (dropItem != null)
            {
                dropItem.Equip(this);
            }
        }

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragItem != null)
            {
                dragItem.Equip(this);
            }
            if (dropItem != null)
            {
                dropItem.Unequip(this);
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

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddablesStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddablesStacks, dragItemSlot.Amount);

        // Add stacks until drop slot is full
        // Remove the same number of stacks from drag item
        dropItemSlot.Amount += stacksToAdd;
        dragItemSlot.Amount -= stacksToAdd;
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
                statPanel.UpdateStatValues();
                itemTooltip.HideTooltip();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
            itemTooltip.HideTooltip();
        }
    }

    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
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