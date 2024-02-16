using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public TextMeshProUGUI QuestNameTxt;
    public TextMeshProUGUI QuestGiverTxt;
    public TextMeshProUGUI QuestPlaceTxt;
    public TextMeshProUGUI QuestDescriptionTxt;
    public TextMeshProUGUI CurrentStepDescriptionTxt;
    [Space]
    public GameObject QuestDetailsContentObj;
    public GameObject QuestRewardObj;
    public Transform QuestRewardParent;
    public GameObject QuestRewardPrefab;
    public GameObject QuestCompletedObj;
    [Space]
    public GameObject QuestDetailsObj;
    public GameObject NoQuestDetailsObj;
    [Space]
    public GameObject TrackingQuestUIObj;
    public TextMeshProUGUI TrackingQuestName;
    public TextMeshProUGUI TrackingQuestStep;
    [Space]
    public Color questIconDisabledColor;
    public Color questIconEnabledColor;
    [Space]
    public Image[] questTypeIcon = new Image[4];
    [Space]
    public GameObject[] questTypeSelected = new GameObject[4];
    [Space]
    public GameObject[] questTypeTextObj = new GameObject[4];
    [Space]
    public GameObject[] questTab = new GameObject[4];
    [Space]
    public Transform[] questTabContent = new Transform[4];
    [Space]
    public int currQuestTab;

    public GameObject questUIPrefab;

    public Quest currSelectedQuest;

    public GameObject QuestGoalObj;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetQuestUIActive()
    {
        GameManager.Instance.plInputMan.CommonUIObject.SetActive(true);

        GameManager.Instance.plInputMan.QuestsUIObject.SetActive(true);
        GameManager.Instance.plInputMan.InventoryObj.SetActive(false);
        GameManager.Instance.plInputMan.SkillsUIObject.SetActive(false);
        GameManager.Instance.plInputMan.SystemUIObject.SetActive(false);

        // Update the quest description on the right
        if (questTabContent[currQuestTab].childCount > 0)
        {
            ChangeToQuestTab(currQuestTab);
        }
    }


    public void ChangeToQuestTab(int tab)
    {
        if (tab < 0 || tab > 3)
        {
            return;
        }

        // Do with animations!!!

        questTypeIcon[currQuestTab].color = questIconDisabledColor;
        questTypeSelected[currQuestTab].SetActive(false);
        questTypeTextObj[currQuestTab].SetActive(false);
        questTab[currQuestTab].SetActive(false);

        currQuestTab = tab;

        questTypeIcon[currQuestTab].color = questIconEnabledColor;
        questTypeSelected[currQuestTab].SetActive(true);
        questTypeTextObj[currQuestTab].SetActive(true);
        questTab[currQuestTab].SetActive(true);

        // Update the quest description on the right
        if (questTabContent[tab].childCount > 0)
        {
            QuestDetailsObj.SetActive(true);
            NoQuestDetailsObj.SetActive(false);

            UpdateQuestDetails(questTabContent[tab].GetChild(0).GetComponent<QuestUI>().quest);
        }
        else
        {
            QuestDetailsObj.SetActive(false);
            NoQuestDetailsObj.SetActive(true);
        }
    }


    public GameObject AddQuestUI(int questType)
    {
        return Instantiate(questUIPrefab, questTabContent[questType]);
    }


    public void UpdateQuestDetails(Quest quest)
    {
        if (quest == null) return;

        QuestNameTxt.text = quest.questScrObj.questName;
        QuestGiverTxt.text = quest.questScrObj.questGiver;
        QuestPlaceTxt.text = quest.questScrObj.questPlace;
        QuestDescriptionTxt.text = quest.questScrObj.questDescription;

        if (currSelectedQuest.questUI != null)
        {
            currSelectedQuest.questUI.HighlightObj.SetActive(false);
        }
        currSelectedQuest = quest;
        quest.questUI.HighlightObj.SetActive(true);

        if (quest.questState == QuestState.COMPLETED)
        {
            QuestCompletedObj.SetActive(true);
            QuestRewardObj.SetActive(false);

            CurrentStepDescriptionTxt.gameObject.SetActive(false);

            // Update height only for description
            QuestDescriptionTxt.gameObject.GetComponent<FixHeight>().UpdateHeight();

            FixHeight contentFix = QuestDetailsContentObj.GetComponent<FixHeight>();

            contentFix.multiplier = 1;
            contentFix.UpdateHeight();
        }
        else
        {
            QuestRewardObj.SetActive(true);
            QuestCompletedObj.SetActive(false);

            CurrentStepDescriptionTxt.gameObject.SetActive(true);

            string currStepDescription = "<b><color=yellow>Current step:</color></b>\n";

            currStepDescription += DetermineStepDescription(quest);

            CurrentStepDescriptionTxt.text = currStepDescription;

            // Update height for description and step
            CurrentStepDescriptionTxt.gameObject.GetComponent<FixHeight>().UpdateHeight();
            QuestDescriptionTxt.gameObject.GetComponent<FixHeight>().UpdateHeight();

            FixHeight contentFix = QuestDetailsContentObj.GetComponent<FixHeight>();

            contentFix.multiplier = 2;
            contentFix.UpdateHeight();

            // Clear rewards
            foreach (Transform t in QuestRewardParent)
            {
                Destroy(t.gameObject);
            }

            // Create new rewards
            foreach (QuestReward reward in quest.questScrObj.questRewards)
            {
                GameObject rewardObj = Instantiate(QuestRewardPrefab, QuestRewardParent);

                switch (reward.type)
                {
                    case QuestRewardType.MONEY:
                        rewardObj.transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = reward.ammount.ToString();
                        break;

                    case QuestRewardType.WEAPON:
                        rewardObj.transform.GetChild(2).GetComponent<Image>().sprite = reward.weapScrObj.itemIcon;
                        rewardObj.transform.GetChild(3).gameObject.SetActive(false);
                        break;

                    default:
                        Debug.Log("Error: reward type not found.");
                        break;
                }
            }
        }
    }


    public string DetermineStepDescription(Quest quest)
    {
        string currStepDescription = quest.steps[quest.currStep].stepDescription;

        if (quest.steps[quest.currStep].stepType == QuestStepType.COLLECT_ITEM)
        {
            string collectItemList = "";

            foreach (ID_Curr_Ammount itemToCollect in quest.steps[quest.currStep].collectItemQuest.items)
            {
                collectItemList += "\n   •  " + itemToCollect.objName + ": " + itemToCollect.currAmount + "/" + itemToCollect.amount;
            }

            currStepDescription += collectItemList;
        }
        else if (quest.steps[quest.currStep].stepType == QuestStepType.KILL)
        {
            string collectItemList = "";

            foreach (ID_Curr_Ammount itemToCollect in quest.steps[quest.currStep].killQuest.targets)
            {
                collectItemList += "\n   •  " + itemToCollect.objName + ": " + itemToCollect.currAmount + "/" + itemToCollect.amount;
            }

            currStepDescription += collectItemList;
        }

        return currStepDescription;
    }


    // Update the tracking quest UI for the next step
    public void UpdateTrackingQuestUI(Quest quest)
    {
        TrackingQuestUIObj.GetComponent<Animator>().SetTrigger("Enable");
        TrackingQuestName.text = quest.questScrObj.questName;
        TrackingQuestStep.text = DetermineStepDescription(quest);
    }


    // Update the tracking quest UI with the "Completed style"
    public void UpdateCompletedTrackingQuestUI(Quest quest)
    {
        TrackingQuestUIObj.GetComponent<Animator>().SetTrigger("Enable");
        TrackingQuestName.text = quest.questScrObj.questName;
        TrackingQuestStep.text = "<color=green>Quest completed</color>";
    }


    // Enables for a period of time an object that shows the quest step goal
    public void TrackQuestInWorld(Quest quest)
    {

    }
}
