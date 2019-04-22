using UnityEngine;

public class ItemChest : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;
    [SerializeField] SpriteRenderer spriteRenderer;
    // [SerializeField] GameObject playerGameObject;
    [SerializeField] Color emptyColor;
    [SerializeField] KeyCode itemPickupKeyCode = KeyCode.E;

    private bool isInRange;
    private bool isEmpty;

    private void OnValidate()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        spriteRenderer.sprite = item.Icon;
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(itemPickupKeyCode))
        {
            if (!isEmpty)
            {
                inventory.AddItem(Instantiate(item));
                isEmpty = true;
                spriteRenderer.color = emptyColor;
            }
            
        }
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
        if (gameObject.gameObject.CompareTag("Player"))
        {
            isInRange = state;
            spriteRenderer.enabled = state;
        }
    }
}
