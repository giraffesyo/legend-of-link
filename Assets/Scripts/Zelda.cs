using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zelda : MonoBehaviour
{
    public int jumpHeight = 5;
    public float speed = 1.5f;
    public float airSpeed = 0.5f;                           // used to reduce movement speed in air
    //private Vector3 flip;                                 // not used currently

    public bool grounded = true;
    private bool facingRight = true;

    public BoxCollider2D zeldaHitbox;
    public CircleCollider2D swordAttackbox;
    public EdgeCollider2D edCol;                            // used for zelda floor collider
    public Rigidbody2D rb;                                  // used for zelda rigid body

    public Animator anim;
    private AnimatorClipInfo[] clipinfo;

    private AudioSource chime;
    private AudioSource laugh;

    public Camera cam;
    private CameraControl camControl;
    private bool cameraLeft;                                 // can the camera move left
    private float camMargin;                                 // used to determine when camera should move (relative to the player)
    private Vector3 totalMove;                               // used to change camera position
    private bool teleported;
    private float teleportedTimer;

    public Transform groundcheck;
    public Transform groundcheck1;
    public Transform groundcheck2;
    public Transform groundcheck3;
    public Transform groundcheck4;
    public LayerMask backstage;
    public LayerMask stage;

    //Used to calculate teleport distance into lost woods
    public GameObject teleportTakeOff;
    public GameObject teleportLanding;
    private Vector3 teleportDown;

    void Start()
    {
        clipinfo = anim.GetCurrentAnimatorClipInfo(0);       // idk
        rb = this.GetComponent<Rigidbody2D>();               // gets the rigidbody of the object the script is attached to

        totalMove = Vector3.zero;                            // initialize camera pos modifier to 0
        camControl = cam.GetComponent<CameraControl>();
        camMargin = cam.pixelWidth / 3;                      // higher # makes camera start moving sooner (relative to player)
        cameraLeft = false;

        AudioSource[] sounds = GetComponents<AudioSource>(); // renamed becuase not really temp
        chime = sounds[1];
        laugh = sounds[0];

        Physics2D.IgnoreCollision(zeldaHitbox, swordAttackbox);
        Physics2D.IgnoreCollision(zeldaHitbox, edCol);
        Physics2D.IgnoreCollision(edCol, swordAttackbox);

        teleportedTimer = 0;
        teleported = false;
        teleportDown = Vector3.down * (Mathf.Abs(teleportLanding.transform.position.y - teleportTakeOff.transform.position.y));
    }

    void Update()
    {
        // pretend to not see this monstrosity
        grounded = ((Physics2D.Linecast(transform.position, groundcheck.position, stage) || Physics2D.Linecast(transform.position, groundcheck.position, backstage)) ||
            (Physics2D.Linecast(transform.position, groundcheck1.position, stage) || Physics2D.Linecast(transform.position, groundcheck1.position, backstage)) ||
                (Physics2D.Linecast(transform.position, groundcheck2.position, stage) || Physics2D.Linecast(transform.position, groundcheck2.position, backstage) ||
                    (Physics2D.Linecast(transform.position, groundcheck3.position, stage) || Physics2D.Linecast(transform.position, groundcheck3.position, backstage)) ||
                        (Physics2D.Linecast(transform.position, groundcheck4.position, stage) || Physics2D.Linecast(transform.position, groundcheck4.position, backstage))));

        if (grounded == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                grounded = false;
                rb.AddForce(Vector3.up * jumpHeight);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
                totalMove += changeRight;
                transform.position += changeRight;
                if (!facingRight)
                {
                    facingRight = true;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
                anim.Play("Running");
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
                totalMove += changeLeft;
                transform.position += changeLeft;
                if (facingRight)
                {
                    facingRight = false;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
                anim.Play("Running");
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                swordAttackbox.enabled = true;
                anim.Play("Attacking");
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                swordAttackbox.enabled = false;
                anim.Play("Idle");
            }
            else
            {
                anim.Play("Idle");
            }

            if (teleported)
            {
                teleportedTimer += Time.deltaTime;
                if (teleportedTimer >= 1)
                {
                    teleported = false;
                    teleportedTimer = 0;
                }
            }
        }

        else if (grounded == false && rb.velocity.y > 0)
        {
            // in air, going up
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 changeRight = Vector3.right * speed * Time.deltaTime * airSpeed;
                //Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
                transform.position += changeRight;
                totalMove += changeRight;

                if (!facingRight)
                {
                    facingRight = true;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 changeLeft = Vector3.left * speed * Time.deltaTime * airSpeed;
                //Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
                transform.position += changeLeft;
                totalMove += changeLeft;

                if (facingRight)
                {
                    facingRight = false;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                swordAttackbox.enabled = true;
                anim.Play("Attacking");
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                swordAttackbox.enabled = false;
                anim.Play("Idle");
            }
        }

        else if (grounded == false && rb.velocity.y < 0)
        {
            // in air, falling
            swordAttackbox.enabled = false;
            anim.Play("Falling");

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 changeRight = Vector3.right * speed * Time.deltaTime * airSpeed;
                transform.position += changeRight;
                totalMove += changeRight;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 changeLeft = Vector3.left * speed * Time.deltaTime * airSpeed;
                transform.position += changeLeft;
                totalMove += changeLeft;
            }
        }
        else
        {
            anim.Play("Idle");
            grounded = true;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                grounded = false;
                rb.AddForce(Vector3.up * jumpHeight);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
                totalMove += changeRight;
                transform.position += changeRight;
                if (!facingRight)
                {
                    facingRight = true;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
                anim.Play("Running");
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
                totalMove += changeLeft;
                transform.position += changeLeft;
                if (facingRight)
                {
                    facingRight = false;
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
                anim.Play("Running");
            }
        }
    }

    void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("backstage"), rb.velocity.y > 0);
    }

    private void LateUpdate()
    {
        Vector3 playerPos = cam.WorldToScreenPoint(this.transform.position);
        if (!cameraLeft && playerPos.x > camMargin)
        {
            camControl.Adjust(totalMove);
        }
        else if (cameraLeft)
        {
            camControl.Adjust(totalMove);
        }
        totalMove = Vector3.zero;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TeleportIn"))
        {
            if (!teleported)
            {
                laugh.Play();
                other.gameObject.SetActive(false);
                //Vector3 teleportVector = Vector3.down * 76f;
                Vector3 teleportVector = teleportDown;
                transform.position += teleportVector;
                camControl.CamMode(true);
                cameraLeft = true;
                camControl.Teleport(teleportVector);
                teleported = true;
            }
        }
        else if (other.gameObject.CompareTag("TeleportBack"))
        {
            if (!teleported)
            {
                laugh.Play();
                Vector3 teleportVector = Vector3.left * 121.6f;
                transform.position += teleportVector;
                camControl.Teleport(teleportVector);
                teleported = true;
            }
        }
        else if (other.gameObject.CompareTag("TeleportOut"))
        {
            if (!teleported)
            {
                chime.Play();
                Vector3 teleportVector = new Vector3(61f, 77.1f, 0f);
                transform.position += teleportVector;
                camControl.CamMode(false);
                cameraLeft = false;
                camControl.Teleport(teleportVector);
                teleported = true;
            }
        }
        else if (other.gameObject.CompareTag("FreezeCamera"))
        {
            camControl.AffixCamera();
        }
    }
}