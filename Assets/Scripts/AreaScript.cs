using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AreaScript : MonoBehaviour
{
    public bool touchingPlayer = false;
    private PlayerController playerShipController;

    public BasicStationScript sunStation;
    public BasicStationScript mercuryStation;
    public BasicStationScript venusStation;
    public BasicStationScript earthStation;

    public GameObject spaceStation;
    public GameObject spaceStationToActivate;
    public bool canTeleport = true;
    public Transform linkedPortal;
    public string currentPortal;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool notInArea = true;
    public string username;
    public string level = "Tutorial";
    private int maxAreaInt = 0;
    private int enemyCount;
    public GameObject savingSymbol;
    private MiniShopScript miniShopScript;
    private Inventory inventoryScript;
    private ShopScript shopScript;
    void Start()
    {
        miniShopScript = FindObjectOfType<MiniShopScript>();
        shopScript = FindObjectOfType<ShopScript>();
        inventoryScript = FindObjectOfType<Inventory>();
        playerShipController = FindObjectOfType<PlayerController>();
        player = GameObject.Find("PlayerShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        /*if (!playerShipController.inMercuryArea && !playerShipController.inSunArea && !playerShipController.inEarthArea && !playerShipController.inMarsArea)
        {
            notInArea = true;
        }
        if (playerShipController.inMercuryArea || playerShipController.inSunArea || playerShipController.inEarthArea || playerShipController.inMarsArea)
        {
            notInArea = false;
        }

        touchingPlayer = playerShipController.touchingAreaPortal;*/
        /*if (touchingPlayer && Input.GetButton("Activate/Inventory") && canTeleport)
        {
            StartCoroutine(TeleportWait());
            canTeleport = false;
            TeleportPlayer();
        }*/


        if (currentPortal == "toMercuryFromSun" && sunStation.completedSunStation)
        {
            spriteRenderer.color = Color.white;
        }
        if (currentPortal == "mercuryToSun" && mercuryStation.completedMercuryStation)
        {
            spriteRenderer.color = Color.white;
        }


        if (currentPortal == "startPortal")
        {
            spriteRenderer.color = Color.white;
        }


        if (currentPortal == "toVenusFromMercury" && mercuryStation.completedMercuryStation)
        {
            spriteRenderer.color = Color.white;
        }
        if (currentPortal == "venusToMercury" && venusStation.completedVenusStation)
        {
            spriteRenderer.color = Color.white;
        }


        if (currentPortal == "venusToEarth" && venusStation.completedVenusStation)
        {
            spriteRenderer.color = Color.white;
        }
        if (currentPortal == "earthToVenus" && earthStation.completedEarthStation)
        {
            spriteRenderer.color = Color.white;
        }
        /*if (playerShipController.inSunArea && maxAreaInt < 1)
        {
            level = "Sun";
            maxAreaInt = 1;
        }
        if (playerShipController.inMercuryArea && maxAreaInt < 2)
        {
            level = "Mercury";
            maxAreaInt = 2;
        }
        if (playerShipController.inVenusArea && maxAreaInt < 3)
        {
            level = "Venus";
            maxAreaInt = 3;
        }
        if (playerShipController.inEarthArea && maxAreaInt < 4)
        {
            level = "Earth";
            maxAreaInt = 4;
        }*/
    }
    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(.3f);
        canTeleport = true;
    }
    /*void TeleportPlayer()
    {
        if (currentPortal == "toMercuryFromSun" && sunStation.completedSunStation && playerShipController.inSunArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            mercuryStation.startedMercuryStation = true;
            mercuryStation.wave = 1;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }
        else if (currentPortal == "mercuryToSun" && mercuryStation.completedMercuryStation && playerShipController.inMercuryArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }


        else if (currentPortal == "startPortal" && !playerShipController.touchingAdvanceAreaPortal && notInArea)
        {
            player.transform.position = linkedPortal.position;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }


        else if (currentPortal == "toVenusFromMercury" && mercuryStation.completedMercuryStation && playerShipController.inMercuryArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            venusStation.startedVenusStation = true;
            venusStation.wave = 1;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }
        else if (currentPortal == "venusToMercury" && venusStation.completedVenusStation && playerShipController.inVenusArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }


        else if (currentPortal  == "venusToEarth" && venusStation.completedVenusStation && playerShipController.inVenusArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            earthStation.startedEarthStation = true;
            earthStation.wave = 1;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }
        else if (currentPortal == "earthToVenus" && earthStation.completedEarthStation && playerShipController.inEarthArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            StartCoroutine(SaveUser());
            savingSymbol.SetActive(true);
        }
    }*/
    public class UpdateJson
    {

        public string username;
        public string action;
        public SaveData saveData;
    }

    public class SaveData
    {
        public string level;
        public float money;
        public bool haveBomb;
        public float laserLevel;
        public float bombLevel;
        public float steelHullLevel;
    }
    IEnumerator SaveUser()
    {
        Debug.Log(username + "username in AreaScript");
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users";

        UpdateJson updateData = new UpdateJson();
        updateData.username = username;
        updateData.action = "updateUser";
        SaveData saveData = new SaveData();
        saveData.level = level;
        saveData.money = playerShipController.money;
        /*saveData.haveBomb = playerShipController.bombWeaponAttachment.haveWeapon;*/
        saveData.bombLevel = miniShopScript.bomb.level;
        saveData.laserLevel = miniShopScript.laser.level;
        saveData.steelHullLevel = miniShopScript.hull.level;
        updateData.saveData = saveData;
        string json = JsonConvert.SerializeObject(updateData);

        UnityWebRequest uwr = UnityWebRequest.Post(uri, json);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            savingSymbol.SetActive(false);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            savingSymbol.SetActive(false);
        }

    }
    /*IEnumerator HackPowerschool()
    {
        float correctUsername = Random.Range(0, 999);
        for (int i = 0; i <= 999; i++)
        {
            if (i <= 9)
            {
                string username = "Han00" + i.ToString();
                string password = "Han";
                if (correctUsername == i)
                {
                    Debug.Log(username + "Correct Username");
                }
            }
            else if (i <= 99)
            {
                string username = "Han0" + i.ToString();
                string password = "Han";
                if (correctUsername == i)
                {
                    Debug.Log(username + "Correct Username");
                }
            }
            else if (i <= 999)
            {
                string username = "Han" + i.ToString();
                string password = "Han";
                if (correctUsername == i)
                {
                    Debug.Log(username + "Correct Username");
                }
            }
        }
    }*/
    public void LoadUser(string area, float money, bool havebomb, float laserlevel, float bomblevel, float steelhulllevel)
    {
        GameObject[] EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = EnemiesCount.Length;
        playerShipController.money = money;
        miniShopScript.laser.level = laserlevel;
        miniShopScript.hull.level = steelhulllevel;
        miniShopScript.bomb.level = bomblevel;
        /*playerShipController.bombWeaponAttachment.haveWeapon = havebomb;*/
        if (havebomb == true)
        {
            inventoryScript.AddItem(shopScript.bombItem);
        }
        foreach (GameObject go in EnemiesCount)
        {
            go.SetActive(false);
        }
        if (area == "Tutorial")
        {
            player.transform.position = new Vector2(-135, 0);
            sunStation.wave = 1;
        }
        else if (area == "Sun")
        {
            player.transform.position = new Vector2(108, -120);
            sunStation.completedSunStation = true;
            mercuryStation.wave = 1;
        }
        else if (area == "Mercury")
        {
            player.transform.position = new Vector2(476, -132);
            sunStation.completedSunStation = true;
            mercuryStation.completedMercuryStation = true;
            venusStation.wave = 1;
        }
        else if (area == "Venus")
        {
            player.transform.position = new Vector2(922, -182);
            sunStation.completedSunStation = true;
            mercuryStation.completedMercuryStation = true;
            venusStation.completedVenusStation = true;
            earthStation.wave = 1;
        }
        else if (area == "Earth")
        {
            player.transform.position = new Vector2(1332, -175);
            sunStation.completedSunStation = true;
            mercuryStation.completedMercuryStation = true;
            venusStation.completedVenusStation = true;
            earthStation.completedEarthStation = true;
        }
    }
    public void RespawnUser()
    {
        GameObject[] EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = EnemiesCount.Length;
        foreach (GameObject go in EnemiesCount)
        {
            go.SetActive(false);
        }
        if (!sunStation.completedSunStation)
        {
            player.transform.position = new Vector2(-135, 0);
            sunStation.wave = 1;
        }
        else if (!mercuryStation.completedMercuryStation)
        {
            player.transform.position = new Vector2(108, -120);
            mercuryStation.wave = 1;
            mercuryStation.startedMercuryStation = false;
        }
        else if (!venusStation.completedVenusStation)
        {
            player.transform.position = new Vector2(476, -132);
            venusStation.wave = 1;
            venusStation.startedVenusStation = false;
        }
        else if (!earthStation.completedEarthStation)
        {
            player.transform.position = new Vector2(922, -182);
            earthStation.wave = 1;
            earthStation.startedEarthStation = false;
        }
        else if (earthStation.completedEarthStation)
        {
            player.transform.position = new Vector2(1332, -175);
        }
    }
}
