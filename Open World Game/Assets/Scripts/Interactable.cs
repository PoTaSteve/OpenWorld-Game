using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteracionType
{
    PickUp,
    NPC,
    Shop
}

public abstract class Interactable : MonoBehaviour
{
    public Sprite icon;
    [Tooltip("String to display on the UI to interact with the item")]
    public string interactionName;
    public InteracionType interacionType;

    public abstract void Interact();
}
