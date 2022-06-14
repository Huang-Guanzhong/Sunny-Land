using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D DisColl;
    public AudioSource JumpAudio;
    public AudioSource HurtAudio;
    public AudioSource CherryAudio;
    public Transform CeilingCheck;

    public float speed;
    public float JumpForce;

    public LayerMask ground;
    public int Cherry;

    public Text CherryNum;
    private bool isHurt; // the default is false

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt)
        {
            Movement(); 
        }
        SwitchAnim();
    }

    void Movement() //Movement
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        //chareacter movement
        if (HorizontalMove != 0)
        {
            rb.velocity = new Vector2(HorizontalMove * speed, rb.velocity.y);
            anim.SetFloat("Running", Mathf.Abs(facedirection));
        }

        if (facedirection != 0) 
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        
        }

        //character jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            JumpAudio.Play();
            anim.SetBool("Jumping", true);
        }

        Crouch();
    
    }

    //Switch Animations
    void SwitchAnim() 
    {
        anim.SetBool("Idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim. SetBool("Falling", true) ;
        }

        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }

        else if (isHurt)
        {
            anim.SetBool("Hurt", true);
            anim.SetFloat("Running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.3f) 
            {
                anim.SetBool("Hurt", false);
                anim.SetBool("Idle", true);
                isHurt = false;
            }
        }

        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Idle", true);
        }
    
    }

    //Cherry Collections
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            CherryAudio.Play();
            Destroy(collision.gameObject);
            Cherry += 1;
            CherryNum.text = Cherry.ToString();
        }
    }

    //Elimate Enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("Falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                anim.SetBool("Jumping", true);
            }
            //Hurt
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-8, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(8, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CeilingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("Crouching", true);
                DisColl.enabled = false;
            }

            else
            {
                anim.SetBool("Crouching", false);
                DisColl.enabled = true;
            }
        }
        
    }
}

