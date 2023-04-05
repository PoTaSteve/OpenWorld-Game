using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ProductPrefab;
    
    public Transform ProductContent;

    //[HideInInspector]
    public List<GameObject> products;
    //[HideInInspector]
    public List<int> CurrCounts;
    //[HideInInspector]
    public List<int> StartCounts;

    [HideInInspector]
    public Shop currentShop;

    public GameObject ItemDetails;

    public GameObject selectedProduct;
    public int selectedCount;

    public GameObject buyPopUp;

    public void CreateShop()
    {
        foreach (Transform t in ProductContent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < products.Count; i++)
        {
            GameObject newProduct = Instantiate(ProductPrefab, ProductContent);

            newProduct.GetComponent<Button>().onClick.AddListener(delegate { UpdateDetails(newProduct); });

            ShopProduct prod = newProduct.GetComponent<ShopProduct>();

            prod.startCount = StartCounts[i];
            prod.currCount = CurrCounts[i];
            prod.index = i;

            if (products[i].TryGetComponent(out WeaponInfo weapInfo))
            {
                prod.weapInfo = weapInfo;
                prod.itemType = ItemType.Weapon;
                prod.cost = weapInfo.scrObj.buyCost;

                prod.icon = weapInfo.scrObj.icon;
                prod.itemName = weapInfo.scrObj.weaponName;
                prod.description = weapInfo.scrObj.description;

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = weapInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text= weapInfo.scrObj.weaponName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text= weapInfo.scrObj.buyCost.ToString();
                if (CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + CurrCounts[i] + "/" + StartCounts[i];
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

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = matInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = matInfo.scrObj.materialName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = matInfo.scrObj.buyCost.ToString();
                if (CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + CurrCounts[i] + "/" + StartCounts[i];
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

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = foodInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = foodInfo.scrObj.foodName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = foodInfo.scrObj.buyCost.ToString();
                if (CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + CurrCounts[i] + "/" + StartCounts[i];
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

                newProduct.transform.GetChild(3).GetComponent<Image>().sprite = specItemInfo.scrObj.icon;
                newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = specItemInfo.scrObj.specialItemName;

                newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = specItemInfo.scrObj.buyCost.ToString();
                if (CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + CurrCounts[i] + "/" + StartCounts[i];
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
        window.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedProduct.GetComponent<ShopProduct>().currCount.ToString(); // Max

        window.GetChild(6).GetComponent<Slider>().minValue = 1;
        window.GetChild(6).GetComponent<Slider>().maxValue = selectedProduct.GetComponent<ShopProduct>().currCount;
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
        // Check for money then call inventory AddItem
    }

    public void UpdateDetails(GameObject obj)
    {
        selectedProduct = obj;

        Color borderColor = Color.white;
        Color textColor = Color.white;
        Color symbolColor = Color.white;

        ShopProduct product = obj.GetComponent<ShopProduct>();
        InventoryManager invMan = GameManager.Instance.invMan;

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

    public void IncreaseSliderValue()
    {
        Transform window = buyPopUp.transform.GetChild(1);
        Slider slider = window.GetChild(6).GetComponent<Slider>();

        if (selectedCount < slider.maxValue)
        {
            selectedCount++;
            slider.value = selectedCount;
        }

        window.GetChild(5).GetComponent<TextMeshProUGUI>().text = selectedCount.ToString();

        Transform itemDets = window.GetChild(11);
        itemDets.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = (selectedCount * selectedProduct.GetComponent<ShopProduct>().cost).ToString();

        itemDets.GetChild(4).GetChild(2).GetComponent<FixWidth>().UpdateWidth();
        itemDets.GetChild(4).GetComponent<FixWidth>().UpdateWidth();
    }

    public void DecreaseSliderValue()
    {
        Transform window = buyPopUp.transform.GetChild(1);
        Slider slider = window.GetChild(6).GetComponent<Slider>();

        if (selectedCount > slider.minValue)
        {
            selectedCount--;
            slider.value = selectedCount;
        }

        window.GetChild(5).GetComponent<TextMeshProUGUI>().text = selectedCount.ToString();

        Transform itemDets = window.GetChild(11);
        itemDets.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = (selectedCount * selectedProduct.GetComponent<ShopProduct>().cost).ToString();

        itemDets.GetChild(4).GetChild(2).GetComponent<FixWidth>().UpdateWidth();
        itemDets.GetChild(4).GetComponent<FixWidth>().UpdateWidth();
    }

    
    public void BuyWeapon(ShopProduct product)
    {
        
    }

    public void BuyMaterial(ShopProduct product)
    {
        
    }
        
    public void BuyFood(ShopProduct product)
    {
        
    }

    public void BuySpecialItem(ShopProduct product)
    {
        
    }
}
