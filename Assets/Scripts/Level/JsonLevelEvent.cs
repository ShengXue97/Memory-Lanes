using System;
using UnityEngine;

[Serializable]
public struct JsonEvent
{
    public JsonTrigger trigger;
    public JsonActivator[] activators;
}

public struct JsonTrigger
{
    public Vector3 position; // x,y coordinates of trigger position
    public string type; // type of trigger - currently the only type is 'Switch'
    public string action;
}

public struct JsonActivator
{
    public Vector3 position;
    public string type; // types: 'Door', 'Platform'
    public string action; // actions: 'On', 'Off', 'Toggle'
}
