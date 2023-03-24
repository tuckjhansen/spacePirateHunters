using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AreaScript : MonoBehaviour
{
    public bool touchingPlayer = false;
    private PlayerShipController playerShipController;

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
    void Start()
    {
        playerShipController = FindObjectOfType<PlayerShipController>();
        player = GameObject.Find("PlayerShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!playerShipController.inMercuryArea && !playerShipController.inSunArea && !playerShipController.inEarthArea && !playerShipController.inMarsArea)
        {
            notInArea = true;
        }
        if (playerShipController.inMercuryArea || playerShipController.inSunArea || playerShipController.inEarthArea || playerShipController.inMarsArea)
        {
            notInArea = false;
        }

        touchingPlayer = playerShipController.touchingAreaPortal;
        if (touchingPlayer && Input.GetButton("Activate/Inventory") && canTeleport)
        {
            StartCoroutine(TeleportWait());
            canTeleport = false;
            TeleportPlayer();
            StartCoroutine(SaveUser());
        }


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
        if (playerShipController.inSunArea && maxAreaInt < 1)
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
        }
    }
    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(.3f);
        canTeleport = true;
    }
    void TeleportPlayer()
    {
        if (currentPortal == "toMercuryFromSun" && sunStation.completedSunStation && playerShipController.inSunArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            mercuryStation.startedMercuryStation = true;
            mercuryStation.wave = 1;
        }
        else if (currentPortal == "mercuryToSun" && mercuryStation.completedMercuryStation && playerShipController.inMercuryArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
        }


        else if (currentPortal == "startPortal" && !playerShipController.touchingAdvanceAreaPortal && notInArea)
        {
            player.transform.position = linkedPortal.position;
        }


        else if (currentPortal == "toVenusFromMercury" && mercuryStation.completedMercuryStation && playerShipController.inMercuryArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            venusStation.startedVenusStation = true;
            venusStation.wave = 1;
        }
        else if (currentPortal == "venusToMercury" && venusStation.completedVenusStation && playerShipController.inVenusArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
        }


        else if (currentPortal  == "venusToEarth" && venusStation.completedVenusStation && playerShipController.inVenusArea && playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
            earthStation.startedEarthStation = true;
            earthStation.wave = 1;
        }
        else if (currentPortal == "earthToVenus" && earthStation.completedEarthStation && playerShipController.inEarthArea && !playerShipController.touchingAdvanceAreaPortal)
        {
            player.transform.position = linkedPortal.position;
        }
    }
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
    }
    IEnumerator SaveUser()
    {
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users";

        UpdateJson updateData = new UpdateJson();
        updateData.username = username;
        updateData.action = "updateUser";
        SaveData saveData = new SaveData();
        saveData.level = level;
        saveData.money = playerShipController.money;
        updateData.saveData = saveData;
        string json = JsonConvert.SerializeObject(updateData);

        UnityWebRequest uwr = UnityWebRequest.Post(uri, json);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

    }
    public void LoadUser(string area, float money)
    {
        GameObject[] EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = EnemiesCount.Length;
        playerShipController.money = money;
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
        }
        if (sunStation.completedSunStation)
        {
            player.transform.position = new Vector2(108, -120);
        }
        if (mercuryStation.completedMercuryStation)
        {
            player.transform.position = new Vector2(476, -132);
        }
        if (venusStation.completedVenusStation)
        {
            player.transform.position = new Vector2(922, -182);
        }
        if (earthStation.completedEarthStation)
        {
            player.transform.position = new Vector2(1332, -175);
        }   
    }
}
