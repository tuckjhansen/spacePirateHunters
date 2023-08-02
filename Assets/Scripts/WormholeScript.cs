using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using System.Reflection;

public class WormholeScript : MonoBehaviour
{
    private PlayerController playerShipController;
    [SerializeField] private GameObject destination;
    [SerializeField] private GameObject Hub;
    [SerializeField] private string portalName;
    [SerializeField] private GameObject HUD;
    public VideoPlayer loadingScenePlayer;
    private SaveScript saveScript;
    private TMP_Text areaText;
    [SerializeReference] private InputActionReference returnAction;
    [SerializeField] private GameObject MercuryCompletedDestination;
    [SerializeField] private GameObject VenusCompletedDestination;
    [SerializeField] private GameObject EarthCompletedDestination;
    [SerializeField] private GameObject arenaWormhole;
    private GameObject area;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == playerShipController.gameObject.name)
        {
            Teleport(area, destination.transform.parent.transform.parent.gameObject);
            areaText.text = portalName.Split("-")?[0]?.ToString();
        }
    }
    void Start()
    {
        playerShipController = FindObjectOfType<PlayerController>();
        saveScript = FindObjectOfType<SaveScript>();
        GameObject areaTextGameobject = GameObject.Find("AreaText");
        areaText = areaTextGameobject.GetComponent<TMP_Text>();
        if (gameObject.name == "CampaignWormhole")
        {
            area = arenaWormhole.transform.parent.transform.parent.gameObject;
        }
        else
        {
            area = transform.parent.transform.parent.gameObject;
        }
    }
    private void Update()
    {
        if (!CommandScript.IsPaused)
        {
            /*deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();*/
            FindDestination();
            if (returnAction.action.ReadValue<float>() != 0 && portalName != "Arena-portal" && portalName != "Sun-portal" && saveScript.level != "Tutorial")
            {
                Teleport(transform.parent.transform.parent.gameObject, Hub.transform.parent.transform.parent.gameObject, true);
                areaText.text = "Hub";
            }
        }
    }
    void FindDestination()
    {
        if (gameObject.name == "CampaignWormhole")
        {
            if (saveScript.level == "Sun")
            {
               
            }
            else if (saveScript.level == "Mercury")
            {
                destination = MercuryCompletedDestination;
            }
            else if (saveScript.level == "Venus")
            {
                destination = VenusCompletedDestination;
            }
            else if (saveScript.level == "Earth")
            {
                destination = EarthCompletedDestination;
            }
        }
    }

    void Teleport(GameObject parent, GameObject newArea, bool returning = false)
    {
        loadingScenePlayer.enabled = true;
        if (returning)
        {
            StartCoroutine(LoadWait(newArea, parent, true));
        }
        else
        {
            StartCoroutine(LoadWait(newArea, parent));
        }
    }
    IEnumerator LoadWait(GameObject newArea, GameObject parent, bool returning = false)
    {
        HUD.SetActive(false);
        yield return new WaitForSeconds(.7f);
        newArea.SetActive(true);
        while (!newArea.activeInHierarchy)
        {
            yield return null;
        }
        if (returning)
        {
            GameObject campaignWormhole = GameObject.Find("CampaignWormhole");
            SpriteRenderer spriteRenderer = campaignWormhole.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
            playerShipController.transform.position = new Vector2(Hub.transform.position.x, Hub.transform.position.y + 30);
        }
        else
        {
            if (gameObject.name == "CampaignWormhole")
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.enabled = false;
            }
            playerShipController.transform.position = destination.transform.position;
            BasicStationScript newStation = newArea.GetComponentInChildren<BasicStationScript>();
            if (newStation != null)
            {
                newStation.InitializeScript();
            }
            else
            {
                ArenaScript arenaStation = newArea.GetComponentInChildren<ArenaScript>();
                if (arenaStation != null)
                {
                    arenaStation.InitializeScript();
                }
            }
        }
        StartCoroutine(saveScript.SaveUser());
        yield return new WaitForSeconds(.5f);
        loadingScenePlayer.enabled = false;
        HUD.SetActive(true);
        parent.SetActive(false); 
        if (returning)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.SetActive(false);
            }
        }  // nothing after this in LoadWait Coroutine
    }
}


