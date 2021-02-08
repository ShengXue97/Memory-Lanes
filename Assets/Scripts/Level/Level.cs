using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<LevelEvent> events;

    private Dictionary<string, MyTrigger> triggersByName;
    private Dictionary<string, MyActivator> activatorsByName;
    private Dictionary<string, Enemy> npcsByName;

    private Dictionary<TriggerEvent, List<ActivatorEvent>> eventBindings;

    [HideInInspector]
    public GameObject[,] backgroundObjects;
    [HideInInspector]
    public GameObject[,] foregroundObjects;

    public void Start()
    {
        LoadReferences();
        LoadEvents();
    }
    
    public void LoadReferences()
    {
        triggersByName = triggers.ToDictionary(k => k.name, v => v);
        activatorsByName = activators.ToDictionary(k => k.name, v => v);
        npcsByName = npcs.ToDictionary(k => k.name, v => v);
    }
    public void LoadEvents()
    {
        eventBindings = events.ToDictionary(e => e.triggerEvent, e => e.activatorEvents);
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
    
    public MyTrigger GetTrigger(string id)
    {
        return triggersByName[id];
    }    

    public MyActivator GetActivator(string id)
    {
        return activatorsByName[id];
    }

    public Enemy GetNpc(string id)
    {
        return npcsByName[id];
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
