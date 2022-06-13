using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator Anim;
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint;
    public Transform rightpoint;
    public float Speed;
    public float JumpForce;
    private float leftx;
    private float rightx;

    private bool Faceleft = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement () 
    {
        if (Faceleft) //Face Left
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }
            
            if (transform.position.x < leftx) //Overpass left point turn around
            {
                transform.localScale = new Vector3(-1,1,1);
                Faceleft = false;
            }
        }
        else //Face Right
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
            if (transform.position.x > rightx) //Overpass right point turn around
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
                
        }
    
    }

    void SwitchAnim()
    {
        if (Anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        if (Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))
        {
            Anim.SetBool("falling", false);
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        Anim.SetTrigger("death");
    }

}
