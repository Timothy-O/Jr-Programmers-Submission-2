using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (!gameManager.resources.Contains(gameObject))
        {
            gameManager.resources.Add(gameObject);
        }
    }
    private void OnDestroy()
    {
        gameManager.resources.Remove(gameObject);
    }
    private void RandomResourceSpawn()
    {
        Instantiate(gameObject, transform);
    }
}
