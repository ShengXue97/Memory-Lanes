using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct JsonLevel
{
    public string name;

    public string tilemap;
    public string music;

    public JsonTileSettings[] triggerSettings;
    public JsonTileSettings[] activatorSettings;
    public JsonTileSettings[] npcSettings;

    public JsonLevelEvent[] events;
    
    public List<LevelEvent> CreateLevelEvents(Level level)
    {
        List<LevelEvent> parsedEvents = new List<LevelEvent>();

        // Populate LevelEvents using information from Json
        foreach (JsonLevelEvent evt in events)
        {
            parsedEvents.Add(evt.CreateFromJson(level));
        }

        return parsedEvents;
    }
    
    public void ApplyLevelSettings(Level level)
    {
        ApplyTriggerSettings(level);
        ApplyActivatorSettings(level);
        ApplyNpcSettings(level);
    }

    public void ApplyTriggerSettings(Level level)
    {
        foreach (JsonTileSettings settings in triggerSettings)
        {
            var trigger = level.triggers[settings.id];
            trigger.on = settings.on;
            trigger.transform.eulerAngles = settings.EulerAngles;
        }
    }
    
    public void ApplyActivatorSettings(Level level)
    {
        foreach (JsonTileSettings settings in activatorSettings)
        {
            var activator = level.activators[settings.id];
            activator.on = settings.on;
            activator.transform.eulerAngles = settings.EulerAngles;
        }
    }
    
    public void ApplyNpcSettings(Level level)
    {
        foreach (JsonTileSettings settings in npcSettings)
        {
            var npc = level.npcs[settings.id];
            npc.movementSpeed = settings.speed;
            npc.transform.eulerAngles = settings.EulerAngles;
        }
    }
}

[Serializable]
public struct JsonLevelEvent
{
    // Note: All object ids are counted from bottom-left to top-right of the tilemap
    // Note: Triggers and activators are counted separately

    // The id of the detected trigger
    public int trigger;

    // The ids of the affected activators
    public int[] opens;
    public int[] closes;
    public int[] toggles;
    
    public LevelEvent CreateFromJson(Level level)
    {
        LevelEvent levelEvent = new LevelEvent();
        levelEvent.triggerEvent = new TriggerEvent()
        {
            trigger = level.triggers[trigger],
            action = TriggerAction.On,
        };

        levelEvent.activatorEvents = new List<ActivatorEvent>();
        foreach (int activator in opens)
        {
            levelEvent.activatorEvents.Add(new ActivatorEvent()
            {
                activator = level.activators[activator],
                action = ActivatorAction.On,
            });
        }
        
        foreach (int activator in closes)
        {
            levelEvent.activatorEvents.Add(new ActivatorEvent()
            {
                activator = level.activators[activator],
                action = ActivatorAction.Off,
            });
        }
        
        foreach (int activator in toggles)
        {
            levelEvent.activatorEvents.Add(new ActivatorEvent()
            {
                activator = level.activators[activator],
                action = ActivatorAction.Toggle,
            });
        }

        return levelEvent;
    }
}

[Serializable]
public struct JsonTileSettings
{
    // The id of the tile
    public int id;
    
    // Which direction the tile should face
    public string facing;
    
    // Whether the tile should be on/off (for mechanisms)
    public bool on;
    
    // The movement speed of the tile (for npcs)
    public float speed;

    public Vector3 EulerAngles
    {
        get
        {
            switch (facing)
            {
                case "R":
                    return new Vector3(0, 90, 0);
                case "D":
                    return new Vector3(0f, 180, 0);
                case "L":
                    return new Vector3(0, -90, 0);
                case "U":
                default:
                    return new Vector3(0, 0, 0);
            }
        }
    }
}
