using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntCombatControl : MonoBehaviour
{
    public float health;
    public float damage;
    public float resource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision");
            Debug.Log("Defend!");
            Debug.Log("Contact");
            health -= collision.gameObject.GetComponent<ContactControl>().damage;
            if (health < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DamageCalculation()
    {

    }
}
