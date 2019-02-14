using UnityEngine;

public class CarStatsScript : MonoBehaviour
{
    public int maxEngineHealth = 100;
    public int currentEngineHealth { get; private set; }

    public Stats frontBumperArmor;
    public Stats rearBumperArmor;
    public Stats rightFlankArmor;
    public Stats leftFlankArmor;
    public Stats wheelArmor;
    public Stats tiresArmor;
    public int currentFrontBumperArmor { get; private set; }
    public int currentRearBumperArmor { get; private set; }
    public int currentRightFlankArmor { get; private set; }
    public int currentLeftFlankArmor { get; private set; }
    public int currentWheelArmor { get; private set; }
    public int currentTiresArmor { get; private set; }

    public Stats maximumSpeed;
    public Stats accelerationSpeed;
    public Stats decelerationSpeed;
    public float defaultMaximumSpeed = 20f;
    public float defaultAccelerationSpeed = 0.5f;
    public float defaultDecelerationSpeed = 0.5f;
    public float currentMaximumSpeed { get; private set; }
    public float currentAccelerationSpeed { get; private set; }
    public float currentDecelerationSpeed { get; private set; }

    public Stats maneuverability;
    public int currentManeuverability { get; private set; }

    public Stats damage;
    public int currentDamage { get; private set; }

    void Awake()
    {
        currentEngineHealth = maxEngineHealth;

        currentFrontBumperArmor = 0;
        currentRearBumperArmor = 0;
        currentRightFlankArmor = 0;
        currentLeftFlankArmor = 0;
        currentWheelArmor = 0;
        currentTiresArmor = 0;

        currentMaximumSpeed = defaultMaximumSpeed;
        currentAccelerationSpeed = defaultAccelerationSpeed;
        currentDecelerationSpeed = defaultDecelerationSpeed;

        currentManeuverability = 0;

        currentDamage = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeFrontDamage(10);
        }
    }

    public void TakeFrontDamage(int damage)
    {
        damage -= (int)frontBumperArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public void TakeRearDamage(int damage)
    {
        damage -= (int)rearBumperArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public void TakeRightDamage(int damage)
    {
        damage -= (int)rightFlankArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public void TakeLeftDamage(int damage)
    {
        damage -= (int)leftFlankArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public void TakeWheelDamage(int damage)
    {
        damage -= (int)wheelArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public void TakeTiresDamage(int damage)
    {
        damage -= (int)tiresArmor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentEngineHealth -= damage;

        if (currentEngineHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }
}