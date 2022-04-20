using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryObj;

    public int currency;

    [Space]
    public WeaponTab weap;
    [Space]
    public MaterialTab mat;
    [Space]
    public IngredientTab ingr;
    [Space]
    public FoodTab food;
    [Space]
    public SpecialItemTab specIt;

    [Space]
    public int currInvTab;
    [Space]
    public List<string> WeaponsTab = new List<string>();
    public List<int> MaterialsTab = new List<int>();
    public List<int> IngredientsTab = new List<int>();
    public List<int> FoodTab = new List<int>();
    public List<int> SpecialItemsTab = new List<int>();
    [Space]
    public List<WeaponScriptableObject> WeaponsTabObj = new List<WeaponScriptableObject>();
    public List<MaterialScriptableObject> MaterialsTabObj = new List<MaterialScriptableObject>();
    public List<IngredientScriptableObject> IngredientsTabObj = new List<IngredientScriptableObject>();
    public List<FoodScriptableObject> FoodTabObj = new List<FoodScriptableObject>();
    public List<SpecialItemScriptableObject> SpecialItemsTabObj = new List<SpecialItemScriptableObject>();
    [Space]
    public GameObject[] Windows = new GameObject[5]; // Windows containing the items of the tab
    public GameObject[] Tabs = new GameObject[5]; // Buttons to go to a specific tab (use for animations)
    public Transform[] TabsContent = new Transform[5]; // Parents for inventory slots
    [Space]
    public Sprite CommonBG;
    public Sprite NotCommonBG;
    public Sprite RareBG;
    public Sprite EpicBG;
    public Sprite LegendaryBG;
    [Space]
    public Sprite PadlockOpen;
    public Sprite PadlockClosed;
    public Color openPadlockColBG = Color.white;
    public Color closedPadlockColBG = new Color(75f / 255f, 75f / 255f, 75f / 255f);
    public Color openPadlockCol = new Color(175f / 255f, 175f / 255f, 175f / 255f);
    public Color closedPadlockCol = new Color(1, 100f / 255f, 100f / 255f);
    [Space]
    public GameObject DomainSourcePrefab;
    public GameObject OtherSourcePrefab;
    [Space]
    public Sprite CraftingIngredientIcon;
    public Sprite CraftingResultIcon;
    [Space]
    public Sprite BaseIngredientIcon;
    public Sprite SpecificIngredientIcon;
    [Space]
    [Header("Food buff icons")]
    public Sprite AttackFoodBuffIcon;
    public Sprite DefenceFoodBuffIcon;
    public Sprite HealFoodBuffIcon;
    public Sprite RegenFoodBuffIcon;
    public Sprite SpeedFoodBuffIcon;
    public Sprite StaminaConsumptionFoodBuffIcon;
    public Sprite PotionFoodTypeIcon;
    [Space]
    [Header("Inventory slot prefabs")]
    public GameObject WeaponInvSlot;
    public GameObject MaterialInvSlot;
    public GameObject IngredientInvSlot;
    public GameObject FoodInvSlot;
    public GameObject SpecialItemInvSlot;
    [Space]
    [Header("Inventory details")]
    public GameObject[] InvDetails = new GameObject[5];
    [Space]
    public Color inactiveLevelAscension = new Color(75f / 255f, 75f / 255f, 75f / 255f, 1f);
    public Color activeLevelAscension = Color.white;
    [Space]
    public Color[] DetailsSymboloColor = new Color[5];
    public Color[] DetailsBorderColor = new Color[5];
    public Color[] DetailsTextColor = new Color[5];

    public GameObject BottomButton;

    public GameObject LastSelectedSlot;

    public GameObject WeaponEnhanceObj;

    // Start is called before the first frame update
    void Start()
    {
        Windows[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNextInvTab()
    {
        Windows[currInvTab].SetActive(false);

        if (currInvTab == 4)
        {
            currInvTab = 0;
        }
        else
        {
            currInvTab++;
        }

        Windows[currInvTab].SetActive(true);

        if (TabsContent[currInvTab].transform.childCount > 0)
        {
            InvDetails[currInvTab].SetActive(true);
            UpdateInvSlotDetails(TabsContent[currInvTab].transform.GetChild(0).gameObject);
        }
        else
        {
            InvDetails[currInvTab].SetActive(false);
        }
    }

    public void GoToPreviousInvTab()
    {
        Windows[currInvTab].SetActive(false);

        if (currInvTab == 0)
        {
            currInvTab = 4;
        }
        else
        {
            currInvTab--;
        }

        Windows[currInvTab].SetActive(true);

        if (TabsContent[currInvTab].transform.childCount > 0)
        {
            InvDetails[currInvTab].SetActive(true);
            UpdateInvSlotDetails(TabsContent[currInvTab].transform.GetChild(0).gameObject);
        }
        else
        {
            InvDetails[currInvTab].SetActive(false);
        }
    }

    public void GoToSpecificInvTab(int tab)
    {
        if (tab < 0 || tab > 4)
        {
            GoToSpecificInvTab(0);

            return;
        }

        if (tab != currInvTab)
        {
            int prevTab = currInvTab;
            currInvTab = tab;

            if (TabsContent[currInvTab].transform.childCount > 0)
            {
                InvDetails[currInvTab].SetActive(true);
                UpdateInvSlotDetails(TabsContent[currInvTab].transform.GetChild(0).gameObject);
            }
            else
            {
                InvDetails[currInvTab].SetActive(false);
            }

            if (prevTab != -1)
            {
                Windows[prevTab].SetActive(false);
            }

            Windows[currInvTab].SetActive(true);
        }
    }

    public void OpenInventory()
    {
        if (currInvTab < 0 || currInvTab > 4)
        {
            currInvTab = 0;
        }

        if (TabsContent[currInvTab].transform.childCount > 0)
        {
            InvDetails[currInvTab].SetActive(true);
            UpdateInvSlotDetails(TabsContent[currInvTab].transform.GetChild(0).gameObject);
        }
        else
        {
            InvDetails[currInvTab].SetActive(false);
        }

        Windows[currInvTab].SetActive(true);
    }

    public void BottomButtonFunc()
    {
        if (currInvTab == 0)
        {
            // Open Weapon Details
            GameManager.Instance.plInMan.OpenWeapDetails();
        }
        else if (currInvTab == 3)
        {
            // Use Food
        }
    }

    public void LockWeapon()
    {
        WeaponInfo weapInfo = LastSelectedSlot.GetComponent<WeaponInfo>();

        weapInfo.isLocked = !weapInfo.isLocked;

        if (weapInfo.isLocked)
        {
            weap.PadlockBG.color = closedPadlockColBG;
            weap.PadlockIcon.sprite = PadlockClosed;
            weap.PadlockIcon.color = closedPadlockCol;

            LastSelectedSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            weap.PadlockBG.color = openPadlockColBG;
            weap.PadlockIcon.sprite = PadlockOpen;
            weap.PadlockIcon.color = openPadlockCol;

            LastSelectedSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UpdateInvSlotDetails(GameObject obj)
    {
        if (currInvTab == 0)
        {
            UpdateWeaponInvSlotDetails(obj);
        }
        else if (currInvTab == 1)
        {
            UpdateMaterialInvSlotDetails(obj);
        }
        else if (currInvTab == 2)
        {
            UpdateIngredientInvSlotDetails(obj);
        }
        else if (currInvTab == 3)
        {
            UpdateFoodInvSlotDetails(obj);
        }
        else if (currInvTab == 4)
        {
            UpdateSpecialItemInvSlotDetails(obj);
        }
        else
        {
            Debug.Log("Error updating inventory details. CurrInvTab out of range.");
        }
    }

    public void UpdateWeaponInvSlotDetails(GameObject obj)
    {
        WeaponInfo weapInfo = obj.GetComponent<WeaponInfo>();

        // Deactivate "NEW" message
        weapInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        weapInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = obj;

        // Top
        weap.NameText.text = weapInfo.scrObj.weaponName;
        weap.Icon.sprite = weapInfo.scrObj.icon;

        weap.WeaponTypeText.text = weapInfo.StringWeaponType();

        weap.BaseAtkText.text = Mathf.RoundToInt(weapInfo.baseATK).ToString();

        if (weapInfo.scrObj.rarity <= 2)
        {
            weap.SubstatTypeText.text = "";
            weap.SubstatText.text = "";
        }
        else
        {
            weap.SubstatTypeText.text = weapInfo.scrObj.subStatType;
            if (weapInfo.scrObj.isSubStatPercentage)
            {
                weap.SubstatText.text = (weapInfo.currentSubstat * 100).ToString() + "%";
            }
            else
            {
                weap.SubstatText.text = weapInfo.currentSubstat.ToString();
            }
        }

        // Rarity
        foreach (Transform t in weap.Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < weapInfo.scrObj.rarity; i++)
        {
            weap.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        // Add level references
        // Add refinement references

        weap.LevelText.text = "Lv. " + weapInfo.currentLevel + "/" + weapInfo.currentMaxLevel;
        weap.RefinementText.text = "Refinement level " + weapInfo.refinementLevel;

        // Add is locked references
        if (weapInfo.isLocked)
        {
            weap.PadlockBG.color = closedPadlockColBG;
            weap.PadlockIcon.sprite = PadlockClosed;
            weap.PadlockIcon.color = closedPadlockCol;
        }
        else
        {
            weap.PadlockBG.color = openPadlockColBG;
            weap.PadlockIcon.sprite = PadlockOpen;
            weap.PadlockIcon.color = openPadlockCol;
        }

        if (weapInfo.scrObj.rarity <= 2)
        {
            // Only set the description
            weap.DescriptionText.text = "<b>Description</b>\n<size=14> </size>\n" + weapInfo.scrObj.description;
        }
        else
        {
            // Set the description and the effect
            weap.DescriptionText.text =
                "<b>Effect</b>\n<size=14> </size>\n" + weapInfo.scrObj.effectName + "\n" + weapInfo.scrObj.effect + "\n\n<b>Description</b>\n<size=14> </size>\n" + weapInfo.scrObj.description;
        }

        // Fix Colors
        weap.NameText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.WeaponTypeText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.BaseATKFixedText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.BaseAtkText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.SubstatTypeText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.SubstatText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];

        weap.LevelText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];
        weap.RefinementText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];

        weap.DescriptionText.color = DetailsTextColor[weapInfo.scrObj.rarity - 1];

        weap.SymbolIconBG[0].color = DetailsSymboloColor[weapInfo.scrObj.rarity - 1];
        weap.SymbolIconBG[1].color = DetailsSymboloColor[weapInfo.scrObj.rarity - 1];

        weap.Border.color = DetailsBorderColor[weapInfo.scrObj.rarity - 1];

        // Fix UI Height and position
        weap.DescriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        weap.Content.GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateMaterialInvSlotDetails(GameObject obj)
    {
        MaterialInfo matInfo = obj.GetComponent<MaterialInfo>();

        // Deactivate "NEW" message
        matInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        matInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = obj;

        // Update UI
        mat.NameText.text = matInfo.scrObj.materialName;
        mat.Icon.sprite = matInfo.scrObj.icon;

        foreach (Transform t in mat.Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < matInfo.scrObj.rarity; i++)
        {
            mat.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        mat.DescriptionText.text = matInfo.scrObj.description;

        //Fix UI Height and position
        mat.DescriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        mat.Content.GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        mat.Border.color = DetailsBorderColor[matInfo.scrObj.rarity - 1];

        mat.SymbolIconBG[0].color = DetailsSymboloColor[matInfo.scrObj.rarity - 1];
        mat.SymbolIconBG[1].color = DetailsSymboloColor[matInfo.scrObj.rarity - 1];

        mat.NameText.color = DetailsTextColor[matInfo.scrObj.rarity - 1];
        mat.DescriptionText.color = DetailsTextColor[matInfo.scrObj.rarity - 1];
    }

    public void UpdateIngredientInvSlotDetails(GameObject obj)
    {
        IngredientInfo ingrInfo = obj.GetComponent<IngredientInfo>();

        // Deactivate "NEW" message
        ingrInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        ingrInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = obj;

        // Update UI
        ingr.NameText.text = ingrInfo.scrObj.ingredientName;
        ingr.Icon.sprite = ingrInfo.scrObj.icon;

        foreach (Transform t in ingr.Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < ingrInfo.scrObj.rarity; i++)
        {
            ingr.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        ingr.DescriptionText.text = ingrInfo.scrObj.description;

        //Fix UI Height and position
        ingr.DescriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        ingr.Content.GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        ingr.Border.color = DetailsBorderColor[ingrInfo.scrObj.rarity - 1];

        ingr.SymbolIconBG[0].color = DetailsSymboloColor[ingrInfo.scrObj.rarity - 1];
        ingr.SymbolIconBG[1].color = DetailsSymboloColor[ingrInfo.scrObj.rarity - 1];

        ingr.NameText.color = DetailsTextColor[ingrInfo.scrObj.rarity - 1];
        ingr.DescriptionText.color = DetailsTextColor[ingrInfo.scrObj.rarity - 1];
    }

    public void UpdateFoodInvSlotDetails(GameObject obj)
    {
        FoodInfo foodInfo = obj.GetComponent<FoodInfo>();

        // Deactivate "NEW" message
        foodInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        foodInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = obj;

        // Update UI
        food.NameText.text = foodInfo.scrObj.foodName;
        food.Icon.sprite = foodInfo.scrObj.icon;

        foreach (Transform t in food.Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < foodInfo.scrObj.rarity; i++)
        {
            food.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        food.DescriptionText.text = foodInfo.scrObj.description;

        //Fix UI Height and position
        food.DescriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        food.Content.GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        food.Border.color = DetailsBorderColor[foodInfo.scrObj.rarity - 1];

        food.SymbolIconBG[0].color = DetailsSymboloColor[foodInfo.scrObj.rarity - 1];
        food.SymbolIconBG[1].color = DetailsSymboloColor[foodInfo.scrObj.rarity - 1];

        food.NameText.color = DetailsTextColor[foodInfo.scrObj.rarity - 1];
        food.DescriptionText.color = DetailsTextColor[foodInfo.scrObj.rarity - 1];
    }

    public void UpdateSpecialItemInvSlotDetails(GameObject obj)
    {
        SpecialItemInfo specItemInfo = obj.GetComponent<SpecialItemInfo>();

        // Deactivate "NEW" message
        specItemInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        specItemInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = obj;

        // Update UI
        specIt.NameText.text = specItemInfo.scrObj.specialItemName;
        specIt.Icon.sprite = specItemInfo.scrObj.icon;

        foreach (Transform t in specIt.Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < specItemInfo.scrObj.rarity; i++)
        {
            specIt.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        specIt.DescriptionText.text = specItemInfo.scrObj.description;

        //Fix UI Height and position
        specIt.DescriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        specIt.Content.GetComponent<FixHeight>().UpdateHeight();
    }
}

[System.Serializable]
public class WeaponTab
{
    public GameObject DetailsObj;
    public GameObject Content;

    public Image[] SymbolIconBG = new Image[2];
    public TextMeshProUGUI NameText;
    public Image Icon;
    public TextMeshProUGUI WeaponTypeText;
    public TextMeshProUGUI BaseATKFixedText;
    public TextMeshProUGUI BaseAtkText;
    public TextMeshProUGUI SubstatTypeText;
    public TextMeshProUGUI SubstatText;

    public Transform Rarity;

    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI RefinementText;

    public Image PadlockBG;
    public Image PadlockIcon;

    public TextMeshProUGUI DescriptionText;

    public Image Border;
}

[System.Serializable]
public class MaterialTab
{
    public GameObject DetailsObj;
    public GameObject Content;

    public Image[] SymbolIconBG = new Image[2];
    public TextMeshProUGUI NameText;
    public Image Icon;
    public Transform Rarity;
    public TextMeshProUGUI DescriptionText;

    public Image Border;
}

[System.Serializable]
public class IngredientTab
{
    public GameObject DetailsObj;
    public GameObject Content;

    public Image[] SymbolIconBG = new Image[2];
    public TextMeshProUGUI NameText;
    public Image Icon;
    public Transform Rarity;
    public TextMeshProUGUI DescriptionText;

    public Image Border;
}

[System.Serializable]
public class FoodTab
{
    public GameObject DetailsObj;
    public GameObject Content;

    public Image[] SymbolIconBG = new Image[2];
    public TextMeshProUGUI NameText;
    public Image Icon;
    public Transform Rarity;
    public TextMeshProUGUI DescriptionText;

    public Image Border;
}

[System.Serializable]
public class SpecialItemTab
{
    public GameObject DetailsObj;
    public GameObject Content;

    public Image[] SymbolIconBG = new Image[2];
    public TextMeshProUGUI NameText;
    public Image Icon;
    public Transform Rarity;
    public TextMeshProUGUI DescriptionText;

    public Image Border;
}