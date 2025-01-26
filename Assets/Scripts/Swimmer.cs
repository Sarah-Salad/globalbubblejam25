using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;

public class Swimmer : MonoBehaviour
{
    public GameObject timerPrefab;
    private GameObject timerInstance;
    private GameObject canvasObject;
    public GameObject abducteePrefab;
    private GameObject abducteeInstance;
    public double desiredTimerValue = 120f;
    
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
            spawnAbducteeAtOpenWaypoint();
            handleAbductedSwimmer();
        }
    }

    private void spawnTimer() {
        timerInstance = Instantiate(timerPrefab);
        timerInstance.transform.SetParent(canvasObject.transform);
        timerInstance.GetComponent<Timer>().sourceSwimmer = this.gameObject;
        timerInstance.GetComponent<Timer>().timerValue = desiredTimerValue;
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.AddAbductee(name);
        timerInstance.GetComponent<Timer>().swimmerName = name;
    }

    private void spawnAbducteeAtOpenWaypoint() {
        // Find open waypoint
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        GameObject[] openWaypoints = waypoints.Where(waypoint => waypoint.GetComponent<Waypoint>().isOpen()).ToArray();
        //Vector2[] openWaypointCoords = openWaypoints.Select(
        //    waypoint => new Vector2(waypoint.transform.position.x, waypoint.transform.position.x)).ToArray();
        GameObject destWaypoint = openWaypoints[UnityEngine.Random.Range(0, openWaypoints.Length - 1)];
        Vector2 destWaypointCoords = new Vector2(destWaypoint.transform.position.x, destWaypoint.transform.position.y);
        // Spawn abudctee at waypoint and spawn in the correct location
        abducteeInstance = Instantiate(abducteePrefab);
        abducteeInstance.transform.SetParent(destWaypoint.transform);
        abducteeInstance.transform.position = destWaypointCoords;
        // tie it to the waypoint object so it will be considered inhabited next go around
        destWaypoint.GetComponent<Waypoint>().inhabitedBy = abducteeInstance;
    }

    private void handleAbductedSwimmer() {
        this.gameObject.SetActive(false);
    }
}