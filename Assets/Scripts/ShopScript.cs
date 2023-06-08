using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemClass
{
    public ItemClass(string name, string description, float cost, bool boughtItem, TMP_Text titleText, TMP_Text descriptionText, TMP_Text costText, Button buyItemButton, Image buyItemButtonImage) 
    { 
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.boughtItem = boughtItem;
        this.titleText = titleText;
        this.descriptionText = descriptionText;
        this.costText = costText;
        this.buyItemButton = buyItemButton;
        this.buyItemButtonImage = buyItemButtonImage;
    }  

    public string name;
    public string description;
    public float cost;
    public bool boughtItem;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public Button buyItemButton;
    public Image buyItemButtonImage;
}

public class ShopScript : MonoBehaviour
{
    
    public bool touchingPlayer = false;
    public GameObject ShopUI;
    public GameObject HUD;
    private bool shopOpen = false;
    public ItemClass bomb = new ("bomb", "Shoots a slow projectile that targets enemies doing a lot of damage", 180, false, null, null, null, null, null);
    public Transform itemList;
    public GameObject itemPrefab;
    public GameObject itemListGameobject;
    private PlayerController playerShipController;
    public TMP_Text moneyText;
    public Item laserItem;
    public Item bombItem;
    public Item steelHullItem;
    private Inventory inventory;
    public List<Item> Itemslist = new ();
    private MiniShopScript miniShopScript;

    void Start()
    {
        playerShipController = FindObjectOfType<PlayerController>();
        miniShopScript = FindObjectOfType<MiniShopScript>();
        inventory = FindObjectOfType<Inventory>();
        Itemslist.Add(laserItem);
        Itemslist.Add(bombItem);
        Itemslist.Add(steelHullItem);
        foreach (Item item in Itemslist) 
        { 
            if (item.haveItem)
            {
                inventory.AddItem(item);
            }
        }
    }
    void CreateItemSlots(ItemClass item)
    {
        GameObject itemGameobject = Instantiate(itemPrefab, itemList);
        item.titleText = itemGameobject.transform.Find("TitleText").GetComponent<TMP_Text>();
        item.descriptionText = itemGameobject.transform.Find("DescriptionText").GetComponent<TMP_Text>();
        item.costText = itemGameobject.transform.Find("CostText").GetComponent<TMP_Text>();
        item.buyItemButton = itemGameobject.transform.Find("BuyButton").GetComponent<Button>();
        item.buyItemButtonImage = itemGameobject.transform.Find("BuyButton").GetComponent<Image>();
        item.titleText.text = item.name;
        item.descriptionText.text = item.description;
        item.costText.text = item.cost.ToString();
        item.buyItemButton.onClick.AddListener(delegate{BuyItem(item);});
    }
    void BuyItem(ItemClass item)
    {
        foreach (Item iteminItemList in Itemslist)
        {
            if (iteminItemList.itemName == item.name)
            {
                item.boughtItem = true;
                playerShipController.money -= item.cost;
                /*foreach (Weapon weapon in playerShipController.attachmentWeaponList)
                {
                    if (weapon.name == item.name)
                    {
                        weapon.haveWeapon = true;
                    }
                }*/
                foreach (UpgradeClass weapon in miniShopScript.upgradeClassList)
                {
                    if (weapon.name == item.name)
                    {
                        weapon.haveItem = true;
                    }
                }
                CloseShop();
                inventory.AddItem(iteminItemList);
                break;
            }
            else
            {
                Debug.LogWarning("Error in finding item. Item does not exist. (Shopscript.C#) Item parameter name: " + item.name + " Item Item name: " + iteminItemList.itemName);
            }
        }

    }

    void Update()
    {
        moneyText.text = "Money: " + playerShipController.money;
        /*if (touchingPlayer && Input.GetKeyDown(KeyCode.E) && !shopOpen) 
        {
            OpenShop();
        }
        else if (Input.GetKeyDown(KeyCode.E) && shopOpen)
        {
            CloseShop();
        }*/
        if (shopOpen)
        {
            if (!bomb.boughtItem)
            {
                if (!bomb.boughtItem)
                {
                    ManageItems(bomb);
                }
            }
        }
    }
    void ManageItems(ItemClass item)
    {
        if (playerShipController.money < item.cost)
        {
            item.costText.color = Color.red;
            item.buyItemButtonImage.color = Color.red;
            item.buyItemButton.interactable = false;
        }
        if (playerShipController.money >= item.cost)
        {
            item.costText.color = Color.yellow;
            item.buyItemButtonImage.color = Color.white;
            item.buyItemButton.interactable = true;
        }
    }
    void OpenShop()
    {
        if (!bomb.boughtItem)
        {
            CreateItemSlots(bomb);
        }
        ShopUI.SetActive(true);
        HUD.SetActive(false);
        shopOpen = true;
    }
    void CloseShop()
    {
        ShopUI.SetActive(false);
        HUD.SetActive(true);
        shopOpen = false;
        foreach (Transform child in itemList) 
        {
            Destroy(child.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            touchingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            touchingPlayer = false;
        }
    }
}
