using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private LayerMask abducteeLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Interact"].performed += DoInteract;
    }

    private void OnDisable()
    {
        _playerInput.actions["Interact"].performed -= DoInteract;
    }
    private void DoInteract(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Interact");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 2f, abducteeLayer);

        if (!hit) return;

        if (!hit.transform.TryGetComponent(out InteractableObject interactable)) return;


        interactable.Interact();
    }
}
