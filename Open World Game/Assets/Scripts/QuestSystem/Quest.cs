using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NOT_UNLOCKED, // Not in world (Conditions to start quest are not met)
    NOT_STARTED, // In world but not started (Conditionts are met but not started the quest yet)
    STARTED_TRACKING,
    STARTED_NOT_TRACKING,
    COMPLETED
}

[Serializable]
public class Quest
{
    // when saving save this to determine if the quest should be enabled on launch
    public bool toEnableAtStart;

    public QuestScrObj questScrObj;

    public QuestState questState;

    [HideInInspector]
    public QuestUI questUI;

    public List<QuestStep> steps = new List<QuestStep>();
    
    public int currStep;

    public bool doesCompleteSetAccessible;
    [DrawIf("doesCompleteSetAccessible", true)]
    public QuestIDToSetAccessible questIDToSetAccessible;

    public bool doesCompleteStartQuest;
    [DrawIf("doesCompleteStartQuest", true)]
    public string questIDToStart;

    public void SetState(QuestState state)
    {
        questState = state;
    }
}

[Serializable]
public class QuestIDToSetAccessible
{
    public List<string> ID = new List<string>();
}
