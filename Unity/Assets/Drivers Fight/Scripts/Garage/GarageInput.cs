using UnityEngine;

public class GarageInput : MonoBehaviour
{
    [SerializeField] GameObject garagePanelGameObject;
    [SerializeField] GameObject characterPanelGameObject;
    [SerializeField] KeyCode garageOpenKeyCode = KeyCode.U;

    private bool isInRange;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(garageOpenKeyCode))
        {
            ToggleGarage();
        }
        else if (!isInRange && garagePanelGameObject.activeSelf)
        {
            garagePanelGameObject.SetActive(false);
            if (!characterPanelGameObject.activeSelf)
            {
                HideMouseCursor();
            }
        }
    }

    private void ToggleGarage()
    {
        if (!garagePanelGameObject.activeSelf)
        {
            garagePanelGameObject.SetActive(true);
            ShowMouseCursor();
        }
        else
        {
            garagePanelGameObject.SetActive(false);
            if (!characterPanelGameObject.activeSelf)
            {
                HideMouseCursor();
            }
        }
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
            isInRange = state;
        }
    }
}