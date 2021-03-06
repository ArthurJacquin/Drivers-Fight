﻿using UnityEngine;
using Drivers.CharacterStats;
using System.Collections;
using Photon.Pun;

namespace DriversFight.Scripts
{
    public class Harrow : MonoBehaviour
    {
        public PhotonView photonView;
        private bool isInRange;

        public float MaximumSpeed;
        public float AccelerationSpeed;
        public float DecelerationSpeed;
        public float Maneuverability;
        public float Duration = 5;

        private void OnTriggerEnter(Collider other)
        {
            CheckCollision(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckCollision(other.gameObject, false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollision(collision.gameObject, true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CheckCollision(collision.gameObject, false);
        }

        private void CheckCollision(GameObject gameObject, bool state)
        {
            if (gameObject.CompareTag("Car"))
            {
                var character = gameObject.GetComponent<Character>();
                var player = gameObject.GetComponent<AvatarExposerScript>();
                isInRange = state;

                photonView.RPC("playSoundRPC", RpcTarget.AllBuffered, "Mine");

                if (!character.Invincibility)
                {
                    MaximumSpeed = -player.Stats.MaximumSpeed.Value / 2;
                    AccelerationSpeed = -player.Stats.AccelerationSpeed.Value / 2;
                    DecelerationSpeed = -player.Stats.DecelerationSpeed.Value / 2;
                    Maneuverability = -player.Stats.Maneuverability.Value / 2;

                    StatModifier statModifierMaximumSpeed = new StatModifier(MaximumSpeed, StatModType.Flat);
                    StatModifier statModifierAccelerationSpeed = new StatModifier(AccelerationSpeed, StatModType.Flat);
                    StatModifier statModifierDecelerationSpeed = new StatModifier(DecelerationSpeed, StatModType.Flat);
                    StatModifier statModifierManeuverability = new StatModifier(Maneuverability, StatModType.Flat);

                    character.MaximumSpeed.AddModifier(statModifierMaximumSpeed);
                    character.AccelerationSpeed.AddModifier(statModifierAccelerationSpeed);
                    character.DecelerationSpeed.AddModifier(statModifierDecelerationSpeed);
                    character.Maneuverability.AddModifier(statModifierManeuverability);

                    character.UpdateStatValues();
                    character.StartCoroutine(RemoveBuff(character, statModifierMaximumSpeed, statModifierAccelerationSpeed, statModifierDecelerationSpeed, statModifierManeuverability, Duration));
                }

                Destroy(this.gameObject);
            }
        }

        private static IEnumerator RemoveBuff(Character character, StatModifier statModifierMaximumSpeed, StatModifier statModifierAccelerationSpeed, StatModifier statModifierDecelerationSpeed, StatModifier statModifierManeuverability, float duration)
        {
            yield return new WaitForSeconds(duration);
            character.MaximumSpeed.RemoveModifier(statModifierMaximumSpeed);
            character.AccelerationSpeed.RemoveModifier(statModifierAccelerationSpeed);
            character.DecelerationSpeed.RemoveModifier(statModifierDecelerationSpeed);
            character.Maneuverability.RemoveModifier(statModifierManeuverability);
            character.UpdateStatValues();
        }

        [PunRPC]
        private void playSoundRPC(string title)
        {
            FindObjectOfType<AudioManager>().Play(title);
        }
    }
}