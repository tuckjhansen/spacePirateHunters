using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
    private readonly Tags sun = new(.9f, null, "Sun", "damage", 1, true);
    private readonly Tags enemyLaser = new(.3f, null, "Laser", "damage", 5, true);
    private readonly Tags gravityObject = new(0f, null, "Planet", "gravity", 0, true);
    
    public GameObject laserPrefabEnemy;
    public float health = 125;
    public float maxHealth = 125;
    public Slider healthSlider;
    public TMP_Text totalEnemiesKilledText;
    public TMP_Text enemiesKilledBeforeDeathText;
    public TMP_Text moneyText;
    public float money = 0;
    public Weapon laserWeaponAttachment = new("laser", true, null, true, true, .43f);
    public Weapon bombWeaponAttachment = new("bomb", false, null, false, true, 2.15f);
    public GameObject LaserPrefab;
    public GameObject playerBomb;
    public List<Weapon> attachmentWeaponList = new();
    private Rigidbody2D rb;
    private Animator animator;
    public string weaponEquiped;
    public Transform shootPoint;
    public struct Stats
    {
        public static float totalEnemiesKilled;
        public static float enemiesKilledBeforeDeath;
    }

    [SerializeField]
    private InputActionReference movement, rotation, attack, shootPointbuttons, use;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        laserWeaponAttachment.prefab = LaserPrefab;
        bombWeaponAttachment.prefab = playerBomb;
        attachmentWeaponList.Add(laserWeaponAttachment);
        attachmentWeaponList.Add(bombWeaponAttachment);
        TagsList.Add(sun);
        foreach (Tags tagObject in TagsList)
        {
            AssignGameobject(tagObject.name, tagObject);
        }
        enemyLaser.gameObject = laserPrefabEnemy;
        TagsList.Add(enemyLaser);
        TagsList.Add(gravityObject);
    }
    void AssignGameobject(string name, Tags tagsobject)
    {
        tagsobject.gameObject = GameObject.Find(name);
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
        moneyText.text = "Money: " + money;
        totalEnemiesKilledText.text = "Total Enemies Killed: " + Stats.totalEnemiesKilled;
        enemiesKilledBeforeDeathText.text = "Enemies Killed Before Death: " + Stats.enemiesKilledBeforeDeath;
        healthSlider.value = health / maxHealth;
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
        int planetsNumber = 3;
        GameObject[] planets = new GameObject[planetsNumber];
        for (int i = 0; i < planetsNumber; i++)
        {
            planets[i] = GameObject.Find("Planet");
        }
        float closestPlanetDistance = int.MaxValue;
        GameObject closestPlanet = null;
        foreach (GameObject planet in planets)
        {
            if (planet != null)
            {
                if ((transform.position - planet.transform.position).sqrMagnitude < (closestPlanetDistance * closestPlanetDistance))
                {
                    closestPlanetDistance = (transform.position - planet.transform.position).sqrMagnitude;
                    closestPlanet = planet;
                }
            }
            else
            {
                Debug.LogError("PlayerController.C# error in closest planet checking! Gameobject: " + planet);
            }
        }
        gravityObject.gameObject = closestPlanet;
    }
    private void FixedUpdate()
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
        if (attack.action.ReadValue<float>() != 0)
        {
            Attack(weaponEquiped);
        }
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
        if (weaponused == "laser" && weapon.cooldownDone)
        {
            weapon.cooldownDone = false;
            GameObject Laser = Instantiate(laserWeaponAttachment.prefab, shootPoint.transform.parent.position, shootPoint.transform.rotation);
            Rigidbody2D rb = Laser.GetComponent<Rigidbody2D>();
            rb.AddForce(shootPoint.up * 28, ForceMode2D.Impulse);
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
        else
        {
            Debug.LogError(gameObject + "/" + name + " Wait Coroutine Error");
        }
    }
}
