using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpTextManager : MonoBehaviour
{
    private static readonly int Dismissed = Animator.StringToHash("Dismissed");
    public GameObject panel;
    public TMP_Text helpText;

    private string GetPathText(string effectivePath)
    {
        return effectivePath.Split("/")[1].ToUpper();
    }
    
    public void ShowHelpText()
    {
        panel.SetActive(true);
        string divePath = GetPathText(InputSystem.actions["Dive"].bindings[0].effectivePath);
        helpText.text = $"Press <b>{divePath}</b> to dive under the water";
    }

    public void ShowHelpTextMall()
    {
        panel.SetActive(true);
        string interactPath = GetPathText(InputSystem.actions["Interact"].bindings[0].effectivePath);
        helpText.text = $"Press <b>{interactPath}</b> to talk to others.";
    }

    public void HideHelpText()
    {
        helpText.gameObject.GetComponent<Animator>().SetBool(Dismissed, true);
    }

    public void OnHelpTextFinished()
    {
        panel.SetActive(false);
    }
}
