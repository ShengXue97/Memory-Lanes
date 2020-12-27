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

    public Transform movePoint;
    public LayerMask whatStopsMovement;

    private bool canMoveHorizontal = true;
    private bool canMoveVertical = true;
    private float currentTime;
    // Use this for initialization
    void Start()
    {
        control = FindObjectOfType<SaveLoadIndicator>();
        agent = GetComponent<NavMeshAgent>();
        joystick = FindObjectOfType<Joystick>();

        currentTime = -999f;
        movePoint.parent = null;
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("o"))
        {
            control.Save();
        }
        else if (Input.GetKeyUp("p"))
        {
            control.Load();
        }


        print(joystick.Horizontal + ";" + joystick.Vertical);
        float pointer_x = Input.GetAxisRaw("Horizontal");
        float pointer_y = Input.GetAxisRaw("Vertical");
        // if (Mathf.Abs(joystick.Horizontal) > 0.2f)
        // {
        //     pointer_x = joystick.Horizontal;
        // }

        // if (Mathf.Abs(joystick.Vertical) > 0.2f)
        // {
        //     pointer_y = joystick.Vertical;
        // }

        if (Mathf.Abs(pointer_x) == 0f)
        {
            //Prevents holding down arrow keys to move, must release
            canMoveHorizontal = true;
        }

        if (Mathf.Abs(pointer_y) == 0f)
        {
            //Prevents holding down arrow keys to move, must release
            canMoveVertical = true;
        }

        //Debug.Log(pointer_x + ";" + pointer_y);

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 20f * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05F)
        {
            if (Mathf.Abs(pointer_x) == 1f && canMoveHorizontal)
            {
                canMoveHorizontal = false;
                if (Physics.OverlapSphere(movePoint.position + new Vector3(pointer_x * 1, 0f, 0f), 0.2f, whatStopsMovement).Length == 0)
                {
                    if (pointer_x == -1f)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    //anim.SetBool("Hop", true);
                    movePoint.position += new Vector3(pointer_x, 0f, 0f);
                }
            }
            else if (Mathf.Abs(pointer_y) == 1f && canMoveVertical)
            {
                canMoveVertical = false;
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0f, 0f, pointer_y * 1), 0.2f, whatStopsMovement).Length == 0)
                {
                    if (pointer_y == -1f)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    // anim.SetBool("Hop", true);
                    movePoint.position += new Vector3(0f, 0f, pointer_y);
                }
            }
            else
            {
                //anim.SetBool("Hop", false);
            }
        }
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
            }
            else if (!doorScript.isOpen && doorStates[i])
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
