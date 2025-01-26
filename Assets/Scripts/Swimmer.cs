using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;

public class Swimmer : MonoBehaviour
{
    public GameObject timerPrefab;
    private GameObject timerInstance;
    private GameObject canvasObject;
    private Rigidbody2D rigidBody2D;
    public double desiredTimerValue = 120f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        // Probably a more efficient way to do this other than grabbing the canvas for every swimmer but should be fine for this purpose
        canvasObject = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        Vector2 randomDirection = new Vector2(
            UnityEngine.Random.Range(0f,0.2f), UnityEngine.Random.Range(0f,0.2f));
        rigidBody2D.AddForce(randomDirection);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bubble") {
            // Swimmer collided with bubble, create timer and register in Game Manager
            spawnTimer();
            registerToGameManager();
            handleAbductedSwimmer();
        }
    }

    private void spawnTimer() {
        timerInstance = Instantiate(timerPrefab);
        timerInstance.transform.SetParent(canvasObject.transform);
        timerInstance.GetComponent<Timer>().sourceSwimmer = this.gameObject;
        timerInstance.GetComponent<Timer>().timerValue = desiredTimerValue;
    }

    private void registerToGameManager() {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.AddAbductee(this.gameObject, timerInstance.gameObject);
    }

    private void handleAbductedSwimmer() {
        this.gameObject.SetActive(false);
    }
}