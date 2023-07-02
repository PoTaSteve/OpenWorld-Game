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

    [HideInInspector]
    public Shop currentShop;

    public GameObject ItemDetails;
    public RectTransform ItemDetailsContent;

    public ShopProduct selectedProduct;
    public int selectedCount;

    [Header("Item Details")]
    public TextMeshProUGUI detailsItemNameText;
    public TextMeshProUGUI detailsItemDescriptionText;

    public Image detailsItemIcon;

    public FixHeight detailsDescriptionFixHeight;
    public FixHeight detailsContentFixHeight;

    [Header("PopUp")]
    public GameObject buyPopUp;

    public TextMeshProUGUI popUpMinText;
    public TextMeshProUGUI popUpMaxText;
    public TextMeshProUGUI popUpCurrAmtText;

    public TextMeshProUGUI popUpItemNameText;
    public TextMeshProUGUI popUpItemDescriptionText;
    public TextMeshProUGUI popUpItemCostText;

    public Image popUpItemIcon;

    public Slider popUpSlider;

    public FixWidth popUpCostFixWidth;
    public FixWidth popUpCostTextFixWidth;

    public void CreateShop()
    {
        foreach (Transform t in ProductContent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < currentShop.products.Count; i++)
        {
            GameObject newProduct = Instantiate(ProductPrefab, ProductContent);

            newProduct.GetComponent<Button>().onClick.AddListener(delegate { UpdateDetails(newProduct.GetComponent<ShopProduct>()); });

            ShopProduct prod = newProduct.GetComponent<ShopProduct>();

            prod.index = i;

            if (currentShop.CurrCounts[i] <= 0)
            {
                newProduct.GetComponent<Animator>().Play("Disabled");
            }

            if (currentShop.products[i].TryGetComponent(out WeaponInfo weapInfo))
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

                if (currentShop.CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.CurrCounts[i] + "/" + currentShop.StartCounts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (currentShop.products[i].TryGetComponent(out MaterialInfo matInfo))
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

                if (currentShop.CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.CurrCounts[i] + "/" + currentShop.StartCounts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (currentShop.products[i].TryGetComponent(out FoodInfo foodInfo))
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

                if (currentShop.CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.CurrCounts[i] + "/" + currentShop.StartCounts[i];
                }
                else
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
                }
            }
            else if (currentShop.products[i].TryGetComponent(out SpecialItemInfo specItemInfo))
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

                if (currentShop.CurrCounts[i] > 0)
                {
                    newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.CurrCounts[i] + "/" + currentShop.StartCounts[i];
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
        UpdateDetails(ProductContent.GetChild(0).GetComponent<ShopProduct>());
    }

    public void SetPopUpActive()
    {
        buyPopUp.SetActive(true);

        popUpMinText.text = "1"; // Min
        popUpMaxText.text = currentShop.CurrCounts[selectedProduct.index].ToString(); // Max

        popUpSlider.minValue = 1;
        popUpSlider.maxValue = currentShop.CurrCounts[selectedProduct.index];
        popUpSlider.value = 1;

        popUpCurrAmtText.text = "1";
        selectedCount = 1;

        popUpItemIcon.sprite = selectedProduct.icon;
        popUpItemNameText.text = selectedProduct.itemName;
        popUpItemDescriptionText.text = selectedProduct.description;
        popUpItemCostText.text = selectedProduct.cost.ToString();

        popUpCostTextFixWidth.UpdateWidth();
        popUpCostFixWidth.UpdateWidth();
    }

    public void Buy()
    {
        // Check for money then call inventory AddItem
        int index = selectedProduct.index;
        ItemType type = selectedProduct.itemType;
        GameObject item = currentShop.products[index];

        GameManager.Instance.invMan.AddItemToInventory(type, item, selectedCount);

        // Decrease by bought ammount
        currentShop.CurrCounts[index] -= selectedCount;

        buyPopUp.SetActive(false);

        if (currentShop.CurrCounts[index] > 0)
        {
            ProductContent.GetChild(index).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.CurrCounts[index] + "/" + currentShop.StartCounts[index];
        }
        else
        {
            ProductContent.GetChild(index).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";

            selectedProduct.GetComponent<Animator>().Play("Disabled");
        }
    }

    public void UpdateDetails(ShopProduct product)
    {
        if (selectedProduct != null)
        {
            selectedProduct.GetComponent<Animator>().SetBool("isSelected", false);
        }        

        selectedProduct = product;

        selectedProduct.GetComponent<Animator>().SetBool("isSelected", true);

        detailsItemNameText.text = product.itemName;

        detailsItemIcon.sprite = product.icon;

        // Rarity?

        detailsItemDescriptionText.text = product.description;

        // Fix UI
        detailsDescriptionFixHeight.UpdateHeight();
        detailsContentFixHeight.UpdateHeight();
    }

    public void UpdateSliderValue()
    {
        selectedCount = (int)popUpSlider.value;

        popUpCurrAmtText.text = selectedCount.ToString();

        popUpItemCostText.text = (selectedCount * selectedProduct.cost).ToString();

        popUpCostTextFixWidth.UpdateWidth();
        popUpCostFixWidth.UpdateWidth();
    }

    public void IncreaseSliderValue()
    {
        if (selectedCount < popUpSlider.maxValue)
        {
            selectedCount++;
            popUpSlider.value = selectedCount;
        }

        popUpCurrAmtText.text = selectedCount.ToString();

        popUpItemCostText.text = (selectedCount * selectedProduct.cost).ToString();

        popUpCostTextFixWidth.UpdateWidth();
        popUpCostFixWidth.UpdateWidth();
    }

    public void DecreaseSliderValue()
    {
        if (selectedCount > popUpSlider.minValue)
        {
            selectedCount--;
            popUpSlider.value = selectedCount;
        }

        popUpCurrAmtText.text = selectedCount.ToString();

        popUpItemCostText.text = (selectedCount * selectedProduct.cost).ToString();

        popUpCostTextFixWidth.UpdateWidth();
        popUpCostFixWidth.UpdateWidth();
    }
}
