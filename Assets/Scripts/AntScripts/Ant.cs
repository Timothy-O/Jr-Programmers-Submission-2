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
    [SerializeField]private int range = 3;
    private int pileIndex;

    private Vector3 mouseDirection;
    private Vector3 mouseWorldPos;

    private GameObject enemyobject;
    public GameObject[] resourceObject;
    public GameObject activePile;
    protected GameObject antBase;
    private SphereCollider antView;
    protected SphereCollider basePerimeter;

    public bool isSafe;
    public bool isIdle;
    public bool isAttackType;
    public bool isGathering;
    public bool withResource;
    public bool isControlled;
    
    //Assigns all values and runs the resource tracking method
    void Start()
    {
        antBase = GameObject.Find("Base");
        enemyobject = gameObject;
        antView = GetComponent<SphereCollider>();
        basePerimeter = antBase.GetComponent<SphereCollider>();
        antView.radius = range;
        ResourceTracking();
        isSafe = true;
        isIdle = true;
        isAttackType = false;
        withResource = false;
        isControlled = false;
    }
    // runs the GoGather method which handles most of the ant's movements
    void Update()
    {
        GoGather();
        ControlledState();
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
            ConflictState();
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
    public void ControlledState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MouseWorldPosition();
            StartCoroutine(ControlledMovement());
        }
    }
    IEnumerator ControlledMovement()
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

    public virtual void ConflictState()
    {
        isSafe = false;
        if (isAttackType)
        {
            MoveTo(enemyobject);
        }
        else
        {
            MoveAway(enemyobject);
        }
    }

    public void ResourceTracking()
    {
        resourceObject = GameObject.FindGameObjectsWithTag("Resource");
        if (resourceObject.Length != 0)
        {
            pileIndex = Random.Range(0, resourceObject.Length);
            activePile = resourceObject[pileIndex];
        }
    }
    public void GoGather()
    {
        isGathering = true;
        if (isSafe && resourceObject.Length != 0 && isIdle && !isControlled)
        {
            if (resourceObject[pileIndex] != null)
            {
                MoveTo(activePile);
            }
            else
            {
                ResourceTracking();
            }
        }
        else if (withResource && isSafe && !isIdle && !isControlled)
        {
            MoveTo(antBase);
        }
        else if (isSafe && isIdle && resourceObject.Length == 0 && !isControlled)
        {
            if( Vector3.Distance(transform.position, antBase.transform.position) > basePerimeter.radius)
            {
                MoveTo(antBase);
            }
        }
    }

    public virtual void Idle()
    {

    }
    public void AttackTarget(GameObject enemy)
    {
        MoveTo(enemy);
    }
    public void MoveTo(GameObject target)
    {
        Vector3 directionVector = target.transform.position - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    public void MoveTo(Vector3 target)
    {
        Vector3 directionVector = target - transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
    public void MoveAway(GameObject target)
    {
        Vector3 directionVector = transform.position- target.transform.position;
        transform.Translate(directionVector.normalized * speed * Time.deltaTime);
    }
}
