using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform uppoint, downpoint;
    public float Speed;
    private float upy, downy;

    private bool Faceup = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
        upy = uppoint.position.y;
        downy = downpoint.position.y;
        Destroy(uppoint.gameObject);
        Destroy(downpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Faceup)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > upy)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceup = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < downy)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceup = true;
            }

        }

    }
}
