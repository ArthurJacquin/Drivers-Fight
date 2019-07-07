using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotExposerScript : MonoBehaviour
{
    public Rigidbody AvatarRigidBody;
    public Transform AvatarRootTransform;
    public GameObject AvatarRootGameObject;
    public BotControllerScript BotControllerScript;
    public Collider MainCollider;
    public CharacterController CharacterController;
    public NavMeshAgent NavMeshAgent;
    public Character Stats;
    public bool IsAlive = true;
}
