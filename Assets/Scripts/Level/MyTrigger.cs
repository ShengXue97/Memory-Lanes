using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct TriggerEvent
{
    public MyTrigger trigger;
    public TriggerAction action;
}

public enum TriggerAction
{
    None,
    On,
    Off,
}

public interface ITriggerListener
{
    void OnTriggerEvent(TriggerEvent triggerEvent);
}

public class MyTrigger : MonoBehaviour
{
    private static readonly int AnimatorLoad = Animator.StringToHash("load");
    private static readonly int AnimatorOn = Animator.StringToHash("on");

    public bool on;

    private Animator animator;

    public UnityEvent triggerOn;
    public UnityEvent triggerOff;
    
    private ITriggerListener listener;
    
    //TODO: bool on can update the trigger in the Editor
    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (on)
        {
            animator.SetBool(AnimatorOn, true);
        }
        else
        {
            animator.SetBool(AnimatorOn, false);
        }

        animator.SetTrigger(AnimatorLoad);
    }

    public void SetListener(ITriggerListener listener)
    {
        this.listener = listener;
    }

    public void On()
    {
        if (on)
        {
            return;
        }
        on = true;
        animator.SetBool(AnimatorOn, true);

        listener?.OnTriggerEvent(new TriggerEvent(){trigger = this, action = TriggerAction.On});
        triggerOn?.Invoke();
    }

    public void Off()
    {
        if (!on)
        {
            return;
        }
        on = false;
        animator.SetBool(AnimatorOn, false);

        listener?.OnTriggerEvent(new TriggerEvent(){trigger = this, action = TriggerAction.Off});
        triggerOff?.Invoke();
    }

    public void TriggerOn()
    {
        // foreach (UnityEvent target in triggerOn)
        // {
        //     target.Invoke();
        // }
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
        // foreach (UnityEvent target in triggerOff)
        // {
        //     target.Invoke();
        // }
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
