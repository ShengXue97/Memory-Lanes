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
    public Transform triggerContainer;
    public Transform activatorContainer;

    [HideInInspector]
    public MyTrigger[] triggers;
    [HideInInspector]
    public MyActivator[] activators;

    [SerializeField]
    private LevelEvent[] events;
    private Dictionary<TriggerEvent, List<ActivatorEvent>> eventBindings;

    public GameObject objref;
    
    public List<LevelEvent> parseEvents(string filename)
    {
        List<LevelEvent> parsedLevelEvents = new List<LevelEvent>();
        JsonLevelEvent jsonLevelEvents = JsonLevelEvent.CreateFromJSON(objref, filename);

        // Populate LevelEvents using information from Json
        foreach (JsonEvent evt in jsonLevelEvents.events)
        {
            JsonTrigger trigger = evt.trigger;
            JsonActivator[] activators = evt.activators;

            TriggerEvent triggerEvent = new TriggerEvent();
            var triggerPosition = trigger.position;
            // triggerEvent.trigger = Level.GetObject(triggerPosition).GetComponent<Trigger>();
            triggerEvent.action = trigger.action=="On" ? TriggerAction.On : TriggerAction.Off;

            List<ActivatorEvent> activatorEvents = new List<ActivatorEvent>();
            foreach (JsonActivator act in activators) {
                ActivatorEvent activatorEvent = new ActivatorEvent();
                var activatorPosition = act.position;
                // activatorEvent.activator = Level.GetObject(activatorPosition).GetComponent<Activator>();
                activatorEvent.action = act.action == "On" ? ActivatorAction.On : act.action == "Off" ? ActivatorAction.Off : ActivatorAction.Toggle;
            }

            LevelEvent levelEvent = new LevelEvent();
            levelEvent.triggerEvent = triggerEvent;
            levelEvent.activatorEvents = activatorEvents;
            parsedLevelEvents.Add(levelEvent);
        }
        
        Debug.Log(parsedLevelEvents);
        return parsedLevelEvents;
    } 

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
}
