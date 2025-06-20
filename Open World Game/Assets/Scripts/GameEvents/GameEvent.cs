using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    public void Invoke(object firstData, object secondData)
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.RaiseEvent(firstData, secondData);
        }
    }

    public void Register(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void Unregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
