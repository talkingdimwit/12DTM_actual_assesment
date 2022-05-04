using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public float clickCount = 0;
    public float time = 0;
    public float roundSeconds;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI clickCountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        roundSeconds = (float)Math.Round(time, 0);
        timerText.text = "Timer " + roundSeconds;
        if (Input.GetMouseButtonDown(0))
        {
            clickCount += 1;
        }
        clickCountText.text = "Clicks:" + clickCount;
    }
}
