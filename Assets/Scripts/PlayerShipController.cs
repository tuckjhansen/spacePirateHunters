/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Weapon
{
    public Weapon(string name, bool equiped, GameObject prefab, bool haveWeapon)
    {
        this.name = name;
        this.equiped = equiped;
        this.prefab = prefab;
        this.haveWeapon = haveWeapon;
    }
    public string name;
    public bool equiped;
    public GameObject prefab;
    public bool haveWeapon;
}

public class PlayerShipController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float health;
    private bool touchingSun = false;
    private bool meltAble = true;
    public Slider healthSlider;
    private bool moveAble = true;
    private float coolDownTime = .43f;
    private bool coolDownOver = true;
    public Transform shootPoint;
    private bool touchingEnemyLaser = false;
    private bool ableToGetLasered = true;
    private bool touchingBomb = false;
    private bool ableToGetBombed = true;
    private Animator animator;
    private bool touchingStation = false;
    public float maxHealth;
    private bool canHealStation = true;
    public float totalEnemiesKilled = 0;
    public float enemiesKilledBeforeDeath = 0;
    public float money = 0;
    public TMP_Text totalEnemiesKilledText;
    public TMP_Text enemiesKilledBeforeDeathText;
    public TMP_Text moneyText;
    public Image meltingImage;
    public bool touchingAreaPortal = false;
    public bool touchingAdvanceAreaPortal = false;
    public bool inSunArea = false;
    public bool inMercuryArea = false;
    public bool inVenusArea = false;
    public bool inEarthArea = false;
    public bool inMarsArea = false;
    public Transform MapCamera;
    private SpriteRenderer spriteRenderer;
    private bool boostAble = true;

    public Weapon laserWeaponAttachment = new("laser", true, null, true);
    public Weapon bombWeaponAttachment = new("bomb", false, null, false);
    public string weaponEquiped;
    public GameObject LaserPrefab;
    private AreaScript areaScript;

    public TMP_Text EnemiesRemainingText;
    public TMP_Text areaText;
    private float enemyCount;
    public Sprite playerShip;
    private bool dieAble = true;
    private MiniShopScript miniShopScript;
    public List<Weapon> attachmentWeaponList = new();
    public GameObject playerBomb;
    public GameObject deadText;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        laserWeaponAttachment.prefab = LaserPrefab;
        bombWeaponAttachment.prefab = playerBomb;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        areaScript = FindObjectOfType<AreaScript>();
        animator = GetComponent<Animator>();
        maxHealth = health;
        miniShopScript = FindObjectOfType<MiniShopScript>();
        attachmentWeaponList.Add(laserWeaponAttachment);
        attachmentWeaponList.Add(bombWeaponAttachment);
    }
    private void Update()
    {
        foreach (Weapon weapon in attachmentWeaponList)
        {
            if (weaponEquiped == weapon.name)
            {
                weapon.equiped = true;
                foreach (Weapon weaponNotEquiped in attachmentWeaponList)
                {
                    if (weaponNotEquiped.name != weapon.name)
                    {
                        weaponNotEquiped.equiped = false;
                    }
                }
            }
        }
        if (health <= 0)
        {
            Respawn();
        }
        Vector3 position = new(transform.position.x, transform.position.y, -10);
        MapCamera.position = position;
        if (inSunArea)
        {
            areaText.text = "The Sun";
        }
        else if (inMercuryArea)
        {
            areaText.text = "Mercury";
        }
        else if (inVenusArea)
        {
            areaText.text = "Venus";
        }
        else if (inEarthArea)
        {
            areaText.text = "Earth";
        }
        totalEnemiesKilledText.text = "Total Enemies Killed: " + totalEnemiesKilled;
        enemiesKilledBeforeDeathText.text = "Enemies Killed Before Death: " + enemiesKilledBeforeDeath;
        moneyText.text = "Money: " + money;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (touchingStation && canHealStation)
        {
            health += 3;
            canHealStation = false;
            StartCoroutine(HealWait());
        }
        GameObject[] EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = EnemiesCount.Length;
        EnemiesRemainingText.text = "Pirates Remaining: " + enemyCount;
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject go in EnemiesCount)
            {
                go.SetActive(false);
            }
        }
        if (touchingEnemyLaser && ableToGetLasered)
        {
            float damage = 5;
            damage -= miniShopScript.hull.level / 3;
            health -= damage;
            ableToGetLasered = false;
            StartCoroutine(PainWait());
        }
        healthSlider.value = health / maxHealth;
        if (health <= 0 && dieAble)
        {
            moveAble = false;
            animator.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(DeathWait());
            transform.localScale = new Vector3(.5f, .5f, 1);
            enemiesKilledBeforeDeath = 0;
            dieAble = false;
        }

        if (touchingSun && meltAble)
        {
            float damage = 1;
            damage -= miniShopScript.hull.level / 3;
            health -= damage;
            meltAble = false;
            StartCoroutine("PainWait");
        }
        if (Input.GetButton("Shoot") && moveAble && coolDownOver)
        {
            if (laserWeaponAttachment.haveWeapon && laserWeaponAttachment.equiped)
            {
                coolDownOver = false;
                GameObject Laser = Instantiate(laserWeaponAttachment.prefab, shootPoint.position, shootPoint.rotation);
                Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.up * 28, ForceMode2D.Impulse);
                StartCoroutine(ShootWait("Laser"));
            }
            if (bombWeaponAttachment.haveWeapon && bombWeaponAttachment.equiped)
            {
                coolDownOver = false;
                Instantiate(bombWeaponAttachment.prefab, shootPoint.position, shootPoint.rotation);
                StartCoroutine(ShootWait("Bomb"));
            }
        }
        if (touchingBomb && ableToGetBombed)
        {
            float damage = 10;
            damage -= miniShopScript.hull.level / 2;
            health -= damage;
            ableToGetBombed = false;
            StartCoroutine(PainWait());
        }
        if (Input.GetKey(KeyCode.N))
        {
            health = 10;
        }
    }

    void Respawn()
    {
        deadText.SetActive(true);
        if (Input.GetKey(KeyCode.E))
        {
            moveAble = true;
            health = maxHealth;
            areaScript.RespawnUser();
            Debug.Log("Respawned");
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = playerShip;
            animator.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector3 newRotation = new Vector3(0, 0, 0);
            transform.eulerAngles = newRotation;
            dieAble = true;
            transform.localScale = new Vector2(.2f, .2f);
            deadText.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyLaser"))
        {
            touchingEnemyLaser = true;
        }
        if (collision.CompareTag("Sun"))
        {
            touchingSun = true;
            meltingImage.enabled = true;
        }
        if (collision.CompareTag("Bomb"))
        {
            touchingBomb = true;
        }
        if (collision.CompareTag("Station"))
        {
            touchingStation = true;
        }
        if (collision.CompareTag("Interactable"))
        {
            touchingAreaPortal = true;
            if (collision.gameObject.name == "ToMercuryFromSun" || collision.gameObject.name == "ToVenusFromMercury" || collision.gameObject.name == "ToEarthFromVenus")
            {
                touchingAdvanceAreaPortal = true;
            }
        }
        if (collision.gameObject.name == "SunArea")
        {
            inSunArea = true;
        }
        if (collision.gameObject.name == "MercuryArea")
        {
            inMercuryArea = true;
        }
        if (collision.gameObject.name == "VenusArea")
        {
            inVenusArea = true;
        }
        if (collision.gameObject.name == "EarthArea")
        {
            inEarthArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyLaser"))
        {
            touchingEnemyLaser = false;
        }
        if (collision.CompareTag("Sun"))
        {
            touchingSun = false;
            meltingImage.enabled = false;
        }
        if (collision.CompareTag("Bomb"))
        {
            touchingBomb = false;
        }
        if (collision.CompareTag("Station"))
        {
            touchingStation = false;
        }
        if (collision.CompareTag("Interactable"))
        {
            touchingAreaPortal = false;
            if (collision.gameObject.name == "ToMercuryFromSun" || collision.gameObject.name == "ToVenusFromMercury" || collision.gameObject.name == "ToEarthFromVenus")
            {
                touchingAdvanceAreaPortal = false;
            }
        }
        if (collision.gameObject.name == "SunArea")
        {
            inSunArea = false;
        }
        if (collision.gameObject.name == "MercuryArea")
        {
            inMercuryArea = false;
        }
        if (collision.gameObject.name == "VenusArea")
        {
            inVenusArea = false;
        }
        if (collision.gameObject.name == "EarthArea")
        {
            inEarthArea = false;
        }
    }
    IEnumerator PainWait()
    {
        while (!meltAble)
        {
            yield return new WaitForSeconds(.9f);
            meltAble = true;
        }
        while (!ableToGetLasered)
        {
            yield return new WaitForSeconds(.3f);
            ableToGetLasered = true;
        }
        while (!ableToGetBombed)
        {
            yield return new WaitForSeconds(.4f);
            ableToGetBombed = true;
        }
    }
    IEnumerator ShootWait(string weaponUsed)
    {
        while (!coolDownOver && weaponUsed == "Laser")
        {
            yield return new WaitForSeconds(coolDownTime);
            coolDownOver = true;
        }
        while (!coolDownOver && weaponUsed == "Bomb")
        {
            yield return new WaitForSeconds(coolDownTime * 5f);
            coolDownOver = true;
        }
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(.4f);
        spriteRenderer.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    IEnumerator HealWait()
    {
        yield return new WaitForSeconds(1f);
        canHealStation = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityObject") || collision.tag == "Sun")
        {
            float distance = Vector2.Distance(collision.transform.position, transform.position);

            Vector2 gravityVector = (collision.transform.position - transform.position).normalized;

            float force = 1003000 / (distance * distance);

            rb.AddForce(gravityVector * force * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity *= .7f;
        if (Input.GetAxisRaw("Horizontal") > 0 && moveAble) // d key
        {
            Vector3 newRotation = new Vector3(0, 0, transform.rotation.z - 3.5f);
            transform.eulerAngles += newRotation;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && moveAble) // A key
        {
            Vector3 newRotation = new Vector3(0, 0, transform.rotation.z + 3.5f);
            transform.eulerAngles += newRotation;
        }
        if (Input.GetAxisRaw("Vertical") == 1 && moveAble)
        {
            transform.Translate(transform.up * 17 * Time.deltaTime, Space.World);
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && moveAble)
        {
            transform.Translate(-transform.up * 17 * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.Z) && moveAble && boostAble)
        {
            transform.Translate(transform.up * 45, Space.World);
            boostAble = false;
            StartCoroutine(BoostWait());
        }
    }
    IEnumerator BoostWait()
    {
        while (!boostAble)
        {
            yield return new WaitForSeconds(.2f);
            boostAble = true;
        }
    }
}*/