using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;

public class Timer : MonoBehaviour
{
    RectTransform rTransform;
    float timerWidth;
    float timerHeight;
    float xOffset;
    float yOffset;
    float xPos;
    float yPos;
    float lowestYPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
