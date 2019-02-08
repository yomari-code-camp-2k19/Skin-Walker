using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public float damageDuration;
    public Transform healthBar;
    float maxHealth;
    Animator animator;
    bool waitForEnd = false;
    void Start()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Vector3 scale = healthBar.localScale;
        scale.x *= (health / maxHealth + 0.1f);
        healthBar.localScale = scale;
        if (health <= 0)
        {
            healthBar.localScale = Vector3.zero;
            animator.SetBool("dead", true);
            GetComponent<EnemyController>().moveSpeed = 0.0f;
            GetComponent<EnemyAI>().skip = true;
            StartCoroutine(DeathAnimation());
        }
        else
        {
            StartCoroutine(DamageAnimation());
        }
    }


    IEnumerator DamageAnimation()
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(damageDuration);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
    }
    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

}
