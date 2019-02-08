using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public bool facingRight;
    public float moveSpeed;
    public float speedMultiplier = 1.0f;
    Rigidbody2D rigidBody;
    Animator animator;

    public float knockBackCount;
    public bool knockFromRight;
    public float knockBack;
    public float knockBackLength;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        float direction = facingRight ? 1.0f : -1.0f;
        if (knockBackCount <= 0.0f)
        {
            rigidBody.velocity = new Vector2(speedMultiplier * moveSpeed * direction * Time.deltaTime, rigidBody.velocity.y);
        }
        else
        {
            if (knockFromRight)
            {
                rigidBody.velocity = new Vector2(-knockBack, rigidBody.velocity.y);
            }
            else
            {
                rigidBody.velocity = new Vector2(knockBack, rigidBody.velocity.y);
            }
            knockBackCount -= Time.deltaTime;
        }
           
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.knockBackCount = player.knockBackLength;
            if (transform.position.x < player.transform.position.x)
            {
                player.knockFromRight = false;
            }
            else
            {
                player.knockFromRight = true;
            }
            player.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}

