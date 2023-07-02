using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FirstQuest : Quest
{
    [Space]
    public NPC FirstNPC;
    public NPC SecondNPC;
    public NPC ThirdNPC;

    public TextAsset FirstNPCNormalDialogue;
    //public TextAsset FirstNPCQuestDialogue;
    public TextAsset SecondNPCNormalDialogue;
    public TextAsset SecondNPCQuestDialogue;
    public TextAsset ThirdNPCNormalDialogue;
    public TextAsset ThirdNPCQuestDialogue;

    public override void ContinueQuest()
    {
        // Talk to NPC 1 -> Start quest
        if (currStep == 0)
        {
            // Increase step
            currStep++;

            // Change previous NPC dialogue
            FirstNPC.inkJSON = FirstNPCNormalDialogue;

            // Change next NPC dialogue
            SecondNPC.inkJSON = SecondNPCQuestDialogue;
        }

        // Talk to NPC 2 -> Continue quest
        else if (currStep == 1)
        {
            // Increase step
            currStep++;

            // Change previous NPC dialogue
            SecondNPC.inkJSON = SecondNPCNormalDialogue;

            // Change next NPC dialogue
            ThirdNPC.inkJSON = ThirdNPCQuestDialogue;
        }

        // Talk to NPC 3 -> Complete quest
        else if (currStep == 2)
        {
            // Change previous NPC dialogue
            ThirdNPC.inkJSON = ThirdNPCNormalDialogue;

            CompleteQuest();
        }

        else
        {
            Debug.Log("Error loading next quest step.");

            return;
        }
    }
}
