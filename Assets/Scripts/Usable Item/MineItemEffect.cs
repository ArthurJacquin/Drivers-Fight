using Drivers.LocalizationSettings;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Mine")]
public class MineItemEffect : UsableItemEffect
{
    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        GameObject Mine = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Mine.name = "Mine";
        Mine.AddComponent<Mine>();
        PlayerScript playerScript = character.gameObject.GetComponent<PlayerScript>();
        Mine.transform.position = playerScript.targetTransform.position - (playerScript.targetTransform.forward * 3) + (playerScript.targetTransform.up * .01f);
        Mine.transform.localScale = new Vector3(0.1f, 1, 0.1f);
        Mine.transform.rotation = playerScript.targetTransform.rotation;
        Mine.GetComponent<Renderer>().material.color = Color.grey;
        Mine.AddComponent<SphereCollider>();
        Mine.GetComponent<SphereCollider>().isTrigger = true;
        Mine.GetComponent<SphereCollider>().radius = 1;
    }

    public override string GetDescription()
    {
        return LocalizationManager.Instance.GetText("MINE_DESCRIPTION");
    }
}