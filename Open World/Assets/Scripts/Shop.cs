using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject ProductPrefab;
    public Transform Content;

    public GameObject EquipmentDetails;
    public GameObject MaterialDetails; // Same for ingredient
    public GameObject FoodDetails;
    
    public List<MaterialInfo> Materials;
    public List<IngredientInfo> Ingredients;
    public List<FoodInfo> Food;

    public List<int> MaterialsMaxCount;
    public List<int> IngredientsMaxCount;
    public List<int> FoodMaxCount;

    [Space]
    [SerializeField]
    private Scrollbar windowScrollbar;

    [Space]
    [Header("Purchase PopUp")]
    [SerializeField]
    private GameObject PurchasePopUp;
    [SerializeField]
    private TextMeshProUGUI minCount;
    [SerializeField]
    private TextMeshProUGUI maxCount;
    [SerializeField]
    private TextMeshProUGUI currentCount;
    [SerializeField]
    private Slider sliderAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateShop()
    {
        int i = 0;
        foreach (MaterialInfo mat in Materials)
        {
            GameObject newProduct = Instantiate(ProductPrefab, Content);

            MaterialInfo info = newProduct.AddComponent<MaterialInfo>();

            info = mat;

            newProduct.transform.GetChild(1).GetComponent<Image>().sprite = info.MaterialSO.icon;
            newProduct.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = info.MaterialSO.materialName;

            newProduct.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = info.count.ToString() + "/" + MaterialsMaxCount[i].ToString() + " Available";
            newProduct.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.MaterialSO.cost.ToString();

            i++;
        }

        i = 0;
        foreach (IngredientInfo ingr in Ingredients)
        {
            GameObject newProduct = Instantiate(ProductPrefab, Content);

            IngredientInfo info = newProduct.AddComponent<IngredientInfo>();

            info = ingr;

            newProduct.transform.GetChild(1).GetComponent<Image>().sprite = info.IngredientSO.icon;
            newProduct.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = info.IngredientSO.ingredientName;

            newProduct.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = info.count.ToString() + "/" + IngredientsMaxCount[i].ToString() + " Available";
            newProduct.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.IngredientSO.cost.ToString();

            i++;
        }

        i = 0;
        foreach (FoodInfo food in Food)
        {
            GameObject newProduct = Instantiate(ProductPrefab, Content);

            FoodInfo info = newProduct.AddComponent<FoodInfo>();

            info = food;

            newProduct.transform.GetChild(1).GetComponent<Image>().sprite = info.FoodSO.icon;
            newProduct.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = info.FoodSO.foodName;

            newProduct.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = info.count.ToString() + "/" + FoodMaxCount[i].ToString() + " Available";
            newProduct.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.FoodSO.cost.ToString();
            i++;
        }


        if (windowScrollbar.size == 1)
        {
            windowScrollbar.gameObject.SetActive(false);
        }
        else
        {
            windowScrollbar.gameObject.SetActive(true);
        }
    }

    public void PressPurchaseButton()
    {
        PurchasePopUp.SetActive(true);
        sliderAmount.value = 1;
        currentCount.text = "1";

        // Set max value from item selected
    }

    public void Purchase()
    {
        //Add to inventory
    }

    public void Cancel()
    {
        sliderAmount.value = 1;
        currentCount.text = "1";
        PurchasePopUp.SetActive(false);
    }

    public void UpdateCurrCount()
    {
        currentCount.text = sliderAmount.value.ToString();
    }

    public void AddOne()
    {
        int count = int.Parse(currentCount.text);

        if (count + 1 <= int.Parse(maxCount.text))
        {
            count++;
            currentCount.text = count.ToString();
            sliderAmount.value = count;
        }
    }

    public void RemoveOne()
    {
        int count = int.Parse(currentCount.text);

        if (count - 1 > 0)
        {
            count--;
            currentCount.text = count.ToString();
            sliderAmount.value = count;
        }
    }
}
