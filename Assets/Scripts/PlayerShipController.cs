using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Weapon
{
    public Weapon(string name, bool isSelected, GameObject prefab, bool haveWeapon)
    {
        this.name = name;
        this.isSelected = isSelected;
        this.prefab = prefab;
        this.haveWeapon = haveWeapon;
    }
    public string name;
    public bool isSelected;
    public GameObject prefab;
    public bool haveWeapon;
}

public class PlayerShipController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float health = 125;
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
    public float maxHealth = 125;
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
    private bool boostAble = true;

    public Weapon Weapon1 = new ("laser", true, null, true);
    public Weapon Weapon2 = new ("Bomb", false, null, false);
    public GameObject LaserPrefab;

    public TMP_Text EnemiesRemainingText;
    public TMP_Text areaText;
    private float enemyCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon1.prefab = LaserPrefab;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        StartCoroutine(UnFreeze());
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, -10);
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
            health -= 4;
            ableToGetLasered = false;
            rb.velocity *= .5f;
            StartCoroutine(PainWait());
        }
        healthSlider.value = health / 100;
        if (health <= 0)
        {
            moveAble = false;
            animator.enabled = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(DeathWait());
            transform.localScale = new Vector3(.5f, .5f, 1);
            enemiesKilledBeforeDeath = 0;
        }
        
        if (touchingSun && meltAble)
        {
            health--;
            meltAble = false;
            StartCoroutine("PainWait");
        }
        if (Input.GetButton("Shoot") && moveAble && coolDownOver)
        {
            if (Weapon1.haveWeapon && Weapon1.isSelected)
            {
                coolDownOver = false;
                GameObject Laser = Instantiate(Weapon1.prefab, shootPoint.position, shootPoint.rotation);
                Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.up * 28, ForceMode2D.Impulse);
                StartCoroutine(ShootWait());
            }
            if (Weapon2.haveWeapon && Weapon2.isSelected)
            {

            }
        }
        if (touchingBomb && ableToGetBombed)
        {
            health -= 10;
            ableToGetBombed = false;
            StartCoroutine(PainWait());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyLaser")
        {
            touchingEnemyLaser = true;
        }
        if (collision.tag == "Sun")
        {
            touchingSun = true;
            meltingImage.enabled = true;
        }
        if (collision.tag == "Bomb")
        {
            touchingBomb = true;
        }
        if (collision.tag == "Station")
        {
            touchingStation = true;
        }
        if (collision.tag == "Interactable")
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
        if (collision.tag == "EnemyLaser")
        {
            touchingEnemyLaser = false;
        }
        if (collision.tag == "Sun")
        {
            touchingSun = false;
            meltingImage.enabled = false;
        }
        if (collision.tag == "Bomb")
        {
            touchingBomb = false;
        }
        if (collision.tag == "Station")
        {
            touchingStation = false;
        }
        if (collision.tag == "Interactable")
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
    IEnumerator ShootWait()
    {
        while (!coolDownOver)
        {
            yield return new WaitForSeconds(coolDownTime);
            coolDownOver = true;
        }
    }
    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(1f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
    IEnumerator HealWait()
    {
         yield return new WaitForSeconds(1f);
         canHealStation = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "GravityObject" || collision.tag == "Sun")
        {
            float distance = Vector2.Distance(collision.transform.position, transform.position);

            Vector2 gravityVector = (collision.transform.position - transform.position).normalized;

            float force = 1003000 / (distance * distance);

            rb.AddForce(gravityVector * force * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
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
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Z) && moveAble && boostAble)
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
}