using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{

    public float toleranceDistance;
    public float forwardRange;
    public Transform startPos;
    public LayerMask ground;
    public bool skip = false;
    void Start()
    {

    }

    void Update()
    {
        if (skip)
            return;

        bool facingRight = GetComponent<EnemyController>().facingRight;
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(startPos.position, Vector2.down, Mathf.Infinity, ground.value);
        RaycastHit2D forwardHit = Physics2D.Raycast(startPos.position, direction, forwardRange, ground.value);

        if (!hit || hit.distance > toleranceDistance || forwardHit)
            Flip();
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        GetComponent<EnemyController>().facingRight = !GetComponent<EnemyController>().facingRight;
    }

}
