using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Trigger
{
    public float delay = 1f;
    private float remainingDelay;
    private bool depressTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainingDelay -= Time.deltaTime;
        if (depressTimer && remainingDelay <= 0)
        {
            depressTimer = false;
            TriggerOff();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Player Collision Enter");
            TriggerOn();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            depressTimer = true;
            remainingDelay = delay;
            Debug.Log("Player Collision Exit");
        }
    }
}
