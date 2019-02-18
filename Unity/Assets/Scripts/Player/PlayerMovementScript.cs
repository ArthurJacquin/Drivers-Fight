using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

namespace DriversFight.Scripts
{
    public class PlayerMovementScript : MonoBehaviour
    {
        [SerializeField]
        private AvatarExposerScript avatar;

        [SerializeField]
        private PhotonView photonView;

        private Transform targetTransform;
        private CarStatsScript stats;

        private bool wantToMoveForward;
        private bool wantToMoveBackward;

        private bool wantToMoveLeft;
        private bool wantToMoveRight;

        private bool wantToStopTheCar;

        private void Start()
        {
            targetTransform = avatar.AvatarRootTransform;
            stats = avatar.Stats;
        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetKey(KeyCode.Z) && wantToMoveBackward == false)
            {
                wantToMoveForward = true;
                wantToStopTheCar = false;

                if (stats.currentSpeed < stats.currentMaximumSpeed.GetValue() && wantToStopTheCar == false)
                {
                    stats.currentSpeed += stats.currentAccelerationSpeed.GetValue();
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

                if (stats.currentSpeed < stats.currentMaximumSpeed.GetValue() && wantToStopTheCar == false)
                {
                    stats.currentSpeed += stats.currentAccelerationSpeed.GetValue();
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
                if (stats.currentSpeed > 0f)
                {
                    stats.currentSpeed -= stats.currentDecelerationSpeed.GetValue();

                    if (stats.currentSpeed < 0f)
                    {
                        stats.currentSpeed = 0f;
                    }
                }

                if (stats.currentSpeed <= 0f)
                {
                    stats.currentSpeed = 0f;

                    wantToMoveForward = false;
                    wantToMoveBackward = false;

                    wantToStopTheCar = false;
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (wantToMoveForward)
            {
                if (wantToMoveLeft)
                {
                    targetTransform.position += transform.forward * stats.currentSpeed * Time.deltaTime;
                    targetTransform.Rotate(0.0f, -stats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                }
                else if (wantToMoveRight)
                {
                    targetTransform.position += transform.forward * stats.currentSpeed * Time.deltaTime;
                    targetTransform.Rotate(0.0f, stats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                }
                else
                {
                    targetTransform.position += transform.forward * stats.currentSpeed * Time.deltaTime;
                }
            }
            else if (wantToMoveBackward)
            {
                if (wantToMoveLeft)
                {
                    targetTransform.position += -transform.forward * stats.currentSpeed * Time.deltaTime;
                    targetTransform.Rotate(0.0f, stats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                }
                else if (wantToMoveRight)
                {
                    targetTransform.position += -transform.forward * stats.currentSpeed * Time.deltaTime;
                    targetTransform.Rotate(0.0f, -stats.currentManeuverability.GetValue() * Time.deltaTime, 0.0f);
                }
                else
                {
                    targetTransform.position += -transform.forward * stats.currentSpeed * Time.deltaTime;
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.IsMine)
                return;

            if(other.gameObject.tag == "Car")
            {
                if (stats.currentSpeed > 0)
                {
                    DealDamage( Mathf.RoundToInt(stats.currentSpeed * 2), other.gameObject.GetComponent<PhotonView>().ViewID);
                }
            }
            else
            {
                Debug.Log("Collision avec un truc");
            }
        }


        [PunRPC]
        private void DealDamage(int damage, int viewID)
        {
            PhotonView.Find(viewID).gameObject.SendMessage("TakeFrontDamage", damage);
        }
    }
}