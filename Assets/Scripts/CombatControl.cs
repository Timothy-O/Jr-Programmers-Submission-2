using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    //reource variable will only be applicable to enemies, entity is the parent game object;
    public float health;
    public float damage;
    public float resource;
    private GameObject entity;

    //Assigns values that require assigning
    void Start()
    {
        entity = transform.parent.gameObject;
    }
    //Makes sure parent and child positions are the same;
    void Update()
    {
        if (transform.position != entity.transform.position)
        {
            transform.position = entity.transform.position;
        }
    }
    
    // Decides what happens during contact;
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "Enemy")
        {
            if (collision.gameObject.tag == "Ant")
            {
                health -= collision.gameObject.GetComponent<CombatControl>().damage;
                GetComponentInParent<Enemy>().isAttacking = false;
                GetComponentInParent<Enemy>().RandomDirection();
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
        }
        else if (gameObject.tag == "Ant")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                health -= collision.gameObject.GetComponent<CombatControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
            else if (collision.gameObject.tag == "Resource" && GetComponentInParent<Ant>().isSafe)
            {
                Destroy(collision.gameObject);
                GetComponentInParent<Ant>().withResource = true;
                GetComponentInParent<Ant>().isIdle = false;
                
            }
            else if (collision.gameObject.name == "Base" && GetComponentInParent<Ant>().isSafe)
            {
                GetComponentInParent<Ant>().ResourceTracking();
                if (GetComponentInParent<Ant>().resourceObject.Length == 0)
                {
                    GetComponentInParent<Ant>().Idle();
                }
                GetComponentInParent<Ant>().isIdle = true;
                GetComponentInParent<Ant>().withResource = false;

            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (gameObject.tag == "Enemy")
        {
            if (collision.gameObject.tag == "Ant")
            {
                health -= collision.gameObject.GetComponent<CombatControl>().damage;
                GetComponentInParent<Enemy>().isAttacking = false;
                GetComponentInParent<Enemy>().RandomDirection();
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
        }
        else if (gameObject.tag == "Ant")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                health -= collision.gameObject.GetComponent<CombatControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
        }
    }
}
