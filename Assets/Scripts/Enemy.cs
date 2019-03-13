using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2;
    public float redModify = 1;
    public float blueModify=1;
    private Animator redAnim;
    private Animator blueAnim;
    private bool notDead = true;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(5, 12);
        if (this.gameObject.name.Equals("Red"))
        {
            redAnim = this.gameObject.GetComponent<Animator>();
            redAnim.Play("EnemyWalk");
        }
             
       
        if (this.gameObject.name.Equals("Blue"))
        {
            blueAnim = this.gameObject.GetComponent<Animator>();
            blueAnim.Play("BlueWalk");


        }
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }
    private IEnumerator Die()
    {
        
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(notDead)
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.isTrigger);
        {
            if (this.gameObject.name.Equals("Red"))
            {
                redAnim.Play("EnemyDeath");
                StartCoroutine(Die());
                Collider2D[] colliders = rb.GetComponents<Collider2D>();
                foreach (Collider2D g in colliders)
                {
                    rb.gravityScale = 0;
                    g.enabled = false;
                }
                notDead = false;
            }
            else
            {

                blueAnim.Play("BlueKnockback&Die");
                Collider2D[] colliders = rb.GetComponents<Collider2D>();
                foreach(Collider2D g in colliders)
                {
                    g.enabled = false;
                }
                rb.AddForce(new Vector2(100, 200));
                notDead = false;
                StartCoroutine(Die());
            }

        }
    }
}
