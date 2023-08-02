using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

interface IWaveConfig
{

    float numberOfStandardEnemies { get; set; }
    float numberOfBomberEnemies { get; set; }
    float numberOfEngineerEnemies { get; set; }

}
class MyWaveConfig : IWaveConfig
{
    public float numberOfStandardEnemies { get; set; }
    public float numberOfBomberEnemies { get; set; }
    public float numberOfEngineerEnemies { get; set; }
}


public class BasicStationScript : MonoBehaviour
{
    private string stationName;
    public float wave = 1;

    public GameObject standardEnemyPrefab;
    public GameObject bomberEnemyPrefab;
    public GameObject engineerEnemyPrefab;
    public Transform DockBayTransform;
    private SpriteRenderer spriteRendererSpaceStation;
    public Sprite goodSpaceStationSprite;
    private EdgeCollider2D edgeCollider;
    private BoxCollider2D healCollider;
    private bool completedStation;
    
    public TMP_Text waveText;
    private GameObject portal;
    private SaveScript saveScript;
    public List<GameObject> enemies = new();
    private void Start()
    {
        stationName = gameObject.name.Split("Station")[0];
        spriteRendererSpaceStation = GetComponent<SpriteRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        healCollider = GetComponent<BoxCollider2D>();
        portal = FindObjectOfType<WormholeScript>().gameObject;
        saveScript = FindObjectOfType<SaveScript>();
    }
    public void InitializeScript()
    {
        wave = 1;
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
    }

    
    void Update()
    {
        if (completedStation)
        {
            BoxCollider2D teleporter = portal.GetComponent<BoxCollider2D>();
            teleporter.isTrigger = true;
            SpriteRenderer wormholeStable = portal.GetComponent<SpriteRenderer>();
            wormholeStable.color = Color.white;
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }

        if (stationName == "Space" && !completedStation)
        {
            if (wave >= 5)
            {
                wave = 0;
            }
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1) { 
                wave++;
                MyWaveConfig waveConfig = new ()
                {
                    numberOfStandardEnemies = 3,
                    numberOfBomberEnemies = 0,
                    numberOfEngineerEnemies = 0
                };
                HandleWave(waveConfig);
            }
            
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new ()
                {
                    numberOfStandardEnemies = 4,
                    numberOfBomberEnemies = 1,
                    numberOfEngineerEnemies = 0
                };
                HandleWave(waveConfig);

            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new ()
                {
                    numberOfStandardEnemies = 3,
                    numberOfBomberEnemies = 1,
                    numberOfEngineerEnemies = 1
                };
                HandleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                foreach (GameObject go in enemies)
                {
                    Destroy(go);
                }
                enemies.Clear();
                spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
                spriteRendererSpaceStation.color = Color.white;
                transform.localScale = new Vector3(3, 3, 1);
                edgeCollider.enabled = false;
                healCollider.enabled = true;
                completedStation = true;
                saveScript.level = "Sun";
                wave++;
            }
        }
        else if (stationName == "Mercury" && !completedStation)
        {
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 2;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 2;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 2;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 2;
                HandleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                foreach (GameObject go in enemies)
                {
                    Destroy(go);
                }
                enemies.Clear();
                spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
                spriteRendererSpaceStation.color = Color.white;
                wave++;
                transform.localScale = new Vector3(3, 3, 1);
                edgeCollider.enabled = false;
                healCollider.enabled = true;
                completedStation = true;
                saveScript.level = "Mercury";
            }

        }
        else if (stationName == "Venus" && !completedStation)
        {
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                HandleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                HandleWave(waveConfig);
            }
            else if (wave == 6 && allInactive)
            {
                foreach (GameObject go in enemies)
                {
                    Destroy(go);
                }
                enemies.Clear();
                spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
                spriteRendererSpaceStation.color = Color.white;
                wave++;
                transform.localScale = new Vector3(3, 3, 1);
                edgeCollider.enabled = false;
                healCollider.enabled = true;
                completedStation = true;
                saveScript.level = "Venus";
            }
        }
        else if (stationName == "Earth" && !completedStation)
        {
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                HandleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                HandleWave(waveConfig);
            }
            else if (wave == 6 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 3;
                HandleWave(waveConfig);
            }
            else if (wave == 7 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                HandleWave(waveConfig);
            }
            else if (wave == 8 && allInactive)
            {
                foreach (GameObject go in enemies)
                {
                    Destroy(go);
                }
                enemies.Clear();
                spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
                spriteRendererSpaceStation.color = Color.white;
                wave++;
                transform.localScale = new Vector3(3, 3, 1);
                edgeCollider.enabled = false;
                healCollider.enabled = true;
                completedStation = true;
                saveScript.level = "Earth";
            }
        }
        else if (stationName == "Mars" && !completedStation)
        {
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                HandleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                HandleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                HandleWave(waveConfig);
            }
            else if (wave == 6 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 3;
                HandleWave(waveConfig);
            }
            else if (wave == 7 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                HandleWave(waveConfig);
            }
            else if (wave == 8 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                HandleWave(waveConfig);
            }
            else if (wave == 9 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                HandleWave(waveConfig);
            }
            else if (wave == 10 && allInactive)
            {
                foreach (GameObject go in enemies)
                {
                    Destroy(go);
                }
                enemies.Clear();
                spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
                spriteRendererSpaceStation.color = Color.white;
                wave++;
                transform.localScale = new Vector3(3, 3, 1);
                edgeCollider.enabled = false;
                healCollider.enabled = true;
                completedStation = true;
                saveScript.level = "Mars";
            }
        }
    }

    void HandleWave(IWaveConfig waveConfig)
    {

        // make loop to create number of enemies
        for (var i = 0; i < waveConfig.numberOfStandardEnemies; i++) { 
            GameObject standardEnemy = Instantiate(standardEnemyPrefab, DockBayTransform.position, Quaternion.identity);
            enemies.Add(standardEnemy);
        }

        for (var i = 0; i < waveConfig.numberOfBomberEnemies; i++)
        {
            GameObject bomberEnemy = Instantiate(bomberEnemyPrefab, DockBayTransform.position, Quaternion.identity);
            enemies.Add(bomberEnemy);
        }

        for (var i = 0; i < waveConfig.numberOfEngineerEnemies; i++)
        {
            GameObject engineerEnemy = Instantiate(engineerEnemyPrefab, DockBayTransform.position, Quaternion.identity);
            enemies.Add(engineerEnemy);
        }
    }
}
