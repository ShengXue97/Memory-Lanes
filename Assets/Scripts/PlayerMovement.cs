using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 3;
    public bool onFloor = true;
    public List<bool> doorStates = new List<bool>();
    public List<bool> platformStates = new List<bool>();
    public GameObject doors;
    public GameObject platforms;
    public LayerMask whatCanBeClickedOn;
    public NavMeshAgent agent;
    public Joystick joystick;
    public SaveLoadIndicator control;
    // Use this for initialization
    void Start()
    {
        control = FindObjectOfType<SaveLoadIndicator>();
        agent = GetComponent<NavMeshAgent>();
        joystick = FindObjectOfType<Joystick>();
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyUp("o"))
        {
            control.Save();
        } else if (Input.GetKeyUp("p"))
        {
            control.Load();
        }

        // if (Input.GetMouseButtonDown(0)) {
        //     Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hitInfo;

        //     if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn)) {
        //         agent.SetDestination(hitInfo.point);
        //     }
        // }

        print(joystick.Horizontal + ";" + joystick.Vertical);
        
        if (1 == 1){
            if (Input.GetKey("w"))
            {
                moveUp(1f);
            }
            else if (Input.GetKey("s"))
            {
                moveDown(-1f);    
            }

            if (Input.GetKey("a") && !Input.GetKey("d"))
            {
                moveLeft(-1f);
            }
            else if (Input.GetKey("d") && !Input.GetKey("a"))
            {
                moveRight(1f);
            }
        } 
        if (1 == 1) {
            if (joystick.Vertical > 0.2f)
            {
                moveUp(joystick.Vertical);
            }
            else if (joystick.Vertical < -0.2f)
            {
                moveDown(joystick.Vertical);    
            }

            if (joystick.Horizontal < -0.2f)
            {
                moveLeft(joystick.Horizontal);
            }
            else if (joystick.Horizontal > 0.2f)
            {
                moveRight(joystick.Horizontal);
            }
        }
    }

    public void moveUp(float factor){
        transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed * factor;
    }

    public void moveDown(float factor){
        transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed * factor;
    }

    public void moveLeft(float factor){
        transform.position -= transform.TransformDirection(Vector3.left) * Time.deltaTime * movementSpeed * factor;
    }

    public void moveRight(float factor){
        transform.position -= transform.TransformDirection(Vector3.left) * Time.deltaTime * movementSpeed * factor;
    }

    public void saveState()
    {
        Debug.Log("Save State");
        doorStates.Clear();
        foreach (Transform child in doors.transform)
        {
            doorStates.Add(child.GetComponent<Door>().isOpen);
        }

        if (platforms != null)
        {
            platformStates.Clear();
            foreach (Transform child in platforms.transform)
            {
                platformStates.Add(child.GetComponent<Platform>().isOpen);
            }
        }

    }

    public void loadState()
    {
        Debug.Log("Load State");
        for (int i = 0; i < doorStates.Count; ++i)
        {

            Debug.Log(i);
            Door doorScript = doors.transform.GetChild(i).GetComponent<Door>();
            
            if (doorScript.isOpen && !doorStates[i])
            {
                doorScript.Trigger();
            } else if (!doorScript.isOpen && doorStates[i])
            {
                doorScript.Trigger();
            }
        }
        doorStates.Clear();

        if (platforms != null)
        {
            for (int i = 0; i < platformStates.Count; ++i)
            {

                Debug.Log(i);
                Platform platformScript = platforms.transform.GetChild(i).GetComponent<Platform>();

                if (platformScript.isOpen && !platformStates[i])
                {
                    platformScript.Trigger();
                }
                else if (!platformScript.isOpen && platformStates[i])
                {
                    platformScript.Trigger();
                }
            }
            platformStates.Clear();
        }

    }
}

    //public void OnTriggerEnter(Collider collider)
    //{
    //    Debug.Log("Enter trigger");
    //    if (collider.tag == "Switch")
    //    {
    //        collider.enabled = false;
    //    }
    //}

    //public void OnTriggerExit(Collider collider)
    //{
    //    Debug.Log("Exit trigger");
    //    if (collider.tag == "Switch")
    //    {
    //        collider.enabled = true;
    //    }
    //}

