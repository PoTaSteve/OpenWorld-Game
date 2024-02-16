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

        for (int i = 0; i < currentShop.Products.Count; i++)
        {
            GameObject newProduct = Instantiate(ProductPrefab, ProductContent);

            newProduct.GetComponent<Button>().onClick.AddListener(delegate { UpdateDetails(newProduct.GetComponent<ShopProduct>()); });

            ShopProduct prod = newProduct.GetComponent<ShopProduct>();

            prod.index = i;

            if (currentShop.Products[i].currCount <= 0)
            {
                newProduct.GetComponent<Animator>().Play("Disabled");
            }

            prod.item = currentShop.Products[i].item;
            prod.cost = prod.item.GetBuyCost();

            prod.icon = prod.item.GetIcon();
            prod.itemName = prod.item.GetName();
            prod.description = prod.item.GetDescription();

            newProduct.transform.GetChild(3).GetComponent<Image>().sprite = prod.icon;
            newProduct.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = prod.itemName;

            newProduct.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = prod.cost.ToString();

            if (currentShop.Products[i].currCount > 0)
            {
                newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.Products[i].currCount + "/" + currentShop.Products[i].startCount;
            }
            else
            {
                newProduct.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Available";
            }
        }

        // Select first item
        UpdateDetails(ProductContent.GetChild(0).GetComponent<ShopProduct>());
    }

    public void SetPopUpActive()
    {
        buyPopUp.SetActive(true);

        popUpMinText.text = "1"; // Min
        popUpMaxText.text = currentShop.Products[selectedProduct.index].currCount.ToString(); // Max

        popUpSlider.minValue = 1;
        popUpSlider.maxValue = currentShop.Products[selectedProduct.index].currCount;
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

        GameManager.Instance.invMan.AddItemToInventory(selectedProduct.item, selectedCount);

        // Decrease by bought ammount
        currentShop.Products[index].currCount -= selectedCount;

        buyPopUp.SetActive(false);

        if (currentShop.Products[index].currCount > 0)
        {
            ProductContent.GetChild(index).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Available " + currentShop.Products[index].currCount + "/" + currentShop.Products[index].startCount;
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
