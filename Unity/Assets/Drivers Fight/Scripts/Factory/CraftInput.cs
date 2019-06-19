using UnityEngine;

public class CraftInput : MonoBehaviour
{
    [SerializeField] GameObject craftPanelGameObject;
    [SerializeField] KeyCode craftOpenKeyCode = KeyCode.U;

    private bool isInRange;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(craftOpenKeyCode))
        {
            ToggleCraft();
        }
        else if (!isInRange && craftPanelGameObject.activeSelf)
        {
            craftPanelGameObject.SetActive(false);
            HideMouseCursor();
        }
    }

    private void ToggleCraft()
    {
        if (!craftPanelGameObject.activeSelf)
        {
            craftPanelGameObject.SetActive(true);
            ShowMouseCursor();
        }
        else
        {
            craftPanelGameObject.SetActive(false);
            HideMouseCursor();
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

    /*public void ToggleEquipmentPanel()
    {
        equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
        statPanelGameObject.SetActive(!statPanelGameObject.activeSelf);
    }*/

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