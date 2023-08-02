using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemClassInIventory
{
    public ItemClassInIventory(Item item, bool itemEquipped)
    {
        this.item = item;
        this.itemEquipped = itemEquipped;
    }
    public Item item;
    public bool itemEquipped;
}
public class Inventory : MonoBehaviour
{
    public List<Item> items = new ();
    public static bool inventoryOpen = false;
    private bool inventoryOpenable = true;
    public GameObject inventory;
    public GameObject HUD;
    private PlayerController playerController;
    private MiniShopScript miniShopScript;
    public Transform ItemContent;
    public GameObject InventoryItem;
    private ShopScript shopScript;
    public Transform weaponPrefabSpawnPoint;
    public Transform armorPrefabSpawnPoint;
    public Transform specialWeaponPrefabSpawnPoint;
    public Transform enginePrefabSpawnPoint;
    [SerializeField] private Image weaponCoolDown;
    [SerializeField] private Image specialWeaponCoolDown;
    [SerializeField] private Image engineCoolDown;

    [SerializeField]
    private InputActionReference use;

    public void AddItem(Item item)
    {
        items.Add(item);
    }
    public void EquipItem(Item item)
    {
        foreach (Item iteminItemsList in items)
        {
            if (item.occupation == iteminItemsList.occupation)
            {
                Transform prefabSpawnPoint = (Transform)this.GetType().GetField(item.occupation + "PrefabSpawnPoint").GetValue(this);
                foreach (Transform child in prefabSpawnPoint)
                {
                    Destroy(child.gameObject);
                }
                GameObject itemEquiped = Instantiate(InventoryItem, prefabSpawnPoint);
                itemEquiped.transform.localScale = new Vector3(.9f, .9f, 1);
                TMP_Text itemName = itemEquiped.transform.Find("ItemName").GetComponent<TMP_Text>();
                Image itemIcon = itemEquiped.transform.Find("Image").GetComponent<Image>();
                itemIcon.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
                if (item.occupation == Item.Occupation.weapon)
                {
                    playerController.weaponEquiped = item.itemName;
                    weaponCoolDown.sprite = item.icon;
                    weaponCoolDown.color = Color.white;
                }
                else if (item.occupation == Item.Occupation.specialWeapon)
                {
                    playerController.specialWeaponEquiped = item.itemName;
                    specialWeaponCoolDown.sprite = item.icon;
                    specialWeaponCoolDown.color = Color.white;
                }
            }
        }
    }

    public void ListItems()
    {
        foreach (Transform iteminInventory in ItemContent)
        {
            Destroy(iteminInventory.gameObject);
        }
        foreach (Item item in items) 
        {
            GameObject itemCreated = Instantiate(InventoryItem, ItemContent);
            TMP_Text itemName = itemCreated.transform.Find("ItemName").GetComponent<TMP_Text>();
            Image itemIcon = itemCreated.transform.Find("Image").GetComponent<Image>();
            Button itemButton = itemCreated.GetComponent<Button>();
            itemButton.onClick.AddListener(delegate { EquipItem(item); });
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        miniShopScript = FindObjectOfType<MiniShopScript>();
        shopScript = FindObjectOfType<ShopScript>();
        foreach (Item item in items)
        {
            if (item.haveItem)
            {
                CreateSlotHoldersForEquipedItems(item);
            }
        }
    }
    void CreateSlotHoldersForEquipedItems(Item item)
    {
        Transform prefabSpawnPoint = (Transform)this.GetType().GetField(item.occupation + "PrefabSpawnPoint").GetValue(this);
        GameObject itemEquipedStart = Instantiate(InventoryItem, prefabSpawnPoint);
        TMP_Text itemName = itemEquipedStart.transform.Find("ItemName").GetComponent<TMP_Text>();
        Image itemIcon = itemEquipedStart.transform.Find("Image").GetComponent<Image>();
        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
        itemIcon.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        itemEquipedStart.transform.localScale = new Vector3(.9f, .9f, 1);
    }
    private void Update()
    {
        if (!CommandScript.IsPaused)
        {
            if (use.action.ReadValue<float>() != 0 && !inventoryOpen && inventoryOpenable && !miniShopScript.touchingShop && !shopScript.touchingPlayer && playerController.health > 0)
            {
                OpenInventory();
            }
            if (use.action.ReadValue<float>() != 0 && inventoryOpen && inventoryOpenable)
            {
                CloseInventory();
            }
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
    }
    public void CloseInventory()
    {
        inventoryOpen = false;
        inventoryOpenable = false;
        HUD.SetActive(true);
        inventory.SetActive(false);
        StartCoroutine(InventoryOpenWait());
    }
    
    IEnumerator InventoryOpenWait()
    {
        yield return new WaitForSeconds(.3f);
        inventoryOpenable = true;
    }
}
