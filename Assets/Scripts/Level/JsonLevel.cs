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
