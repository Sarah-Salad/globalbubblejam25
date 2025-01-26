using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class InteractableObject: MonoBehaviour, IInteractable
{
    private UnityEvent _stopInteract;
    private UnityEvent _onInteract;
    private DialogueRunner dialogueRunner;
    private PlayerMovement playerMovement;
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
        _onInteract = new UnityEvent();
        _stopInteract = new UnityEvent();
        dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        abducteePersonality = GetComponent<AbducteePersonality>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        _onInteract.AddListener(ConversationStart);
        _stopInteract.AddListener(ConversationEnd);
    }

    private void ConversationEnd()
    {
        playerMovement.enabled = true;
    }

    private void ConversationStart()
    {
        playerMovement.enabled = false;
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
            Debug.Log($"Stopped conversation with {name}.");

        }
    }

    public void Interact() {
        if (interactable && !dialogueRunner.IsDialogueRunning)
        {
            StartConversation();
        }
    } 
}
