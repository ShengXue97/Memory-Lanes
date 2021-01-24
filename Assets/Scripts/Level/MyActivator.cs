using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct ActivatorEvent
{
    public MyActivator activator;
    public ActivatorAction action;
}

public enum ActivatorAction
{
    None,
    On,
    Off,
    Toggle,
}

public class MyActivator : MonoBehaviour
{
    private static readonly int AnimatorLoad = Animator.StringToHash("load");
    private static readonly int AnimatorOn = Animator.StringToHash("on");

    public bool on;
    
    [SerializeField]
    private UnityEvent activatorOn;
    [SerializeField]
    private UnityEvent activatorOff;

    private Animator animator;

    //TODO: bool on can update the activator in the Editor

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

    public void On()
    {
        if (on)
        {
            return;
        }
        on = true;
        animator.SetBool(AnimatorOn, true);
        activatorOn?.Invoke();
    }

    public void Off()
    {
        if (!on)
        {
            return;
        }
        on = false;
        animator.SetBool(AnimatorOn, false);
        activatorOff?.Invoke();

    }

    public void Toggle()
    {
        if (on)
        {
            Off();
        }
        else
        {
            On();
        }
    }
}
