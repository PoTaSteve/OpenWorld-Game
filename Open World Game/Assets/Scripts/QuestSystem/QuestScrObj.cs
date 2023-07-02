using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestScrObj : ScriptableObject
{
    public string questName;
    public string questID;
    public string questGiver;
    public string questPlace;
    [TextArea(4, 10)]
    public string questDescription;
    public Sprite questIcon;

    public QuestType questType;

    public List<QuestStepType> stepsType = new List<QuestStepType>();
    public List<string> stepDescriptions = new List<string>();
}