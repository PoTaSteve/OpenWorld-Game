using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

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
                GameManager.Instance.plInMan.ExitDialogueSequence();
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
                GameManager.Instance.plInMan.ExitDialogueSequence();
            }
        }
    }

    void LoadStoryLine()
    {
        string text = story.Continue();

        dialogue.text = text;

        List<string> tags = story.currentTags;

        speaker.text = tags[0];

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

    public void BindFunctionToChoice(string s)
    {
        // When creating a choice check for a tag 
        // That tag will decide which function to bind to the button (if sequence)
        // Tags are gotten right after the narrator finish to speak
    }
}
