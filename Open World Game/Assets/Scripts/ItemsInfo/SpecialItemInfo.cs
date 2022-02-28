using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialItemInfo : Interactable
{
    public SpecialItemScriptableObject SpecialItemSO;

    public override void Interact()
    {
        // Instantiate Slot: IngredientInfo script
        SpecialItemInfo newSlot = Instantiate<SpecialItemInfo>(InventoryManager.Instance.IngredientInvSlot.GetComponent<SpecialItemInfo>(), InventoryManager.Instance.TabsContent[4]);

        InventoryManager.Instance.SpecialItemsTab.Add(newSlot);
        InventoryManager.Instance.SpecialItemsTabStr.Add(SpecialItemSO.specialItemName);
        newSlot.GetComponent<Button>().onClick.AddListener(InventoryManager.Instance.UpdateSpecialItemInvSlotDetails);

        newSlot.SpecialItemSO = SpecialItemSO;

        newSlot.transform.GetChild(2).gameObject.SetActive(false);

        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = SpecialItemSO.icon;

        Transform rarity = newSlot.transform.GetChild(3);
        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < newSlot.SpecialItemSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        newSlot.transform.GetChild(4).gameObject.SetActive(true);
    }
}
