using System.Collections;
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
        spaceStation = GameObject.Find("SpaceStation");
        saveScript = FindObjectOfType<SaveScript>();
    }

    void Update()
    {
        if (!CommandScript.IsPaused && !frozen && !Inventory.inventoryOpen)
        {
            if (playerShipController.health <= 0)
            {
                Vector3 diff = spaceStation.transform.position - transform.position;
                float angle = Mathf.Atan2(diff.y, diff.x);
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
                direction = spaceStation.transform.position - transform.position;
                rb.velocity = Vector2.MoveTowards(rb.velocity, direction, 7 * Time.deltaTime);
                distance = Vector2.Distance(transform.position, spaceStation.transform.position);
            }
            if (playerShipController.health > 0)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                Vector3 diff = player.transform.position - transform.position;
                float angle = Mathf.Atan2(diff.y, diff.x);
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
                distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance >= 14)
                {
                    direction = player.transform.position - transform.position;
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
                PlayerController.Stats.totalEnemiesKilled++;
                PlayerController.Stats.enemiesKilledBeforeDeath++;
                playerShipController.money += Random.Range(3, 6);
                alive = false;
                animator.enabled = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(ExplosionWait());
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
            damageable = false;
            damage = saveScript.laser.level + 4;
            health -= damage;
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
    IEnumerator Itime()
    {
        while (!damageable)
        {
            yield return new WaitForSeconds(.4f);
            damageable = true;
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
    IEnumerator FrozenWait()
    {
        yield return new WaitForSeconds(4f);
        frozen = false;
    }
}
