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
    //resource variable will only be applicable to enemies, entity is the parent game object;
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
    {//why couldnt you use triggerenter instead
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
                if (GetComponentInParent<Ant>().resourceObject.Length == 0)
                {
                    GetComponentInParent<Ant>().Idle();
                    GetComponentInParent<Ant>().isGathering = false;
                }
                GetComponentInParent<Ant>().ResourceTracking();//should be corrected soon
                GetComponentInParent<Ant>().isIdle = true;
                GetComponentInParent<Ant>().withResource = false;
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
