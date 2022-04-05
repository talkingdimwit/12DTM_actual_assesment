using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 20f;
    public bool isOnground = true;
    public Rigidbody2D rb;
    public LineRenderer linerenderer;
    private Vector2 initialVilocity;
    private Vector2 mouseCorrection;
    private const int lineScale = 10;
    Vector2 lastClickedPos;
    
    // Start is called before the first frame update
    void Start()
    {
        linerenderer.positionCount = lineScale;
        mouseCorrection.y = 5;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        isOnground = true;
        linerenderer.enabled = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isOnground = false;
        linerenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        initialVilocity = (lastClickedPos + mouseCorrection) - rb.position;
        LineUpdate();
        if (Input.GetMouseButtonDown(0) && isOnground)
            {
                rb.AddForce(initialVilocity * speed, ForceMode2D.Impulse);
            }
    }

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
