using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class playerMovement : MonoBehaviour
{
    // Varibles
    public float speed = 20f;
    public bool isOnground = true;
    public bool gameOver = false;
    public Rigidbody2D rb;
    public LineRenderer linerenderer;
    private int lineScale = 10;
    private bool havePower = false;
    private float time = 0;
    private float roundSeconds;
    private float deathCount = 0;
    private float collectables = 0;
    private Vector2 spawnLocation;
    Vector2 lastClickedPos;
    private Vector2 initialVilocity;
    private Vector2 mouseCorrection;
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoresText;

    // Start is called before the first frame update
    void Start()
    {
        linerenderer.positionCount = lineScale;
        mouseCorrection.y = 5;
        winText.text = "";
        scoresText.text = "";
        spawnLocation = new Vector2(24, 1021);
    }

    // Detects when the player leaves and touches the ground
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("ground"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition; // This is showing and hideing the players trejectory and sticking the player to the wall
            isOnground = true;
            linerenderer.enabled = true; 
        }
        if (collision.collider.gameObject.CompareTag("lava"))
        {
            rb.position = new Vector2(spawnLocation.x, spawnLocation.y); // Sends player to checkpoint if they touch lava
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            deathCount += 1;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    { // if the player is not touching the ground stops them from jumping twice
        if (collision.collider.gameObject.CompareTag("ground"))
        {
            isOnground = false;
            linerenderer.enabled = false;
        }
        if (collision.collider.gameObject.CompareTag("noStick"))
        {
            isOnground = false;
            linerenderer.enabled = false;
        }
        if (collision.collider.gameObject.CompareTag("bounce"))
        {
            isOnground = false;
            linerenderer.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    { // detects if player grabs power up or hits a checkpoint
        if (other.CompareTag("power up"))
        {
            havePower = true;
            isOnground = true;
            linerenderer.enabled = true;
        }
        if (other.CompareTag("checkPoint"))
        {
            spawnLocation = new Vector2(rb.position.x, rb.position.y);
            Time.timeScale = 1f;
        }
        if (other.CompareTag("collectable"))
        {
            other.gameObject.active = false;
            collectables += 1;
        }
        if (other.CompareTag("end"))
        {
            gameOver = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            deathCountText.text = "Deaths: " + deathCount;
            winText.text = "";
            scoresText.text = "";
            Timer();
        }
        if (gameOver == true)
        {
            deathCountText.text = "";
            timerText.text = "";
            winText.text = "You Win\n\n\nPress Space To Restart";
            scoresText.text = "time: " + roundSeconds + "s" + "\ndeaths: " + deathCount + "\ncollectables: " + collectables + "/3";
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            linerenderer.enabled = false;
            if (Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Gets mouse position then makes the player be thrown towards it
        lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialVilocity = (lastClickedPos + mouseCorrection) - rb.position;
        LineUpdate();
        if (Input.GetMouseButtonDown(0) && isOnground)
            {
                Time.timeScale = 0.5f; // slows time on click hold
            }
        if (Input.GetMouseButtonUp(0) && isOnground)
        {
            Time.timeScale = 1f;
            if (havePower == true)
            {
                isOnground = false;
                linerenderer.enabled = false;
                havePower = false;
            }
            // unsticks player to walls
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(initialVilocity * speed, ForceMode2D.Impulse); //applies thurst to player
        }

        if (Input.GetKeyDown("r"))
        {
            rb.position = new Vector2(spawnLocation.x, spawnLocation.y); // Sends player to checkpoint if they touch lava
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // This is a big math equation that draws the trejectory of the player
    private void LineUpdate()
    {
        float g = Physics.gravity.magnitude;
        float velocity = initialVilocity.magnitude;
        float angle = Mathf.Atan2(initialVilocity.y, initialVilocity.x);
        float timeStep = 0.1f;
        float fTime = 0f;

        Vector2 start = rb.position;

        for (int i = 0; i < lineScale; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle);
            float dy = velocity * fTime * Mathf.Sin(angle) - (g * fTime * fTime / 2f);
            Vector2 pos = new Vector2(start.x + dx, start.y + dy);
            linerenderer.SetPosition(i, pos);
            fTime += timeStep;
        }
    }
    // counts how long the player has
    private void Timer()
    {
        time += Time.deltaTime;
        roundSeconds = (float)Math.Round(time, 0);
        timerText.text = "Time: " + roundSeconds + "s";
    }
}
