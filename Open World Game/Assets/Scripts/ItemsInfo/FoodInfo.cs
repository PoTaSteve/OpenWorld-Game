using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodInfo : Interactable
{
    public FoodScriptableObject scrObj;

    public int count;

    public override void Interact()
    {
        // Instantiate Slot: FoodInfo script
        if (GameManager.Instance.invMan.FoodTab.Contains(scrObj.TypeID))
        {
            // Add the count
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[3].transform)
            {
                FoodInfo info = t.GetComponent<FoodInfo>();
                if (info.scrObj.foodName == scrObj.foodName)
                {
                    info.count += count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            FoodInfo newSlot = Instantiate<FoodInfo>(GameManager.Instance.invMan.FoodInvSlot.GetComponent<FoodInfo>(), GameManager.Instance.invMan.TabsContent[3]);

            GameManager.Instance.invMan.FoodTab.Add(scrObj.TypeID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateFoodInvSlotDetails(newSlot.gameObject); });

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

            if (newSlot.scrObj.foodType == FoodType.Potion)
            {
                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.PotionFoodTypeIcon;
            }
            else if (newSlot.scrObj.foodType == FoodType.Food)
            {
                if (newSlot.scrObj.buffType == FoodBuffType.Attack)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.AttackFoodBuffIcon;
                }
                else if (newSlot.scrObj.buffType == FoodBuffType.Defence)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.DefenceFoodBuffIcon;
                }
                else if (newSlot.scrObj.buffType == FoodBuffType.Heal)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.HealFoodBuffIcon;
                }
                else if (newSlot.scrObj.buffType == FoodBuffType.Regen)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.RegenFoodBuffIcon;
                }
                else if (newSlot.scrObj.buffType == FoodBuffType.Speed)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.SpeedFoodBuffIcon;
                }
                else if (newSlot.scrObj.buffType == FoodBuffType.StaminaConsumption)
                {
                    newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.StaminaConsumptionFoodBuffIcon;
                }
            }

            newSlot.transform.GetChild(6).gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }
}
