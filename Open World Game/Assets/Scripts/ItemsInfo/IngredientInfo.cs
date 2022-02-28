using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientInfo : Interactable
{
    public IngredientScriptableObject IngredientSO;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: IngredientInfo script
        if (InventoryManager.Instance.IngredientsTabStr.Contains(IngredientSO.ingredientName))
        {
            // Add the count
            foreach (Transform t in InventoryManager.Instance.TabsContent[2].transform)
            {
                IngredientInfo info = t.GetComponent<IngredientInfo>();
                if (info.IngredientSO.ingredientName == IngredientSO.ingredientName)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            IngredientInfo newSlot = Instantiate<IngredientInfo>(InventoryManager.Instance.IngredientInvSlot.GetComponent<IngredientInfo>(), InventoryManager.Instance.TabsContent[2]);

            InventoryManager.Instance.IngredientsTab.Add(newSlot);
            InventoryManager.Instance.IngredientsTabStr.Add(IngredientSO.ingredientName);
            newSlot.GetComponent<Button>().onClick.AddListener(InventoryManager.Instance.UpdateIngredientInvSlotDetails);

            newSlot.IngredientSO = IngredientSO;
            newSlot.count = count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = IngredientSO.icon;

            Transform rarity = newSlot.transform.GetChild(3);
            foreach (Transform t in rarity)
            {
                t.gameObject.SetActive(false);
            }
            for (int i = 0; i < newSlot.IngredientSO.rarity; i++)
            {
                rarity.GetChild(i).gameObject.SetActive(true);
            }

            newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

            if (newSlot.IngredientSO.ingredientType == IngredientType.Base)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.BaseIngredientIcon;
            }
            else if (newSlot.IngredientSO.ingredientType == IngredientType.Specific)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.SpecificIngredientIcon;
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }
    }

    public string StringIngredientType()
    {
        string type;

        if (IngredientSO.ingredientType == IngredientType.Base)
        {
            type = "Base";
        }
        else if (IngredientSO.ingredientType == IngredientType.Specific)
        {
            type = "Specific";
        }
        else
        {
            type = "Error";
        }

        return type;
    }
}
