using System;
using System.Collections.Generic;
using UnityEngine;

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
