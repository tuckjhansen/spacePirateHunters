using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Equipment equippedArmor;
    public Equipment equippedWeapon;
    private bool inventoryOpen = false;
    private bool inventoryOpenable = true;
    public GameObject inventory;
    public GameObject HUD;
    private PlayerShipController playerShipController;
    public class Item
    {
        public string name;
        public Sprite sprite;
    }
    public class Equipment : Item
    {
        public int defense;
        public int attack;
    }
    private void Start()
    {
        playerShipController = FindObjectOfType<PlayerShipController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inventoryOpen && inventoryOpenable && !playerShipController.touchingAdvanceAreaPortal && !playerShipController.touchingAreaPortal)
        {
            inventoryOpen = true;
            inventoryOpenable = false;
            HUD.SetActive(false);
            inventory.SetActive(true);
            StartCoroutine(inventoryOpenWait());
        }
        if (Input.GetKeyDown(KeyCode.E) && inventoryOpen && inventoryOpenable && !playerShipController.touchingAdvanceAreaPortal && !playerShipController.touchingAreaPortal)
        {
            inventoryOpen = false;
            inventoryOpenable = false;
            HUD.SetActive(true);
            inventory.SetActive(false);
            StartCoroutine(inventoryOpenWait());
        }
    }
    /*public void EquipItem(Equipment equipment)
    {
        if (equipment.GetType() == typeof(Armor))
        {
            equippedArmor = equipment;
            Debug.Log("Armor equipped: " + equipment.name);
        }
        else if (equipment.GetType() == typeof(Weapon))
        {
            equippedWeapon = equipment;
            Debug.Log("Weapon equipped: " + equipment.name);
        }
    }

    public void UnequipItem(Equipment equipment)
    {
        if (equipment.GetType() == typeof(Armor) && equippedArmor == equipment)
        {
            equippedArmor = null;
            Debug.Log("Armor unequipped: " + equipment.name);
        }
        else if (equipment.GetType() == typeof(Weapon) && equippedWeapon == equipment)
        {
            equippedWeapon = null;
            Debug.Log("Weapon unequipped: " + equipment.name);
        }
    }*/
    IEnumerator inventoryOpenWait()
    {
        yield return new WaitForSeconds(.1f);
        inventoryOpenable = true;
    }
}
