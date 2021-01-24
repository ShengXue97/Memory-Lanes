using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelDebugger : MonoBehaviour
{
    [Serializable]
    public struct DebugEvent
    {
        public KeyCode keyCode;
        public UnityEvent debugEvent;
    }

    public DebugEvent[] events;

    private void Update()
    {
        foreach (var evt in events)
        {
            if (Input.GetKeyDown(evt.keyCode))
            {
                evt.debugEvent.Invoke();
            }
        }
    }
}
