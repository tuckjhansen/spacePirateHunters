using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
    public bool touchingPlayer = false;
    private PlayerShipController playerShipController;
    private BasicStationScript basicStationScript;
    private BasicStationScript basicStationScriptToActivate;
    public GameObject spaceStation;
    public GameObject spaceStationToActivate;
    public bool canTeleport = true;
    public Transform linkedPortal;
    public string currentPortal;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        playerShipController = FindObjectOfType<PlayerShipController>();
        basicStationScript = spaceStation.GetComponent<BasicStationScript>();
        basicStationScriptToActivate = spaceStationToActivate.GetComponent<BasicStationScript>();
        player = GameObject.Find("PlayerShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        touchingPlayer = playerShipController.touchingAreaPortal;
        if (touchingPlayer && Input.GetButton("Activate/Inventory") && canTeleport)
        {
            StartCoroutine(TeleportWait());
            canTeleport = false;
            TeleportPlayer();
        }
        if (currentPortal == "toMercuryFromSun" && basicStationScript.completedSunStation)
        {
            spriteRenderer.color = Color.white;
        }
        if (currentPortal == "mercuryToSun" && basicStationScript.completedMercuryStation)
        {
            spriteRenderer.color = Color.white;
        }
    }
    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(.3f);
        canTeleport = true;
    }
    void TeleportPlayer()
    {
        if (currentPortal == "toMercuryFromSun" && basicStationScript.completedSunStation && playerShipController.inSunArea)
        {
            player.transform.position = linkedPortal.position;
            basicStationScriptToActivate.startedMercuryStation = true;
            basicStationScriptToActivate.wave = 1;
        }
        if (currentPortal == "mercuryToSun" && basicStationScript.completedMercuryStation && playerShipController.inMercuryArea)
        {
            player.transform.position = linkedPortal.position;
        }
    }
}
