using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public TextAsset inkJSON;

    public override void Interact()
    {
        // Start dialogue
        GameManager.Instance.plInMan.StartDialogue();
        GameManager.Instance.dialMan.inkJSON = inkJSON;
        GameManager.Instance.dialMan.StartDialogue();
    } 
}
