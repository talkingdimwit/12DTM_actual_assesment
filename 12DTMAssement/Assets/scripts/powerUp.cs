using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed);
    }

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