using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public int Cherry;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        SwitchAnim();
    }

    void Movement() 
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        //chareacter movement
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
            anim.SetFloat("Running", Mathf.Abs(facedirection));
        }

        if (facedirection != 0) 
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        
        }

        //character jump
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            anim.SetBool("Jumping", true);
        }

    
    }

    void SwitchAnim() 
    {
        anim.SetBool("Idle", false);
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }

        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Idle", true);
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            Cherry += 1;
        }
    }
}

