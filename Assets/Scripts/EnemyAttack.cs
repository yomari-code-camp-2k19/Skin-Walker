using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;
    public LayerMask playerLayer;
    public Transform startPos;
    public float detectRadius;
    public float attackDistance;
    public float momentum;
    public float hitForce;
    Animator animator;

    bool attacking = false;

    void Start()
    {
        animator = GetComponent<Animator>(); ;
        
    }

    void Update()
    {
        Vector3 origin = startPos.position;
        RaycastHit2D hitL = Physics2D.Raycast(origin, Vector3.left, detectRadius, playerLayer);
        RaycastHit2D hitR = Physics2D.Raycast(origin, Vector3.right, detectRadius, playerLayer);

        if (hitL || hitR)
        {

            bool facingRight = GetComponent<EnemyController>().facingRight;
            if (hitL && facingRight)
                GetComponent<EnemyAI>().Flip();
            else if (hitR && !facingRight)
                GetComponent<EnemyAI>().Flip();

            GetComponent<EnemyController>().speedMultiplier = momentum;
            facingRight = GetComponent<EnemyController>().facingRight;


            float hitdistance = Mathf.Max(hitL.distance, hitR.distance);
            if (hitdistance < attackDistance)
            {
                attacking = true;
            }
            else
                attacking = false;
        }
        else
        {
            GetComponent<EnemyController>().speedMultiplier = 1.0f;
            attacking = false;
        }
    
        animator.SetBool("attack", attacking);

    }
}
