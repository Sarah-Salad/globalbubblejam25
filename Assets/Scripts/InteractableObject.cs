using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class InteractableObject: MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _stopInteract;
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private DialogueRunner dialogueRunner;
    private AbducteePersonality abducteePersonality;
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
        abducteePersonality = GetComponent<AbducteePersonality>();
    }
    private void StartConversation()
    {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        _onInteract.Invoke();
        dialogueRunner.StartDialogue(abducteePersonality.GetPersonalityType());
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
