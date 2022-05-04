using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public float time = 0;
    public float roundSeconds;
    public TextMeshProUGUI timerText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { // makes the timer in top left of screen count up in seconds
        time += Time.deltaTime;
        roundSeconds = (float)Math.Round(time, 0);
        timerText.text = "Time: " + roundSeconds;
    }
}
