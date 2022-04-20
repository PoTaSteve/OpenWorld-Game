using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool doesInteractionDestory;
    public int ID;
    public Sprite icon;
    [Tooltip("String to display on the UI to interact with the item")]
    public string interactionType;

    public abstract void Interact();
}
