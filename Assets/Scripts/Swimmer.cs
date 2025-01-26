using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public class Swimmer : MonoBehaviour
{
    public GameObject timerPrefab;
    private GameObject timerInstance;
    private GameObject canvasObject;
    private Rigidbody2D rigidBody2D;
    public double desiredTimerValue = 120f;

    public float accelerationTime = 2f;
    public float maxSpeed = 1f;
    private Vector2 movement;
    private float timeLeft;
    private float rotationSpeed = 10f;
    
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
        // https://discussions.unity.com/t/unity-2d-random-movement/189637/2
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            movement = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
  }
    }

    void FixedUpdate() {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, movement);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rigidBody2D.MoveRotation(rotation);
        rigidBody2D.AddForce(movement * maxSpeed);
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