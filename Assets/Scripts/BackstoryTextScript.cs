using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BackstoryTextScript : MonoBehaviour
{

    public string[] textlist;
    public GameObject backgroundImage;
    public Sprite[] sprites;
    public float textDelay = 0.3f;
    private string current = "";
    private int count = 0;
    private bool showing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(showText());
    }

    IEnumerator showText() {
        //show letters one at a time
        for (var i = 0; i <= textlist[count].Length; i++)
        {
            if (!showing)
            {
                current = textlist[count].Substring(0, i);
                this.GetComponent<TMPro.TextMeshProUGUI>().text = current;
                yield return new WaitForSeconds(textDelay);
            }  
        }
    
    }

    void Update()
    {
        //TODO: This should use the input system
        if (Input.GetButtonDown("Submit"))
        {
            if (showing && count + 1 == textlist.Length) {
                count++;
                SceneManager.LoadScene("MainScene");
            }
            else if (showing) {
                showing = false;
                count++;
                StartCoroutine(showText());
                backgroundImage.GetComponent<Image>().sprite = sprites[count];
            }
            else {
                this.GetComponent<TMPro.TextMeshProUGUI>().text = textlist[count];
                showing = true;
            }
        }
    }
}

