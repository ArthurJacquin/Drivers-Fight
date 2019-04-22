using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private CarStatsScript carStats;

    private bool wantToMoveForward;
    private bool wantToMoveBackward;

    private bool wantToMoveLeft;
    private bool wantToMoveRight;

    private bool wantToStopTheCar;

    private int carEngineHealth;

    private int carFrontBumperArmor;
    private int carRearBumperArmor;
    private int carRightFlankArmor;
    private int carLeftFlankArmor;
    private int carWheelArmor;
    private int carTiresArmor;

    private float carSpeed = 0f;
    private float carMaximumSpeed;
    private float carAccelerationSpeed;
    private float carDecelerationSpeed;

    private int carManeuverability;

    private int carDamage;

    public void updateCarEngineHealth()
    {
        carEngineHealth = carStats.currentEngineHealth;
    }

    public void updateCarFrontBumperArmor()
    {
        carFrontBumperArmor = carStats.currentFrontBumperArmor;
    }

    public void updateCarRearBumperArmor()
    {
        carRearBumperArmor = carStats.currentRearBumperArmor;
    }

    public void updateCarRightFlankArmor()
    {
        carRightFlankArmor = carStats.currentRightFlankArmor;
    }

    public void updateCarLeftFlankArmor()
    {
        carLeftFlankArmor = carStats.currentLeftFlankArmor;
    }

    public void updateCarWheelArmor()
    {
        carWheelArmor = carStats.currentWheelArmor;
    }

    public void updateCarTiresArmor()
    {
        carTiresArmor = carStats.currentTiresArmor;
    }

    public void updateCarMaximumSpeed()
    {
        carMaximumSpeed = carStats.currentMaximumSpeed;
    }

    public void updateCarAccelerationSpeed()
    {
        carAccelerationSpeed = carStats.currentAccelerationSpeed;
    }

    public void updateCarDecelerationSpeed()
    {
        carDecelerationSpeed = carStats.currentDecelerationSpeed;
    }

    public void updateCarManeuverability()
    {
        carManeuverability = carStats.currentManeuverability;
    }

    public void updateCarDamage()
    {
        carDamage = (int)carSpeed / 2;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Interactable interactable = collisionInfo.gameObject.GetComponent<Interactable>();
            if (interactable == null)
            {
                Debug.Log("null");
                return;
            }
            float distance = interactable.radius;
            if (Vector3.Distance(targetTransform.position, transform.position) <= distance)
            {
                Debug.Log("Interaction");
            }
            Debug.Log(Vector3.Distance(targetTransform.position, transform.position) <= distance);
        }
    }

    void Start()
    {
        // Récupérer les stats du joueur
        carEngineHealth = carStats.currentEngineHealth;

        carFrontBumperArmor = carStats.currentFrontBumperArmor;
        carRearBumperArmor = carStats.currentRearBumperArmor;
        carRightFlankArmor = carStats.currentRightFlankArmor;
        carLeftFlankArmor = carStats.currentLeftFlankArmor;
        carWheelArmor = carStats.currentWheelArmor;
        carTiresArmor = carStats.currentTiresArmor;

        carMaximumSpeed = carStats.currentMaximumSpeed;
        carAccelerationSpeed = carStats.currentAccelerationSpeed;
        carDecelerationSpeed = carStats.currentDecelerationSpeed;

        carManeuverability = carStats.currentManeuverability;

        carDamage = carStats.currentDamage;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Life = " + carEngineHealth);

        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/

        if (Input.GetKey(KeyCode.Z) && wantToMoveBackward == false)
        {
            wantToMoveForward = true;
            wantToStopTheCar = false;

            if (carSpeed < carMaximumSpeed && wantToStopTheCar == false)
            {
                carSpeed += carAccelerationSpeed;
                updateCarDamage();
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            wantToStopTheCar = true;
        }

        if (Input.GetKey(KeyCode.S) && wantToMoveForward == false)
        {
            wantToMoveBackward = true;
            wantToStopTheCar = false;

            if (carSpeed < carMaximumSpeed && wantToStopTheCar == false)
            {
                carSpeed += carAccelerationSpeed;
                updateCarDamage();
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            wantToStopTheCar = true;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            wantToMoveLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            wantToMoveLeft = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            wantToMoveRight = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            wantToMoveRight = false;
        }

        if (wantToStopTheCar)
        {
            if (carSpeed > 0f)
            {
                carSpeed -= carDecelerationSpeed;

                if (carSpeed < 0f)
                {
                    carSpeed = 0f;
                }

                updateCarDamage();
            }

            if (carSpeed <= 0f)
            {
                carSpeed = 0f;

                wantToMoveForward = false;
                wantToMoveBackward = false;

                wantToStopTheCar = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (wantToMoveForward)
        {
            if (wantToMoveLeft)
            {
                targetTransform.position += transform.forward * carSpeed * Time.deltaTime;
                targetTransform.Rotate(0.0f, -40.0f * Time.deltaTime, 0.0f);
            }
            else if (wantToMoveRight)
            {
                targetTransform.position += transform.forward * carSpeed * Time.deltaTime;
                targetTransform.Rotate(0.0f, 40.0f * Time.deltaTime, 0.0f);
            }
            else
            {
                targetTransform.position += transform.forward * carSpeed * Time.deltaTime;
            }
        }
        else if (wantToMoveBackward)
        {
            if (wantToMoveLeft)
            {
                targetTransform.position += -transform.forward * carSpeed * Time.deltaTime;
                targetTransform.Rotate(0.0f, 40.0f * Time.deltaTime, 0.0f);
            }
            else if (wantToMoveRight)
            {
                targetTransform.position += -transform.forward * carSpeed * Time.deltaTime;
                targetTransform.Rotate(0.0f, -40.0f * Time.deltaTime, 0.0f);
            }
            else
            {
                targetTransform.position += -transform.forward * carSpeed * Time.deltaTime;
            }
        }
    }
}