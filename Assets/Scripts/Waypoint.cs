using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public GameObject inhabitedBy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isOpen() {
        if (inhabitedBy == null) return true;
        else return false;
    }
}
