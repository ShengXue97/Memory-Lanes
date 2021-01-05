using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelEvent
{
    public TriggerEvent triggerEvent;
    public ActivatorEvent[] activatorEvents;
}

public class Level : MonoBehaviour
{
    public AudioClip music;

    public Agent player;
    public MyGoal goal;
    public MySaveLoadIndicator saveLoadIndicator;

    public Transform boundaryContainer;
    public Transform pathContainer;
    public Transform triggerContainer;
    public Transform activatorContainer;

    [HideInInspector]
    public MyTrigger[] triggers;
    [HideInInspector]
    public MyActivator[] activators;

    [SerializeField]
    private LevelEvent[] events;
    private Dictionary<TriggerEvent, ActivatorEvent[]> eventBindings;

    public void Awake()
    {
        eventBindings = new Dictionary<TriggerEvent, ActivatorEvent[]>();

        foreach (LevelEvent evt in events)
        {
            eventBindings[evt.triggerEvent] = evt.activatorEvents;
        }
    }

    public ActivatorEvent[] GetTriggerEvents(TriggerEvent triggerEvent)
    {
        ActivatorEvent[] activatorEvents;
        if (!eventBindings.TryGetValue(triggerEvent, out activatorEvents))
        {
            return new ActivatorEvent[0];
        }
        return activatorEvents;
    }
}
