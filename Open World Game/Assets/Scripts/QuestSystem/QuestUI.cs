using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        anim.SetBool("isCompleted", quest.isCompleted);
        anim.SetBool("isTracking", quest.isTracking);
    }

    public void InitializeQuestUI(Quest quest)
    {
        anim = GetComponent<Animator>();

        this.quest = quest;

        questIcon.sprite = quest.questScrObj.questIcon;
        questName.text = quest.questScrObj.questName;
        questPlace.text = quest.questScrObj.questPlace;

        quest.isTracking = false;
        quest.isCompleted = false;
    }

    public void CompleteQuestUI()
    {
        quest.isTracking = false;
        quest.isCompleted = true;

        // Move quest to the bottom
        gameObject.transform.SetAsLastSibling();
    }

    public void ClickQuest()
    {
        // Update quest details
        GameManager.Instance.QuestsMan.UpdateQuestDetails(quest);

        // Highlight
        HighlightObj.SetActive(true);
    }

    public void ClickTrackQuest()
    {
        quest.isTracking = !quest.isTracking;

        anim.SetBool("isTracking", quest.isTracking);
    }
}
