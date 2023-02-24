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
    public string savedata = "Tutorial";
    private int maxAreaInt = 0;
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
            savedata = "Sun";
            maxAreaInt = 1;
        }
        if (playerShipController.inMercuryArea && maxAreaInt < 2)
        {
            savedata = "Mercury";
            maxAreaInt = 2;
        }
        if (playerShipController.inVenusArea && maxAreaInt < 3)
        {
            savedata = "Venus";
            maxAreaInt = 3;
        }
        if (playerShipController.inEarthArea && maxAreaInt < 4)
        {
            savedata = "Earth";
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

    IEnumerator SaveUser()
    {
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users?action=updateUser&username=" + username + "&savedata=" + savedata;
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
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
    public void LoadUser(string area)
    {
        if (area == "Tutorial")
        {
            player.transform.position = new Vector2(-135, 0);
        }
        else if (area == "Sun")
        {
            player.transform.position = new Vector2(108, -120);
            sunStation.completedSunStation = true;
        }
        else if (area == "Mercury")
        {
            player.transform.position = new Vector2(476, -132);
            sunStation.completedSunStation = true;
            mercuryStation.completedMercuryStation = true;
        }
        else if (area == "Venus")
        {
            player.transform.position = new Vector2(922, -182);
            sunStation.completedSunStation = true;
            mercuryStation.completedMercuryStation = true;
            venusStation.completedVenusStation = true;
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
}
