using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float groundCheckBound = 0.2f;
    public float jumpHeight;

    public float knockBackCount;
    public bool knockFromRight;
    public float knockBack;
    public float knockBackLength;

    public Transform attackPos;
    public float attackRadius;

    public int damage;
    public int health;
    public float damageDuration;

    bool attack = false;
    bool grounded = false; 

    bool facingRight;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public LayerMask enemyLayer;

    Rigidbody2D rigidBody;
    Animator animator;
    
    void Start()
    {
        facingRight = false;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            Movement();
        else
            rigidBody.velocity = Vector3.zero;

        Attack();
        ResetValue();
    }

    void Movement()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            grounded = false;
            animator.SetBool("isGrounded", grounded);
            rigidBody.AddForce(new Vector2(0.0f, jumpHeight));
        }


        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckBound, groundLayer);
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("verticalSpeed", rigidBody.velocity.y);

        float horizontal = Input.GetAxisRaw("Horizontal") * maxSpeed * Time.deltaTime;
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        if (knockBackCount <= 0)
        {
            rigidBody.velocity = new Vector2(horizontal, rigidBody.velocity.y);
        }
        else
        {
            if (knockFromRight)
            {
                rigidBody.velocity = new Vector2(-knockBack, rigidBody.velocity.y);
            }else
            {
                rigidBody.velocity = new Vector2(knockBack, rigidBody.velocity.y);
            }
            knockBackCount -= Time.deltaTime;
        }



        if (horizontal > 0.0 && !facingRight)
            Flip();
        else if (horizontal < 0.0 && facingRight)
            Flip();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Z))
            attack = true;

    }

    private void Attack()
    {

        animator.SetBool("attack", attack);

        if (!attack) return;

        Collider2D enemyToDamage = Physics2D.OverlapCircle(attackPos.position, attackRadius, enemyLayer);

        if (!enemyToDamage) return;

        enemyToDamage.GetComponent<EnemyController>().knockBackCount = knockBackLength;
        if (transform.position.x < enemyToDamage.transform.position.x)
        {
            enemyToDamage.GetComponent<EnemyController>().knockFromRight = false;
        }
        else
        {
            enemyToDamage.GetComponent<EnemyController>().knockFromRight = true;
        }
        enemyToDamage.GetComponent<EnemyHealth>().TakeDamage(damage);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        //GetComponent<SpriteRenderer>().flipX = facingRight;
    }

    private void ResetValue()
    {
        attack = false;
    }

    private void Damage(Collider2D enemy)
    {
        enemy.GetComponent<EnemyController>().knockBackCount = knockBackLength;
        if(transform.position.x < enemy.transform.position.x)
        {
            enemy.GetComponent<EnemyController>().knockFromRight = false;
        }
        else
        {
            enemy.GetComponent<EnemyController>().knockFromRight = true;
        }

        enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPos.position, attackRadius);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageAnimation());
    }
    //@Todo need abstraction
    IEnumerator DamageAnimation()
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(damageDuration);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
    }
}
