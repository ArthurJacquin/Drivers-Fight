using UnityEngine;
using Photon.Pun;

public class Mine : MonoBehaviour
{
    private bool isInRange;

    public PhotonView photonView;

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
            isInRange = state;

            photonView.RPC("playSoundRPC", RpcTarget.AllBuffered, "Mine");

            if (!character.Invincibility)
            {
                character.EngineHealth -= 50;

                if (character.EngineHealth < 0)
                {
                    character.EngineHealth = 0;
                }
            }

            Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void playSoundRPC(string title)
    {
        FindObjectOfType<AudioManager>().Play(title);
    }
}