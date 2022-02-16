using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public PlayerInputManager plInMan;

	private NPCTalk npc;

	public GameObject ShopObj;

	//public static event Action<Story> OnCreateStory;
	public TextAsset inkJSONAsset;

	public Story story;

	// UI Prefabs
	[SerializeField]
	private TextMeshProUGUI dialogue;
	[SerializeField]
	private TextMeshProUGUI speakingChar;
	[SerializeField]
	private Transform Answers;
	[SerializeField]
	private GameObject AnswerUIPrefab;

	private bool isWaiting;
	public bool isWritingText;
	public bool canSkipText;

	public bool isInDialogueMode;


	// Creates a new Story object with the compiled story which we can then play!
	public void StartStory(NPCTalk NPC)
	{
		plInMan.FreeLookCam.SetActive(false);

		npc = NPC;

		isWaiting = true;
		isWritingText = false;
		canSkipText = false;

		isInDialogueMode = true;

		plInMan.GameUIObj.SetActive(false);
		gameObject.SetActive(true);

		story = new Story(npc.inkJSONAsset.text);

		ContinueDialogue();
	}

	public void ContinueDialogue()
	{
		if (story.canContinue)
		{
			string text = story.Continue();

			string[] arr = text.Split('>');

			string charName = arr[0];
			string restText = arr[1];

			speakingChar.text = charName;
			Vector2 dim = new Vector2(speakingChar.preferredWidth + 20f, speakingChar.gameObject.GetComponent<RectTransform>().sizeDelta.y);
			speakingChar.gameObject.GetComponent<RectTransform>().sizeDelta = dim;
			StartCoroutine(TypeText(restText));
		}
	}

	public IEnumerator TypeText(string sentence)
	{
		dialogue.text = "";
		isWritingText = true;

		foreach (char c in sentence.ToCharArray())
		{
			if (canSkipText)
			{
				dialogue.text = sentence;
				break;
			}
			dialogue.text += c;
			yield return new WaitForSeconds(0.07f);
		}

		StartCoroutine(WaitForInput());
	}

	public IEnumerator WaitForInput()
	{
		canSkipText = false;
		isWritingText = false;

		while (isWaiting)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
			{
				isWaiting = false;
			}
			yield return null;
		}

		if (story.canContinue)
		{
			ContinueDialogue();
		}
		else if (story.currentChoices.Count > 0)
		{
			GenerateChoices();
		}
        else
        {
			//Exit dialogue mode
			StartCoroutine(WaitToExitDialogueMode());
		}
	}

	public void GenerateChoices()
	{
        foreach (Choice choice in story.currentChoices)
        {
			GameObject NewChoice = Instantiate(AnswerUIPrefab, Answers);

			string[] text = choice.text.Split('>');

			NewChoice.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = text[0];

			if (text.Length > 1)
            {
				if (text[1] == "Shop")
				{
					NewChoice.GetComponent<Button>().onClick.AddListener(delegate { ShopObj.SetActive(true); });

					ShopObj.SetActive(true);
					Shop shop = ShopObj.GetComponent<Shop>();

					shop.Materials.Clear();
					shop.Ingredients.Clear();
					shop.Food.Clear();

					int i = 0;
                    foreach (MaterialInfo mat in npc.Materials)
                    {
						mat.count = npc.materialCount[i];
						shop.Materials.Add(mat);
						shop.MaterialsMaxCount[i] = npc.materialMaxCount[i];
						i++;
                    }

					i = 0;
					foreach (IngredientInfo ingr in npc.Ingredients)
					{
						ingr.count = npc.ingredientCount[i];
						shop.Ingredients.Add(ingr);
						shop.IngredientsMaxCount[i] = npc.ingredientCount[i];
						i++;
					}

					i = 0;
					foreach (FoodInfo food in npc.Food)
					{
						food.count = npc.foodCount[i];
						shop.Food.Add(food);
						shop.FoodMaxCount[i] = npc.foodMaxCount[i];
						i++;
					}

					shop.CreateShop();
				}
				else if (text[1] == "")
				{

				}
			}			

			NewChoice.GetComponent<Button>().onClick.AddListener(delegate { OnChoiceSelect(choice); });
		}
	}

	public void OnChoiceSelect(Choice choice)
    {
		story.ChooseChoiceIndex(choice.index);
		ClearChoicesUI();
		story.Continue();
		ContinueDialogue();
    }

	public void ClearChoicesUI()
    {
        foreach (Transform t in Answers)
        {
			Destroy(t.gameObject);
        }
    }

	public IEnumerator WaitToExitDialogueMode()
    {
		isWaiting = true;

		while (isWaiting)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
			{
				isWaiting = false;
			}
			yield return null;
		}

		plInMan.ExitDialogueMode();

		isInDialogueMode = false;
		gameObject.SetActive(false);
	}
}

