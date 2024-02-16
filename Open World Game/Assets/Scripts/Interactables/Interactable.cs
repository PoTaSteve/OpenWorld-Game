using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteracionType
{
    PickUp,
    Inspect,
    Quest,
    NPC,
    Shop
}

public abstract class Interactable : MonoBehaviour
{
    [Tooltip("String to display on the UI to interact with the item")]
    public string interactionName;
    //public InteracionType interacionType;

    public abstract void Interact();
}
