using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotExposerScript : MonoBehaviour
{
    public Rigidbody BotRigidBody;
    public Transform BotRootTransform;
    public GameObject BotRootGameObject;
    public BotControllerScript BotControllerScript;
    public Collider MainCollider;
    public CharacterController CharacterController;
    public NavMeshAgent NavMeshAgent;
    public bool IsAlive = true;
}
