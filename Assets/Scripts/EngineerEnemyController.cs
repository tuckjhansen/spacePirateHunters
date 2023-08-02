using System.Collections;
using UnityEngine;

public class EngineerEnemyController : MonoBehaviour
{
    private float distance;
    private GameObject player;
    private bool coolDownOver = true;
    private bool alive = true;
    public float health = 30;
    public Transform shootPointTransform;
    public GameObject laserPrefab;
    public GameObject sentryPrefab;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float randomAttack;
    private Animator animator;
    private PlayerController playerShipController;
    private float damage;
    private SaveScript saveScript;
    private bool damageable = true;
    private bool frozen = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerShipController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        saveScript = FindObjectOfType<SaveScript>();
    }

    void Update()
    {
        if (!frozen && !CommandScript.IsPaused && !Inventory.inventoryOpen)
        {
            Vector3 diff = (player.transform.position - transform.position);
            float angle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            if (health <= 0 && alive)
            {
                PlayerController.Stats.totalEnemiesKilled++;
                PlayerController.Stats.enemiesKilledBeforeDeath++;
                playerShipController.money += Random.Range(4, 8);
                alive = false;
                animator.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(ExplosionWait());
            }
            distance = Vector2.Distance(transform.position, player.transform.position);
            rb.constraints = RigidbodyConstraints2D.None;
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
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser") && damageable)
        {
            damage = saveScript.laser.level + 5;
            health -= damage;
            damageable = false;
            StartCoroutine(Itime());
        }
        if (collision.CompareTag("PlayerBomb") && damageable)
        {
            damage = saveScript.bomb.level + 10;
            health -= damage;
            damageable = false;
            StartCoroutine(Itime());
        }
        if (collision.CompareTag("EMP") && damageable)
        {
            health -= 10;
            damageable = false;
            StartCoroutine(Itime());
            frozen = true;
            StartCoroutine(FrozenWait());
            Destroy(collision.gameObject);
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
    IEnumerator Itime()
    {
        while (!damageable)
        {
            yield return new WaitForSeconds(.4f);
            damageable = true;
        }
    }
    IEnumerator ExplosionWait()
    {
        yield return new WaitForSeconds(.4f);
        gameObject.SetActive(false);
    }
    IEnumerator FrozenWait()
    {
        yield return new WaitForSeconds(4f);
        frozen = false;
    }
}
