public class UsableSlot : ItemSlot
{
    public UsableType UsableType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = UsableType.ToString() + " Slot";
    }

    public override bool CanReceiveItem(Item item)
    {
        if (item == null)
        {
            return true;
        }

        UsableItem usableItem = item as UsableItem;
        return usableItem != null && usableItem.UsableType == UsableType;
    }
}
