using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 20f;
    public bool isOnground = true;
    public Rigidbody2D rb;
    private Vector2 initialVilocity;
    Vector2 lastClickedPos;
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        isOnground = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isOnground = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isOnground)
            {
                lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialVilocity = lastClickedPos - rb.position;
                rb.AddForce(initialVilocity, ForceMode2D.Impulse);
            }
    }
}
