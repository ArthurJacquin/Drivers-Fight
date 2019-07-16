using Drivers.LocalizationSettings;
using UnityEngine;
using Photon.Pun;

namespace DriversFight.Scripts
{
    [CreateAssetMenu(menuName = "Item Effects/Harrow")]
    public class HarrowItemEffect : UsableItemEffect
    {
        [PunRPC]
        public override void ExecuteEffect(UsableItem parentItem, Character character)
        {
            GameObject Harrow = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Harrow.name = "Harrow";
            Harrow.AddComponent<Harrow>();
            AvatarExposerScript playerScript = character.gameObject.GetComponent<AvatarExposerScript>();
            Harrow.transform.position = playerScript.AvatarRootTransform.position - (playerScript.AvatarRootTransform.forward * 3) + (playerScript.AvatarRootTransform.up * .01f);
            Harrow.transform.localScale = new Vector3(0.2f, 1, 0.1f);
            Harrow.transform.rotation = playerScript.AvatarRootTransform.rotation;
            Harrow.GetComponent<Renderer>().material.color = new Color(0.3f, 0.4f, 0.6f);
            Harrow.AddComponent<SphereCollider>();
            Harrow.GetComponent<SphereCollider>().isTrigger = true;
            Harrow.GetComponent<SphereCollider>().radius = 1;
            Harrow.AddComponent<PhotonView>();
            Harrow.GetComponent<Harrow>().photonView = Harrow.GetComponent<PhotonView>();
        }

        public override string GetDescription()
        {
            return LocalizationManager.Instance.GetText("HARROW_DESCRIPTION");
        }
    }
}