using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, int> ItemToCount; // = new Dictionary<string, int>()
    [Space]
    public List<WeaponInfo> EquipmentTab = new List<WeaponInfo>();
    public List<MaterialInfo> MaterialsTab = new List<MaterialInfo>();
    public List<IngredientInfo> IngredientsTab = new List<IngredientInfo>();
    public List<FoodInfo> FoodTab = new List<FoodInfo>();
    public List<SpecialItemInfo> SpecialItemsTab = new List<SpecialItemInfo>();
    [Space]
    public List<string> EquipmentTabStr = new List<string>();
    public List<string> MaterialsTabStr = new List<string>();
    public List<string> IngredientsTabStr = new List<string>();
    public List<string> FoodTabStr = new List<string>();
    public List<string> SpecialItemsTabStr = new List<string>();
    [Space]
    public GameObject[] Tabs = new GameObject[5];
    public GameObject[] TabIcons = new GameObject[5];
    [Space]
    public Animator TabIconsAnim;
    public GameObject[] TabSections = new GameObject[5];
    [Space]
    public Color TabActiveColor;
    public Color TabInactiveColor;
    public Color TabSectionActiveColor;
    public int currentActiveTab = 0;
    [Space]
    public Sprite CommonBG;
    public Sprite NotCommonBG;
    public Sprite RareBG;
    public Sprite EpicBG;
    public Sprite LegendaryBG;
    [Space]
    private Color CommonNameSpace = new Color(144f / 255f, 144f / 255f, 144f / 255f);
    private Color NotCommonNameSpace = new Color(24f / 255f, 126f / 255f, 24f / 255f);
    private Color RareNameSpace = new Color(31f / 255f, 84f / 255f, 255f / 169f);
    private Color EpicNameSpace = new Color(146f / 255f, 38f / 255f, 197f / 255f);
    private Color LegendaryNameSpace = new Color(217f / 255f, 130f / 255f, 0f / 255f);
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
    public Sprite AttackBuffIcon;
    public Sprite DefenceBuffIcon;
    public Sprite HealBuffIcon;
    public Sprite SpeedBuffIcon;
    public Sprite StaminaConsumptionBuffIcon;
    public Sprite PotionIcon;
    [Space]
    public GameObject WeaponInvSlot;
    public GameObject MaterialInvSlot;
    public GameObject IngredientInvSlot;
    public GameObject FoodInvSlot;
    public GameObject SpecialItemInvSlot;
    [Space]
    public GameObject WeaponInvWindow;
    public GameObject MaterialInvWindow;
    public GameObject IngredientInvWindow;
    public GameObject FoodInvWindow;
    public GameObject SpecialItemInvWindow;
    [Space]
    public GameObject WeaponInvDetails;
    public GameObject MaterialInvDetails;
    public GameObject IngredientInvDetails;
    public GameObject FoodInvDetails;
    public GameObject SpecialItemInvDetails;
    [Space]
    public Color inactiveLevelAscension = new Color(75f / 255f, 75f / 255f, 75f / 255f, 1f);
    public Color activeLevelAscension = Color.white;
    [Space]
    private GameObject LastSelectedWeapSlot = null;
    private GameObject LastSelectedMatSlot = null;
    private GameObject LastSelectedIngrSlot = null;
    private GameObject LastSelectedFoodSlot = null;
    //private GameObject LastSelectedSpecItemSlot = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in TabIcons)
        {
            obj.GetComponent<Image>().color = TabInactiveColor;
        }

        foreach (GameObject obj in Tabs)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
              
    }

    public void GoToNextInventoryTab()
    {
        TabIcons[currentActiveTab].GetComponent<Image>().color = TabInactiveColor;

        ColorBlock colors1 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors1.normalColor = Color.clear;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors1;

        Tabs[currentActiveTab].SetActive(false);

        if (currentActiveTab == 4)
        {
            currentActiveTab = 0;
        }
        else
        {
            currentActiveTab++;
        }

        TabIcons[currentActiveTab].GetComponent<Image>().color = TabActiveColor;

        Tabs[currentActiveTab].SetActive(true);

        SelectFirstInvItem(currentActiveTab);

        ColorBlock colors2 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors2.normalColor = TabSectionActiveColor;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors2;
    }

    public void GoToPreviousInvetoryTab()
    {
        TabIcons[currentActiveTab].GetComponent<Image>().color = TabInactiveColor;

        ColorBlock colors1 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors1.normalColor = Color.clear;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors1;

        Tabs[currentActiveTab].SetActive(false);

        if (currentActiveTab == 0)
        {
            currentActiveTab = 4;
        }
        else
        {
            currentActiveTab--;
        }

        TabIcons[currentActiveTab].GetComponent<Image>().color = TabActiveColor;

        Tabs[currentActiveTab].SetActive(true);

        SelectFirstInvItem(currentActiveTab);

        ColorBlock colors2 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors2.normalColor = TabSectionActiveColor;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors2;
    }

    public void GoToSpecificInventoryTab(int n)
    {
        TabIcons[currentActiveTab].GetComponent<Image>().color = TabInactiveColor;

        ColorBlock colors1 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors1.normalColor = Color.clear;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors1;

        Tabs[currentActiveTab].SetActive(false);

        currentActiveTab = n;

        TabIcons[currentActiveTab].GetComponent<Image>().color = TabActiveColor;

        Tabs[currentActiveTab].SetActive(true);

        SelectFirstInvItem(n);

        ColorBlock colors2 = TabSections[currentActiveTab].GetComponent<Button>().colors;
        colors2.normalColor = TabSectionActiveColor;
        TabSections[currentActiveTab].GetComponent<Button>().colors = colors2;
    }

    // Update Inv Details Methods
    public void UpdateWeaponInvSlotDetails()
    {
        WeaponInfo weapInfo = EventSystem.current.currentSelectedGameObject.GetComponent<WeaponInfo>();

        // Activate selected border
        if (LastSelectedWeapSlot != null)
        {
            LastSelectedWeapSlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        LastSelectedWeapSlot = weapInfo.gameObject;
        weapInfo.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        // Deactivate "NEW" message
        weapInfo.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        // Top
        WeaponInvDetails.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = FromIntToRarityColor(weapInfo.SO.rarity);
        WeaponInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().color = FromIntToRarityColor(weapInfo.SO.rarity);
        WeaponInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetComponent<Image>().color = FromIntToRarityColor(weapInfo.SO.rarity);

        WeaponInvDetails.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.weaponName;

        WeaponInvDetails.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = FromIntToRaritySprite(weapInfo.SO.rarity);
        WeaponInvDetails.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = weapInfo.SO.icon;

        // Top Details
        Transform details = WeaponInvDetails.transform.GetChild(0).GetChild(5);

        details.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.weaponType;
        
        if (weapInfo.SO.rarity <= 2)
        {
            details.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            details.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            details.GetChild(1).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.subStatType;
            if (weapInfo.SO.isSubStatPercentage)
            {
                details.GetChild(2).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.subStat.ToString() + "%";
            }
            else
            {
                details.GetChild(2).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.subStat.ToString();
            }
        }
        
        details.GetChild(4).GetComponent<TextMeshProUGUI>().text = ((int)weapInfo.baseATK).ToString();

        // Rarity
        Transform rarity = details.GetChild(5);
        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < weapInfo.SO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }


        // Middle
        WeaponInvDetails.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = weapInfo.currentLevel.ToString();
        WeaponInvDetails.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "/" + weapInfo.currentMaxLevel.ToString();

        // Ascension
        Transform ascension = WeaponInvDetails.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).transform;

        if (weapInfo.SO.rarity <= 2)
        {
            ascension.GetChild(4).gameObject.SetActive(false);
            ascension.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
            ascension.GetChild(4).gameObject.SetActive(true);
            ascension.GetChild(5).gameObject.SetActive(true);
        }

        foreach (Transform t in ascension)
        {
            t.GetComponent<Image>().color = inactiveLevelAscension;
            t.GetChild(0).GetComponent<Image>().color = inactiveLevelAscension;
            t.GetChild(0).GetChild(0).GetComponent<Image>().color = inactiveLevelAscension;
        }
        for (int i = 0; i < weapInfo.ascensionLevel; i++)
        {
            ascension.GetChild(i).GetComponent<Image>().color = activeLevelAscension;
            ascension.GetChild(i).GetChild(0).GetComponent<Image>().color = activeLevelAscension;
            ascension.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = activeLevelAscension;
        }

        // isLocked
        if (weapInfo.isLocked)
        {
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Image>().color = closedPadlockColBG;
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().color = closedPadlockCol;
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = PadlockClosed;
        }
        else
        {
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Image>().color = openPadlockColBG;
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().color = openPadlockCol;
            WeaponInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = PadlockOpen;
        }

        // Details
        Transform details2 = WeaponInvDetails.transform.GetChild(1).GetChild(3);

        if (weapInfo.SO.rarity <= 2)
        {
            details2.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

            FixHeight fixDet = details2.GetComponent<FixHeight>();

            fixDet.multiplier = 1;
            fixDet.offsets[1] = 0;
            fixDet.objects[0] = details2.GetChild(1).GetComponent<RectTransform>();
            fixDet.objects[1] = null;

            details2.GetChild(1).GetComponent<FixUIPosition>().offsetY = 0;
        }
        else
        {
            details2.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.effectName + "\n  • " + weapInfo.SO.effect;

            FixHeight fixDet = details2.GetComponent<FixHeight>();

            fixDet.multiplier = 2;
            fixDet.offsets[1] = 10;
            fixDet.objects[0] = details2.GetChild(0).GetComponent<RectTransform>();
            fixDet.objects[1] = details2.GetChild(1).GetComponent<RectTransform>();

            details2.GetChild(1).GetComponent<FixUIPosition>().offsetY = 10;
        }
        details2.GetChild(1).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.description;

        // Fix UI Height and position
        details2.GetChild(1).GetComponent<FixHeight>().UpdateHeight();
        details2.GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        details2.GetChild(1).GetComponent<FixUIPosition>().UpdatePosition();

        details2.GetComponent<FixHeight>().UpdateHeight();

        WeaponInvDetails.transform.GetChild(1).GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateMaterialInvSlotDetails()
    {
        MaterialInfo matInfo = EventSystem.current.currentSelectedGameObject.GetComponent<MaterialInfo>();

        // Activate selected border
        if (LastSelectedMatSlot != null)
        {
            LastSelectedMatSlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        LastSelectedMatSlot = matInfo.gameObject;
        matInfo.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        // Deactivate "NEW" message
        matInfo.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        // Top
        MaterialInvDetails.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = FromIntToRarityColor(matInfo.MaterialSO.rarity);
        MaterialInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().color = FromIntToRarityColor(matInfo.MaterialSO.rarity);
        MaterialInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetComponent<Image>().color = FromIntToRarityColor(matInfo.MaterialSO.rarity);

        MaterialInvDetails.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.materialName;

        MaterialInvDetails.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = FromIntToRaritySprite(matInfo.MaterialSO.rarity);
        MaterialInvDetails.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = matInfo.MaterialSO.icon;

        // Details
        MaterialInvDetails.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.MaterialTypeEnumToString(matInfo.MaterialSO.materialType);
        Transform rarity = MaterialInvDetails.transform.GetChild(0).GetChild(5).GetChild(1).transform;

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < matInfo.MaterialSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        // Middle
        MaterialInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.description;
        GameObject buttons = MaterialInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(2).gameObject;
        FixHeight buttonHeight = buttons.GetComponent<FixHeight>();

        // Clean Middle
        foreach (Transform t in buttons.transform)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            buttonHeight.offsets[i] = 0;
        }

        // Fix buttonHeight
        buttonHeight.multiplier = matInfo.MaterialSO.numberOfSources;
        for (int i = 0; i < matInfo.MaterialSO.numberOfSources; i++)
        {
            GameObject source = Instantiate(DomainSourcePrefab, buttons.transform);

            source.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.sources[i];

            buttonHeight.objects[i] = source.GetComponent<RectTransform>();
        }

        // Fix all UI position and height
        buttonHeight.UpdateHeight();
        MaterialInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixHeight>().UpdateHeight();
        MaterialInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        MaterialInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixUIPosition>().UpdatePosition();

        MaterialInvDetails.transform.GetChild(1).GetChild(2).GetComponent<FixHeight>().UpdateHeight();

        MaterialInvDetails.transform.GetChild(1).GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateIngredientInvSlotDetails()
    {
        IngredientInfo ingrInfo = EventSystem.current.currentSelectedGameObject.GetComponent<IngredientInfo>();

        // Activate selected border
        if (LastSelectedIngrSlot != null)
        {
            LastSelectedIngrSlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        LastSelectedIngrSlot = ingrInfo.gameObject;
        ingrInfo.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        // Deactivate "NEW" message
        ingrInfo.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        // Top
        IngredientInvDetails.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = FromIntToRarityColor(ingrInfo.IngredientSO.rarity);
        IngredientInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().color = FromIntToRarityColor(ingrInfo.IngredientSO.rarity);
        IngredientInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetComponent<Image>().color = FromIntToRarityColor(ingrInfo.IngredientSO.rarity);

        IngredientInvDetails.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.ingredientName;

        IngredientInvDetails.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = FromIntToRaritySprite(ingrInfo.IngredientSO.rarity);
        IngredientInvDetails.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = ingrInfo.IngredientSO.icon;

        // Details
        IngredientInvDetails.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.IngredientTypeEnumToString(ingrInfo.IngredientSO.ingredientType);
        Transform rarity = IngredientInvDetails.transform.GetChild(0).GetChild(5).GetChild(1).transform;

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < ingrInfo.IngredientSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        // Middle        
        IngredientInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.description;
        GameObject buttons = IngredientInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(2).gameObject;
        FixHeight buttonHeight = buttons.GetComponent<FixHeight>();

        // Clean Middle
        foreach (Transform t in buttons.transform)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            buttonHeight.offsets[i] = 0;
        }

        // Fix buttonHeight
        buttonHeight.multiplier = ingrInfo.IngredientSO.numberOfSources;
        for (int i = 0; i < ingrInfo.IngredientSO.numberOfSources; i++)
        {
            GameObject source = Instantiate(DomainSourcePrefab, buttons.transform);

            source.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.sources[i];
            
            buttonHeight.objects[i] = source.GetComponent<RectTransform>();
        }

        // Fix all UI position and height
        buttonHeight.UpdateHeight();
        IngredientInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixHeight>().UpdateHeight();
        IngredientInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        IngredientInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixUIPosition>().UpdatePosition();

        IngredientInvDetails.transform.GetChild(1).GetChild(2).GetComponent<FixHeight>().UpdateHeight();

        IngredientInvDetails.transform.GetChild(1).GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateFoodInvSlotDetails()
    {
        FoodInfo foodInfo = EventSystem.current.currentSelectedGameObject.GetComponent<FoodInfo>();

        // Activate selected border
        if (LastSelectedFoodSlot != null)
        {
            LastSelectedFoodSlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        LastSelectedFoodSlot = foodInfo.gameObject;
        foodInfo.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        // Deactivate "NEW" message
        foodInfo.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        // Top
        FoodInvDetails.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = FromIntToRarityColor(foodInfo.FoodSO.rarity);
        FoodInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<Image>().color = FromIntToRarityColor(foodInfo.FoodSO.rarity);
        FoodInvDetails.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).GetComponent<Image>().color = FromIntToRarityColor(foodInfo.FoodSO.rarity);

        FoodInvDetails.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.foodName;

        FoodInvDetails.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = FromIntToRaritySprite(foodInfo.FoodSO.rarity);
        FoodInvDetails.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = foodInfo.FoodSO.icon;

        // Details
        FoodInvDetails.transform.GetChild(0).GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.FoodTypeEnumToString(foodInfo.FoodSO.foodType);
        FoodInvDetails.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Image>().sprite = FromBuffStringToSprite(foodInfo.FoodSO.FoodBuffTypeEnumToString(foodInfo.FoodSO.buffType));
        Transform rarity = FoodInvDetails.transform.GetChild(0).GetChild(5).GetChild(2).transform;

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < foodInfo.FoodSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        // Middle        
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "  • " + foodInfo.FoodSO.effect;
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.description;
        GameObject buttons = FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(2).GetChild(2).gameObject;
        FixHeight buttonHeight = buttons.GetComponent<FixHeight>();

        // Clean Middle
        foreach (Transform t in buttons.transform)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            buttonHeight.offsets[i] = 0;
        }

        // Fix buttonHeight
        buttonHeight.multiplier = foodInfo.FoodSO.numberOfSources;
        for (int i = 0; i < foodInfo.FoodSO.numberOfSources; i++)
        {
            GameObject source = Instantiate(DomainSourcePrefab, buttons.transform);

            source.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.sources[i];

            buttonHeight.objects[i] = source.GetComponent<RectTransform>();
        }

        // Fix all UI position and height
        buttonHeight.UpdateHeight();
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(2).GetComponent<FixHeight>().UpdateHeight();
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixHeight>().UpdateHeight();
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<FixUIPosition>().UpdatePosition();
        FoodInvDetails.transform.GetChild(1).GetChild(2).GetChild(2).GetComponent<FixUIPosition>().UpdatePosition();

        FoodInvDetails.transform.GetChild(1).GetChild(2).GetComponent<FixHeight>().UpdateHeight();

        FoodInvDetails.transform.GetChild(1).GetComponent<FixHeight>().UpdateHeight();
    }
//------------------------------------------------------------ TO DO ----------------------------------------------------------------------------
    public void UpdateSpecialItemDetails()
    {

    }


    public void SortWeaponInvTab(string sortType)
    {
        if (sortType == "Rarity")
        {
            RarityComp comparer = new RarityComp();

            EquipmentTab.Sort(comparer);

            for (int i = 0; i < EquipmentTab.Count; i++)
            {
                EquipmentTab[i].gameObject.transform.SetSiblingIndex(i);
            }
        }
        else if (sortType == "Level")
        {
            LevelComp comparer = new LevelComp();

            EquipmentTab.Sort(comparer);

            for (int i = 0; i < EquipmentTab.Count; i++)
            {
                EquipmentTab[i].gameObject.transform.SetSiblingIndex(i);
            }
        }
        else if (sortType == "Weapon Type")
        {
            
        }
    }

    public void SelectFirstInvItem(int index)
    {
        if (index == 0)
        {
            if (EquipmentTab.Count > 0)
            {
                WeaponInvDetails.SetActive(true);
                EventSystem.current.SetSelectedGameObject(EquipmentTab[0].gameObject);
                UpdateWeaponInvSlotDetails();
            }
            else
            {
                WeaponInvDetails.SetActive(false);
            }
        }
        else if (index == 1)
        {
            if (MaterialsTab.Count > 0)
            {
                MaterialInvDetails.SetActive(true);
                EventSystem.current.SetSelectedGameObject(MaterialsTab[0].gameObject);
                UpdateMaterialInvSlotDetails();
            }
            else
            {
                MaterialInvDetails.SetActive(false);
            }
        }
        else if (index == 2)
        {
            if (IngredientsTab.Count > 0)
            {
                IngredientInvDetails.SetActive(true);
                EventSystem.current.SetSelectedGameObject(IngredientsTab[0].gameObject);
                UpdateIngredientInvSlotDetails();
            }
            else
            {
                IngredientInvDetails.SetActive(false);
            }
        }
        else if (index == 3)
        {
            if (FoodTab.Count > 0)
            {
                FoodInvDetails.SetActive(true);
                EventSystem.current.SetSelectedGameObject(FoodTab[0].gameObject);
                UpdateFoodInvSlotDetails();
            }
            else
            {
                FoodInvDetails.SetActive(false);
            }
        }
        else
        {
            if (SpecialItemsTab.Count > 0)
            {
                SpecialItemInvDetails.SetActive(true);
                EventSystem.current.SetSelectedGameObject(SpecialItemsTab[0].gameObject);
                UpdateSpecialItemDetails();
            }
            else
            {
                SpecialItemInvDetails.SetActive(false);
            }
        }
    }


    public Sprite FromIntToRaritySprite(int rarity)
    {
        if (rarity == 1)
        {
            return CommonBG;
        }
        else if (rarity == 2)
        {
            return NotCommonBG;
        }
        else if (rarity == 3)
        {
            return RareBG;
        }
        else if (rarity == 4)
        {
            return EpicBG;
        }
        else
        {
            return LegendaryBG;
        }
    }

    public Color FromIntToRarityColor(int rarity)
    {
        if (rarity == 1)
        {
            return CommonNameSpace;
        }
        else if (rarity == 2)
        {
            return NotCommonNameSpace;
        }
        else if (rarity == 3)
        {
            return RareNameSpace;
        }
        else if (rarity == 4)
        {
            return EpicNameSpace;
        }
        else
        {
            return LegendaryNameSpace;
        }
    }

    public Sprite FromBuffStringToSprite(string buffType)
    {
        if (buffType == "Attack")
        {
            return AttackBuffIcon;
        }
        else if (buffType == "Defence")
        {
            return DefenceBuffIcon;
        }
        else if (buffType == "Heal")
        {
            return HealBuffIcon;
        }
        else if (buffType == "Speed")
        {
            return SpeedBuffIcon;
        }   
        else if (buffType == "Stamina Consumption")
        {
            return StaminaConsumptionBuffIcon;
        }
        else
        {
            return PotionIcon;
        }
    }

    public void HeapSorting(GameObject[] arr)
    { 
        int n = arr.Length;

        while (n > 2)
        {
            #region Create Min Heap
            int i = n - 1;

            while (i > 0)
            {
                int pI = Mathf.FloorToInt((float)(i - 1) / 2);

                int c1I = 2 * pI + 1;
                int c2I = 2 * pI + 2;

                if (c2I > n - 1) // There is just the left child
                {
                    if (arr[c1I].GetComponent<WeaponInfo>().SO.rarity < arr[pI].GetComponent<WeaponInfo>().SO.rarity) // Swap the positions
                    {
                        GameObject val = arr[c1I];
                        arr[c1I] = arr[pI];
                        arr[pI] = val;
                    }
                    else if ((arr[c1I].GetComponent<WeaponInfo>().SO.rarity == arr[pI].GetComponent<WeaponInfo>().SO.rarity)) // Support option
                    {
                        if (arr[c1I].GetComponent<WeaponInfo>().currentLevel < arr[pI].GetComponent<WeaponInfo>().currentLevel) // Swap the positions
                        {
                            GameObject val = arr[c1I];
                            arr[c1I] = arr[pI];
                            arr[pI] = val;
                        }
                    }
                }
                else // There are both children
                {
                    int minI = c1I;

                    if (arr[c2I].GetComponent<WeaponInfo>().SO.rarity < arr[c1I].GetComponent<WeaponInfo>().SO.rarity)
                    {
                        minI = c2I;
                    }
                    else if (arr[c2I].GetComponent<WeaponInfo>().SO.rarity == arr[c1I].GetComponent<WeaponInfo>().SO.rarity)
                    {
                        if (arr[c2I].GetComponent<WeaponInfo>().currentLevel < arr[c1I].GetComponent<WeaponInfo>().currentLevel)
                        {
                            minI = c2I;
                        }
                    }

                    if (arr[minI].GetComponent<WeaponInfo>().SO.rarity < arr[pI].GetComponent<WeaponInfo>().SO.rarity)
                    {
                        GameObject val = arr[minI];
                        arr[minI] = arr[pI];
                        arr[pI] = val;
                    }
                    else if (arr[minI].GetComponent<WeaponInfo>().SO.rarity == arr[pI].GetComponent<WeaponInfo>().SO.rarity)
                    {
                        if (arr[minI].GetComponent<WeaponInfo>().currentLevel < arr[pI].GetComponent<WeaponInfo>().currentLevel)
                        {
                            GameObject val = arr[minI];
                            arr[minI] = arr[pI];
                            arr[pI] = val;
                        }
                    }
                }

                i -= 2;
            }
            #endregion

            GameObject minVal = arr[0];
            arr[0] = arr[n - 1];
            arr[n - 1] = minVal;

            n--;
        }

        if (arr[0].GetComponent<WeaponInfo>().SO.rarity < arr[1].GetComponent<WeaponInfo>().SO.rarity)
        {
            GameObject maxVal = arr[1];
            arr[1] = arr[0];
            arr[0] = maxVal;
        }
        else if (arr[0].GetComponent<WeaponInfo>().SO.rarity == arr[1].GetComponent<WeaponInfo>().SO.rarity)
        {
            if (arr[0].GetComponent<WeaponInfo>().currentLevel < arr[1].GetComponent<WeaponInfo>().currentLevel)
            {
                GameObject maxVal = arr[1];
                arr[1] = arr[0];
                arr[0] = maxVal;
            }
        }

        // Transform.SetSibilingIndex

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].transform.SetSiblingIndex(i);
        }
    }
}

// Rarity -> Level
public class RarityComp : IComparer<WeaponInfo> 
{
    public int Compare(WeaponInfo x, WeaponInfo y)
    {
        if (x == null)
        {
            if (y == null) // If x is null and y is null, they're equal.
            {
                return 0;
            }
            else // If x is null and y is not null, y is greater.
            {                
                return -1;
            }
        }
        else // If x is not null...
        {
            if (y == null) // ...and y is null, x is greater.
            {
                return 1;
            }
            else // ...and y is not null, compare the lengths of the two strings.
            {
                int difference = y.SO.rarity.CompareTo(x.SO.rarity);

                if (difference != 0) // If the strings are not of equal length, the longer string is greater.
                {
                    return difference;
                }
                else // If the strings are of equal length, use a second option.
                {
                    difference = y.currentLevel.CompareTo(x.currentLevel);

                    if (difference != 0)
                    {
                        return difference;
                    }
                    else // I return they are equal, but I could keep going with other options
                    {
                        return 0;
                    }
                }
            }
        }
    }
}

// Level -> Rarity
public class LevelComp : IComparer<WeaponInfo>
{
    public int Compare(WeaponInfo x, WeaponInfo y)
    {
        if (x == null)
        {
            if (y == null) // If x is null and y is null, they're equal.
            {
                return 0;
            }
            else // If x is null and y is not null, y is greater.
            {
                return -1;
            }
        }
        else // If x is not null...
        {
            if (y == null) // ...and y is null, x is greater.
            {
                return 1;
            }
            else // ...and y is not null, compare the lengths of the two strings.
            {
                int difference = y.currentLevel.CompareTo(x.currentLevel);

                if (difference != 0) // If the strings are not of equal length, the longer string is greater.
                {
                    return difference;
                }
                else // If the strings are of equal length, use a second option.
                {
                    difference = y.SO.rarity.CompareTo(x.SO.rarity);

                    if (difference != 0)
                    {
                        return difference;
                    }
                    else // I return they are equal, but I could keep going with other options
                    {
                        return 0;
                    }
                }
            }
        }
    }
}


