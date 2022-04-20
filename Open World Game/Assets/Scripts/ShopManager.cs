using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Weapon,
    Material,
    Ingredient,
    Food,
    SpecialItem
}

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ProductPrefab;
    [SerializeField]
    private Transform ProductContent;

    [HideInInspector]
    public List<GameObject> products;
    [HideInInspector]
    public List<int> counts;

    [HideInInspector]
    public Shop currentShop;

    [SerializeField]
    private GameObject ItemDetails;

    private GameObject selectedProduct;
    private int selectedCount;

    [SerializeField]
    private GameObject buyPopUp;

    public void CreateShop()
    {
        for (int i = 0; i < products.Count; i++)
        {
            GameObject newProduct = Instantiate(ProductPrefab, ProductContent);

            newProduct.GetComponent<Button>().onClick.AddListener(delegate { UpdateDetails(newProduct); });

            ShopProduct prod = newProduct.GetComponent<ShopProduct>();

            prod.count = counts[i];

            if (products[i].TryGetComponent(out WeaponInfo weapInfo))
            {
                prod.weapInfo = weapInfo;
                prod.itemType = ItemType.Weapon;
                prod.cost = weapInfo.scrObj.buyCost;

                prod.icon = weapInfo.scrObj.icon;
                prod.itemName = weapInfo.scrObj.weaponName;
                prod.description = weapInfo.scrObj.description;

                prod.rarity = weapInfo.scrObj.rarity;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = weapInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text= weapInfo.scrObj.weaponName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text= weapInfo.scrObj.buyCost.ToString();
                if (counts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + counts[i] + "/" + counts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (products[i].TryGetComponent(out MaterialInfo matInfo))
            {
                prod.matInfo = matInfo;
                prod.itemType = ItemType.Material;
                prod.cost = matInfo.scrObj.buyCost;

                prod.icon = matInfo.scrObj.icon;
                prod.itemName = matInfo.scrObj.materialName;
                prod.description = matInfo.scrObj.description;

                prod.rarity = matInfo.scrObj.rarity;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = matInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = matInfo.scrObj.materialName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = matInfo.scrObj.buyCost.ToString();
                if (counts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + counts[i] + "/" + counts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (products[i].TryGetComponent(out IngredientInfo ingrInfo))
            {
                prod.ingrInfo = ingrInfo;
                prod.itemType = ItemType.Ingredient;
                prod.cost = ingrInfo.scrObj.buyCost;

                prod.icon = ingrInfo.scrObj.icon;
                prod.itemName = ingrInfo.scrObj.ingredientName;
                prod.description = ingrInfo.scrObj.description;

                prod.rarity = ingrInfo.scrObj.rarity;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = ingrInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = ingrInfo.scrObj.ingredientName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = ingrInfo.scrObj.buyCost.ToString();
                if (counts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + counts[i] + "/" + counts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (products[i].TryGetComponent(out FoodInfo foodInfo))
            {
                prod.foodInfo = foodInfo;
                prod.itemType = ItemType.Food;
                prod.cost = foodInfo.scrObj.buyCost;

                prod.icon = foodInfo.scrObj.icon;
                prod.itemName = foodInfo.scrObj.foodName;
                prod.description = foodInfo.scrObj.description;

                prod.rarity = foodInfo.scrObj.rarity;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = foodInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = foodInfo.scrObj.foodName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = foodInfo.scrObj.buyCost.ToString();
                if (counts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + counts[i] + "/" + counts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (products[i].TryGetComponent(out SpecialItemInfo specItemInfo))
            {
                prod.specItemInfo = specItemInfo;
                prod.itemType = ItemType.SpecialItem;
                prod.cost = specItemInfo.scrObj.buyCost;

                prod.icon = specItemInfo.scrObj.icon;
                prod.itemName = specItemInfo.scrObj.specialItemName;
                prod.description = specItemInfo.scrObj.description;

                prod.rarity = specItemInfo.scrObj.rarity;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = specItemInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = specItemInfo.scrObj.specialItemName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = specItemInfo.scrObj.buyCost.ToString();
                if (counts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + counts[i] + "/" + counts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else
            {
                Debug.Log("Error: Product object is not an item that can be sold");
            }
        }

        // Select first item
        selectedProduct = ProductContent.GetChild(0).gameObject;
        UpdateDetails(selectedProduct);
    }

    public void BuyPopUp()
    {
        buyPopUp.SetActive(true);

        ShopProduct prod = selectedProduct.GetComponent<ShopProduct>();

        Transform window = buyPopUp.transform.GetChild(1);

        window.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = "1"; // Min
        window.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedProduct.GetComponent<ShopProduct>().count.ToString(); // Max

        window.GetChild(6).GetComponent<Slider>().minValue = 1;
        window.GetChild(6).GetComponent<Slider>().maxValue = selectedProduct.GetComponent<ShopProduct>().count;
        window.GetChild(6).GetComponent<Slider>().value = 1;

        window.GetChild(5).GetComponent<TextMeshProUGUI>().text = "1";

        Transform itemDets = window.GetChild(11);

        itemDets.GetChild(1).GetComponent<Image>().sprite = prod.icon;
        itemDets.GetChild(2).GetComponent<TextMeshProUGUI>().text = prod.itemName;
        itemDets.GetChild(3).GetComponent<TextMeshProUGUI>().text = prod.description;
        itemDets.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = prod.cost.ToString();

        itemDets.GetChild(4).GetChild(2).GetComponent<FixWidth>().UpdateWidth();
        itemDets.GetChild(4).GetComponent<FixWidth>().UpdateWidth();
    }

    public void Buy()
    {
        ShopProduct prod = selectedProduct.GetComponent<ShopProduct>();
        
        if (GameManager.Instance.invMan.currency >= prod.cost * selectedCount)
        {
            if (prod.itemType == ItemType.Weapon)
            {
                BuyWeapon(prod, selectedCount);
            }
            else if (prod.itemType == ItemType.Material)
            {
                BuyMaterial(prod, selectedCount);
            }
            else if (prod.itemType == ItemType.Ingredient)
            {
                BuyIngredient(prod, selectedCount);
            }
            else if (prod.itemType == ItemType.Food)
            {
                BuyFood(prod, selectedCount);
            }
            else if (prod.itemType == ItemType.SpecialItem)
            {
                BuySpecialItem(prod, selectedCount);
            }
            else
            {
                Debug.Log("Error: Trying to buy an invalid item");
            }
        }
    }

    public void UpdateDetails(GameObject obj)
    {
        selectedProduct = obj;

        Color borderColor = Color.white;
        Color textColor = Color.white;
        Color symbolColor = Color.white;

        ShopProduct product = obj.GetComponent<ShopProduct>();
        InventoryManager invMan = GameManager.Instance.invMan;

        borderColor = invMan.DetailsBorderColor[product.rarity - 1];
        textColor = invMan.DetailsTextColor[product.rarity - 1];
        symbolColor = invMan.DetailsSymboloColor[product.rarity - 1];

        Transform content = ItemDetails.transform.GetChild(0);

        content.GetChild(3).GetComponent<Image>().color = borderColor;

        content.GetChild(1).GetChild(0).GetComponent<Image>().color = symbolColor;
        content.GetChild(1).GetChild(1).GetComponent<Image>().color = symbolColor;

        Transform detContent = content.GetChild(2);

        detContent.GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;
        detContent.GetChild(0).GetComponent<TextMeshProUGUI>().text = product.itemName;

        detContent.GetChild(1).GetComponent<Image>().sprite = product.icon;

        Transform Rarity = detContent.GetChild(2);

        foreach (Transform t in Rarity)
        {
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < product.rarity; i++)
        {
            Rarity.GetChild(i).gameObject.SetActive(true);
        }

        detContent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = product.description;
        detContent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;

        // Fix UI
        detContent.GetChild(3).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        content.GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateSliderValue()
    {
        Transform window = buyPopUp.transform.GetChild(1);
        Slider slider = window.GetChild(6).GetComponent<Slider>();

        selectedCount = (int)slider.value;

        window.GetChild(5).GetComponent<TextMeshProUGUI>().text = selectedCount.ToString();

        Transform itemDets = window.GetChild(11);
        itemDets.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = (selectedCount * selectedProduct.GetComponent<ShopProduct>().cost).ToString();

        itemDets.GetChild(4).GetChild(2).GetComponent<FixWidth>().UpdateWidth();
        itemDets.GetChild(4).GetComponent<FixWidth>().UpdateWidth();
    }
    
    public void BuyWeapon(ShopProduct product, int count)
    {
        // Instantiate Slot: WeaponInfo script
        WeaponInfo newSlot = Instantiate<WeaponInfo>(GameManager.Instance.invMan.WeaponInvSlot.GetComponent<WeaponInfo>(), GameManager.Instance.invMan.TabsContent[0]);
        WeaponInfo weapInfo = product.weapInfo;

        GameManager.Instance.invMan.WeaponsTab.Add(weapInfo.GetIdentificationString());
        newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateWeaponInvSlotDetails(newSlot.gameObject); });

        newSlot.scrObj = weapInfo.scrObj;

        newSlot.baseATK = weapInfo.SetAtkFromLevel(weapInfo.currentLevel);
        newSlot.currentSubstat = weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel);

        newSlot.currentXp = weapInfo.currentXp;
        newSlot.xpForNextLevel = weapInfo.XpForNextLevel(weapInfo.currentLevel);

        newSlot.currentLevel = weapInfo.currentLevel;
        newSlot.currentMaxLevel = weapInfo.currentMaxLevel;
        newSlot.ascensionLevel = weapInfo.ascensionLevel;
        newSlot.refinementLevel = weapInfo.refinementLevel;
        newSlot.isLocked = weapInfo.isLocked;

        newSlot.UniqueID = newSlot.GetIdentificationString();

        // Instantiate Slot: Slot UI
        newSlot.transform.GetChild(2).gameObject.SetActive(false);
        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + newSlot.currentLevel;
        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = newSlot.scrObj.icon;

        Transform Rarity = newSlot.transform.GetChild(3);
        foreach (Transform t in Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < newSlot.scrObj.rarity; i++)
        {
            Rarity.GetChild(i).gameObject.SetActive(true);
        }

        if (newSlot.isLocked)
        {
            newSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Image>().color = GameManager.Instance.invMan.closedPadlockColBG;
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.PadlockClosed;
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<Image>().color = GameManager.Instance.invMan.closedPadlockCol;
        }
        else
        {
            newSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
        }

        newSlot.transform.GetChild(5).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.refinementLevel.ToString();

        newSlot.transform.GetChild(6).gameObject.SetActive(true);
    }

    public void BuyMaterial(ShopProduct product, int count)
    {
        MaterialInfo matInfo = product.matInfo;

        // Instantiate Slot: MaterialInfo script

        if (GameManager.Instance.invMan.MaterialsTab.Contains(matInfo.scrObj.TypeID))
        {
            // Update the count 
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[1].transform)
            {
                MaterialInfo info = t.GetComponent<MaterialInfo>();
                if (info.scrObj.materialName == matInfo.scrObj.materialName)
                {
                    info.count += matInfo.count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            MaterialInfo newSlot = Instantiate<MaterialInfo>(GameManager.Instance.invMan.MaterialInvSlot.GetComponent<MaterialInfo>(), GameManager.Instance.invMan.TabsContent[1]);

            GameManager.Instance.invMan.MaterialsTab.Add(matInfo.scrObj.TypeID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateMaterialInvSlotDetails(newSlot.gameObject); });

            newSlot.scrObj = matInfo.scrObj;
            newSlot.count = matInfo.count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = matInfo.scrObj.icon;

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
    }

    public void BuyIngredient(ShopProduct product, int count)
    {
        IngredientInfo ingrInfo = product.ingrInfo;

        // Instantiate Slot: IngredientInfo script
        if (GameManager.Instance.invMan.IngredientsTab.Contains(ingrInfo.scrObj.TypeID))
        {
            // Add the count
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[2].transform)
            {
                IngredientInfo info = t.GetComponent<IngredientInfo>();
                if (info.scrObj.ingredientName == ingrInfo.scrObj.ingredientName)
                {
                    info.count += ingrInfo.count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            IngredientInfo newSlot = Instantiate<IngredientInfo>(GameManager.Instance.invMan.IngredientInvSlot.GetComponent<IngredientInfo>(), GameManager.Instance.invMan.TabsContent[2]);

            GameManager.Instance.invMan.IngredientsTab.Add(ingrInfo.scrObj.TypeID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateIngredientInvSlotDetails(newSlot.gameObject); });

            newSlot.scrObj = ingrInfo.scrObj;
            newSlot.count = ingrInfo.count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = ingrInfo.scrObj.icon;

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
    }

    public void BuyFood(ShopProduct product, int count)
    {
        FoodInfo foodInfo = product.foodInfo;

        // Instantiate Slot: FoodInfo script
        if (GameManager.Instance.invMan.FoodTab.Contains(foodInfo.scrObj.TypeID))
        {
            // Add the count
            foreach (Transform t in GameManager.Instance.invMan.TabsContent[3].transform)
            {
                FoodInfo info = t.GetComponent<FoodInfo>();
                if (info.scrObj.foodName == foodInfo.scrObj.foodName)
                {
                    info.count += foodInfo.count;
                    info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                }
            }
        }
        else
        {
            // Instantiate slot
            FoodInfo newSlot = Instantiate<FoodInfo>(GameManager.Instance.invMan.FoodInvSlot.GetComponent<FoodInfo>(), GameManager.Instance.invMan.TabsContent[3]);

            GameManager.Instance.invMan.FoodTab.Add(foodInfo.scrObj.TypeID);
            newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateFoodInvSlotDetails(newSlot.gameObject); });

            newSlot.scrObj = foodInfo.scrObj;
            newSlot.count = foodInfo.count;

            newSlot.transform.GetChild(2).gameObject.SetActive(false);

            newSlot.transform.GetChild(1).GetComponent<Image>().sprite = foodInfo.scrObj.icon;

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
    }

    public void BuySpecialItem(ShopProduct product, int count)
    {
        SpecialItemInfo specItemInfo = product.specItemInfo;

        // Instantiate Slot: IngredientInfo script
        SpecialItemInfo newSlot = Instantiate<SpecialItemInfo>(GameManager.Instance.invMan.IngredientInvSlot.GetComponent<SpecialItemInfo>(), GameManager.Instance.invMan.TabsContent[4]);

        GameManager.Instance.invMan.SpecialItemsTab.Add(specItemInfo.scrObj.TypeID);
        newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateSpecialItemInvSlotDetails(newSlot.gameObject); });

        newSlot.scrObj = specItemInfo.scrObj;

        newSlot.transform.GetChild(2).gameObject.SetActive(false);

        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = specItemInfo.scrObj.icon;

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
    }
}
