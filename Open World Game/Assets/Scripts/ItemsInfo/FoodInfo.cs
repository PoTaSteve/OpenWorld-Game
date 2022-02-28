using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodInfo : Interactable
{
    public FoodScriptableObject FoodSO;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: FoodInfo script
        if (InventoryManager.Instance.FoodTabStr.Contains(FoodSO.foodName))
        {
            // Add the count
            foreach (Transform t in InventoryManager.Instance.TabsContent[3].transform)
            {
                FoodInfo info = t.GetComponent<FoodInfo>();
                if (info.FoodSO.foodName == FoodSO.foodName)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            FoodInfo newSlot = Instantiate<FoodInfo>(InventoryManager.Instance.FoodInvSlot.GetComponent<FoodInfo>(), InventoryManager.Instance.TabsContent[3]);

            InventoryManager.Instance.FoodTab.Add(newSlot);
            InventoryManager.Instance.FoodTabStr.Add(FoodSO.foodName);
            newSlot.GetComponent<Button>().onClick.AddListener(InventoryManager.Instance.UpdateFoodInvSlotDetails);

            newSlot.FoodSO = FoodSO;
            newSlot.count = count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = FoodSO.icon;

            Transform rarity = newSlot.transform.GetChild(3);
            foreach (Transform t in rarity)
            {
                t.gameObject.SetActive(false);
            }
            for (int i = 0; i < newSlot.FoodSO.rarity; i++)
            {
                rarity.GetChild(i).gameObject.SetActive(true);
            }

            newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

            if (newSlot.FoodSO.foodType == FoodType.Potion)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.PotionFoodTypeIcon;
            }
            else if (newSlot.FoodSO.foodType == FoodType.Food)
            {
                if (newSlot.FoodSO.buffType == FoodBuffType.Attack)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.AttackFoodBuffIcon;
                }
                else if (newSlot.FoodSO.buffType == FoodBuffType.Defence)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.DefenceFoodBuffIcon;
                }
                else if (newSlot.FoodSO.buffType == FoodBuffType.Heal)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.HealFoodBuffIcon;
                }
                else if (newSlot.FoodSO.buffType == FoodBuffType.Regen)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.RegenFoodBuffIcon;
                }
                else if (newSlot.FoodSO.buffType == FoodBuffType.Speed)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.SpeedFoodBuffIcon;
                }
                else if (newSlot.FoodSO.buffType == FoodBuffType.StaminaConsumption)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.StaminaConsumptionFoodBuffIcon;
                }
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }
    }
}
