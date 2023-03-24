using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public bool inventoryOpen = false;
    private bool inventoryOpenable = true;
    public GameObject inventory;
    public GameObject HUD;
    private PlayerShipController playerShipController;
    private MiniShopScript miniShopScript;
    public static Inventory Instance;
    public Transform ItemContent;
    public GameObject InventoryItem;
    private ShopScript shopScript;
    private void Awake()
    {
        Instance = this; 
    }

    public void Add(Item item)
    {
        items.Add(item);
    }
    public void Remove(Item item)
    {
        items.Remove(item);
    }
    public void ListItems()
    {
        foreach (var item in items) 
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.name;
            itemIcon.sprite = item.icon;
        }
    }
    private void Start()
    {
        playerShipController = FindObjectOfType<PlayerShipController>();
        miniShopScript = FindObjectOfType<MiniShopScript>();
        shopScript = FindObjectOfType<ShopScript>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inventoryOpen && inventoryOpenable && !playerShipController.touchingAdvanceAreaPortal && !playerShipController.touchingAreaPortal && !miniShopScript.touchingShop && !shopScript.touchingPlayer)
        {
            OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.E) && inventoryOpen && inventoryOpenable && !playerShipController.touchingAdvanceAreaPortal && !playerShipController.touchingAreaPortal)
        {
            CloseInventory();
        }
    }
    public void OpenInventory()
    {
        inventoryOpen = true;
        inventoryOpenable = false;
        HUD.SetActive(false);
        inventory.SetActive(true);
        ListItems();
        StartCoroutine(InventoryOpenWait());
        Time.timeScale = 0.0001f;
    }
    public void CloseInventory()
    {
        inventoryOpen = false;
        inventoryOpenable = false;
        HUD.SetActive(true);
        inventory.SetActive(false);
        StartCoroutine(InventoryOpenWait());
        Time.timeScale = 1f;
    }
    
    IEnumerator InventoryOpenWait()
    {
        yield return new WaitForSeconds(.1f);
        inventoryOpenable = true;
    }
}
