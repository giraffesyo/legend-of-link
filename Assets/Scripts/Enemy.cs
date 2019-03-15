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
    private float timer=0;
    private int attackTime = 2;
    private Zelda player;
    private bool facingLeft;

    //public GameObject redStompCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Zelda>();
        if (this.gameObject.tag.Equals("Red"))
        {
            redAnim = this.gameObject.GetComponent<Animator>();
            redAnim.Play("EnemyWalk");
        }
             
       
        if (this.gameObject.CompareTag("Blue"))
        {
            blueAnim = this.gameObject.GetComponent<Animator>();
            blueAnim.Play("BlueWalk");


        }
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        facingLeft = true;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("stompLayer"), LayerMask.NameToLayer("stage"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("stompLayer"), LayerMask.NameToLayer("backstage"));
    }
    private IEnumerator Die()
    {
        
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (notDead)
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            if (this.gameObject.tag.Equals("Red"))
            {
                redAnim.Play("EnemyDeath");
                Collider2D[] colliders = rb.GetComponents<Collider2D>();
                
                foreach (Collider2D g in colliders)
                {
                    rb.gravityScale = 0;
                    g.enabled = false;
                }
                notDead = false;
                StartCoroutine(Die());
            }
            else
            {
                Debug.Log("Blue Hit");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timer >= attackTime)
            {
                Attack();
                timer = 0;
            }
        }
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("enemyBounds") || collision.collider.gameObject.layer == LayerMask.NameToLayer("red"))
        {
            transform.RotateAround(transform.position, transform.up, 180f);
            StartCoroutine("Wait");
        }
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
    }
    
    private void Attack()
    {
        player.Attack(gameObject.transform);
    }
}
