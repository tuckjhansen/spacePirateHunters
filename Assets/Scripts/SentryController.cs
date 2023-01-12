using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryController : MonoBehaviour
{
    private float distance;
    private GameObject player;
    private bool coolDownOver = true;
    private bool alive = true;
    private float health = 15;
    public Transform shootPointTransform;
    public GameObject laserPrefab;
    private Animator animator;
    private PlayerShipController playerShipController;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerShipController = player.GetComponent<PlayerShipController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 diff = (player.transform.position - transform.position);
        float angle = Mathf.Atan2(diff.y, diff.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        if (health <= 0 && alive)
        {
            playerShipController.totalEnemiesKilled++;
            playerShipController.enemiesKilledBeforeDeath++;
            playerShipController.money += Random.Range(2, 4);
            alive = false;
            transform.localScale = new Vector3(.5f, .5f, 1);
            animator.enabled = true;
            StartCoroutine(ExplosionWait());
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 17 && coolDownOver && alive)
        {
            coolDownOver = false;
            GameObject Laser = Instantiate(laserPrefab, shootPointTransform.position, shootPointTransform.rotation);
            Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
            rb.AddForce(shootPointTransform.up * 20, ForceMode2D.Impulse);
            StartCoroutine(ShootWait());
        }
    }
    IEnumerator ShootWait()
    {
        while (!coolDownOver)
        {
            yield return new WaitForSeconds(2.5f);
            coolDownOver = true;
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(.4f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            health -= 5;
        }
    }
}
