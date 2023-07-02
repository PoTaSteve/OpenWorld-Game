using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Quest : MonoBehaviour
{
    public QuestScrObj questScrObj;
    
    [HideInInspector]
    public int currStep;
    [HideInInspector]
    public bool isCompleted;
    [HideInInspector]
    public bool isTracking;

    [HideInInspector]
    public QuestUI questUI;

    // Initialize values
    // Add quest to OngoingQuests
    // Add quest to UI
    // Update UI
    public void StartQuest()
    {
        currStep = 0;

        QuestsManager questsManager = GameManager.Instance.QuestsMan;

        questsManager.onGoingQuestsID.Add(questScrObj.questID);

        GameObject questUIObj = Instantiate(questsManager.questUIPrefab, questsManager.questTabContent[(int)questScrObj.questType]);

        questUI = questUIObj.GetComponent<QuestUI>();

        questUI.InitializeQuestUI(this);

        ContinueQuest();
    }

    // Remove quest from OngoingQuests
    // Add to CompletedQuests
    // Update values and UI
    public void CompleteQuest()
    {
        QuestsManager questsManager = GameManager.Instance.QuestsMan;

        questsManager.onGoingQuestsID.Remove(questScrObj.questID);

        questsManager.CompletedQuestsID.Add(questScrObj.questID);

        questUI.CompleteQuestUI();
    }

    // Check for next step
    // Increase step
    // If step was last -> CompleteQuest()
    public abstract void ContinueQuest();
    // Dialogue part of quest managed by dialMan
}
