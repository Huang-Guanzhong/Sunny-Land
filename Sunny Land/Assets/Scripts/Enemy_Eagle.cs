using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
<<<<<<< Updated upstream

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
=======
    private Collider2D coll;
    public Transform top;
    public Transform bottom;
    public float Speed;
    public float TopY;
    public float BottomY;

    private bool isUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        TopY = top.position.y;
        BottomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

<<<<<<< Updated upstream
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

=======
    void Movement ()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > TopY)
            {
                isUp = false;
            }
        }

        else
        {
            rb.velocity = new Vector2 (rb.velocity.x, -Speed);
            if (transform.position.y < BottomY)
            {
                isUp = true;
            }
        }
>>>>>>> Stashed changes
    }
}
