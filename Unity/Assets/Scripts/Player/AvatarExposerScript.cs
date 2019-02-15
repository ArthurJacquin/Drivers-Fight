﻿using Photon.Pun;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class AvatarExposerScript : MonoBehaviour
    {
        public Rigidbody AvatarRigidBody;
        public PhotonRigidbodyView AvatarRigidBodyView;
        public Transform AvatarRootTransform;
        public GameObject AvatarRootGameObject;


        public enum SideHit
        {
            Front,
            Back,
            Right,
            Left
        }
    }
}