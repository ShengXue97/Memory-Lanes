using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{

    public UnityEvent[] triggerOn;
    public UnityEvent[] triggerOff;

    public void TriggerOn()
    {
        foreach (UnityEvent target in triggerOn)
        {
            target.Invoke();
        }
        //foreach (GameObject target in targets)
        //{
        //    if (target.GetComponent<Activator>())
        //    {
        //        Activator activator = target.GetComponent<Activator>();
        //        activator.TriggerOn();
        //    }
        //}
    }

    public void TriggerOff()
    {
        foreach (UnityEvent target in triggerOff)
        {
            target.Invoke();
        }
        //foreach (GameObject target in targets)
        //{
        //    if (target.GetComponent<Activator>())
        //    {
        //        Activator activator = target.GetComponent<Activator>();
        //        activator.TriggerOff();
        //    }
        //}
    }
}
