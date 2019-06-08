using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private Character carStats;

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

    private float carSpeed;
    private float carMaximumSpeed;
    private float carAccelerationSpeed;
    private float carDecelerationSpeed;

    private int carManeuverability;

    private int carDamage;

    public void updateCarEngineHealth()
    {
        carEngineHealth = (int)carStats.EngineHealth;
    }

    public void updateCarFrontBumperArmor()
    {
        carFrontBumperArmor = (int)carStats.FrontBumperArmor.Value;
    }

    public void updateCarRearBumperArmor()
    {
        carRearBumperArmor = (int)carStats.RearBumperArmor.Value;
    }

    public void updateCarRightFlankArmor()
    {
        carRightFlankArmor = (int)carStats.RightFlankArmor.Value;
    }

    public void updateCarLeftFlankArmor()
    {
        carLeftFlankArmor = (int)carStats.LeftFlankArmor.Value;
    }

    public void updateCarWheelArmor()
    {
        carWheelArmor = (int)carStats.WheelArmor.Value;
    }

    public void updateCarTiresArmor()
    {
        carTiresArmor = (int)carStats.TiresArmor.Value;
    }

    public void updateCarSpeed()
    {
        carSpeed = carStats.currentSpeed;
    }

    public void updateCarMaximumSpeed()
    {
        carMaximumSpeed = carStats.MaximumSpeed.Value;
    }

    public void updateCarAccelerationSpeed()
    {
        carAccelerationSpeed = carStats.AccelerationSpeed.Value;
    }

    public void updateCarDecelerationSpeed()
    {
        carDecelerationSpeed = carStats.DecelerationSpeed.Value;
    }

    public void updateCarManeuverability()
    {
        carManeuverability = (int)carStats.Maneuverability.Value;
    }

    public void updateCarDamage()
    {
        carDamage = (int)carSpeed / 2;
    }

    void Start()
    {
        // Récupérer les stats du joueur
        carEngineHealth = carStats.EngineHealth;

        carFrontBumperArmor = (int)carStats.FrontBumperArmor.Value;
        carRearBumperArmor = (int)carStats.RearBumperArmor.Value;
        carRightFlankArmor = (int)carStats.RightFlankArmor.Value;
        carLeftFlankArmor = (int)carStats.LeftFlankArmor.Value;
        carWheelArmor = (int)carStats.WheelArmor.Value;
        carTiresArmor = (int)carStats.TiresArmor.Value;

        carSpeed = carStats.currentSpeed;
        carMaximumSpeed = carStats.MaximumSpeed.Value;
        carAccelerationSpeed = carStats.AccelerationSpeed.Value;
        carDecelerationSpeed = carStats.DecelerationSpeed.Value;

        carManeuverability = (int)carStats.Maneuverability.Value;

        carDamage = (int)carStats.Damage.Value;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Speed = " + carSpeed);

        // Update Stats
        updateCarDamage();
        updateCarMaximumSpeed();
        updateCarAccelerationSpeed();
        updateCarDecelerationSpeed();
        updateCarEngineHealth();
        updateCarFrontBumperArmor();
        updateCarLeftFlankArmor();
        updateCarRearBumperArmor();
        updateCarRightFlankArmor();
        updateCarManeuverability();
        updateCarTiresArmor();
        updateCarWheelArmor();

        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        */

        if (Input.GetKey(KeyCode.Z) && wantToMoveBackward == false)
        {
            wantToMoveForward = true;
            wantToStopTheCar = false;

            if (carSpeed < carMaximumSpeed && wantToStopTheCar == false)
            {
                carSpeed += carAccelerationSpeed;
                if (carSpeed > carMaximumSpeed)
                {
                    carSpeed = carMaximumSpeed;
                }
                updateCarDamage();
            }
            else if (carSpeed > carMaximumSpeed && wantToStopTheCar == false)
            {
                carSpeed = carMaximumSpeed;
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
                if (carSpeed > carMaximumSpeed)
                {
                    carSpeed = carMaximumSpeed;
                }
                updateCarDamage();
            }
            else if (carSpeed > carMaximumSpeed && wantToStopTheCar == false)
            {
                carSpeed = carMaximumSpeed;
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

        // Player stop press touch for moving
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
        // Player movement
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