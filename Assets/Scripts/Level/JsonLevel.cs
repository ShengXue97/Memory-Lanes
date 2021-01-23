using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct JsonLevel
{
    public string name;

    public string tilemap;
    public string music;
    
    public JsonVector3 tree;

    public JsonTileSettings[] triggerSettings;
    public JsonTileSettings[] activatorSettings;
    public JsonTileSettings[] npcSettings;

    public JsonLevelEvent[] events;

    public void ApplyLevelSettings(Level level)
    {
        ApplyTreeSettings(level);
        ApplyTriggerSettings(level);
        ApplyActivatorSettings(level);
        ApplyNpcSettings(level);
        ApplyLevelEvents(level);
    }
    
    private void ApplyTreeSettings(Level level)
    {
        var saveLoadIndicator = level.saveLoadIndicator;
        saveLoadIndicator.transform.position = tree.Deserialize();
    }

    private void ApplyTriggerSettings(Level level)
    {
        for (int i = 0; i < triggerSettings.Length; i++)
        {
            var settings = triggerSettings[i];
            var tile = level.triggers[i];

            tile.name = settings.id;
            tile.on = settings.on;
            tile.transform.eulerAngles = settings.EulerAngles;
        }
    }
    
    private void ApplyActivatorSettings(Level level)
    {
        for (int i = 0; i < activatorSettings.Length; i++)
        {
            var settings = activatorSettings[i];
            var tile = level.activators[i];

            tile.name = settings.id;
            tile.on = settings.on;
            tile.transform.eulerAngles = settings.EulerAngles;
        }
    }
    
    private void ApplyNpcSettings(Level level)
    {
        for (int i = 0; i < npcSettings.Length; i++)
        {
            var settings = npcSettings[i];
            var tile = level.npcs[i];

            tile.name = settings.id;
            tile.movementSpeed = settings.speed;
            tile.transform.eulerAngles = settings.EulerAngles;
        }
    }
    
    private void ApplyLevelEvents(Level level)
    {
        level.LoadReferences();
        level.events = new List<LevelEvent>();

        // Populate LevelEvents using information from Json
        foreach (JsonLevelEvent evt in events)
        {
            level.events.Add(evt.CreateFromJson(level));
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
    public string id;
    
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

[Serializable]
public struct JsonVector3
{
    public float x;
    public float y;
    public float z;

    public Vector3 Deserialize()
    {
        return new Vector3(x, y, z);
    }
}
