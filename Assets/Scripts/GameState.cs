using UnityEngine;
using Yarn.Unity;

public class GameState : MonoBehaviour
{
    [SerializeField] private VariableStorageBehaviour _storage;
    [SerializeField] private DialogueRunner _dialogueRunner;
    public int souls;
    public int loss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dialogueRunner.onDialogueComplete.AddListener(EndConversation);
    }

    private void EndConversation()
    {
        _storage.TryGetValue("$Souls", out souls);
        _storage.TryGetValue("$Loss", out loss);
        Debug.Log("Souls: "+ souls);
        Debug.Log("Loss: "+ loss);
    }
}
