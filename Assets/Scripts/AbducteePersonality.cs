using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbducteePersonality : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string personalityType;
    private List<string> personalityTypes;
    void Start()
    {
        personalityTypes = new List<string> { "Tsundere", "Shy", "Trickster" };
        int rand = Random.Range(0, personalityTypes.Count());
        personalityType = personalityTypes[rand];
    }

    public string GetPersonalityType()
    {
        return personalityType;
    }
    
}
