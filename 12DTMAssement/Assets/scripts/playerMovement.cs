using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Vector2 lastClickedPos;
    
    // Start is called before the first frame update
    void Start()
    {
        linerenderer.positionCount = lineScale;
        mouseCorrection.y = 5;
    }

    // Detects when the player leaves and touches the ground
    public void OnCollisionEnter2D(Collision2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        isOnground = true;
        linerenderer.enabled = true; // This is showing and hideing the players trejectory
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        isOnground = false;
        linerenderer.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("power up"))
        {
            havePower = true;
            isOnground = true;
            linerenderer.enabled = true;
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
        if (Input.GetMouseButtonDown(0) && isOnground)
            {
                if (havePower == true)
                    {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    isOnground = false;
                    linerenderer.enabled = false;
                    havePower = false;
                    }
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.AddForce(initialVilocity * speed, ForceMode2D.Impulse);
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
