using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

namespace DriversFight.Scripts
{

    public class PauseMenu : MonoBehaviour {

        public static bool gameIsPaused = false;
        public static bool inGame = false;

        [SerializeField]
        private GameObject pauseMenuUI;

        [SerializeField]
        private GameObject dropItemArea;

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && inGame)
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            dropItemArea.SetActive(true);
            HideMouseCursor();
        }

        void Pause()
        {
            pauseMenuUI.SetActive(true);
            dropItemArea.SetActive(false);
            ShowMouseCursor();
        }

        public void ShowMouseCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void HideMouseCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}