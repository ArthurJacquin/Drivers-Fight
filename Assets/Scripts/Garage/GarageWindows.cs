using UnityEngine;

public class GarageWindows : MonoBehaviour
{
    [Header("Public")]
    [SerializeField] EquipmentPanel equipmentPanel;
    public ItemContainer ItemContainer;

    public void RepairFrontArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.FrontArmor);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }

    public void RepairRearArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.RearArmor);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }

    public void RepairRightArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.RightArmor);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }

    public void RepairLeftArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.LeftArmor);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }

    public void RepairWheelArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.Wheel);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }

    public void RepairTiresArmor()
    {
        var item = ItemContainer.ItemSearch("Item repair kit");

        if (item != null)
        {
            // Get armor
            var armor = equipmentPanel.ContainsEquipmentType(EquipmentType.Tires);

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
                Debug.Log("You don't have armor");
            }
        }
        else
        {
            Debug.Log("You don't have a repair kit");
        }
    }
}
