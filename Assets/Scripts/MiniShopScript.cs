using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeClass 
{
    public UpgradeClass(string name, float level, float cost, bool haveItem, TMP_Text moneyText, TMP_Text levelText, Image upgradeButton, float maxLevel, Button upgradeButtonbutton, TMP_Text titleText)
    {
        this.name = name;
        this.level = level;
        this.cost = cost;
        this.haveItem = haveItem;
        this.moneyText = moneyText;
        this.levelText = levelText;
        this.upgradeButton = upgradeButton;
        this.maxLevel = maxLevel;
        this.upgradeButtonbutton = upgradeButtonbutton;
        this.titleText = titleText;
    }
    public string name;
    public float level;
    public float cost;
    public bool haveItem;
    public TMP_Text moneyText;
    public TMP_Text levelText;
    public TMP_Text titleText;
    public Image upgradeButton;
    public float maxLevel;
    public Button upgradeButtonbutton;
}

public class MiniShopScript : MonoBehaviour
{
    public GameObject HUD;
    public GameObject ShopMenu;
    public bool touchingShop = false;
    public UpgradeClass laser = new UpgradeClass ("laser", 0, 50, true, null, null, null, 15, null, null);
    public UpgradeClass bomb = new UpgradeClass("bomb", 0, 60, false, null, null, null, 15, null, null);
    public UpgradeClass hull = new UpgradeClass("Steel Hull", 0, 60, true, null, null, null, 6, null, null);
    private PlayerController playerShipController;
    public TMP_Text replyText;
    private bool shopOpen;
    public TMP_Text moneyText;
    public GameObject UpgradeList;
    public GameObject UpgradeSlotPrefab;
    public Transform UpgradelistContentTransform;
    public List<UpgradeClass> upgradeClassList = new ();
    public Transform ContentHolder;

    private void Start()
    {
        playerShipController = FindObjectOfType<PlayerController>();
        if (laser.haveItem)
        {
            CreateUpgradeSlot(laser);
        }
        if (hull.haveItem)
        {
            CreateUpgradeSlot(hull);
        }
        upgradeClassList.Add(bomb);
        upgradeClassList.Add(hull);
        upgradeClassList.Add(laser);
    }
    void CreateUpgradeSlot(UpgradeClass item)
    {

        GameObject itemGameobject = Instantiate(UpgradeSlotPrefab, UpgradelistContentTransform);
        item.titleText = itemGameobject.transform.Find("Name").GetComponent<TMP_Text>();
        item.moneyText = itemGameobject.transform.Find("Cost").GetComponent<TMP_Text>();
        item.levelText = itemGameobject.transform.Find("Level").GetComponent<TMP_Text>();
        item.upgradeButton = itemGameobject.transform.Find("UpgradeButton").GetComponent<Image>();
        item.upgradeButtonbutton = itemGameobject.transform.Find("UpgradeButton").GetComponent<Button>();
        item.titleText.text = item.name;
        item.moneyText.text = item.cost.ToString();
        item.levelText.text = "Level: " + item.level;
        item.upgradeButtonbutton.onClick.AddListener(delegate {UpgradeItem(item);});
    }
    void Update()
    {
        if (shopOpen)
        {
            foreach (UpgradeClass upgradeClass in upgradeClassList)
            {
                if (upgradeClass.haveItem)
                {
                    ManageUpgradesItem(upgradeClass);
                }
            }
        }

        /*moneyText.text = "Money: " + playerShipController.money;*/
        /*if (Input.GetKeyDown(KeyCode.E) && touchingShop && !shopOpen)
        {
            OpenShop();
        }
        else if (Input.GetKeyDown(KeyCode.E) && shopOpen)
        {
            CloseShop();
        }*/
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            touchingShop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            touchingShop = false; 
        }
    }
    void OpenShop()
    {
        shopOpen = true;
        ShopMenu.SetActive(true);
        HUD.SetActive(false);
        foreach (Transform child in ContentHolder)
        {
            Destroy(child.gameObject);
        }
        foreach (UpgradeClass upgradeToCreate in upgradeClassList)
        {
            if (upgradeToCreate.haveItem) 
            { 
                CreateUpgradeSlot(upgradeToCreate);
            }
        }
    }
    void CloseShop()
    {
        shopOpen = false;
        ShopMenu.SetActive(false);
        HUD.SetActive(true);
    }
    public void UpgradeItem(UpgradeClass item)
    {
        item.level += 1;
        playerShipController.money -= item.cost;
        item.cost += item.level * 15;
        replyText.text = "Upgraded " + item.name + " to level: " + item.level;
    }
    void ManageUpgradesItem(UpgradeClass item)
    {
        if (item.level < item.maxLevel)
        {
            item.levelText.text = "Level: " + item.level;
        }
        item.moneyText.text = item.cost.ToString();
        if (playerShipController.money < item.cost)
        {
            item.moneyText.color = Color.red;
            item.upgradeButton.color = Color.red;
            item.upgradeButtonbutton.interactable = false;
        }
        if (playerShipController.money >= item.cost)
        {
            item.moneyText.color = Color.yellow;
            item.upgradeButton.color = Color.white;
            item.upgradeButtonbutton.interactable = true;
        }
        if (item.level >= item.maxLevel)
        {
            item.upgradeButton.color = Color.red;
            item.levelText.text = "Level: MAX";
            item.upgradeButtonbutton.interactable = false;
        }
    }
}
