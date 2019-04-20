using System.Collections.Generic;
using UnityEngine;

namespace DriversFight.Scripts
{
    public class OfflineIntentReceiver : AIntentReceiver
    {
        [SerializeField]
        public int avatarIndex;

        private enum PlayerAction
        {
            Left,
            Down,
            Right, 
            Up
        }

        private static readonly Dictionary<int, Dictionary<PlayerAction, KeyCode>> keys =
            new Dictionary<int, Dictionary<PlayerAction, KeyCode>>
            {
                {
                    0, new Dictionary<PlayerAction, KeyCode>
                    {
                        {PlayerAction.Left, KeyCode.Q},
                        {PlayerAction.Down, KeyCode.S},
                        {PlayerAction.Right, KeyCode.D},
                        {PlayerAction.Up, KeyCode.Z}
                    }
                },
                {
                    1, new Dictionary<PlayerAction, KeyCode>
                    {
                        {PlayerAction.Left, KeyCode.LeftArrow},
                        {PlayerAction.Down, KeyCode.DownArrow},
                        {PlayerAction.Right, KeyCode.RightArrow},
                        {PlayerAction.Up, KeyCode.UpArrow}
                    }
                },
                {
                    2, new Dictionary<PlayerAction, KeyCode>
                    {
                        {PlayerAction.Left, KeyCode.J},
                        {PlayerAction.Down, KeyCode.K},
                        {PlayerAction.Right, KeyCode.L},
                        {PlayerAction.Up, KeyCode.I}
                    }
                },
                {
                    3, new Dictionary<PlayerAction, KeyCode>
                    {
                        {PlayerAction.Left, KeyCode.Keypad4},
                        {PlayerAction.Down, KeyCode.Keypad5},
                        {PlayerAction.Right, KeyCode.Keypad6},
                        {PlayerAction.Up, KeyCode.Keypad8}
                    }
                }
            };


        public void Update()
        {
            if (Input.GetKeyDown(keys[avatarIndex][PlayerAction.Left]))
            {
                WantToMoveLeft = true;
            }

            if (Input.GetKey(keys[avatarIndex][PlayerAction.Down]) && WantToMoveForward == false)
            {
                WantToMoveBackward = true;
                WantToStopTheCar = false;
            }

            if (Input.GetKeyDown(keys[avatarIndex][PlayerAction.Right]))
            {
                WantToMoveRight = true;
            }

            if (Input.GetKey(keys[avatarIndex][PlayerAction.Up]) && WantToMoveBackward == false)
            {
                WantToMoveForward = true;
                WantToStopTheCar = false;
            }

            if (Input.GetKeyUp(keys[avatarIndex][PlayerAction.Left]))
            {
                WantToMoveLeft = false;
            }

            if (Input.GetKeyUp(keys[avatarIndex][PlayerAction.Down]))
            {
                WantToStopTheCar = true;
            }

            if (Input.GetKeyUp(keys[avatarIndex][PlayerAction.Right]))
            {
                WantToMoveRight = false;
            }

            if (Input.GetKeyUp(keys[avatarIndex][PlayerAction.Up]))
            {
                WantToStopTheCar = true;
            }
        }
    }
}