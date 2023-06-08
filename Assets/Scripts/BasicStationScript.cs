using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

interface WaveConfig
{

    float numberOfStandardEnemies { get; set; }
    float numberOfBomberEnemies { get; set; }
    float numberOfEngineerEnemies { get; set; }

}
class MyWaveConfig : WaveConfig
{
    public float numberOfStandardEnemies { get; set; }
    public float numberOfBomberEnemies { get; set; }
    public float numberOfEngineerEnemies { get; set; }
}


public class BasicStationScript : MonoBehaviour
{
    public bool sunStation;
    public bool mercuryStation;
    public bool venusStation;
    public bool earthStation;
    public bool marsStation;
    public bool jupiterStation;
    public bool saturnStation;
    public bool uranusStation;
    public bool neptuneStation;
    public bool startedMercuryStation;
    public bool startedVenusStation;
    public bool startedEarthStation;
    public bool startedMarsStation;
    public bool startedJupiterStation;
    public bool startedSaturnStation;
    public bool startedUranusStation;
    public bool startedNeptuneStation;
    public float wave = 1;

    public GameObject standardEnemyPrefab;
    public GameObject bomberEnemyPrefab;
    public GameObject engineerEnemyPrefab;
    public Transform DockBayTransform;
    private SpriteRenderer spriteRendererSpaceStation;
    public Sprite goodSpaceStationSprite;
    private EdgeCollider2D edgeCollider;
    private BoxCollider2D healCollider;
    public bool completedSunStation = false;
    public bool completedMercuryStation = false;
    public bool completedVenusStation = false;
    public bool completedEarthStation = false;
    public bool completedMarsStation = false;
    public bool completedJupiterStation = false;
    public bool completedSaturnStation = false;
    public bool completedUranusStation = false;
    public bool completedNeptuneStation = false;
    
    public TMP_Text waveText;

    
    public List<GameObject> enemies = new List<GameObject>();
    private void Start()
    {
        spriteRendererSpaceStation = GetComponent<SpriteRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        healCollider = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if (completedSunStation && sunStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedMercuryStation && mercuryStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedVenusStation && venusStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedEarthStation && earthStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedMarsStation && marsStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedJupiterStation && jupiterStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedSaturnStation && saturnStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedUranusStation && uranusStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }
        if (completedNeptuneStation && neptuneStation)
        {
            spriteRendererSpaceStation.sprite = goodSpaceStationSprite;
            spriteRendererSpaceStation.color = Color.white;
            transform.localScale = new Vector3(3, 3, 1);
            edgeCollider.enabled = false;
            healCollider.enabled = true;
        }


        /*if (Input.GetKeyDown(KeyCode.F))
        {
            wave++;
        }*/
        if (sunStation && !completedSunStation)
        {
            if (wave >= 5)
            {
                wave = 0;
            }
            waveText.text = "Wave: " + (wave - 1);
            bool allInactive = enemies.All(enemy => !enemy.activeSelf);
            if (wave == 1) { 
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 0;
                waveConfig.numberOfEngineerEnemies = 0;
                handleWave(waveConfig);
            }
            
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 0;
                handleWave(waveConfig);

            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 1;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
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
                completedSunStation = true;
                wave++;
            }
        }
        else if (mercuryStation && !completedMercuryStation && startedMercuryStation)
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
                handleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 2;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 2;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 2;
                handleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
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
                completedMercuryStation = true;
            }

        }
        else if (venusStation && !completedVenusStation && startedVenusStation)
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
                handleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                handleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                handleWave(waveConfig);
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
                completedVenusStation = true;
            }
        }
        else if (earthStation && !completedEarthStation && startedEarthStation)
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
                handleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                handleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                handleWave(waveConfig);
            }
            else if (wave == 6 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 3;
                handleWave(waveConfig);
            }
            else if (wave == 7 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                handleWave(waveConfig);
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
                completedEarthStation = true;
            }
        }
        else if (marsStation && !marsStation && startedMarsStation)
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
                handleWave(waveConfig);
            }
            else if (wave == 2 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 3 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 3;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 2;
                handleWave(waveConfig);
            }
            else if (wave == 4 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 1;
                handleWave(waveConfig);
            }
            else if (wave == 5 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 3;
                handleWave(waveConfig);
            }
            else if (wave == 6 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 2;
                waveConfig.numberOfEngineerEnemies = 3;
                handleWave(waveConfig);
            }
            else if (wave == 7 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                handleWave(waveConfig);
            }
            else if (wave == 8 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                handleWave(waveConfig);
            }
            else if (wave == 9 && allInactive)
            {
                wave++;
                MyWaveConfig waveConfig = new MyWaveConfig();
                waveConfig.numberOfStandardEnemies = 4;
                waveConfig.numberOfBomberEnemies = 3;
                waveConfig.numberOfEngineerEnemies = 4;
                handleWave(waveConfig);
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
                completedEarthStation = true;
            }
        }
    }

    void handleWave(WaveConfig waveConfig)
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
