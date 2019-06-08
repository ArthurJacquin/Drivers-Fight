using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject characterPanelGameObject;
    [SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] GameObject statPanelGameObject;
    [SerializeField] KeyCode[] toggleCharacterPanelKeys;
    [SerializeField] KeyCode[] toggleInventoryKeys;

    void Update()
    {
        for (int i = 0; i < toggleCharacterPanelKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleCharacterPanelKeys[i]))
            {
                characterPanelGameObject.SetActive(!characterPanelGameObject.activeSelf);

                if (characterPanelGameObject.activeSelf)
                {
                    equipmentPanelGameObject.SetActive(true);
                    statPanelGameObject.SetActive(true);
                    ShowMouseCursor();
                }
                else
                {
                    HideMouseCursor();
                }

                break;
            }
        }

        for (int i = 0; i < toggleInventoryKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleInventoryKeys[i]))
            {
                if (!characterPanelGameObject.activeSelf)
                {
                    characterPanelGameObject.SetActive(true);
                    equipmentPanelGameObject.SetActive(false);
                    statPanelGameObject.SetActive(false);
                    ShowMouseCursor();
                }
                else if (equipmentPanelGameObject.activeSelf || statPanelGameObject.activeSelf)
                {
                    equipmentPanelGameObject.SetActive(false);
                    statPanelGameObject.SetActive(false);
                }
                else
                {
                    characterPanelGameObject.SetActive(false);
                    HideMouseCursor();
                }

                break;
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

    public void ToggleEquipmentPanel()
    {
        equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
        statPanelGameObject.SetActive(!statPanelGameObject.activeSelf);
    }
}