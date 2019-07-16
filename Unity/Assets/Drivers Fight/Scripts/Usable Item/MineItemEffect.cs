using Drivers.LocalizationSettings;
using UnityEngine;
using Photon.Pun;

namespace DriversFight.Scripts
{
    [CreateAssetMenu(menuName = "Item Effects/Mine")]
    public class MineItemEffect : UsableItemEffect
    {
        public override void ExecuteEffect(UsableItem parentItem, Character character)
        {
            GameObject Mine = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Mine.name = "Mine";
            Mine.AddComponent<Mine>();
            AvatarExposerScript playerScript = character.gameObject.GetComponent<AvatarExposerScript>();
            Mine.transform.position = playerScript.AvatarRootTransform.position - (playerScript.AvatarRootTransform.forward * 3) + (playerScript.AvatarRootTransform.up * .01f);
            Mine.transform.localScale = new Vector3(0.1f, 1, 0.1f);
            Mine.transform.rotation = playerScript.AvatarRootTransform.rotation;
            Mine.GetComponent<Renderer>().material.color = Color.grey;
            Mine.AddComponent<SphereCollider>();
            Mine.AddComponent<PhotonView>();
            Mine.GetComponent<SphereCollider>().isTrigger = true;
            Mine.GetComponent<SphereCollider>().radius = 1;
            Mine.GetComponent<Mine>().photonView = Mine.GetComponent<PhotonView>();
        }

        public override string GetDescription()
        {
            return LocalizationManager.Instance.GetText("MINE_DESCRIPTION");
        }
    }
}