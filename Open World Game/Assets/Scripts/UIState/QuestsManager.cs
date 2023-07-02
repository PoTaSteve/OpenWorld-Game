using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    STORY = 0,
    TYPE1 = 1,
    TYPE2 = 2,
    CHALLENGE = 3
}

public enum QuestStepType
{
    DIALOGUE, // Completed by only talking
    COLLECT, // Talk to NPC and give him the requested items
    KILL, // Kill targets
    TRAVEL // Reach a place (Enter a trigger)
}

public class QuestsManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI QuestNameTxt;
    public TextMeshProUGUI QuestGiverTxt;
    public TextMeshProUGUI QuestPlaceTxt;
    public TextMeshProUGUI QuestDescriptionTxt;
    public TextMeshProUGUI QuestStepDescriptionTxt;
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

    public QuestScrObj currSelectedQuest;

    public List<string> onGoingQuestsID = new List<string>();
    public List<string> CompletedQuestsID = new List<string>();

    public List<Quest> AllQuests = new List<Quest>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToQuestTab(int tab)
    {
        if (tab < 0 || tab > 3 || tab == currQuestTab)
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
        //UpdateQuestDetails();
    }

    public void UpdateQuestDetails(Quest quest)
    {
        if (quest == null) return;

        QuestNameTxt.text = quest.questScrObj.questName;
        QuestGiverTxt.text = quest.questScrObj.questGiver;
        QuestPlaceTxt.text = quest.questScrObj.questPlace;
        QuestDescriptionTxt.text = quest.questScrObj.questDescription;
        QuestStepDescriptionTxt.text = quest.questScrObj.stepDescriptions[quest.currStep];
    }

    public void CheckQuestProgress(string questID)
    {
        Quest quest = null;

        foreach (Quest q in AllQuests)
        {
            if (q.questScrObj.questID == questID)
            {
                quest = q;
            }
        }

        if (quest == null) return;

        // Check if the quest has been already started or is new
        // if new call StartQuest()
        // else ContinueQuest()
        if (onGoingQuestsID.Contains(questID))
        {
            quest.ContinueQuest();
        }
        else
        {
            quest.StartQuest();
        }
    }
}
