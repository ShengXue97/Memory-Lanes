using System;
using System.Collections.Generic;

[Serializable]
public struct JsonLevel
{
    public string name;

    public string tilemap;
    public string music;

    public JsonLevelEvent[] events;
    
    public List<LevelEvent> CreateLevelEvents(Level levelPrefab)
    {
        List<LevelEvent> parsedEvents = new List<LevelEvent>();

        // Populate LevelEvents using information from Json
        foreach (JsonLevelEvent evt in events)
        {
            parsedEvents.Add(evt.CreateFromJson(levelPrefab));
        }

        return parsedEvents;
    }
}

[Serializable]
public struct JsonLevelEvent
{
    public JsonTriggerEvent trigger;
    public JsonActivatorEvent[] activators;
    
    public LevelEvent CreateFromJson(Level level)
    {
        LevelEvent levelEvent = new LevelEvent();
        levelEvent.triggerEvent = trigger.CreateFromJson(level);

        levelEvent.activatorEvents = new List<ActivatorEvent>();
        foreach (JsonActivatorEvent activator in activators)
        {
            levelEvent.activatorEvents.Add(activator.CreateFromJson(level));
        }

        return levelEvent;
    }
}

[Serializable]
public struct JsonTriggerEvent
{
    // Coordinates of trigger
    public int x;
    public int y;

    // actions: 'On', 'Off',
    public string action;
    
    // public string type; // type of trigger - currently the only type is 'Switch'

    public TriggerEvent CreateFromJson(Level level)
    {
        TriggerEvent triggerEvent = new TriggerEvent();
        triggerEvent.trigger = level.GetForegroundObject(x, y).GetComponent<MyTrigger>();
        Enum.TryParse(action, false, out triggerEvent.action);
        return triggerEvent;
    }
}

[Serializable]
public struct JsonActivatorEvent
{
    // Coordinates of activator
    public int x;
    public int y;
    
    // actions: 'On', 'Off', 'Toggle',
    public string action; 

    // public string type; // types: 'Door', 'Platform'
    
    public ActivatorEvent CreateFromJson(Level level)
    {
        ActivatorEvent activatorEvent = new ActivatorEvent();
        activatorEvent.activator = level.GetForegroundObject(x, y).GetComponent<MyActivator>();
        Enum.TryParse(action, false, out activatorEvent.action);
        return activatorEvent;
    }
}
