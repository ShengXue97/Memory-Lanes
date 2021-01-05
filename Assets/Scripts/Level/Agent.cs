using UnityEngine;

public class Agent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MyTrigger trigger = other.GetComponent<MyTrigger>();
        if (trigger == null)
        {
            return;
        }
        trigger.On();
    }

    public void OnTriggerExit(Collider other)
    {
        MyTrigger trigger = other.GetComponent<MyTrigger>();
        if (trigger == null)
        {
            return;
        }
        trigger.Off();
    }
}
