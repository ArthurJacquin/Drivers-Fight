using Drivers.LocalizationSettings;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemDurabilityText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;

    private string CheckItemNameLanguage(string itemName)
    {
        itemName = itemName.Replace("Engine repair kit", LocalizationManager.Instance.GetText("ENGINE_REPAIR_KIT"));
        itemName = itemName.Replace("Invincibility", LocalizationManager.Instance.GetText("INVINCIBILITY"));
        itemName = itemName.Replace("Scrap coin", LocalizationManager.Instance.GetText("SCRAP_COIN"));
        itemName = itemName.Replace("Engine", LocalizationManager.Instance.GetText("ENGINE"));
        itemName = itemName.Replace("Front bumper", LocalizationManager.Instance.GetText("FRONT_BUMPER"));
        itemName = itemName.Replace("Rear bumper", LocalizationManager.Instance.GetText("REAR_BUMPER"));
        itemName = itemName.Replace("Left protection", LocalizationManager.Instance.GetText("LEFT_PROTECTION"));
        itemName = itemName.Replace("Right protection", LocalizationManager.Instance.GetText("RIGHT_PROTECTION"));
        itemName = itemName.Replace("Steering wheel", LocalizationManager.Instance.GetText("STEERING_WHEEL"));
        itemName = itemName.Replace("Tires", LocalizationManager.Instance.GetText("TIRES"));

        return itemName;
    }

    private string CheckItemTypeLanguage(string itemType)
    {
        itemType = itemType.Replace("Engine", LocalizationManager.Instance.GetText("ENGINE"));
        itemType = itemType.Replace("FrontArmor", LocalizationManager.Instance.GetText("FRONT_BUMPER"));
        itemType = itemType.Replace("RearArmor", LocalizationManager.Instance.GetText("REAR_BUMPER"));
        itemType = itemType.Replace("LeftArmor", LocalizationManager.Instance.GetText("LEFT_PROTECTION"));
        itemType = itemType.Replace("RightArmor", LocalizationManager.Instance.GetText("RIGHT_PROTECTION"));
        itemType = itemType.Replace("Wheel", LocalizationManager.Instance.GetText("STEERING_WHEEL"));
        itemType = itemType.Replace("Tires", LocalizationManager.Instance.GetText("TIRES"));
        itemType = itemType.Replace("Consumable", LocalizationManager.Instance.GetText("CONSUMABLE"));
        itemType = itemType.Replace("Usable", LocalizationManager.Instance.GetText("USABLE"));

        return itemType;
    }

    /*private string CheckItemDurabilityLanguage(string ItemDurabilityText)
    {
        ItemDurabilityText = ItemDurabilityText.Replace("Durability", LocalizationManager.Instance.GetText("DURABILITY"));

        return ItemDurabilityText;
    }*/

    private string CheckItemDescriptionLanguage(string itemDescription)
    {
        itemDescription = itemDescription.Replace("Front armor", LocalizationManager.Instance.GetText("FRONT_ARMOR"));
        itemDescription = itemDescription.Replace("Rear armor", LocalizationManager.Instance.GetText("REAR_ARMOR"));
        itemDescription = itemDescription.Replace("Left armor", LocalizationManager.Instance.GetText("LEFT_ARMOR"));
        itemDescription = itemDescription.Replace("Right armor", LocalizationManager.Instance.GetText("RIGHT_ARMOR"));
        itemDescription = itemDescription.Replace("Tires armor", LocalizationManager.Instance.GetText("TIRES_ARMOR"));
        itemDescription = itemDescription.Replace("Wheel armor", LocalizationManager.Instance.GetText("WHEEL_ARMOR"));
        itemDescription = itemDescription.Replace("Max. speed", LocalizationManager.Instance.GetText("MAX_SPEED"));
        itemDescription = itemDescription.Replace("Acceleration", LocalizationManager.Instance.GetText("ACCELERATION"));
        itemDescription = itemDescription.Replace("Deceleration", LocalizationManager.Instance.GetText("DECELERATION"));
        itemDescription = itemDescription.Replace("Maneuverability", LocalizationManager.Instance.GetText("MANEUVERABILITY"));
        itemDescription = itemDescription.Replace("Damage", LocalizationManager.Instance.GetText("DAMAGE"));

        return itemDescription;
    }

    public void ShowTooltip(Item item)
    {
        string itemName = item.ItemName;
        itemName = CheckItemNameLanguage(itemName);
        ItemNameText.text = itemName;

        string itemType = item.GetItemType();
        itemType = CheckItemTypeLanguage(itemType);
        ItemTypeText.text = itemType;

        /*if (item.GetItemType() == "FrontArmor")
        {
            ItemDurabilityText.text = "Durability " + equipmentDurability.CurrentFrontArmorDurability + " / " + equipmentDurability.FrontArmorDurability;
            ItemDurabilityText.text = CheckItemDurabilityLanguage(ItemDurabilityText.text);
        }
        else if (item.GetItemType() == "RearArmor")
        {
            ItemDurabilityText.text = "Durability " + equipmentDurability.CurrentRearArmorDurability + " / " + equipmentDurability.RearArmorDurability;
            ItemDurabilityText.text = CheckItemDurabilityLanguage(ItemDurabilityText.text);
        }
        else if(item.GetItemType() == "LeftArmor")
        {
            ItemDurabilityText.text = "Durability " + equipmentDurability.CurrentLeftArmorDurability + " / " + equipmentDurability.LeftArmorDurability;
            ItemDurabilityText.text = CheckItemDurabilityLanguage(ItemDurabilityText.text);
        }
        else if(item.GetItemType() == "RightArmor")
        {
            ItemDurabilityText.text = "Durability " + equipmentDurability.CurrentRightArmorDurability + " / " + equipmentDurability.RightArmorDurability;
            ItemDurabilityText.text = CheckItemDurabilityLanguage(ItemDurabilityText.text);
        }*/

        string itemDescription = item.GetDescription();
        itemDescription = CheckItemDescriptionLanguage(itemDescription);
        ItemDescriptionText.text = itemDescription;

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}