using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class MiniShopScript : MonoBehaviour
{
    public GameObject HUD;
    public GameObject ShopMenu;
    public bool touchingShop = false;
    private PlayerController playerShipController;
    public TMP_Text replyText;
    private bool shopOpen;
    public TMP_Text moneyText;
    public GameObject UpgradeList;
    public GameObject UpgradeSlotPrefab;
    public Transform UpgradelistContentTransform;
    public List<UpgradeClass> upgradeClassList = new ();
    public Transform ContentHolder;
    private SaveScript saveScript;
    [SerializeField]
    private InputActionReference use;
    private bool shopOpenable = true;

    private void Start()
    {
        saveScript = FindObjectOfType<SaveScript>();
        playerShipController = FindObjectOfType<PlayerController>();
        if (saveScript.laser.haveItem)
        {
            CreateUpgradeSlot(saveScript.laser);
        }
        if (saveScript.hull.haveItem)
        {
            CreateUpgradeSlot(saveScript.hull);
        }
        upgradeClassList.Add(saveScript.bomb);
        upgradeClassList.Add(saveScript.hull);
        upgradeClassList.Add(saveScript.laser);
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

        moneyText.text = "Money: " + playerShipController.money;
        if (use.action.ReadValue<float>() != 0 && touchingShop && !shopOpen && shopOpenable)
        {
            shopOpenable = false;
            OpenShop();
            StartCoroutine(OpenWait());
        }
        else if (use.action.ReadValue<float>() != 0 && shopOpen && shopOpenable)
        {
            shopOpenable = false;
            CloseShop();
            StartCoroutine(OpenWait());
        }
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchingShop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
    IEnumerator OpenWait()
    {
        yield return new WaitForSeconds(.3f);
        shopOpenable = true;
    }
}
