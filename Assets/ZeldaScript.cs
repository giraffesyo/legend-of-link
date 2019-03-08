using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeldaScript : MonoBehaviour {

	// Use this for initialization
	public Animator anim;
	public BoxCollider2D zeldaHitbox;
	public Rigidbody2D rb;
	public CircleCollider2D swordAttackBox;
	public float speed = 1.5f;
	private Vector3 flip;
	private bool directionRight = true;
	private AnimatorClipInfo[] clipInfo;
	void Start () {
		
		clipInfo = anim.GetCurrentAnimatorClipInfo(0);
		rb = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (clipInfo [0].clip.name.Equals ("Landing")) {
			
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			zeldaHitbox.enabled = false;
			swordAttackBox.enabled = true;
			anim.Play ("Attacking");
		} 
		else if (Input.GetKeyUp (KeyCode.DownArrow)) {
			zeldaHitbox.enabled = true;
			swordAttackBox.enabled = false;
			anim.Play ("Idle");
		}

		else if(Input.GetKey(KeyCode.UpArrow)){
			anim.Play("Jumping");
			rb.AddForce (Vector3.up);

		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * speed * Time.deltaTime;
			if (!directionRight) {
				directionRight = true;
				transform.RotateAround (transform.position, transform.up, 180f);
			}
			anim.Play("Running");
			}
		else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed * Time.deltaTime;
			if (directionRight) {
				directionRight = false;
				transform.RotateAround (transform.position, transform.up, 180f);
			}
			anim.Play ("Running");		
		} 

		else {
			anim.Play ("Idle");
		}

	}
	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("OnCollisionEnter2D");
	}
}
