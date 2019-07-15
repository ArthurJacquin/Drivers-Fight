using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
public class BotControllerScript : MonoBehaviour
{
    private float speed;
    private float directionChangeInterval = 1;
    private float maxHeadingChange = 40;

    //States
    public bool wander;
    public bool attackPlayer { get; set; }
    public bool dodgeWall { get; set; }
    public GameObject targetObject { get; set; }

    [SerializeField]
    private BotExposerScript bot;

    float heading;
    Vector3 targetRotation;

    void Awake()
    {
        //Initial states
        speed = 0;
        wander = true;
        attackPlayer = false;

        // Set initial rotation
        heading = 0;
        transform.eulerAngles = new Vector3(0, bot.transform.rotation.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        if (attackPlayer)
        {
            bot.NavMeshAgent.speed = speed;
            bot.NavMeshAgent.SetDestination(targetObject.transform.position);
        }
        else if (wander && dodgeWall)
        {
            StartCoroutine(NewHeading());
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, new Vector3(0, -transform.rotation.eulerAngles.y, 0), Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(Vector3.forward);
            bot.CharacterController.SimpleMove(forward * speed);
        }
        else if (wander)
        {
            bot.NavMeshAgent.ResetPath();
            StartCoroutine(NewHeading());
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(Vector3.forward);
            bot.CharacterController.SimpleMove(forward * speed);
        }
    }

    // Repeatedly calculates a new direction to move towards.
    IEnumerator NewHeading()
    {
        NewHeadingRoutine();
        if(speed < 10)
            speed += 0.2f;
        yield return new WaitForSeconds(directionChangeInterval);
    }

    // Calculates a new direction to move towards.
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 300);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 300);
        heading = UnityEngine.Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }
}