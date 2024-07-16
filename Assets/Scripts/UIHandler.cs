using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI resourceCount;
    public TextMeshProUGUI unitCount;
    public TextMeshProUGUI enemyCount;
    private int resourceNumber;
    private int enemyNumber;
    private int unitNumber;
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
        resourceNumber = gameManager.resources.Count;
        unitNumber = gameManager.ants.Count;
        enemyNumber = gameManager.enemies.Count;
        resourceCount.text = "Resource Count:" + resourceNumber;
        unitCount.text = "Unit Count:" + unitNumber;
        enemyCount.text = "Enemy Count:" + enemyNumber;
    }
}
