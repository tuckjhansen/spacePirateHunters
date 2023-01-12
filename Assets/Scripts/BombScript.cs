using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 direction;
    private Rigidbody2D rb;
    private float distance;
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        StartCoroutine(ExplosionWait());
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector3 diff = (player.transform.position - transform.position);
        float angle = Mathf.Atan2(diff.y, diff.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        direction = (player.transform.position - transform.position);
        rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 10 * Time.deltaTime);
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 4)
        {
            animator.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            boxCollider2D.enabled = true;
            StartCoroutine(DeathWait());
            transform.localScale = new Vector3(.5f, .5f, 1);
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(5f);
        animator.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        boxCollider2D.enabled = true;
        transform.localScale = new Vector3(.3f, .3f, 1);
        StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
