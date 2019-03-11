using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CircleCollider2D swordAttackBox;

    public float maxSpeed = 1.5f;
    public float moveForce = 365f;
    public float jumpForce;
    public LayerMask groundLayer;
    public Transform groundcheck;


    private AnimatorClipInfo[] clipInfo;
    private Animator anim;
    private Rigidbody2D rb2d;
    private bool grounded = false;

    private bool jump = false;
    private bool facingRight = true;



    void Start()
    {
        // Get references to the components on player
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Determine if we're currently on the ground using a line cast towards the ground

        grounded = Physics2D.Linecast(transform.position, groundcheck.position, groundLayer);


        if (grounded && Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        //if (!grounded && rb.velocity.y < 0)
        //{
        //    swordAttackBox.enabled = false;
        //    anim.Play("Falling");
        //    if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        transform.position += Vector3.right * speed * Time.deltaTime;
        //        if (!directionRight)
        //        {
        //            directionRight = true;
        //            transform.RotateAround(transform.position, transform.up, 180f);
        //        }
        //    }
        //    else if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        transform.position += Vector3.left * speed * Time.deltaTime;
        //        if (directionRight)
        //        {
        //            directionRight = false;
        //            transform.RotateAround(transform.position, transform.up, 180f);
        //        }
        //    }

        //}
        //else if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    zeldaHitbox.enabled = false;
        //    swordAttackBox.enabled = true;
        //    anim.Play("Attacking");
        //}
        //else if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    zeldaHitbox.enabled = true;
        //    swordAttackBox.enabled = false;
        //    anim.Play("Idle");
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        //{
        //    rb.AddForce(Vector2.up * jumpHeight);
        //    grounded = false;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
        //    totalMove += changeRight;
        //    transform.position += changeRight;
        //    if (!directionRight)
        //    {
        //        directionRight = true;
        //        transform.RotateAround(transform.position, transform.up, 180f);
        //    }
        //    anim.Play("Running");
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
        //    totalMove += changeLeft;
        //    transform.position += changeLeft;
        //    if (directionRight)
        //    {
        //        directionRight = false;
        //        transform.RotateAround(transform.position, transform.up, 180f);
        //    }
        //    anim.Play("Running");
        //}

        //else
        //{
        //    swordAttackBox.enabled = false;
        //    if (grounded)
        //        anim.Play("Idle");
        //}

    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 updatedScale = transform.localScale;
        updatedScale.x = updatedScale.x * -1;
        transform.localScale = updatedScale;
    }
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal * rb2d.velocity.x < maxSpeed)
        {
            // Since moveHorizontal can be a negative or positive number, this takes care of left and right movement
            rb2d.AddForce(Vector2.right * moveHorizontal * moveForce);
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            // clamp the max speed in x direction
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }
        if (hideFlags > 0 && !facingRight)
        {
            Flip();
        }
        else if (hideFlags < 0 && facingRight)
        {
            Flip();
        }

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

    }

    //void LateUpdate()
    //{
    //    Vector3 playerPos = cam.WorldToScreenPoint(this.transform.position);
    //    if (playerPos.x > camMargin)
    //    {
    //        if (totalMove.x < 0)
    //        {
    //            totalMove.x = totalMove.x * -.25f;
    //        }
    //        camControl.Adjust(totalMove);

    //    }
    //    totalMove = Vector3.zero;
    //}

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    Debug.Log("OnCollisionEnter2D");
    //    if (col.otherCollider.Equals(edCol))
    //    {
    //        //anim.Play ("Landing");
    //        grounded = true;
    //    }
    //}

}
