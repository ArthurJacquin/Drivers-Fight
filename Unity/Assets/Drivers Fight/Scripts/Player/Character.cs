using UnityEngine;
using UnityEngine.UI;
using Drivers.CharacterStats;
using System.Collections.Generic;
using Photon.Pun;

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

    [Header("Public")]
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] UsablePanel usablePanel;

    [Header("Serialize Field")]
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] GarageWindows garageWindows;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] QuestionDialog questionDialog;

    private BaseItemSlot dragItemSlot;

    public IEnumerable<StatModifier> StatModifiers { get; internal set; }

    private PhotonView photonView;

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
        usablePanel.OnRightClickEvent += UsablePanelRightClick;
        // Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        usablePanel.OnPointerEnterEvent += ShowTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;
        // Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        usablePanel.OnPointerExitEvent += HideTooltip;
        craftingWindow.OnPointerExitEvent += HideTooltip;
        // Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        usablePanel.OnBeginDragEvent += BeginDrag;
        // End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        usablePanel.OnEndDragEvent += EndDrag;
        // Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        usablePanel.OnDragEvent += Drag;
        // Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
        usablePanel.OnDropEvent += Drop;
        dropItemArea.OnDropEvent += DropItemOutsideUI;

        photonView = GetComponent<PhotonView>();
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if (itemSlot.Item is UsableItem)
        {
            EquipUsableItem((UsableItem)itemSlot.Item);
        }
    }

    private void UsablePanelRightClick(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item is UsableItem)
        {
            UnequipUsableItem((UsableItem)itemSlot.Item);
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
            }
            if (dragItem != null)
            {
                dragItem.Equip(this);
            }
        }

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

    private void DestroyItemInSlot(BaseItemSlot itemSlot)
    {
        // If the item is equiped, unequip first
        if (itemSlot is EquipmentSlot)
        {
            EquippableItem equippableItem = (EquippableItem)itemSlot.Item;
            equippableItem.Unequip(this);
            statPanel.UpdateStatValues();
        }

        itemSlot.Item.Destroy();
        itemSlot.Item = null;
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
        }
    }

    public void EquipUsableItem(UsableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            UsableItem previousItem;
            if (usablePanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void UnequipUsableItem(UsableItem item)
    {
        if (inventory.CanAddItem(item) && usablePanel.RemoveItem(item))
        {
            inventory.AddItem(item);
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
            TakeDamage(10, EquipmentType.FrontArmor);
            TakeDamage(10, EquipmentType.RearArmor);
            TakeDamage(10, EquipmentType.RightArmor);
            TakeDamage(10, EquipmentType.LeftArmor);
            TakeDamage(10, EquipmentType.Wheel);
            TakeDamage(10, EquipmentType.Tires);
            Debug.Log(EngineHealth);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("UseItemRPC", RpcTarget.AllBuffered, 0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("UseItemRPC", RpcTarget.AllBuffered, 1);
        }
    }

    public void TakeDamage(int damage, EquipmentType equipmentType)
    {
        if (Invincibility) return;

        CharacterStat stat = null;

        switch (equipmentType)
        {
            case EquipmentType.FrontArmor:
                stat = FrontBumperArmor;
                break;
            case EquipmentType.RearArmor:
                stat = RearBumperArmor;
                break;
            case EquipmentType.RightArmor:
                stat = RightFlankArmor;
                break;
            case EquipmentType.LeftArmor:
                stat = LeftFlankArmor;
                break;
            case EquipmentType.Wheel:
                stat = WheelArmor;
                break;
            case EquipmentType.Tires:
                stat = TiresArmor;
                break;
            default:
                Debug.Log($"No equipped armor.Taking full damage");
                break;
        }

        // Reduce damage taken by armor value
        if (stat != null)
        {
            damage -= (int)stat.Value;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
        }

        // Get armor
        var armor = equipmentPanel.ContainsEquipmentType(equipmentType);

        Debug.Log($"{equipmentType} : {armor}");

        // Armor equipped ?
        if (armor != null)
        {
            // Reduce the armor durability first from damage
            Debug.Log($"{equipmentType} Durability before hit: " + armor.CurrentArmorDurability);
            armor.CurrentArmorDurability -= damage;
            Debug.Log($"{equipmentType} Durability after hit: " + armor.CurrentArmorDurability);

            // Armor destroyed ?
            if (armor.CurrentArmorDurability <= 0)
            {
                EngineHealth -= Mathf.Abs(armor.CurrentArmorDurability);
                armor.CurrentArmorDurability = 0;
            }
        }
        else
        {
            EngineHealth -= damage;
        }
    }

    //Reset stats and inventory
    public void ResetStats()
    {
        EngineHealth = 500;
        currentSpeed = 0f;

        FrontBumperArmor.RemoveAllModifiersFromSource(FrontBumperArmor);
        RearBumperArmor.RemoveAllModifiersFromSource(RearBumperArmor);
        RightFlankArmor.RemoveAllModifiersFromSource(RightFlankArmor);
        LeftFlankArmor.RemoveAllModifiersFromSource(LeftFlankArmor);
        Maneuverability.RemoveAllModifiersFromSource(Maneuverability);
        MaximumSpeed.RemoveAllModifiersFromSource(MaximumSpeed);
        WheelArmor.RemoveAllModifiersFromSource(WheelArmor);
        TiresArmor.RemoveAllModifiersFromSource(TiresArmor);
        AccelerationSpeed.RemoveAllModifiersFromSource(AccelerationSpeed);
        DecelerationSpeed.RemoveAllModifiersFromSource(DecelerationSpeed);

        inventory.Clear();
    }

    [PunRPC]
    private void UseItemRPC(int i)
    {
        UsableItem usableItem = usablePanel.usableSlots[i].Item as UsableItem;
        if (usableItem != null)
        {
            usableItem.Use(this);

            if (usableItem.IsConsumable)
            {
                usablePanel.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }
}