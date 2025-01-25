using UnityEngine;
using UnityEngine.Events;

public class InteractableObject: MonoBehaviour, IInteractable
{
    private bool isON;
    [SerializeField] private UnityEvent _stopInteract;
    [SerializeField] private UnityEvent _onInteract;

    UnityEvent IInteractable.OnInteract
    {
        get => _onInteract;
        set => _onInteract = value;
    }
    public void Interact() {
        if (isON)
        {
            _stopInteract.Invoke();
        }
        else
        {
            _onInteract.Invoke();
        }
        isON = !isON;
    } 
}
