using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestScrObj : ScriptableObject
{
    public string questName;
    public string questID;
    public string questGiver;
    public string questPlace;
    [TextArea(4, 10)]
    public string questDescription;

    public QuestType questType;

    public List<QuestReward> questRewards = new List<QuestReward>();
}

public enum QuestRewardType
{
    MONEY,
    WEAPON
}

[Serializable]
public class QuestReward
{
    public QuestRewardType type;

    [DrawIf("type", QuestRewardType.MONEY)]
    public int ammount;

    [DrawIf("type", QuestRewardType.WEAPON)]
    public WeaponScriptableObject weapScrObj;
}