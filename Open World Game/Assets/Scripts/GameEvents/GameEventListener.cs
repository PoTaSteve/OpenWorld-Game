using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<object, object> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomGameEvent customGameEvent;

    private void Awake()
    {
        gameEvent.Register(this);
    }

    private void OnDestroy()
    {
        gameEvent.Unregister(this);
    }

    public void RaiseEvent(object firstData, object secondData)
    {
        //unityEvent.Invoke();
        customGameEvent.Invoke(firstData, secondData);
    }
}
