using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

public class Timer : MonoBehaviour
{
    public GameObject sourceSwimmer;
    public double timerValue;
    RectTransform rTransform;
    float timerWidth;
    float timerHeight;
    float xOffset;
    float yOffset;
    float xPos;
    float yPos;
    float lowestYPos;
    private bool isTimerFinished = false;
    public string swimmerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up initial value of timer into the TMP Pro text field
        setTimerText();

        // Set up of vars assuming this timer is at the top right of the screen with the correct offset given the size of the TMP GameObject
        rTransform = this.GetComponent<RectTransform>();
        timerWidth = rTransform.rect.size.x;
        timerHeight = rTransform.rect.size.y;
        //Debug.Log($"Timer width: {timerWidth}");
        //Debug.Log($"Timer height: {timerHeight}");

        xOffset = timerWidth/2.0f;
        yOffset = timerHeight/2.0f;
        //Debug.Log($"X Offset: {xOffset}");
        //Debug.Log($"Y Offset: {yOffset}");

        xPos = -xOffset;
        yPos = -yOffset;
        //Debug.Log($"X Pos: {xPos}");
        //Debug.Log($"Y Pos: {yPos}");

        // We need to actually move the timer to this position because the first timer needs a place to go
        Vector2 source = new Vector2(xPos, yPos);
        rTransform.anchoredPosition = source;

        // Find GameObject with Timer tag with lowest y Pos and adjust yOffset accordingly so that the new timer appears below the existing one(s)
        GameObject[] timerList = GameObject.FindGameObjectsWithTag("Timer");
        if (timerList.Length > 1) {
            lowestYPos = timerList.Aggregate(
            0.0f, (min, timerObject) => Math.Min(min, timerObject.GetComponent<RectTransform>().anchoredPosition.y));
            //Debug.Log($"Lowest Timer Y Pos: {lowestYPos}");

            // Set the position of the timer accordingly
            Vector2 dest = new Vector2(xPos, lowestYPos - timerHeight);
            rTransform.anchoredPosition = dest;
        }

        // Set scale to (1, 1, 1) manually because it's getting altered somewhere
        this.rTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if timer finished
        if (!isTimerFinished) {
            decrementTimer();
            setTimerText();
            // Check if timer value is at or below 0, set text to 0, perform finish timer actions, and set timer finished flag true
            if (timerValue <= 0.0f) {
                zeroTimer();
                handleFinishedTimer();
                isTimerFinished = true;
            }
        }
    }
    void setTimerText() {
        // Convert current timer number to text string
        string timerText = timerValue.ToString("F2");
        // Update TMP component with new text
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = timerText;
    }

    void zeroTimer() {
        // Update TMP component with new text
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "0.00";
    }

    void decrementTimer() {
        timerValue -= Time.deltaTime;
    }

    void handleFinishedTimer() {
        Debug.Log("Timer hit 0");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.RemoveAbducteeViaTimer(this.gameObject);
    }
}
