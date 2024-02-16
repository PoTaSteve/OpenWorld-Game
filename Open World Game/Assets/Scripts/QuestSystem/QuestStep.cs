using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum QuestStepType
{
    DIALOGUE, // Completed by only talking
    GIVE_ITEM, // Talk to NPC and give him the requested items
    COLLECT_ITEM, // Collect items
    INTERACT, // Interact with an interacable
    KILL, // Kill targets
    TRAVEL // Reach a place (Enter a trigger)
}

[Serializable]
public class ID_Ammount
{
    public string objName;
    public string ID;
    public int amount;
}

[Serializable]
public class ID_Curr_Ammount
{
    public string objName;
    public string ID;
    //[HideInInspector]
    public int currAmount;
    public int amount;
}

[Serializable]
public class QuestStep
{
    public QuestStepType stepType;

    [TextArea(3, 5)]
    public string stepDescription;

    public GameObject QuestGoal;

    // Dialogue vars
    [DrawIf("stepType", QuestStepType.DIALOGUE)]
    public DialogueQuestStep dialogueQuest;


    // Give vars
    [DrawIf("stepType", QuestStepType.GIVE_ITEM)]
    public GiveItemQuestStep giveItemQuest;


    // Collect vars
    [DrawIf("stepType", QuestStepType.COLLECT_ITEM)]
    public CollectItemQuestStep collectItemQuest;


    // Interact vars
    [DrawIf("stepType", QuestStepType.INTERACT)]
    public InteractQuestStep interactQuest;


    // Kill vars
    [DrawIf("stepType", QuestStepType.KILL)]
    public KillQuestStep killQuest;


    // Travel vars
    [DrawIf("stepType", QuestStepType.TRAVEL)]
    public TravelQuestStep travelQuest;
}

[Serializable]
public class DialogueQuestStep
{
    public NPC NPC;
    public TextAsset dialogueText;
}

[Serializable]
public class GiveItemQuestStep
{
    public NPC NPC;
    public TextAsset dialogueText;
    public ID_Ammount[] items;
}

[Serializable]
public class CollectItemQuestStep
{
    public ID_Curr_Ammount[] items;
}

[Serializable]
public class InteractQuestStep
{
    [Tooltip("True: interactableObj is not in scene when quest is disabled and needs to be set active\n" +
        "False: interactableObj is already in scene when quest is disabled and interaction displays a text")]
    public bool toSetActive;

    [Tooltip("True: interactable is added to the inventory when interacting during quest\n" +
        "False: interactable displays a text when interacting during quest")]
    public bool duringQuestCollectInteraction;

    [DrawIf("duringQuestCollectInteraction", false)]
    public TextAsset duringQuestInteractionText;
    [DrawIf("duringQuestCollectInteraction", false)]
    public TextAsset afterQuestInteractionText;

    public GameObject interactableObj;
    public InteractQuest interactQuest;
}

[Serializable]
public class KillQuestStep
{
    public ID_Curr_Ammount[] targets;
}

[Serializable]
public class TravelQuestStep
{
    public GameObject TriggerPrefab;
    public Vector3 TriggerPos;
    public Vector3 TriggerScale;
}
