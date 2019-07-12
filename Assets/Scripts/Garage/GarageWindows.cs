using UnityEngine;

public class GarageWindows : MonoBehaviour
{
    [Header("Public")]
    [SerializeField] EquipmentPanel equipmentPanel;
    public ItemContainer ItemContainer;


    public void RepairArmor(int index)
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            EquipmentType type = (EquipmentType)index;
            EquippableItem armor = equipmentPanel.ContainsEquipmentType(type);

            // Armor equipped ?
            if (armor != null)
            {
                Debug.Log("Durability before repair: " + armor.CurrentArmorDurability);

                // Armor need repair ?
                if (armor.CurrentArmorDurability < armor.ArmorDurability)
                {
                    armor.CurrentArmorDurability = armor.ArmorDurability;

                    Item oldItem = ItemContainer.RemoveItem(item.ID);
                    oldItem.Destroy();

                    Debug.Log("Durability after repair: " + armor.CurrentArmorDurability);
                }
                else
                {
                    Debug.Log("Armor don't need repair");
                }
            }
            else
            {
                Debug.Log($"No {type} is currently equipped. Cannot repair armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }
}
