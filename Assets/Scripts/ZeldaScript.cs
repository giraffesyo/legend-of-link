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

    void Start () {

		clipInfo = anim.GetCurrentAnimatorClipInfo(0);
		rb = this.GetComponent<Rigidbody2D> ();

        totalMove = Vector3.zero;
        camControl = cam.GetComponent<CameraControl>();
        camMargin = cam.pixelWidth / 3;
    }

	// Update is called once per frame
	void Update ()
	{
		if (!grounded && rb.velocity.y<0){
			swordAttackBox.enabled = false;
			anim.Play("Falling");
			if (Input.GetKey (KeyCode.RightArrow)) {
				transform.position += Vector3.right * speed * Time.deltaTime;
				if (!directionRight) {
					directionRight = true;
					transform.RotateAround (transform.position, transform.up, 180f);
				}
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.position += Vector3.left * speed * Time.deltaTime;
				if (directionRight) {
					directionRight = false;
					transform.RotateAround (transform.position, transform.up, 180f);
				}
			}

		} else if (Input.GetKey (KeyCode.DownArrow)) {
			zeldaHitbox.enabled = false;
			swordAttackBox.enabled = true;
			anim.Play ("Attacking");
		} else if (Input.GetKeyUp (KeyCode.DownArrow)) {
			zeldaHitbox.enabled = true;
			swordAttackBox.enabled = false;
			anim.Play ("Idle");
		} else if (Input.GetKeyDown (KeyCode.UpArrow)&& grounded) {
			rb.AddForce(Vector2.up * jumpHeight);
			grounded = false;
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			Vector3 changeRight= Vector3.right * speed * Time.deltaTime;
            totalMove += changeRight;
            transform.position += changeRight;
            if (!directionRight) {
				directionRight = true;
				transform.RotateAround (transform.position, transform.up, 180f);
			}
			anim.Play("Running");
		}
		else if (Input.GetKey (KeyCode.LeftArrow)) {
			Vector3 changeLeft= Vector3.left * speed * Time.deltaTime;
            totalMove += changeLeft;
            transform.position += changeLeft;
            if (directionRight) {
				directionRight = false;
				transform.RotateAround (transform.position, transform.up, 180f);
			}
			anim.Play ("Running");		
		} 

		else {
			swordAttackBox.enabled = false;
			if (grounded)
				anim.Play ("Idle");
		}

	}

    void LateUpdate()
    {
        Vector3 playerPos = cam.WorldToScreenPoint(this.transform.position);
        if (playerPos.x > camMargin)
        {
            if (totalMove.x < 0)
            {
                totalMove.x = totalMove.x * -.25f;
            }
            camControl.Adjust(totalMove);

        }
        totalMove = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("OnCollisionEnter2D");
		if (col.otherCollider.Equals (edCol)) {
			//anim.Play ("Landing");
			grounded = true;
		}
	}

}
