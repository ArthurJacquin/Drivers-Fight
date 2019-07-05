using Drivers.Localization;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemDurabilityText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;

    // Durability of items
    [Header("Durability of items")]
    [SerializeField] EquipmentDurability equipmentDurability;

    private string CheckItemDescriptionLanguage(string itemDescription)
    {
        if (Locale.CurrentLanguage == "French")
        {
            itemDescription = itemDescription.Replace("Front armor", "Armure frontale");
            itemDescription = itemDescription.Replace("Rear armor", "Armure arrière");
            itemDescription = itemDescription.Replace("Left armor", "Armure gauche");
            itemDescription = itemDescription.Replace("Right armor", "Armure droite");
            itemDescription = itemDescription.Replace("Tires armor", "Armure pneus");
            itemDescription = itemDescription.Replace("Wheel armor", "Armure volant");
            itemDescription = itemDescription.Replace("Max. speed", "Vitesse max.");
            itemDescription = itemDescription.Replace("Acceleration", "Accélération");
            itemDescription = itemDescription.Replace("Deceleration", "Décélération");
            itemDescription = itemDescription.Replace("Maneuverability", "Maniabilité");
            itemDescription = itemDescription.Replace("Damage", "Dommage");
        }

        return itemDescription;
    }

    private string CheckItemNameLanguage(string itemName)
    {
        if (Locale.CurrentLanguage == "French")
        {
            itemName = itemName.Replace("Engine repair kit", "Kit de réparation moteur");
            itemName = itemName.Replace("Invincibility", "Invincibilité");
            itemName = itemName.Replace("Scrap coin", "Pièce de ferraille");
            itemName = itemName.Replace("Engine", "Moteur");
            itemName = itemName.Replace("Front bumper", "Pare-choc avant");
            itemName = itemName.Replace("Rear bumper", "Pare-choc arrière");
            itemName = itemName.Replace("Left protection", "Portière gauche");
            itemName = itemName.Replace("Right protection", "Portière droite");
            itemName = itemName.Replace("Steering wheel", "Volant");
            itemName = itemName.Replace("Tires", "Pneus");
        }

        return itemName;
    }

    private string CheckItemTypeLanguage(string itemType)
    {
        if (Locale.CurrentLanguage == "French")
        {
            itemType = itemType.Replace("Engine", "Moteur");
            itemType = itemType.Replace("FrontArmor", "Pare-choc avant");
            itemType = itemType.Replace("RearArmor", "Pare-choc arrière");
            itemType = itemType.Replace("LeftArmor", "Portière gauche");
            itemType = itemType.Replace("RightArmor", "Portière droite");
            itemType = itemType.Replace("Wheel", "Volant");
            itemType = itemType.Replace("Tires", "Pneus");
            itemType = itemType.Replace("Consumable", "Consommable");
            itemType = itemType.Replace("Usable", "Utilisable");
        }

        if (Locale.CurrentLanguage == "English")
        {
            itemType = itemType.Replace("Engine", "Engine");
            itemType = itemType.Replace("FrontArmor", "Front Bumper");
            itemType = itemType.Replace("RearArmorr", "Rear Bumper");
            itemType = itemType.Replace("LeftArmor", "Left protection");
            itemType = itemType.Replace("RightArmor", "Right protection");
            //itemType = itemType.Replace("Wheel", "Wheel");
            //itemType = itemType.Replace("Tires", "Tires");
        }

        return itemType;
    }

    private string CheckItemDurabilityLanguage(string ItemDurabilityText)
    {
        if (Locale.CurrentLanguage == "French")
        {
            ItemDurabilityText = ItemDurabilityText.Replace("Durability", "Durabilité");
        }

        return ItemDurabilityText;
    }

    public void ShowTooltip(Item item)
    {
        string itemName = item.ItemName;
        itemName = CheckItemNameLanguage(itemName);
        ItemNameText.text = itemName;

        string itemType = item.GetItemType();
        itemType = CheckItemTypeLanguage(itemType);
        ItemTypeText.text = itemType;

        if (item.GetItemType() == "FrontArmor")
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
        }

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