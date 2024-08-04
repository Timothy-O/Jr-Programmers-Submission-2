using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Vector3 randomPos;
    // random position outside of ant base
    private int baseRange;
    //square range marking out base perimeter
    private int mapRange;
    //square range marking out map perimeter
    public int reourcesCollected;
    //number of resources gathered
    public float spawnInterval;
    //number of seconds between each resouce spawn
    public float spawnIntervalE;
    //number of seconds between each enemy spawn
    public GameObject enemyPrefab;
    //Prefab enemy game object, assigned in editor
    public GameObject soldierPrefab;
    //Prefab soldier ant game object, assigned in editor
    public GameObject workerPrefab;
    //Prefab worker ant prefab object, assigned in editor
    public GameObject resourcePrefab;
    //Prefab resource game object, assigned in editor
    public GameObject controlledUnit;
    //GameObject currently being controlled
    public List<GameObject> enemies = new List<GameObject>();
    //List of all enemies in game, assigned by individual enemies
    public List<GameObject> ants = new List<GameObject>();
    //List of all ants in game, assigned by individual ants
    public List<GameObject> resources = new List<GameObject>();
    //List of all reosurces in game, assigned by individual reource piles

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        baseRange = 10;
        mapRange = 30;
        spawnInterval = 10;
        spawnIntervalE = 30;
    }
    // Update is called once per frame
    void Update()
    {
        ConstantSpawn("Resource");
        ConstantSpawn("Enemy");
    }

    //Random position generator that respects a defined boundary(base range)
    private Vector3 RandomPos()
    {
        float randomX;
        float randomZ;
        randomX = Random.Range(-mapRange, +mapRange);
        randomZ = Random.Range(-mapRange, +mapRange);
        if(-baseRange<randomX && randomX < baseRange)
        {
            if(-baseRange < randomZ && randomZ < baseRange)
            {
                RandomPos();
            }
            else
            {
                randomPos = new Vector3(randomX, 0.5f, randomZ);
            }
        }
        else
        {
            randomPos = new Vector3(randomX, 0.5f, randomZ);
        }
        return randomPos;
    }
    public void SpawnResource()
    {
        RandomPos();
        Instantiate(resourcePrefab,randomPos, resourcePrefab.gameObject.transform.rotation);
    }
    public void SpawnEnemy()
    {
        RandomPos();
        Instantiate(enemyPrefab, randomPos, enemyPrefab.gameObject.transform.rotation);
    }
    public void SpawnAnt(string type)
    {
        if (type == "Worker" && reourcesCollected > 1)
        {
            Instantiate(workerPrefab, new Vector3(0, 0, 5), workerPrefab.transform.rotation);
            reourcesCollected -= 2;
        }
        if (type == "Soldier" && reourcesCollected > 4)
        {
            Instantiate(soldierPrefab, new Vector3(0, 0, -5), soldierPrefab.transform.rotation);
            reourcesCollected -= 5;
        }

    }
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
    private void ConstantSpawn(string type)
    {
        if(type == "Resource")
        {
            spawnInterval -= Time.deltaTime;
            if (spawnInterval < 0)
            {
                SpawnResource();
                spawnInterval = 10;
            }
        }
        if (type == "Enemy")
        {
            spawnIntervalE -= Time.deltaTime;
            if (spawnIntervalE < 0)
            {
                SpawnEnemy();
                spawnIntervalE = 30;
            }
        }
    }
}
