using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotExposerScript : MonoBehaviour
{
    public Rigidbody AvatarRigidBody;
    public Transform AvatarRootTransform;
    public GameObject AvatarRootGameObject;
    public BotControllerScript BotControllerScript;
    public Collider MainCollider;
    public bool IsAlive = true;
}
