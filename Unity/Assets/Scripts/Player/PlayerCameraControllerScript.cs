using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script abandonné

public class PlayerCameraControllerScript : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;

    public float yawSpeed = 100f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;
    private float YawMax = 45f;
    private float YawMin = -45f;

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        if (Input.GetMouseButton(2))
        {
            currentYaw -= Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;

            if (currentYaw > YawMax)
            {
                currentYaw = YawMax;
            }

            if (currentYaw < YawMin)
            {
                currentYaw = YawMin;
            }
        }
    }

    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}