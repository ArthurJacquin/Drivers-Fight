using UnityEngine;

public class Inventory : ItemContainer
{
    [SerializeField] Item[] startingItems;
    [SerializeField] Transform itemsParent;

    protected override void Start()
    {
        base.Start();
        SetStartingItems();
    }

    protected override void OnValidate()
    {
        if (itemsParent != null)
        {
            itemsParent.GetComponentsInChildren<ItemSlot>(includeInactive: true, result: itemSlots);
        }

        SetStartingItems();
    }
    
    private void SetStartingItems()
    {
        Clear();

        for (int i = 0; i < startingItems.Length; i++)
        {
            AddItem(startingItems[i].GetCopy());
        }
    }
}