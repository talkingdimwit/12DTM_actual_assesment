using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 20f;
    public bool isOnground = true;
    bool moving;
    Vector2 lastClickedPos;
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnground = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isOnground)
            {
                lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                moving = true;
                
            }

        if (moving && (Vector2)transform.position != lastClickedPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
            isOnground = false;
        }
        else
        {
            moving = false;
        }
    }
}
