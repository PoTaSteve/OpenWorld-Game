using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialInfo : Interactable
{
    public MaterialScriptableObject scrObj;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: MaterialInfo script

        if (GameManager.Instance.invMan.MaterialsTab.Contains(scrObj.TypeID))
        {
            // Update the count 
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[1].transform)
            {
                MaterialInfo info = t.GetComponent<MaterialInfo>();
                if (info.scrObj.materialName == scrObj.materialName)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            MaterialInfo newSlot = Instantiate<MaterialInfo>(GameManager.Instance.invMan.MaterialInvSlot.GetComponent<MaterialInfo>(), GameManager.Instance.invMan.TabsContent[1]);

            GameManager.Instance.invMan.MaterialsTab.Add(scrObj.TypeID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateMaterialInvSlotDetails(newSlot.gameObject); });

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

            if (newSlot.scrObj.materialType == MaterialTypeEnum.CrafingIngredient)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.CraftingIngredientIcon;
            }
            else
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.CraftingResultIcon;
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    public string StringMaterialType()
    {
        string type;

        if (scrObj.materialType == MaterialTypeEnum.CrafingIngredient)
        {
            type = "Ingredient";
        }
        else if (scrObj.materialType == MaterialTypeEnum.CrafingResult)
        {
            type = "Result";
        }
        else
        {
            type = "Error";
        }

        return type;
    }
}
