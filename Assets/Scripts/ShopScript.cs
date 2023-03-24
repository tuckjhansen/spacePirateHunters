using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
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
    public ItemClass bomb = new ItemClass("bomb", "Shoots a slow projectile that targets enemies doing a lot of damage", 180, false, null, null, null, null, null);
    public Transform itemList;
    public GameObject itemPrefab;
    public GameObject itemListGameobject;
    private PlayerShipController playerShipController;
    public TMP_Text moneyText;
    public GameObject worldItemPrefab;
    void Start()
    {
        playerShipController = FindObjectOfType<PlayerShipController>();
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
        item.boughtItem = true;
        CloseShop();
        Instantiate(worldItemPrefab, transform);
    }

    void Update()
    {
        moneyText.text = "Money: " + playerShipController.money;
        if (touchingPlayer && Input.GetKeyDown(KeyCode.E) && !shopOpen) 
        {
            OpenShop();
        }
        else if (Input.GetKeyDown(KeyCode.E) && shopOpen)
        {
            CloseShop();
        }
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
