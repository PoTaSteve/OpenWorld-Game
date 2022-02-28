using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialInfo : Interactable
{
    public MaterialScriptableObject MaterialSO;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: MaterialInfo script

        if (InventoryManager.Instance.MaterialsTabStr.Contains(MaterialSO.materialName))
        {
            // Update the count 
            foreach (Transform t in InventoryManager.Instance.TabsContent[1].transform)
            {
                MaterialInfo info = t.GetComponent<MaterialInfo>();
                if (info.MaterialSO.materialName == MaterialSO.materialName)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            MaterialInfo newSlot = Instantiate<MaterialInfo>(InventoryManager.Instance.MaterialInvSlot.GetComponent<MaterialInfo>(), InventoryManager.Instance.TabsContent[1]);

            InventoryManager.Instance.MaterialsTab.Add(newSlot);
            InventoryManager.Instance.MaterialsTabStr.Add(MaterialSO.materialName);
            newSlot.GetComponent<Button>().onClick.AddListener(InventoryManager.Instance.UpdateMaterialInvSlotDetails);

            newSlot.MaterialSO = MaterialSO;
            newSlot.count = count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = MaterialSO.icon;

            Transform rarity = newSlot.transform.GetChild(3);
            foreach (Transform t in rarity)
            {
                t.gameObject.SetActive(false);
            }
            for (int i = 0; i < newSlot.MaterialSO.rarity; i++)
            {
                rarity.GetChild(i).gameObject.SetActive(true);
            }

            newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

            if (newSlot.MaterialSO.materialType == MaterialTypeEnum.CrafingIngredient)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.CraftingIngredientIcon;
            }
            else
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.CraftingResultIcon;
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }
    }

    public string StringMaterialType()
    {
        string type;

        if (MaterialSO.materialType == MaterialTypeEnum.CrafingIngredient)
        {
            type = "Ingredient";
        }
        else if (MaterialSO.materialType == MaterialTypeEnum.CrafingResult)
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
