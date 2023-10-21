using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    //speed is the objects speend during translation; range is the radius of the sphere colider that detects enemys
    //pileIndex is the random index for the resourceObject; enemyPosition is the detected enemy's position
    //resourceDirection is the vector 3 direction towards a random resource pile
    //enemyObject is the detected enemy oject; resourceobjects is an array of all resources in the scene
    // antView is the sphere collider that detects enemys; isSafe activitaes while ant is not escaping
    //withResources activates after ant collide with resource piles
    public float speed;
    private int range = 3;
    private int pileIndex;

    private Vector3 enemyPosition;
    private Vector3 resourceDirecion;
    private Vector3 mouseDirection;
    private Vector3 mouseWorldPos;

    private GameObject enemyobject;
    public GameObject[] resourceObject;
    private SphereCollider antView;

    public bool isSafe;
    public bool isIdle;
    public bool withResource;
    public bool isControlled;
    
    //Assigns all values and runs the resource tracking method
    void Start()
    {
        enemyobject = gameObject;
        antView = GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        withResource = false;
        isControlled = false;
    }
    // runs the GoGather method which handles most of the ant's movements
    void Update()
    {
        GoGather();
        ControlledMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            //Debug.Log("Predator");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyobject = other.gameObject;
            enemyPosition = enemyobject.transform.position;
            Escape();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemyobject = gameObject;
            isSafe = true;
            ResourceTracking();
        }
    }
    private void OnMouseDown()
    {
        if (isControlled)
        {
            isControlled=false;
        }
        else
        {
            isControlled = true;
        }
    }

    private Vector3 MouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit rayCastHit);
        Vector3 mousePos = rayCastHit.point;
        mouseWorldPos = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        mouseDirection = mouseWorldPos - transform.position;
        return mouseDirection;
    }
    public void ControlledMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MouseWorldPosition();
            StartCoroutine(MovementController());
        }
    }
    IEnumerator MovementController()
    {
        while (Vector3.Distance(transform.position, mouseWorldPos) > 0.05f)
        {
            transform.Translate(mouseDirection.normalized * speed * Time.deltaTime);
            yield return null;
        }
        isControlled = false;
        ResourceTracking();
        yield return null;
    }

    public void Escape()
    {
        isSafe = false;
        Vector3 directionVector = gameObject.transform.position-enemyPosition;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }

    public void ResourceTracking()
    {
        resourceObject = GameObject.FindGameObjectsWithTag("Resource");
        if (resourceObject.Length != 0)
        {
            pileIndex = Random.Range(0, resourceObject.Length);
            resourceDirecion = resourceObject[pileIndex].transform.position - transform.position;
        }
    }
    public void GoGather()
    {
        if (isSafe && resourceObject.Length != 0 && isIdle && !isControlled)
        {
            if (resourceObject[pileIndex] != null)
            {
                transform.Translate(resourceDirecion.normalized * speed * Time.deltaTime);
            }
            else
            {
                ResourceTracking();
            }
        }
        else if (withResource && isSafe && !isIdle && !isControlled)
        {
            Vector3 baseDirection = GameObject.Find("Base").transform.position - transform.position;
            transform.Translate(baseDirection.normalized * speed * Time.deltaTime);
        }
        else if (isSafe && isIdle && resourceObject.Length == 0 && !isControlled)
        {
            Vector3 baseDirection = GameObject.Find("Base").transform.position - transform.position;
            transform.Translate(baseDirection.normalized * speed * Time.deltaTime);
        }
    }
    public virtual void Idle()
    {

    }
}
