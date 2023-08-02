using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
interface IWaveConfig2
{

    float numberOfStandardEnemies2 { get; set; }
    float numberOfBomberEnemies2 { get; set; }
    float numberOfEngineerEnemies2 { get; set; }
    
}
class MyWaveConfig2 : IWaveConfig2
{
    public float numberOfStandardEnemies2 { get; set; }
    public float numberOfBomberEnemies2 { get; set; }
    public float numberOfEngineerEnemies2 { get; set; }
}
public class ArenaScript : MonoBehaviour
{
    [SerializeField] private GameObject standardEnemyPrefab;
    [SerializeField] private GameObject bomberEnemyPrefab;
    [SerializeField] private GameObject engineerEnemyPrefab;
    public List<GameObject> enemies = new();
    public Transform dockbay1;
    public Transform dockbay2;
    public Transform dockbay3;
    private int wave = 1;
    private int minStandardEnemies = 1;
    private int maxStandardEnemies = 4;
    private int minEngineerEnemies = 0;
    private int maxEngineerEnemies = 2;
    private int minBomberEnemies = 0;
    private int maxBomberEnemies = 2;
    private int increaseRate = 2;
    private PlayerController playerController;
    [SerializeField] private TMP_Text waveText;
    private float enemyMultiplier = 1;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
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
        waveText.text = "Wave: " + (wave - 1);
        bool allInactive = enemies.All(enemy => !enemy.activeSelf);
        if (wave % 5 != 0 && allInactive)
        {
            wave++;
            MyWaveConfig2 waveConfig = new()
            {
                numberOfStandardEnemies2 = Random.Range(minStandardEnemies, maxStandardEnemies),
                numberOfBomberEnemies2 = Random.Range(minBomberEnemies, maxBomberEnemies),
                numberOfEngineerEnemies2 = Random.Range(minEngineerEnemies, maxEngineerEnemies)
            };
            HandleWave(waveConfig);
        }
        else if (allInactive && wave % 5 == 0)
        {
            wave++;
            Debug.Log("boss fight!!");
            minBomberEnemies += Mathf.RoundToInt(increaseRate / 2);
            minEngineerEnemies += Mathf.RoundToInt(increaseRate / 3);
            minStandardEnemies += increaseRate;
            foreach (GameObject go in enemies)
            {
                Destroy(go);
            }
            enemies.Clear();
        }
        if (wave % 3 == 0 && allInactive)
        {
            maxStandardEnemies += increaseRate;
            maxEngineerEnemies += Mathf.RoundToInt(increaseRate / 3);
            maxBomberEnemies += Mathf.RoundToInt(increaseRate / 2);
        }
        if (wave % 4 == 0 && allInactive)
        {
            increaseRate += Mathf.RoundToInt(increaseRate / 2);
        }
        if (wave % 10 == 0 && allInactive)
        {
            playerController.money += 300 * (wave / 10);
        }
        if (wave % 11 == 0 && allInactive)
        {
            enemyMultiplier += .25f;
            increaseRate = Mathf.RoundToInt(increaseRate / 2.6f);
        }
    }
    void HandleWave(IWaveConfig2 waveConfig)
    {

        for (var i = 0; i < waveConfig.numberOfStandardEnemies2; i++)
        {
            int randomIndex = Random.Range(1, 4);
            string variableName = "dockbay" + randomIndex;
            Transform prefabSpawnPoint = (Transform)GetType().GetField(variableName).GetValue(this);
            GameObject standardEnemy = Instantiate(standardEnemyPrefab, prefabSpawnPoint.position, Quaternion.identity);
            StandardEnemyController controller = standardEnemy.GetComponent<StandardEnemyController>();
            controller.health *= enemyMultiplier;
            enemies.Add(standardEnemy);
        }

        for (var i = 0; i < waveConfig.numberOfBomberEnemies2; i++)
        {
            Transform prefabSpawnPoint = (Transform)this.GetType().GetField("dockbay" + Random.Range(1, 4).ToString()).GetValue(this);
            GameObject bomberEnemy = Instantiate(bomberEnemyPrefab, prefabSpawnPoint.position, Quaternion.identity);
            BombEnemyController controller = bomberEnemy.GetComponent<BombEnemyController>();
            controller.health *= enemyMultiplier;
            enemies.Add(bomberEnemy);
        }

        for (var i = 0; i < waveConfig.numberOfEngineerEnemies2; i++)
        {
            Transform prefabSpawnPoint = (Transform)this.GetType().GetField("dockbay" + Random.Range(1, 4).ToString()).GetValue(this);
            GameObject engineerEnemy = Instantiate(engineerEnemyPrefab, prefabSpawnPoint.position, Quaternion.identity);
            EngineerEnemyController controller = engineerEnemy.GetComponent<EngineerEnemyController>();
            controller.health *= enemyMultiplier;
            enemies.Add(engineerEnemy);
        }
    }
}
