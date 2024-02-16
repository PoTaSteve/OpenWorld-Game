using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : Interactable
{
    public TextAsset defaultDialogue;

    // questID, dialogue
    public Dictionary<string, TextAsset> activeDialoguesDictionary = new Dictionary<string, TextAsset>();

    private TextAsset currDialogue;

    public override void Interact()
    {
        // Start dialogue
        GameManager.Instance.plInputMan.SetDialogueUI();

        // Set currDialogue
        currDialogue = DetermineDialogue();

        GameManager.Instance.dialMan.inkJSON = currDialogue;

        GameManager.Instance.dialMan.NPC = gameObject;
        GameManager.Instance.dialMan.StartDialogue();
    }

    public TextAsset DetermineDialogue()
    {
        if (activeDialoguesDictionary.Count > 0)
        {
            // I have to choose either the first one or the one being tracked
            if (GameManager.Instance.QuestsMan.trackingQuest.questScrObj != null)
            {
                if (activeDialoguesDictionary.TryGetValue(GameManager.Instance.QuestsMan.trackingQuest.questScrObj.questID, out TextAsset dialogue))
                {
                    return dialogue;
                }
            }

            return activeDialoguesDictionary.First().Value;
        }

        return defaultDialogue;
    }
}
