using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
{
    [SerializeField] protected List<ItemSlot> itemSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    protected virtual void OnValidate()
    {
        GetComponentsInChildren<ItemSlot>(includeInactive: true, result: itemSlots);
    }

    protected virtual void Start()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].OnPointerEnterEvent += slot => { if (OnPointerEnterEvent != null) OnPointerEnterEvent(slot); };
            itemSlots[i].OnPointerExitEvent += slot => { if (OnPointerExitEvent != null) OnPointerExitEvent(slot); };
            itemSlots[i].OnRightClickEvent += slot => { if (OnRightClickEvent != null) OnRightClickEvent(slot); };
            itemSlots[i].OnBeginDragEvent += slot => { if (OnBeginDragEvent != null) OnBeginDragEvent(slot); };
            itemSlots[i].OnEndDragEvent += slot => { if (OnEndDragEvent != null) OnEndDragEvent(slot); };
            itemSlots[i].OnDragEvent += slot => { if (OnDragEvent != null) OnDragEvent(slot); };
            itemSlots[i].OnDropEvent += slot => { if (OnDropEvent != null) OnDropEvent(slot); };
        }
    }

    public virtual bool CanAddItem(Item item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                freeSpaces += item.MaximumStacks - itemSlot.Amount;
            }
        }

        return freeSpaces >= amount;
    }

    public virtual bool AddItem(Item item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].CanAddStack(item))
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        return false;
    }

    public virtual bool RemoveItem(Item item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Amount--;
                return true;
            }
        }

        return false;
    }

    public virtual Item RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Item item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                return item;
            }
        }

        return null;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Item item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                number += itemSlots[i].Amount;
            }
        }

        return number;
    }

    public virtual void Clear()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].Item = null;
        }
    }
}