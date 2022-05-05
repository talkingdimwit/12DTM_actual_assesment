using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    private float rotateSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed); //makes power ups spin
    }

    // code underneath this makes the power up disappear and reappear on grab
    public void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.active = false;
        Invoke("wake", 5);
    }
    void wake()
    {
        gameObject.active = true;
    }
}
