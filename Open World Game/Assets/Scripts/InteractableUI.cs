using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : Interactable
{
    public override void Interact()
    {
        Debug.Log("UI interaction");
    }
}
