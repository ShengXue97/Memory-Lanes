using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelEvent
{
    public TriggerEvent triggerEvent;
    public List<ActivatorEvent> activatorEvents;
}

public class Level : MonoBehaviour
{
    public AudioClip music;

    public Agent player;
    public MyGoal goal;
    public MySaveLoadIndicator saveLoadIndicator;

    public Transform boundaryContainer;
    public Transform pathContainer;
    public Transform agentContainer;
    public Transform triggerContainer;
    public Transform activatorContainer;

    [HideInInspector]
    public List<MyTrigger> triggers;
    [HideInInspector]
    public List<MyActivator> activators;
    [HideInInspector]
    public List<Enemy> npcs;

    public GameObject[,] backgroundObjects;
    public GameObject[,] foregroundObjects;

    public List<LevelEvent> events;
    private Dictionary<TriggerEvent, List<ActivatorEvent>> eventBindings;

    public void Awake()
    {
        eventBindings = new Dictionary<TriggerEvent, List<ActivatorEvent>>();

        foreach (LevelEvent evt in events)
        {
            eventBindings[evt.triggerEvent] = evt.activatorEvents;
            //Debug.Log(evt);
        }
    }

    public List<ActivatorEvent> GetActivatorEvents(TriggerEvent triggerEvent)
    {
        List<ActivatorEvent> activatorEvents;
        if (!eventBindings.TryGetValue(triggerEvent, out activatorEvents))
        {
            return new List<ActivatorEvent>();
        }
        return activatorEvents;
    }
    
    public GameObject GetBackgroundObject(int x, int y)
    {
        return backgroundObjects[x, y];
    }

    public GameObject GetForegroundObject(int x, int y)
    {
        return foregroundObjects[x, y];
    }
}
