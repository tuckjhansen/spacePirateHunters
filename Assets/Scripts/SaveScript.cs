using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using System.Collections.Generic;

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
public class SaveScript : MonoBehaviour
{
    public string username;
    /*[HideInInspector]*/ public string level = "Hub";
    [SerializeField] private GameObject savingSymbol;
    private MiniShopScript miniShopScript;
    private Inventory inventoryScript;
    private ShopScript shopScript;
    private PlayerController playerController;
    public UpgradeClass laser = new ("laser", 0, 50, true, null, null, null, 15, null, null);
    public UpgradeClass bomb = new ("bomb", 0, 60, false, null, null, null, 15, null, null);
    public UpgradeClass hull = new ("Steel Hull", 0, 60, true, null, null, null, 6, null, null);

    private async void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        miniShopScript = FindObjectOfType<MiniShopScript>();
        inventoryScript = FindObjectOfType<Inventory>();
        shopScript = FindObjectOfType<ShopScript>();
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public class UpdateJson
    {

        public string username;
        public string action;
        public SaveData saveData;
    }

    public class SaveData
    {
        public string campaignLevel;
        public float maxArenaLevel;
        public float money;
        public bool haveBomb;
        public float laserLevel;
        public float bombLevel;
        public float steelHullLevel;
    }

    public IEnumerator SaveUser()
    {
        savingSymbol.SetActive(true);
        Debug.Log(username + "username in AreaScript");
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users";

        UpdateJson updateData = new()
        {
            username = username,
            action = "updateUser"
        };
        SaveData saveData = new()
        {
            campaignLevel = level,
            money = playerController.money,
            haveBomb = playerController.bombWeaponAttachment.haveWeapon,
            bombLevel = bomb.level,
            laserLevel = laser.level,
            steelHullLevel = hull.level
        };
        updateData.saveData = saveData;
        string json = JsonConvert.SerializeObject(updateData);
        UnityWebRequest uwr = UnityWebRequest.Post(uri, json);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            savingSymbol.SetActive(true);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text + " Status: " + uwr.responseCode);
            savingSymbol.SetActive(true);
        }
        _ = UCS(saveData, updateData);
    }
    private async Task UCS(SaveData saveData, UpdateJson updateJson)
    {
        Dictionary<string, object> saveDataDictionary = new() { {"username", updateJson.username}, {"SaveData", saveData} };
        await CloudSaveService.Instance.Data.ForceSaveAsync(saveDataDictionary);
    }
}
