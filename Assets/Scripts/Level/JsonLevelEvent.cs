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

public class JsonLevelEvent
{
    public JsonEvent[] events;

    public static JsonLevelEvent CreateFromJSON(GameObject objref, string jsonFile)
    {
        Debug.Log(JsonUtility.FromJson<JsonLevelEvent>(jsonFile));
        return JsonUtility.FromJson<JsonLevelEvent>(jsonFile);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}