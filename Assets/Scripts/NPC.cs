using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;
    [SerializeField]
    private double roundTripTime = 4.0;
    public bool isWalking = true;

    private double currentTimeLeft;
    private bool touchedPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeLeft = roundTripTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking & currentTimeLeft > 0)
        {
            if (!touchedPlayer)
            {
                this.transform.localPosition = currentTimeLeft > roundTripTime / 2 ?
                 startPos.localPosition * (float)(2 * (roundTripTime - currentTimeLeft) / roundTripTime) + endPos.localPosition * (float)(1 - 2 * (roundTripTime - currentTimeLeft) / roundTripTime) :
                 endPos.localPosition * (float)(1 - 2 * currentTimeLeft / roundTripTime) + startPos.localPosition * (float)(2 * currentTimeLeft / roundTripTime);
                 currentTimeLeft -= Time.deltaTime;
            }
            
        }
        else if (isWalking)
        {
            currentTimeLeft = roundTripTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "aPlayer")
        {
            touchedPlayer = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "aPlayer")
        {
            touchedPlayer = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "aPlayer")
        {
            touchedPlayer = false;
        }
    }


}
