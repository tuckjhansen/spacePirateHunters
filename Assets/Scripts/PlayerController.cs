using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class Weapon
{
    public Weapon(string name, bool equiped, GameObject prefab, bool haveWeapon, bool cooldownDone, float cooldowntime)
    {
        this.name = name;
        this.equiped = equiped;
        this.prefab = prefab;
        this.haveWeapon = haveWeapon;
        this.cooldownDone = cooldownDone;
        this.cooldowntime = cooldowntime;
    }
    public string name;
    public bool equiped;
    public GameObject prefab;
    public bool haveWeapon;
    public bool cooldownDone;
    public float cooldowntime;
}
public class Tags
{
    public Tags(float waitTime, GameObject gameObject, string name, string action, float damageTaken, bool interactable)
    {
        this.waitTime = waitTime;
        this.gameObject = gameObject;
        this.name = name;
        this.action = action;
        this.damagetaken = damageTaken;
        this.interactable = interactable;
    }
    public float waitTime;
    public GameObject gameObject;
    public string name;
    public string action;
    public float damagetaken;
    public bool interactable;
}
public class PlayerController : MonoBehaviour
{
    private readonly List<Tags> TagsList = new();
    private readonly Tags enemyLaser = new(.3f, null, "Laser", "damage", 5, true);
    private readonly Tags goodStation = new(.2f, null, "goodStation", "heal", -20, true);
    public GameObject laserPrefabEnemy;
    public float health = 125;
    public float maxHealth = 125;
    public Slider healthSlider;
    [SerializeField] private TMP_Text totalEnemiesKilledText;
    [SerializeField] private TMP_Text enemiesKilledBeforeDeathText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text healthText;
    public float money = 0;
    public Weapon laserWeaponAttachment = new("laser", true, null, true, true, .25f);
    public Weapon bombWeaponAttachment = new("bomb", false, null, false, true, 1.6f);
    public Weapon EMPWeaponAttachment = new("EMP", false, null, false, true, 4);
    public GameObject LaserPrefab;
    public GameObject playerBomb;
    public GameObject playerEMPPrefab;
    public List<Weapon> attachmentWeaponList = new();
    public List<Weapon> attachmentSpecialWeaponList = new();
    private Rigidbody2D rb;
    private Animator animator;
    public string weaponEquiped;
    public string specialWeaponEquiped;
    public Transform shootPoint;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hubArea;
    [SerializeField] private Sprite ship;
    public struct Stats
    {
        public static float totalEnemiesKilled;
        public static float enemiesKilledBeforeDeath;
    }

    [SerializeField]
    private InputActionReference movement, rotation, attack, shootPointbuttons, use, specialAttack;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        laserWeaponAttachment.prefab = LaserPrefab;
        bombWeaponAttachment.prefab = playerBomb;
        EMPWeaponAttachment.prefab = playerEMPPrefab;
        attachmentSpecialWeaponList.Add(EMPWeaponAttachment);
        attachmentWeaponList.Add(laserWeaponAttachment);
        attachmentWeaponList.Add(bombWeaponAttachment);
        enemyLaser.gameObject = laserPrefabEnemy;
        TagsList.Add(enemyLaser);
    }

    void ShootPoint(float axisofShootPointMovement)
    {
        GameObject shootPointCenter = shootPoint.transform.parent.gameObject;
        if (axisofShootPointMovement > 0)
        {
            shootPoint.transform.RotateAround(shootPointCenter.transform.position, Vector3.back, 150f * Time.deltaTime);
        }
        else if (axisofShootPointMovement < 0)
        {
            shootPoint.transform.RotateAround(transform.position, Vector3.forward, 150f * Time.deltaTime);
        }
    }

    void Update()
    {
        Vector3 zPositionChangedTo0 = new (transform.position.x, transform.position.y, 0);
        transform.position = zPositionChangedTo0;
        if (!CommandScript.IsPaused && !Inventory.inventoryOpen)
        {
            moneyText.text = "Money: " + money;
            healthText.text = health + " / " + maxHealth;
            totalEnemiesKilledText.text = "Enemies Killed: " + Stats.totalEnemiesKilled;
            enemiesKilledBeforeDeathText.text = "Kills before death: " + Stats.enemiesKilledBeforeDeath;
            healthSlider.value = health / maxHealth;
            GameObject[] EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy");
            enemyCountText.text = "Enemies Remaining: " + EnemiesCount.Length;
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
            foreach (Weapon weapon in attachmentSpecialWeaponList)
            {
                if (specialWeaponEquiped == weapon.name)
                {
                    weapon.equiped = true;
                    foreach (Weapon weaponNotEquiped in attachmentSpecialWeaponList)
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
                health = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.enabled = true;
                StartCoroutine(Wait(.5f, null, null));
                if (use.action.ReadValue<float>() != 0)
                {
                    StartCoroutine(Respawn());
                }
            }
            TagsList.Remove(goodStation);
            GameObject foundStation = GameObject.FindGameObjectWithTag("Station");
            if (foundStation != null)
            {
                goodStation.gameObject = foundStation;
                TagsList.Add(goodStation);
            }
            else
            {
                goodStation.gameObject = null;
            }
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
        }    }
    
    private void FixedUpdate()
    {
        if (!CommandScript.IsPaused && !Inventory.inventoryOpen)
        {
            if (shootPointbuttons.action.ReadValue<float>() != 0)
            {
                ShootPoint(shootPointbuttons.action.ReadValue<float>());
            }
            if (movement.action.ReadValue<float>() != 0)
            {
                Move(movement.action.ReadValue<float>());
            }
            if (rotation.action.ReadValue<float>() != 0)
            {
                Rotate(rotation.action.ReadValue<float>());
            }
            if (attack.action.ReadValue<float>() != 0 && health > 0)
            {
                Attack(weaponEquiped);
            }
            if (specialAttack.action.ReadValue<float>() != 0 && health > 0)
            {
                SpecialAttack(specialWeaponEquiped);
            }
        }
    }
    IEnumerator Respawn()
    {
        VideoPlayer loadingScreen = FindObjectOfType<VideoPlayer>();
        loadingScreen.enabled = true;
        GameObject currentarea = null;
        string[] areas =
        {
            "Sun",
            "Mercury",
            "Venus",
            "Earth", 
            "Mars",
            "Arena"
        };
        foreach (GameObject areaObject in FindObjectsOfType<GameObject>())
        {
            if ((areaObject.name.EndsWith("Area") || areaObject.name.EndsWith("Arena")) && areaObject.activeInHierarchy)
            {
                foreach (string areaName in areas)
                {
                    if (areaObject.name.Contains(areaName))
                    {
                        currentarea = areaObject;
                        break;
                    }
                }
                break;
            }
        }
        
        currentarea.SetActive(false);
        yield return new WaitForSeconds(1f);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        hubArea.SetActive(true);
        health = maxHealth;
        rb.constraints = RigidbodyConstraints2D.None;
        animator.enabled = false;
        transform.position = new Vector3(-165, -667, 0);
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = ship;
        healthSlider.value = health / maxHealth;
        loadingScreen.enabled = false;
    }
    void Move(float moveDirection)
    {
        if (moveDirection > 0)
        {
            rb.AddForce(transform.up, ForceMode2D.Impulse);
        }
        else if (moveDirection < 0)
        {
            rb.AddForce(transform.up * -1, ForceMode2D.Impulse);
        }
    }
    void Rotate(float rotation)
    {
        if (rotation > 0)
        {
            Vector3 newRotation = new(0, 0, transform.rotation.z - 3.5f);
            transform.eulerAngles += newRotation;
        }
        else if (rotation < 0)
        {
            Vector3 newRotation = new(0, 0, transform.rotation.z + 3.5f);
            transform.eulerAngles += newRotation;
        }
    }
    void Attack(string weaponused)
    {
        Weapon weapon = (Weapon)GetType().GetField(weaponused + "WeaponAttachment").GetValue(this);
        if (weapon.cooldownDone)
        {
            weapon.cooldownDone = false;
            GameObject weaponInstantiated = Instantiate(weapon.prefab, shootPoint.parent.position, shootPoint.rotation);
            StartCoroutine(Wait(weapon.cooldowntime, null, weapon));
            if (weaponused == "laser")
            {
                Rigidbody2D rb = weaponInstantiated.GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.up * 28, ForceMode2D.Impulse);
            }
        }
    }
    void SpecialAttack(string weaponused) 
    {
        Weapon weapon = (Weapon)GetType().GetField(weaponused + "WeaponAttachment").GetValue(this);
        if (weapon.cooldownDone)
        {
            weapon.cooldownDone = false;
            GameObject weaponInstantiated = Instantiate(weapon.prefab, shootPoint.parent.position, shootPoint.rotation);
            StartCoroutine(Wait(weapon.cooldowntime, null, weapon));
        }
    }
    void Interact(Tags tagTouching)
    {
        foreach (Tags tag in TagsList)
        {
            if (tag.gameObject.name == tagTouching.gameObject.name && tagTouching.interactable)
            {
                if (tagTouching.action == "damage")
                {
                    float damage = tagTouching.damagetaken;
                    health -= damage;
                    tagTouching.interactable = false;
                    StartCoroutine(Wait(tagTouching.waitTime, tagTouching, null));
                }
                if (tagTouching.action == "gravity")
                {
                    Vector2 gravityVector = (tagTouching.gameObject.transform.position - transform.position).normalized;
                    float force = 250;
                    rb.AddForce(force * Time.deltaTime * gravityVector);
                }
                if (tagTouching.action == "heal")
                {
                    health -= tagTouching.damagetaken;
                    tagTouching.interactable = false;
                    StartCoroutine(Wait(tagTouching.waitTime, tagTouching, null));
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (Tags transformTouching in TagsList)
        {
            if (transformTouching.gameObject.CompareTag(collision.tag))
            {
                Interact(transformTouching);
            }
        }
    }
    IEnumerator Wait(float waitTime, Tags tagInteactable, Weapon weaponUsable)
    {
        yield return new WaitForSeconds(waitTime);
        if (tagInteactable != null)
        {
            tagInteactable.interactable = true;
        }
        else if (tagInteactable == null && weaponUsable != null)
        {
            weaponUsable.cooldownDone = true;
        }
        else if (health <= 0)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            Debug.LogError(gameObject + "/" + name + " Wait Coroutine Error");
        }
    }
}
