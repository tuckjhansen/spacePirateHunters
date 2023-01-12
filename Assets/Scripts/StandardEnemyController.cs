using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyController : MonoBehaviour
{
    private float distance;
    private GameObject player;
    private GameObject spaceStation;
    private bool coolDownOver = true;
    private bool alive = true;
    public float health = 20;
    public Transform shootPointTransform;
    public GameObject laserPrefab;
    private Rigidbody2D rb;
    public Vector3 direction;
    private Animator animator;
    private PlayerShipController playerShipController;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerShipController = player.GetComponent<PlayerShipController>();
        animator = GetComponent<Animator>();
        spaceStation = GameObject.Find("SpaceStation");
    }

    void Update()
    {
        if (playerShipController.health <= 0)
        {
            Vector3 diff = (spaceStation.transform.position - transform.position);
            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            direction = (spaceStation.transform.position - transform.position);
            rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 7 * Time.deltaTime);
            distance = Vector2.Distance(transform.position, spaceStation.transform.position);
            if (distance < 6)
            {
                gameObject.SetActive(false);
            }
        }
        if (playerShipController.health > 0)
        {
            Vector3 diff = (player.transform.position - transform.position);
            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance >= 14)
            {
                direction = (player.transform.position - transform.position);
                rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 7 * Time.deltaTime);
            }
            if (distance <= 14)
            {
                rb.velocity = new Vector2(0, 0);
            }
            if (distance <= 17 && coolDownOver && alive)
            {
                coolDownOver = false;
                GameObject Laser = Instantiate(laserPrefab, shootPointTransform.position, shootPointTransform.rotation);
                Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPointTransform.up * 20, ForceMode2D.Impulse);
                StartCoroutine(ShootWait());
            }
        }
        if (health <= 0 && alive)
        {
            playerShipController.totalEnemiesKilled++;
            playerShipController.enemiesKilledBeforeDeath++;
            playerShipController.money += Random.Range(3, 6);
            alive = false;
            animator.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ExplosionWait());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            health -= 5;
        }
    }
    IEnumerator ShootWait()
    {
        while (!coolDownOver)
        {
            yield return new WaitForSeconds(2.9f);
            coolDownOver = true;
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(.4f);
        gameObject.SetActive(false);
    }
}
