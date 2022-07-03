using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientInfo : Interactable
{
    public IngredientScriptableObject scrObj;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: IngredientInfo script
        if (GameManager.Instance.invMan.IngredientsTab.Contains(scrObj.ItemID))
        {
            // Add the count
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[2].transform)
            {
                IngredientInfo info = t.GetComponent<IngredientInfo>();
                if (info.scrObj.ItemID == scrObj.ItemID)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            IngredientInfo newSlot = Instantiate<IngredientInfo>(GameManager.Instance.invMan.IngredientInvSlot.GetComponent<IngredientInfo>(), GameManager.Instance.invMan.TabsContent[2]);

            GameManager.Instance.invMan.IngredientsTab.Add(scrObj.ItemID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateIngredientInvSlotDetails(newSlot.gameObject); });

            newSlot.scrObj = scrObj;
            newSlot.count = count;

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

            newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

            if (newSlot.scrObj.ingredientType == IngredientType.Base)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.BaseIngredientIcon;
            }
            else if (newSlot.scrObj.ingredientType == IngredientType.Specific)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.SpecificIngredientIcon;
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    public string StringIngredientType()
    {
        string type;

        if (scrObj.ingredientType == IngredientType.Base)
        {
            type = "Base";
        }
        else if (scrObj.ingredientType == IngredientType.Specific)
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
