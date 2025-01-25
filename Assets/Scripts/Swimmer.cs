using UnityEngine;

public class Swimmer : MonoBehaviour
{
    public GameObject timerPrefab;
    private GameObject timerInstance;
    private GameObject canvasObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Probably a more efficient way to do this other than grabbing the canvas for every swimmer but should be fine for this purpose
        canvasObject = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hopefully collided with bubble");
        if (collider.gameObject.tag == "Bubble") {
            Debug.Log("Okay good it was the bubble");
            spawnTimer();
        }
    }

    private void spawnTimer() {
        timerInstance = Instantiate(timerPrefab);
        timerInstance.transform.SetParent(canvasObject.transform);
        timerInstance.GetComponent<Timer>().sourceSwimmer = this.gameObject;
    }
}
