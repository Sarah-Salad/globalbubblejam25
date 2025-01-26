using UnityEngine;
using Yarn.Unity;

public class GameState : MonoBehaviour
{
    [SerializeField] private VariableStorageBehaviour _storage;
    [SerializeField] private DialogueRunner _dialogueRunner;
    public int souls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dialogueRunner.onDialogueComplete.AddListener(EndConversation);
    }

    private void EndConversation()
    {
        _storage.TryGetValue("$Souls", out souls);
        Debug.Log(souls);
    }
}
