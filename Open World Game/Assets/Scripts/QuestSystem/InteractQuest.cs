using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractQuest : Interactable
{
    public string questID;

    public bool isQuestActive;

    public TextAsset questInactiveText;

    [Space]
    public ItemInfo item;
    //public ItemType itemType;

    //[DrawIf("itemType", ItemType.Weapon)]
    //public WeaponScriptableObject weaponScrObj;

    //[DrawIf("itemType", ItemType.Material)]
    //public MaterialScriptableObject materialScrObj;

    //[DrawIf("itemType", ItemType.Food)]
    //public FoodScriptableObject foodScrObj;

    //[DrawIf("itemType", ItemType.SpecialItem)]
    //public SpecialItemScriptableObject specialItemScrObj;

    public override void Interact()
    {
        if (isQuestActive)
        {
            Debug.Log("Starting interact quest");

            GameManager.Instance.QuestsMan.CheckQuestProgress(questID);
        }
        else
        {
            GameManager.Instance.plInputMan.SetDialogueUI();

            GameManager.Instance.dialMan.inkJSON = questInactiveText;

            GameManager.Instance.dialMan.StartDialogue();
        }
    }
}
