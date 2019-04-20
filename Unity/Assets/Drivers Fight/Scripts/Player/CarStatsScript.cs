using UnityEngine;
using Photon.Pun;

namespace DriversFight.Scripts
{
    public class CarStatsScript : MonoBehaviour
    {
        public int maxEngineHealth = 100;
        public int currentEngineHealth { get; set; }

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
        public float currentSpeed;

        public float defaultMaximumSpeed = 20f;
        public float defaultAccelerationSpeed = 0.1f;
        public float defaultDecelerationSpeed = 0.2f;
        public float defaultManeuvrability = 50f;

        public Stats currentMaximumSpeed;
        public Stats currentAccelerationSpeed;
        public Stats currentDecelerationSpeed;
        public Stats currentManeuverability;


        private void Awake()
        {
            currentEngineHealth = maxEngineHealth;

            currentFrontBumperArmor = 0;
            currentRearBumperArmor = 0;
            currentRightFlankArmor = 0;
            currentLeftFlankArmor = 0;
            currentWheelArmor = 0;
            currentTiresArmor = 0;
            currentSpeed = 0;

            currentMaximumSpeed.AddModifier(defaultMaximumSpeed);
            currentAccelerationSpeed.AddModifier(defaultAccelerationSpeed);
            currentDecelerationSpeed.AddModifier(defaultDecelerationSpeed);
            currentManeuverability.AddModifier(defaultManeuvrability);
        }

        public void TakeFrontDamage(int damage)
        {
            damage -= (int)frontBumperArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;

        }

        public void TakeRearDamage(int damage)
        {
            damage -= (int)rearBumperArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;
        }

        public void TakeRightDamage(int damage)
        {
            damage -= (int)rightFlankArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;
        }

        public void TakeLeftDamage(int damage)
        {
            damage -= (int)leftFlankArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;
        }

        public void TakeWheelDamage(int damage)
        {
            damage -= (int)wheelArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;
        }

        public void TakeTiresDamage(int damage)
        {
            damage -= (int)tiresArmor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentEngineHealth -= damage;
        }
    }
}