using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBombScript : MonoBehaviour
{
    private float lowestDistanceAwayEnemy = int.MaxValue;
    private float distanceFromTarget;
    private GameObject enemyTargeted;
    private Rigidbody2D rb;
    private Vector3 direction;
    private Animator animator;
    private PlayerShipController playerShipController;
    private BoxCollider2D boxCollider;

    void Start()
    {
        StartCoroutine(ExplosionWait());
        animator = GetComponent<Animator>();
        playerShipController = FindObjectOfType<PlayerShipController>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (GameObject.FindGameObjectsWithTag("Enemy").Count() > 0)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                distanceFromTarget = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceFromTarget < lowestDistanceAwayEnemy)
                {
                    lowestDistanceAwayEnemy = distanceFromTarget;
                    enemyTargeted = enemy;
                }
            }
        }
        if (enemyTargeted == null) 
        {
            transform.rotation = playerShipController.transform.rotation;
            direction = transform.position - transform.position;
            rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 10 * Time.deltaTime);
        }
    }

    void Update()
    {
        if (enemyTargeted != null)
        {
            Vector3 diff = (enemyTargeted.transform.position - transform.position);
            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            direction = enemyTargeted.transform.position - transform.position;
            rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 6 * Time.deltaTime);
            distanceFromTarget = Vector2.Distance(transform.position, enemyTargeted.transform.position);
            if (distanceFromTarget <= 4)
            {
                animator.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCollider.enabled = true;
                StartCoroutine(DeathWait());
                transform.localScale = new Vector3(.5f, .5f, 1);
            }
        }
        else
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 6 * Time.deltaTime);
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(4);
        animator.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        boxCollider.enabled = true;
        transform.localScale = new Vector3(.3f, .3f, 1);
        StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
