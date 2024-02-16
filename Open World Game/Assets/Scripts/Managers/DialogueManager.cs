using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Specialized;
using JetBrains.Annotations;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJSON;
    private Story story;

    public TextMeshProUGUI dialogue;
    
    public TextMeshProUGUI speaker;
    
    public Transform choiceParent;
    [SerializeField]
    private GameObject choicePrefab;
    
    public GameObject DialogueWindow;

    public bool needToChoose;

    public GameObject NPC;

    private string currCustomLine;

    public void StartDialogue()
    {
        needToChoose = false;

        story = new Story(inkJSON.text);

        if (!needToChoose)
        {
            if (story.canContinue)
            {
                LoadStoryLine();
            }
            else
            {
                GameManager.Instance.plInputMan.ExitDialogueSequence(); // Disable dialogue UI
            }
        }
    }

    public void ContinueDialogue()
    {
        if (!needToChoose)
        {
            if (story.canContinue)
            {
                LoadStoryLine();
            }
            else
            {
                GameManager.Instance.plInputMan.ExitDialogueSequence();
            }
        }
    }

    void LoadStoryLine()
    {
        string text = story.Continue();

        dialogue.text = text;

        List<string> tags = story.currentTags;

        speaker.text = tags[0];

        // Check for other tags
        if (tags.Count > 1)
        {
            CheckOtherTag();
        }

        if (story.currentChoices.Count > 0)
        {
            needToChoose = true;

            foreach (Choice choice in story.currentChoices)
            {
                GameObject newChoice = Instantiate(choicePrefab, choiceParent);                

                newChoice.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

                newChoice.GetComponent<Button>().onClick.AddListener(delegate { MakeChoice(choice); });
            }
        }
    }

    public void MakeChoice(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);

        needToChoose = false;

        foreach (Transform t in choiceParent.transform)
        {
            Destroy(t.gameObject);
        }

        ContinueDialogue();
    }

    // Checks the other tag in a conversation
    public void CheckOtherTag()
    {
        string tag = story.currentTags[1];

        switch (tag)
        {
            case "Shop":
                // Open Shop
                NPC.GetComponent<Shop>().OpenShop();

                break;

            case "Quest":
                string questID = story.currentTags[2];

                GameManager.Instance.QuestsMan.CheckQuestProgress(questID);

                break;

            case "Give item":
                

                break;

            default:
                break;
        }

        if (story.currentTags.Contains("Custom line"))
        {
            dialogue.text = currCustomLine;
        }
    }
}
