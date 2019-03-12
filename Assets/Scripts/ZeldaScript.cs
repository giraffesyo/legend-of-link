using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeldaScript : MonoBehaviour {

	// Use this for initialization
	public int jumpHeight = 5;
	public bool isJumping;
	public Animator anim;
	public BoxCollider2D zeldaHitbox;
	public Rigidbody2D rb;
	public CircleCollider2D swordAttackBox;
	public float speed = 1.5f;
	private Vector3 flip;
	private bool directionRight = true;
	private AnimatorClipInfo[] clipInfo;
	public bool grounded = true;
	public EdgeCollider2D edCol;

    public Camera cam;
    private CameraControl camControl;
    private Vector3 totalMove;
    private float camMargin;
    private bool cameraLeft;

    private AudioSource chime;
    private AudioSource laugh;

    void Start () {
        
		clipInfo = anim.GetCurrentAnimatorClipInfo(0);
		rb = this.GetComponent<Rigidbody2D> ();

        anim = GetComponent<Animator>();

        totalMove = Vector3.zero;
        camControl = cam.GetComponent<CameraControl>();
        camMargin = cam.pixelWidth / 3;
        cameraLeft = false;

        AudioSource[] temp = GetComponents<AudioSource>();
        chime = temp[1];
        laugh = temp[0];
    }

	// Update is called once per frame
	void Update ()
	{
        if (!grounded && rb.velocity.y < 0)
        {
            swordAttackBox.enabled = false;
            anim.Play("Falling");

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
                transform.position += changeRight;
                totalMove += changeRight;

                if (!directionRight)
                {
                    directionRight = true;
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
            transform.position += changeLeft;
            totalMove += changeLeft;

            if (directionRight)
            {
                directionRight = false;
                transform.RotateAround(transform.position, transform.up, 180f);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //zeldaHitbox.enabled = false;
            swordAttackBox.enabled = true;
            anim.Play("Attacking");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            //zeldaHitbox.enabled = true;
            swordAttackBox.enabled = false;
            anim.Play("Idle");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            rb.AddForce(Vector2.up * jumpHeight);
            grounded = false;
            anim.Play("");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 changeRight = Vector3.right * speed * Time.deltaTime;
            totalMove += changeRight;
            transform.position += changeRight;
            if (!directionRight)
            {
                directionRight = true;
                transform.RotateAround(transform.position, transform.up, 180f);
            }
            anim.Play("Running");
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 changeLeft = Vector3.left * speed * Time.deltaTime;
            totalMove += changeLeft;
            transform.position += changeLeft;
            if (directionRight)
            {
                directionRight = false;
                transform.RotateAround(transform.position, transform.up, 180f);
            }
            anim.Play("Running");
        }

        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.Play("");
        }

        else
        {
            swordAttackBox.enabled = false;
            if (grounded)
                anim.Play("Idle");
        }

	}

    private void FixedUpdate()
    {
        if (grounded == false || rb.velocity.y > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("one-way platform"));
        }
    }        

    void LateUpdate()
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

    void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("OnCollisionEnter2D");
		//if (col.otherCollider.Equals (edCol)) {
        if (col.otherCollider == edCol) { 
			//anim.Play ("Landing");
			grounded = true;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TeleportIn"))
        {
            laugh.Play();
            other.gameObject.SetActive(false);
            Vector3 teleportVector = Vector3.down * 77.01f;
            transform.position += teleportVector;
            camControl.CamMode(true);
            cameraLeft = true;
            camControl.Teleport(teleportVector);
        }
        else if (other.gameObject.CompareTag("TeleportBack"))
        {
            laugh.Play();
            Vector3 teleportVector = Vector3.left * 121.6f;
            transform.position += teleportVector;
            camControl.Teleport(teleportVector);
        }
        else if (other.gameObject.CompareTag("TeleportOut"))
        {
            chime.Play();
            Vector3 teleportVector = new Vector3(61f, 77.1f, 0f);
            transform.position += teleportVector;
            cameraLeft = false;
            camControl.Teleport(teleportVector);
        }
        else if (other.gameObject.CompareTag("FreezeCamera"))
        {
            camControl.AffixCamera();
        }
    }
}
