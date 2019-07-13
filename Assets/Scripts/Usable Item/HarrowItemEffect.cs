using Drivers.LocalizationSettings;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Harrow")]
public class HarrowItemEffect : UsableItemEffect
{
    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        GameObject Harrow = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Harrow.name = "Harrow";
        Harrow.AddComponent<Harrow>();
        PlayerScript playerScript = character.gameObject.GetComponent<PlayerScript>();
        Harrow.transform.position = playerScript.targetTransform.position - (playerScript.targetTransform.forward * 3) + (playerScript.targetTransform.up * .01f);
        Harrow.transform.localScale = new Vector3(0.2f, 1, 0.1f);
        Harrow.transform.rotation = playerScript.targetTransform.rotation;
        Harrow.GetComponent<Renderer>().material.color = new Color(0.3f, 0.4f, 0.6f);
        Harrow.AddComponent<SphereCollider>();
        Harrow.GetComponent<SphereCollider>().isTrigger = true;
        Harrow.GetComponent<SphereCollider>().radius = 1;
    }

    public override string GetDescription()
    {
        return LocalizationManager.Instance.GetText("HARROW_DESCRIPTION");
    }
}