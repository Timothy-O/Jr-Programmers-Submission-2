using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactControl : MonoBehaviour
{
    public float health;
    // health of the entity
    public float damage;
    //damage the entity causes
    public float resource;
    //resource variable will only be applicable to enemies
    public float carryCapacity;
    //amount of resources that can be carried
    private GameObject entity;
    //the ant or enemy parent object

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
                health -= collision.gameObject.GetComponent<ContactControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
                GetComponentInParent<Enemy>().isAttacking = false;
                GetComponentInParent<Enemy>().RandomPos();
            }
        }
        else if (gameObject.tag == "Ant")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                health -= collision.gameObject.GetComponent<ContactControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
            else if (collision.gameObject.tag == "Resource" && GetComponentInParent<Ant>().isSafe && 
                GetComponentInParent<Ant>().isGathering && collision.gameObject == GetComponentInParent<Ant>().activePile)
            {
                Destroy(collision.gameObject);
                GetComponentInParent<Ant>().withResource = true;
                GetComponentInParent<Ant>().isIdle = false;    
            }
            else if (collision.gameObject.name == "Base" && GetComponentInParent<Ant>().isSafe && GetComponentInParent<Ant>().isGathering)
            {
                if (GetComponentInParent<Ant>().gameManager.resources.Count == 0)
                {
                    GetComponentInParent<Ant>().isGathering = false;
                }
                GetComponentInParent<Ant>().ResourceSelection();
                GetComponentInParent<Ant>().isIdle = true;
                GetComponentInParent<Ant>().withResource = false;
                GetComponentInParent<Ant>().isGathering = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)//is this necessary, if they both live after first contact the entire chase process will repeat
    {
        if (gameObject.tag == "Enemy")
        {
            if (collision.gameObject.tag == "Ant")
            {
                health -= collision.gameObject.GetComponent<ContactControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
                GetComponentInParent<Enemy>().isAttacking = false;
                GetComponentInParent<Enemy>().RandomPos();
            }
        }
        else if (gameObject.tag == "Ant")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                health -= collision.gameObject.GetComponent<ContactControl>().damage;
                if (health < 0)
                {
                    Destroy(entity);
                }
            }
        }
    }
}
