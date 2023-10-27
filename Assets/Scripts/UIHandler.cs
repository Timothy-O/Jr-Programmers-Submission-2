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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        resourceNumber = GameObject.FindGameObjectsWithTag("Resource").Length;
        unitNumber = GameObject.FindGameObjectsWithTag("Ant").Length;
        enemyNumber = GameObject.FindGameObjectsWithTag("Enemy").Length;
        resourceCount.text = "Resource Count:" + resourceNumber;
        unitCount.text = "Unit Count:" + unitNumber;
        enemyCount.text = "Enemy Count:" + enemyNumber;
    }
}
