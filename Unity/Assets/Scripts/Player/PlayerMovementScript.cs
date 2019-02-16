using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace DriversFight.Scripts
{
    public class PlayerMovementScript : MonoBehaviour
    {
        [SerializeField]
        private AvatarExposerScript avatar;

        [SerializeField]
        private PhotonView photonView;

        private bool wantToMoveForward;
        private bool wantToMoveBackward;

        private bool wantToMoveLeft;
        private bool wantToMoveRight;

        private bool wantToStopTheCar;

        //public CarStatsScript carStats;

        private float carSpeed = 0f;
        private float carMaximumSpeed = 50f;
        private float carAccelerationSpeed = 0.5f;
        private float carDecelerationSpeed = 0.5f;
        private int carLife = 100;

        private float timeToWait;

        private void Awake()
        {
            timeToWait = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (carLife <= 0)
            {
                NetworkControllerScript.instance.endBadDriverGame();
            }

            if (PlayerNumbering.SortedPlayers.Length < 2)
            {
                if (Time.time - timeToWait > 20 && Time.time - timeToWait < 25)
                {
                    NetworkControllerScript.instance.endBadDriverGame();
                }

                if (Time.time - timeToWait > 25)
                {
                    NetworkControllerScript.instance.endGoodDriverGame();
                }
            }

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

                if (carSpeed < carMaximumSpeed && wantToStopTheCar == false)
                {
                    carSpeed += carAccelerationSpeed;
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
            if (!photonView.IsMine)
            {
                return;
            }

            if (wantToMoveForward)
            {
                if (wantToMoveLeft)
                {
                    avatar.AvatarRootTransform.position += transform.forward * carSpeed * Time.deltaTime;
                    avatar.AvatarRootTransform.Rotate(0.0f, -40.0f * Time.deltaTime, 0.0f);
                }
                else if (wantToMoveRight)
                {
                    avatar.AvatarRootTransform.position += transform.forward * carSpeed * Time.deltaTime;
                    avatar.AvatarRootTransform.Rotate(0.0f, 40.0f * Time.deltaTime, 0.0f);
                }
                else
                {
                    avatar.AvatarRootTransform.position += transform.forward * carSpeed * Time.deltaTime;
                }
            }
            else if (wantToMoveBackward)
            {
                if (wantToMoveLeft)
                {
                    avatar.AvatarRootTransform.position += -transform.forward * carSpeed * Time.deltaTime;
                    avatar.AvatarRootTransform.Rotate(0.0f, 40.0f * Time.deltaTime, 0.0f);
                }
                else if (wantToMoveRight)
                {
                    avatar.AvatarRootTransform.position += -transform.forward * carSpeed * Time.deltaTime;
                    avatar.AvatarRootTransform.Rotate(0.0f, -40.0f * Time.deltaTime, 0.0f);
                }
                else
                {
                    avatar.AvatarRootTransform.position += -transform.forward * carSpeed * Time.deltaTime;
                }
            }
        }

        public void carEnterInSector()
        {
            carLife = 0;
        }
    }
}