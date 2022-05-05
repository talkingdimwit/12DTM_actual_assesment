using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerMovement : MonoBehaviour
{
    // Varibles
    public float speed = 20f;
    public bool isOnground = true;
    public Rigidbody2D rb;
    public LineRenderer linerenderer;
    private Vector2 initialVilocity;
    private Vector2 mouseCorrection;
    private int lineScale = 10;
    private Vector2 endofline;
    private bool havePower = false;
    private float jumpCount = 0;
    private Vector2 spawnLocation;
    Vector2 lastClickedPos;
    public TextMeshProUGUI jumpCountText;

    // Start is called before the first frame update
    void Start()
    {
        linerenderer.positionCount = lineScale;
        mouseCorrection.y = 5;
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
    }

    // Update is called once per frame
    void Update()
    {
        endofline = linerenderer.GetPosition(lineScale - 1);
        Debug.Log(endofline);
        // Gets mouse position then makes the player be thrown towards it
        lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialVilocity = (lastClickedPos + mouseCorrection) - rb.position;
        LineUpdate();
        jumpCountText.text = "Jumps: " + jumpCount;
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
            jumpCount += 1;
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
}
