using UnityEngine;

public class Bubble : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player") {
            Destroy(gameObject);
        }
    }
}
