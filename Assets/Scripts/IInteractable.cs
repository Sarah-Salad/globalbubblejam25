using UnityEngine.Events;

public interface IInteractable
{
    public UnityEvent OnInteract { get; protected set; }
    public void Interact();
}
