using System.Collections;
using UnityEngine;

public class EngineerEnemyController : MonoBehaviour
{
    private float distance;
    private GameObject player;
    private bool coolDownOver = true;
    private bool alive = true;
    private float health = 30;
    public Transform shootPointTransform;
    public GameObject laserPrefab;
    public GameObject sentryPrefab;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float randomAttack;
    private Animator animator;
    private PlayerShipController playerShipController;
    private float damage;
    private MiniShopScript miniShopScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerShipController = player.GetComponent<PlayerShipController>();
        animator = GetComponent<Animator>();
        miniShopScript = FindObjectOfType<MiniShopScript>();
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
            playerShipController.money += Random.Range(4, 8);
            alive = false;
            animator.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ExplosionWait());
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance >= 15)
        {
            direction = (player.transform.position - transform.position);
            rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 6f * Time.deltaTime);
        }
        if (distance < 15)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (distance <= 18 && coolDownOver && alive)
        {
            randomAttack = Random.Range(1, 4);
            if (randomAttack <= 2)
            {
                coolDownOver = false;
                GameObject Laser = Instantiate(laserPrefab, shootPointTransform.position, shootPointTransform.rotation);
                Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPointTransform.up * 20, ForceMode2D.Impulse);
                StartCoroutine(ShootWait());
            }
            else if (randomAttack >= 3)
            {
                coolDownOver = false;
                StartCoroutine(ShootWait());
                Instantiate(sentryPrefab, shootPointTransform.position, shootPointTransform.rotation);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            damage = miniShopScript.laser.level + 5;
            health -= damage;
        }
    }
    IEnumerator ShootWait()
    {
        while (!coolDownOver)
        {
            yield return new WaitForSeconds(3.6f);
            coolDownOver = true;
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(.4f);
        gameObject.SetActive(false);
    }
}
