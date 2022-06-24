using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D DisColl;
    /*public AudioSource JumpAudio;
    public AudioSource HurtAudio;
    public AudioSource CherryAudio;
    public AudioSource GemAudio;*/
    public Transform CeilingCheck;
    public Transform GroundCheck;
    public LayerMask ground;
    public Joystick joystick;

    public float speed;
    public float JumpForce;

  
    public int Cherry;
    public int Gem;
    private int extraJump;

    public Text CherryNum;
    public Text GemNum;

    private bool isHurt; // the default is false
    private bool isGround;// the default is false

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
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);
    }

    void Movement() //Movement on PC
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

        /*void Movement() //Movement on Mobile
       {
           float HorizontalMove = joystick.Horizontal;//-1f~1f
           float facedirection = joystick.Horizontal;

           //chareacter movement
           if (HorizontalMove != 0f)
           {
               rb.velocity = new Vector2(HorizontalMove * speed, rb.velocity.y);
               anim.SetFloat("Running", Mathf.Abs(facedirection));
           }

           if (facedirection > 0f)
           {
               transform.localScale = new Vector3(1, 1, 1);

           }
           if (facedirection < 0f)
           {
               transform.localScale = new Vector3(-1, 1, 1);

           }*/


        //character jump on PC
        /*if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            JumpAudio.Play();
            anim.SetBool("Jumping", true);
        }*/

        //character jump on Mobile
        /*if (joystick.Vertical > 0.5f && coll.IsTouchingLayers(ground))//-1f~1f
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            JumpAudio.Play();
            anim.SetBool("Jumping", true);
        }*/

        Crouch();

        newJump();

}
    //Doubel Jump or Multiple Jumps
    void newJump()
    {
        if (isGround)
        {
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.velocity = Vector2.up * JumpForce;// Vector2.up = new Vector2 (0,1)
            extraJump--;
            //JumpAudio.Play();
            SoundManager.instance.JumpAudio();
            anim.SetBool("Jumping", true);
        }
        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * JumpForce;
            //JumpAudio.Play();
            SoundManager.instance.JumpAudio();
            anim.SetBool("Jumping", true);
        }
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

//Collider Trigger
private void OnTriggerEnter2D(Collider2D collision)
{
    //Cherry Collections
    if (collision.tag == "Cherry")
    {
        //CherryAudio.Play();
        SoundManager.instance.CherryAudio();
        Destroy(collision.gameObject);
        Cherry += 1;
        CherryNum.text = Cherry.ToString();
    }

    //Gem Collections
    if (collision.tag == "Gem")
    {
        //GemAudio.Play();
        SoundManager.instance.GemAudio();
        Destroy(collision.gameObject);
        Gem += 1;
        GemNum.text = Gem.ToString();
    }

    if (collision.tag == "DeadLine")
    {
        GetComponent<AudioSource>().enabled = false;
        Invoke("Restart", 2f);
    }
}

//Elimate Targets
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Enemy")
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (anim.GetBool("Falling") && transform.position.y > (collision.gameObject.transform.position.y + 0.5f))
        {
            enemy.JumpOn();
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            anim.SetBool("Jumping", true);
        }
        //Hurt
        else if (transform.position.x < collision.gameObject.transform.position.x)
        {
            rb.velocity = new Vector2(-8, rb.velocity.y);
            //HurtAudio.Play();
            SoundManager.instance.HurtAudio();
            isHurt = true;
        }
        else if (transform.position.x > collision.gameObject.transform.position.x)
        {
            rb.velocity = new Vector2(8, rb.velocity.y);
            //HurtAudio.Play();
            SoundManager.instance.HurtAudio();
            isHurt = true;
        }
    }
}

//Crouch on PC
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

    //Crouch on Mobile
    /*void Crouch()
    {
        if (!Physics2D.OverlapCircle(CeilingCheck.position, 0.2f, ground))
        {
            if (joystick.Vertical < -0.5f)
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

    }*/

    void Restart()
{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}

