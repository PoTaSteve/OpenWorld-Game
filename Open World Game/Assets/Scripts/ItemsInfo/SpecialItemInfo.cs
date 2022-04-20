using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialItemInfo : Interactable
{
    public SpecialItemScriptableObject scrObj;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: IngredientInfo script
        SpecialItemInfo newSlot = Instantiate<SpecialItemInfo>(GameManager.Instance.invMan.IngredientInvSlot.GetComponent<SpecialItemInfo>(), GameManager.Instance.invMan.TabsContent[4]);

        GameManager.Instance.invMan.SpecialItemsTab.Add(scrObj.TypeID);
        newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateSpecialItemInvSlotDetails(newSlot.gameObject); });

        newSlot.scrObj = scrObj;

        newSlot.transform.GetChild(2).gameObject.SetActive(false);

        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = scrObj.icon;

        Transform rarity = newSlot.transform.GetChild(3);
        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < newSlot.scrObj.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        newSlot.transform.GetChild(4).gameObject.SetActive(true);

        Destroy(gameObject);
    }
}
