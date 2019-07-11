using System;
using System.Linq;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += slot => { if (OnPointerEnterEvent != null) OnPointerEnterEvent(slot); };
            equipmentSlots[i].OnPointerExitEvent += slot => { if (OnPointerExitEvent != null) OnPointerExitEvent(slot); };
            equipmentSlots[i].OnRightClickEvent += slot => { if (OnRightClickEvent != null) OnRightClickEvent(slot); };
            equipmentSlots[i].OnBeginDragEvent += slot => { if (OnBeginDragEvent != null) OnBeginDragEvent(slot); };
            equipmentSlots[i].OnEndDragEvent += slot => { if (OnEndDragEvent != null) OnEndDragEvent(slot); };
            equipmentSlots[i].OnDragEvent += slot => { if (OnDragEvent != null) OnDragEvent(slot); };
            equipmentSlots[i].OnDropEvent += slot => { if (OnDropEvent != null) OnDropEvent(slot); };
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                equipmentSlots[i].Amount = 1;
                return true;
            }
        }

        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                equipmentSlots[i].Amount = 0;
                return true;
            }
        }

        return false;
    }

    public EquippableItem ContainsEquipmentType(EquipmentType type)
    {
        var slot = Array.Find(equipmentSlots, x => x.EquipmentType == type);
        return slot.Item as EquippableItem;
    }
}