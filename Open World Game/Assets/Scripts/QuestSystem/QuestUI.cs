using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [HideInInspector]
    public Quest quest;

    public Image questIcon;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questPlace;
    public GameObject HighlightObj;

    private Animator anim;

    public void OnEnable()
    {
        if (quest.questState == QuestState.COMPLETED)
        {
            anim.SetBool("isCompleted", true);
        }
        else if (quest.questState == QuestState.STARTED_TRACKING)
        {
            anim.SetBool("isTracking", true);
        }
        else if (quest.questState == QuestState.STARTED_NOT_TRACKING)
        {
            anim.SetBool("isTracking", false);
        }
        else
        {
            Debug.Log("Error setting quest UI animator state");
        }
    }

    public void InitializeQuestUI(Quest quest)
    {
        anim = GetComponent<Animator>();

        this.quest = quest;

        questName.text = quest.questScrObj.questName;
        questPlace.text = quest.questScrObj.questPlace;
    }

    public void CompleteQuestUI()
    {
        // Move quest to the bottom
        gameObject.transform.SetAsLastSibling();
    }

    public void ClickQuest()
    {
        // Update quest details
        GameManager.Instance.QuestUIMan.UpdateQuestDetails(quest);
    }

    public void ClickTrackQuest()
    {
        bool tracking = anim.GetBool("isTracking");

        if (!tracking)
        {
            ClickQuest();

            // Start tracking and disable previous tracking
            Quest trackingQuest = GameManager.Instance.QuestsMan.trackingQuest;

            if (trackingQuest.questScrObj != null)
            {
                trackingQuest.questState = QuestState.STARTED_NOT_TRACKING;

                trackingQuest.questUI.StopTrackingUI();
            }

            trackingQuest = quest;

            StartTrackingUI();

            trackingQuest.questState = QuestState.STARTED_TRACKING;
        }
    }

    public void StartTrackingUI()
    {
        anim.SetBool("isTracking", true);
    }

    public void StopTrackingUI()
    {
        anim.SetBool("isTracking", false);
    }
}
