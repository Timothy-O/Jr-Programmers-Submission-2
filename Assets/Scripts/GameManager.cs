using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    //List of all enemies in game
    public List<GameObject> ants = new List<GameObject>();
    //List of all ants in game
    public List<GameObject> resources = new List<GameObject>();
    //List of all reosurces in game.

    // Start is called before the first frame update
    void Start()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        ants.AddRange(GameObject.FindGameObjectsWithTag("Ant"));
        resources.AddRange(GameObject.FindGameObjectsWithTag("Resource"));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
