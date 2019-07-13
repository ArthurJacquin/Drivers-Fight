using System;
using UnityEngine;

public class UsablePanel : MonoBehaviour
{
    [SerializeField] Transform usableSlotsParent;
    [SerializeField] public UsableSlot[] usableSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < usableSlots.Length; i++)
        {
            usableSlots[i].OnPointerEnterEvent += slot => { if (OnPointerEnterEvent != null) OnPointerEnterEvent(slot); };
            usableSlots[i].OnPointerExitEvent += slot => { if (OnPointerExitEvent != null) OnPointerExitEvent(slot); };
            usableSlots[i].OnRightClickEvent += slot => { if (OnRightClickEvent != null) OnRightClickEvent(slot); };
            usableSlots[i].OnBeginDragEvent += slot => { if (OnBeginDragEvent != null) OnBeginDragEvent(slot); };
            usableSlots[i].OnEndDragEvent += slot => { if (OnEndDragEvent != null) OnEndDragEvent(slot); };
            usableSlots[i].OnDragEvent += slot => { if (OnDragEvent != null) OnDragEvent(slot); };
            usableSlots[i].OnDropEvent += slot => { if (OnDropEvent != null) OnDropEvent(slot); };
        }
    }

    private void OnValidate()
    {
        usableSlots = usableSlotsParent.GetComponentsInChildren<UsableSlot>();
    }

    public bool AddItem(UsableItem item, out UsableItem previousItem)
    {
        int addIndex = 0;

        for (int i = 0; i < usableSlots.Length; i++)
        {
            if (usableSlots[i].UsableType == item.UsableType && usableSlots[i].Item == null)
            {
                addIndex = i;
                break;
            }
        }

        previousItem = usableSlots[addIndex].Item as UsableItem;
        usableSlots[addIndex].Item = item;
        usableSlots[addIndex].Amount = 1;
        return true;
    }

    public bool RemoveItem(UsableItem item)
    {
        for (int i = 0; i < usableSlots.Length; i++)
        {
            if (usableSlots[i].Item == item)
            {
                usableSlots[i].Item = null;
                usableSlots[i].Amount = 0;
                return true;
            }
        }

        return false;
    }
}
