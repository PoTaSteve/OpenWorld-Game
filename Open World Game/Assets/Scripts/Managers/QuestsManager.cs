using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using static UnityEditor.Progress;

public enum QuestType
{
    STORY = 0,
    TYPE1 = 1,
    TYPE2 = 2,
    CHALLENGE = 3
}

public class QuestsManager : MonoBehaviour
{
    [SerializeField]
    private QuestUIManager QuestUIMan;


    public List<string> onGoingQuestsID = new List<string>();
    public List<string> CompletedQuestsID = new List<string>();

    public Quest trackingQuest;

    public List<Quest> AllQuests = new List<Quest>();


    // Might create a class for each quest type
    private Dictionary<string, CollectItemQuestStep> activeCollectItemQuestIDAndItems = new Dictionary<string, CollectItemQuestStep>();
    private bool hasActiveCollectItemQuest;

    private Dictionary<string, KillQuestStep> activeKillEnemyQuestIDAndItems = new Dictionary<string, KillQuestStep>();
    private bool hasActiveKillEnemyQuest;


    public Dictionary<string, int> itemsToGive = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        EnableQuestsAtStart();
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Unlocks all quest needed at game launch
    public void EnableQuestsAtStart()
    {
        foreach (Quest quest in AllQuests)
        {
            if (quest.toEnableAtStart)
            {
                UnlockQuestInWorld(quest);
            }
        }
    }


    // Get the quest based on the ID string
    // Check if the quest needs to be started and then continues it in any case (if the quest exists)
    public void CheckQuestProgress(string questID)
    {
        Quest quest = null;

        foreach (Quest q in AllQuests)
        {
            if (q.questScrObj.questID == questID)
            {
                quest = q;
                break;
            }
        }

        if (quest == null)
        {
            Debug.LogWarning("Quest ID not found: " + questID);
            return;
        }

        // Check if the quest has been already started or is new
        if (!onGoingQuestsID.Contains(questID))
        {
            StartQuest(quest);
        }
        
        ContinueQuest(quest);
    }


    // Set step to 0 and instantiate quest UI prefab
    public void StartQuest(Quest quest)
    {
        quest.currStep = 0;

        onGoingQuestsID.Add(quest.questScrObj.questID);

        GameObject questUIObj = QuestUIMan.AddQuestUI((int)quest.questScrObj.questType);

        quest.questUI = questUIObj.GetComponent<QuestUI>();

        quest.questUI.InitializeQuestUI(quest);
    }


    // Reset, set and track new step
    // Check if quest step was last -> Complete quest
    public void ContinueQuest(Quest quest)
    {
        if (quest.currStep + 1 >= quest.steps.Count)
        {
            CompleteQuest(quest);
        }
        else
        {
            ResetLastQuestStep(quest, quest.currStep);

            quest.currStep++;

            SetQuestStep(quest, quest.currStep);

            // Set quest to tracking
            TrackQuest(quest);
        }
    }


    // Set up scene for quest step 0 and set not started state
    public void UnlockQuestInWorld(Quest quest)
    {
        SetQuestStep(quest, 0);

        quest.SetState(QuestState.NOT_STARTED);
    }


    // Update trackingQuestUI, resets last quest step, set quest state
    // Checks for quests to set accessible or to start
    public void CompleteQuest(Quest quest)
    {
        // Temp quest tracking in Game UI
        QuestUIMan.UpdateCompletedTrackingQuestUI(quest);

        onGoingQuestsID.Remove(quest.questScrObj.questID);

        CompletedQuestsID.Add(quest.questScrObj.questID);

        ResetLastQuestStep(quest, quest.currStep);

        // Change trackingQuest to a new quest -> First in order might be fine
        quest.SetState(QuestState.COMPLETED);

        quest.questUI.CompleteQuestUI();

        trackingQuest = null;

        if (quest.doesCompleteSetAccessible)
        {
            // Get the list of IDs and set quests accessible
            foreach (string ID in quest.questIDToSetAccessible.ID)
            {
                Quest newAccessibleQuest = null;

                foreach (Quest q in AllQuests)
                {
                    if (q.questScrObj.questID == ID)
                    {
                        newAccessibleQuest = q;
                    }
                }

                if (newAccessibleQuest != null)
                {
                    UnlockQuestInWorld(newAccessibleQuest);
                }                
            }
        }

        if (quest.doesCompleteStartQuest)
        {
            // Set quest active
            Quest newActiveQuest = null;

            foreach (Quest q in AllQuests)
            {
                if (q.questScrObj.questID == quest.questIDToStart)
                {
                    newActiveQuest = q;
                }
            }

            if (newActiveQuest == null)
            {
                return;
            }

            // Set quest active at step 0 without continuing
            UnlockQuestInWorld(newActiveQuest);

            StartQuest(newActiveQuest);


            // Set quest to be tracking
            TrackQuest(newActiveQuest);
        }
    }


    // Set up the scene for the next step based on the quest type
    public void SetQuestStep(Quest quest, int currStep)
    {
        QuestStepType stepType = quest.steps[currStep].stepType;

        switch (stepType)
        {
            case QuestStepType.DIALOGUE:
                NPC currDialogueNPC = quest.steps[currStep].dialogueQuest.NPC;

                currDialogueNPC.activeDialoguesDictionary.Add(quest.questScrObj.questID, quest.steps[currStep].dialogueQuest.dialogueText);

                break;

            case QuestStepType.GIVE_ITEM:
                // Set NPC dialogue
                NPC currGiveItemNPC = quest.steps[currStep].giveItemQuest.NPC;

                currGiveItemNPC.activeDialoguesDictionary.Add(quest.questScrObj.questID, quest.steps[currStep].giveItemQuest.dialogueText);

                // Set itemsToGive dictionary
                itemsToGive.Clear();

                ID_Ammount[] itemsArr = quest.steps[currStep].giveItemQuest.items;

                foreach (ID_Ammount item in itemsArr)
                {
                    itemsToGive.Add(item.ID, item.amount);
                }
                break;

            case QuestStepType.COLLECT_ITEM:

                if (activeCollectItemQuestIDAndItems.Count == 0)
                {
                    hasActiveCollectItemQuest = true;
                }

                activeCollectItemQuestIDAndItems.Add(quest.questScrObj.questID, quest.steps[currStep].collectItemQuest);

                break;

            case QuestStepType.INTERACT:
                InteractQuestStep interactStep = quest.steps[currStep].interactQuest;

                if (interactStep.toSetActive)
                {
                    interactStep.interactableObj.SetActive(true);
                }

                interactStep.interactQuest.isQuestActive = true;
                interactStep.interactQuest.questID = quest.questScrObj.questID;
                break;

            case QuestStepType.KILL:

                if (activeKillEnemyQuestIDAndItems.Count == 0)
                {
                    hasActiveKillEnemyQuest = true;
                }

                activeKillEnemyQuestIDAndItems.Add(quest.questScrObj.questID, quest.steps[currStep].killQuest);

                break;

            case QuestStepType.TRAVEL:
                GameObject trigger = Instantiate(quest.steps[currStep].travelQuest.TriggerPrefab, quest.steps[currStep].travelQuest.TriggerPos, Quaternion.identity);

                trigger.transform.localScale = quest.steps[currStep].travelQuest.TriggerScale;

                trigger.GetComponent<QuestVolumeTrigger>().questID = quest.questScrObj.questID;
                break;

            default:

                break;
        }
    }


    // Based on last step type it fixes the changes made in the world bringing it back to "normal"
    public void ResetLastQuestStep(Quest quest, int currStep)
    {
        QuestStepType stepType = quest.steps[currStep].stepType;

        switch (stepType)
        {
            case QuestStepType.DIALOGUE:
                // Reset last NPC dialogue
                NPC lastDialogueNPC = quest.steps[currStep].dialogueQuest.NPC;

                lastDialogueNPC.activeDialoguesDictionary.Remove(quest.questScrObj.questID);

                break;

            case QuestStepType.GIVE_ITEM:
                // Reset NPC dialogue
                NPC lastGiveItemNPC = quest.steps[currStep].giveItemQuest.NPC;

                lastGiveItemNPC.activeDialoguesDictionary.Remove(quest.questScrObj.questID);

                break;

            case QuestStepType.COLLECT_ITEM:

                activeCollectItemQuestIDAndItems.Remove(quest.questScrObj.questID);

                if (activeCollectItemQuestIDAndItems.Count == 0)
                {
                    hasActiveCollectItemQuest = false;
                }

                break;

            case QuestStepType.INTERACT:
                if (quest.steps[currStep].interactQuest.duringQuestCollectInteraction)
                {
                    InteractQuest interactQuest = quest.steps[currStep].interactQuest.interactQuest;

                    GameManager.Instance.invMan.AddItemToInventory(interactQuest.item, 1);

                    GameManager.Instance.plInteractMan.InRangeInteractables.Remove(quest.steps[currStep].interactQuest.interactQuest.gameObject);

                    Destroy(quest.steps[currStep].interactQuest.interactQuest.gameObject);
                }
                else
                {
                    // Simply interact and show text but don't add to inventory
                    GameManager.Instance.plInputMan.SetDialogueUI();

                    GameManager.Instance.dialMan.inkJSON = quest.steps[currStep].interactQuest.duringQuestInteractionText;

                    GameManager.Instance.dialMan.StartDialogue();

                    // Set dialogue to end dialogue
                    quest.steps[currStep].interactQuest.interactQuest.isQuestActive = false;
                    quest.steps[currStep].interactQuest.interactQuest.questInactiveText = quest.steps[currStep].interactQuest.afterQuestInteractionText;
                }
                break;

            case QuestStepType.KILL:

                activeKillEnemyQuestIDAndItems.Remove(quest.questScrObj.questID);

                if (activeKillEnemyQuestIDAndItems.Count == 0)
                {
                    hasActiveKillEnemyQuest = false;
                }

                break;

            case QuestStepType.TRAVEL:
                // Nothing happens here
                // Trigger is destroyed after the player enters (inside QuestVolumeTrigger)
                break;

            default:

                break;
        }
    }


    // Check the last tracking quest and set a proper state
    // Switch trackingQuest and set proper state
    // Update Tracking quest UI object and goal
    public void TrackQuest(Quest quest)
    {
        if (trackingQuest.questScrObj != null && trackingQuest.questState != QuestState.COMPLETED)
        {
            trackingQuest.SetState(QuestState.STARTED_NOT_TRACKING);
        }

        trackingQuest = quest;

        trackingQuest.SetState(QuestState.STARTED_TRACKING);

        QuestUIMan.UpdateTrackingQuestUI(quest);

        QuestUIMan.TrackQuestInWorld(quest);
    }


    [Tooltip("firstData: item ID of the collected item\n" +
        "secondData: quantity of the collected item")]
    public void OnItemCollected(object firstData, object secondData)
    {
        // I know an item is collected
        // Check if the collected item is inside the list of the items to collect of one of the quests
        // If it is increase the currAmount

        if (hasActiveCollectItemQuest && firstData is string && secondData is int)
        {
            List<string> completedQuests = new List<string>();

            foreach (string questID in activeCollectItemQuestIDAndItems.Keys)
            {
                bool collectedAllItems = true;

                foreach (ID_Curr_Ammount questItem in activeCollectItemQuestIDAndItems[questID].items)
                {
                    if (questItem.ID == (string)firstData)
                    {
                        questItem.currAmount += (int)secondData;

                        if (questItem.currAmount > questItem.amount)
                        {
                            questItem.currAmount = questItem.amount;
                        }
                    }

                    if (questItem.currAmount < questItem.amount)
                    {
                        collectedAllItems = false;
                    }
                }

                if (collectedAllItems)
                {
                    // Quest is completed
                    completedQuests.Add(questID);
                }
            }

            foreach (string questID in completedQuests)
            {
                CheckQuestProgress(questID);
            }
        }
    }

    [Tooltip("firstData: Enemy ID\n" +
        "secondData: Enemy name")]
    public void OnEnemyKilled(object firstData, object secondData)
    {
        if (hasActiveKillEnemyQuest && firstData is string)
        {
            List<string> completedQuests = new List<string>();

            foreach (string questID in activeKillEnemyQuestIDAndItems.Keys)
            {
                bool killedAllEnemies = true;

                foreach (ID_Curr_Ammount questEnemy in activeKillEnemyQuestIDAndItems[questID].targets)
                {
                    if (questEnemy.ID == (string)firstData)
                    {
                        questEnemy.currAmount++;
                    }

                    if (questEnemy.currAmount < questEnemy.amount)
                    {
                        killedAllEnemies = false;
                    }
                }

                if (killedAllEnemies)
                {
                    // Quest is completed
                    completedQuests.Add(questID);
                }
            }

            foreach (string questID in completedQuests)
            {
                CheckQuestProgress(questID);
            }
        }
    }
}
