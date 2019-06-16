using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCameraScript : MonoBehaviour
{
    public Transform target;

    private Vector3 offset = new Vector3(0, 2, 6.5f);
    private Vector3 rotationVector;

    private float rotationDamping = 3f;
    private float heightDamping = 2f;


    private void Start()
    {
        //if (target == null)
            //target = GameObject.FindGameObjectWithTag("Car").transform;
    }

    private void LateUpdate()
    {

        if (target != null)
        {
            var wantedAngle = target.eulerAngles.y;
            var wantedHeight = target.position.y + offset.y;
            var myAngle = transform.eulerAngles.y;
            var myHeight = transform.position.y;

            myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
            myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);

            var currentRotation = Quaternion.Euler(0, myAngle, 0);

            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * offset.z;
            transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);
            transform.LookAt(target);

        }
    }
}
