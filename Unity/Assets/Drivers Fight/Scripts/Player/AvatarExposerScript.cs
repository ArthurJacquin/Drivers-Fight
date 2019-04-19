using Photon.Pun;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class AvatarExposerScript : MonoBehaviour
    {
        public Rigidbody AvatarRigidBody;
        public Transform AvatarRootTransform;
        public GameObject AvatarRootGameObject;
        public CarStatsScript Stats;
        public PhotonView photonView;
    }
}
