using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BotControllerScript : MonoBehaviour
{
    private float speed;
    private float directionChangeInterval = 1;
    private float maxHeadingChange = 30;

    //States
    public bool wander;
    public bool attackPlayer { get; set; }
    public bool collectObject { get; set; }

    CharacterController controller;
    float heading;
    Vector3 targetRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        speed = 0;
        wander = true;
        attackPlayer = false;
        collectObject = false;

        // Set random initial rotation
        heading = 0;
        transform.eulerAngles = new Vector3(0, heading, 0);
    }

    void Update()
    {
        if (attackPlayer)
        {
            Debug.Log("Attack !!!!");
        }
        else if (collectObject)
        {
            Debug.Log("Collect...");
        }
        else if (wander)
        {
            StartCoroutine(NewHeading());
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(Vector3.forward);
            controller.SimpleMove(forward * speed);
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
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 180);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 180);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }
}