using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlowBubble : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float bubbleSpeed;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void BlowBubble()
    {
        GameObject bubble = Instantiate(bubblePrefab, transform.position, transform.rotation);
        Rigidbody2D rigidbody = bubble.GetComponent<Rigidbody2D>();
        audioSource.Play();
        
        rigidbody.linearVelocity = bubbleSpeed * transform.up;
    }

    private void OnAttack(InputValue inputValue)
    {
        BlowBubble();
    }
}
