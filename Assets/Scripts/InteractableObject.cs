using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class InteractableObject: MonoBehaviour, IInteractable
{
    [SerializeField] private string conversationStartNode;
    [SerializeField] private UnityEvent _stopInteract;
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField]private DialogueRunner dialogueRunner;
    private bool interactable = true;
    private bool isCurrentConversation = false;

    UnityEvent IInteractable.OnInteract
    {
        get => _onInteract;
        set => _onInteract = value;
    }

    private void Start()
    {
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
    }
    private void StartConversation()
    {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        // if (lightIndicatorObject != null) {
        //     lightIndicatorObject.intensity = defaultIndicatorIntensity;
        // }
        _onInteract.Invoke();
    }

    private void EndConversation()
    {
        if (isCurrentConversation)
        {
            isCurrentConversation = false;
            _stopInteract.Invoke();
            Debug.Log($"Started conversation with {name}.");
        }
    }

    public void Interact() {
        if (interactable && !dialogueRunner.IsDialogueRunning)
        {
            StartConversation();
        }
    } 
}
