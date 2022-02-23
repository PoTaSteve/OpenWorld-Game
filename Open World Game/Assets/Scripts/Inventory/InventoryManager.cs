using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    //public static InventoryManager Instance;

    public int currInvTab;
    [Space]
    public List<WeaponInfo> WeaponsTab = new List<WeaponInfo>();
    public List<MaterialInfo> MaterialsTab = new List<MaterialInfo>();
    public List<IngredientInfo> IngredientsTab = new List<IngredientInfo>();
    public List<FoodInfo> FoodTab = new List<FoodInfo>();
    public List<SpecialItemInfo> SpecialItemsTab = new List<SpecialItemInfo>();
    [Space]
    public List<string> WeaponTabStr = new List<string>();
    public List<string> MaterialsTabStr = new List<string>();
    public List<string> IngredientsTabStr = new List<string>();
    public List<string> FoodTabStr = new List<string>();
    public List<string> SpecialItemsTabStr = new List<string>();
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

    private GameObject LastSelectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        Windows[0].SetActive(true);
        Tabs[0].GetComponent<Animator>().SetTrigger("Selected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNextInvTab()
    {
        Windows[currInvTab].SetActive(false);
        Tabs[currInvTab].GetComponent<Animator>().SetTrigger("BackToNormal");

        if (currInvTab == 4)
        {
            currInvTab = 0;
        }
        else
        {
            currInvTab++;
        }

        Windows[currInvTab].SetActive(true);
        Tabs[currInvTab].GetComponent<Animator>().SetTrigger("Selected");

        if (TabsContent[currInvTab].transform.childCount > 0)
        {
            InvDetails[currInvTab].SetActive(true);
            EventSystem.current.SetSelectedGameObject(TabsContent[currInvTab].transform.GetChild(0).gameObject);
            UpdateInvSlotDetails();
        }
        else
        {
            InvDetails[currInvTab].SetActive(false);
        }
    }

    public void GoToPreviousInvTab()
    {
        Windows[currInvTab].SetActive(false);
        Tabs[currInvTab].GetComponent<Animator>().SetTrigger("BackToNormal");

        if (currInvTab == 0)
        {
            currInvTab = 4;
        }
        else
        {
            currInvTab--;
        }

        Windows[currInvTab].SetActive(true);
        Tabs[currInvTab].GetComponent<Animator>().SetTrigger("Selected");

        if (TabsContent[currInvTab].transform.childCount > 0)
        {
            InvDetails[currInvTab].SetActive(true);
            EventSystem.current.SetSelectedGameObject(TabsContent[currInvTab].transform.GetChild(0).gameObject);
            UpdateInvSlotDetails();
        }
        else
        {
            InvDetails[currInvTab].SetActive(false);
        }
    }

    public void GoToSpecificInvTab(int tab)
    {
        if (tab!= currInvTab)
        {
            if (currInvTab != -1)
            {
                Windows[currInvTab].SetActive(false);
                Tabs[currInvTab].GetComponent<Animator>().SetTrigger("BackToNormal");
            }

            Windows[tab].SetActive(true);
            Tabs[tab].GetComponent<Animator>().SetTrigger("Selected");
            currInvTab = tab;

            if (TabsContent[currInvTab].transform.childCount > 0)
            {
                InvDetails[currInvTab].SetActive(true);
                EventSystem.current.SetSelectedGameObject(TabsContent[currInvTab].transform.GetChild(0).gameObject);
                UpdateInvSlotDetails();
            }
            else
            {
                InvDetails[currInvTab].SetActive(false);
            }
        }
    }

    public void UpdateInvSlotDetails()
    {
        if (currInvTab == 0)
        {
            UpdateWeaponInvSlotDetails();
        }
        else if (currInvTab == 1)
        {
            UpdateMaterialInvSlotDetails();
        }
        else if (currInvTab == 2)
        {
            UpdateIngredientInvSlotDetails();
        }
        else if (currInvTab == 3)
        {
            UpdateFoodInvSlotDetails();
        }
        else if (currInvTab == 4)
        {
            UpdateSpecialItemInvSlotDetails();
        }
        else
        {
            Debug.Log("Error updating inventory details. CurrInvTab out of range.");
        }
    }

    public void UpdateWeaponInvSlotDetails()
    {
        WeaponInfo weapInfo = EventSystem.current.currentSelectedGameObject.GetComponent<WeaponInfo>();

        GameObject Content = InvDetails[0].transform.GetChild(0).GetChild(2).gameObject;

        // Deactivate "NEW" message
        weapInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        weapInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = weapInfo.gameObject;

        // Top
        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.weaponName;
        Content.transform.GetChild(1).GetComponent<Image>().sprite = weapInfo.SO.icon;

        Content.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = weapInfo.StringWeaponType();

        Content.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(weapInfo.baseATK).ToString();

        if (weapInfo.SO.rarity <= 2)
        {
            Content.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "";
            Content.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            Content.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.subStatType;
            if (weapInfo.SO.isSubStatPercentage)
            {
                Content.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = (weapInfo.SO.subStat * 100).ToString() + "%";
            }
            else
            {
                Content.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = weapInfo.SO.subStat.ToString();
            }
        }

        // Rarity
        Transform rarity = Content.transform.GetChild(7);
        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < weapInfo.SO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        // Add level references
        // Add refinement references
        Transform MoreDet = Content.transform.GetChild(8);

        MoreDet.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv. " + weapInfo.currentLevel + "/" + weapInfo.currentMaxLevel;
        MoreDet.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Refinement level " + weapInfo.refinementLevel;

        // Add is locked references
        if (weapInfo.isLocked)
        {
            MoreDet.GetChild(2).GetChild(0).GetComponent<Image>().color = closedPadlockColBG;
            MoreDet.GetChild(2).GetChild(2).GetComponent<Image>().sprite = PadlockClosed;
            MoreDet.GetChild(2).GetChild(2).GetComponent<Image>().color = closedPadlockCol;
        }
        else
        {
            MoreDet.GetChild(2).GetChild(0).GetComponent<Image>().color = openPadlockColBG;
            MoreDet.GetChild(2).GetChild(2).GetComponent<Image>().sprite = PadlockOpen;
            MoreDet.GetChild(2).GetChild(2).GetComponent<Image>().color = openPadlockCol;
        }

        if (weapInfo.SO.rarity <= 2)
        {
            // Only set the description
            Content.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<b>Description</b>\n<size=14> </size>\n" + weapInfo.SO.description;
        }
        else
        {
            // Set the description and the effect
            Content.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "<b>Effect</b>\n<size=14> </size>\n" + weapInfo.SO.effectName + "\n" + weapInfo.SO.effect + "\n\n<b>Description</b>\n<size=14> </size>\n" + weapInfo.SO.description;
        }

        // Fix Colors
        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(6).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];

        Content.transform.GetChild(8).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];
        Content.transform.GetChild(8).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];

        Content.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[weapInfo.SO.rarity - 1];

        InvDetails[0].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = DetailsSymboloColor[weapInfo.SO.rarity - 1];
        InvDetails[0].transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>().color = DetailsSymboloColor[weapInfo.SO.rarity - 1];

        InvDetails[0].transform.GetChild(0).GetChild(3).GetComponent<Image>().color = DetailsBorderColor[weapInfo.SO.rarity - 1];

        // Fix UI Height and position
        Content.transform.GetChild(9).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        InvDetails[0].transform.GetChild(0).GetComponent<FixHeight>().UpdateHeight();
    }

    public void UpdateMaterialInvSlotDetails()
    {
        MaterialInfo matInfo = EventSystem.current.currentSelectedGameObject.GetComponent<MaterialInfo>();

        GameObject Content = InvDetails[1].transform.GetChild(0).GetChild(2).gameObject;

        // Deactivate "NEW" message
        matInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        matInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = matInfo.gameObject;

        // Update UI
        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.materialName;
        Content.transform.GetChild(1).GetComponent<Image>().sprite = matInfo.MaterialSO.icon;

        Transform rarity = Content.transform.GetChild(2);

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < matInfo.MaterialSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = matInfo.MaterialSO.description;

        //Fix UI Height and position
        Content.transform.GetChild(3).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        InvDetails[1].transform.GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        InvDetails[1].transform.GetChild(0).GetChild(3).GetComponent<Image>().color = DetailsBorderColor[matInfo.MaterialSO.rarity - 1];

        Transform BGMask = InvDetails[1].transform.GetChild(0).GetChild(1);
        foreach (Transform t in BGMask)
        {
            t.gameObject.GetComponent<Image>().color = DetailsSymboloColor[matInfo.MaterialSO.rarity - 1];
        }

        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[matInfo.MaterialSO.rarity - 1];
        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[matInfo.MaterialSO.rarity - 1];
    }

    public void UpdateIngredientInvSlotDetails()
    {
        IngredientInfo ingrInfo = EventSystem.current.currentSelectedGameObject.GetComponent<IngredientInfo>();

        GameObject Content = InvDetails[2].transform.GetChild(0).GetChild(2).gameObject;

        // Deactivate "NEW" message
        ingrInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        ingrInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = ingrInfo.gameObject;

        // Update UI
        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.ingredientName;
        Content.transform.GetChild(1).GetComponent<Image>().sprite = ingrInfo.IngredientSO.icon;

        Transform rarity = Content.transform.GetChild(2);

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < ingrInfo.IngredientSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = ingrInfo.IngredientSO.description;

        //Fix UI Height and position
        Content.transform.GetChild(3).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        InvDetails[2].transform.GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        InvDetails[2].transform.GetChild(0).GetChild(3).GetComponent<Image>().color = DetailsBorderColor[ingrInfo.IngredientSO.rarity - 1];

        Transform BGMask = InvDetails[2].transform.GetChild(0).GetChild(1);
        foreach (Transform t in BGMask)
        {
            t.gameObject.GetComponent<Image>().color = DetailsSymboloColor[ingrInfo.IngredientSO.rarity - 1];
        }

        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[ingrInfo.IngredientSO.rarity - 1];
        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[ingrInfo.IngredientSO.rarity - 1];
    }

    public void UpdateFoodInvSlotDetails()
    {
        FoodInfo foodInfo = EventSystem.current.currentSelectedGameObject.GetComponent<FoodInfo>();

        GameObject Content = InvDetails[3].transform.GetChild(0).GetChild(2).gameObject;

        // Deactivate "NEW" message
        foodInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);

        // Highlight Border
        if (LastSelectedSlot != null)
        {
            LastSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);
        }

        foodInfo.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        LastSelectedSlot = foodInfo.gameObject;

        // Update UI
        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.foodName;
        Content.transform.GetChild(1).GetComponent<Image>().sprite = foodInfo.FoodSO.icon;

        Transform rarity = Content.transform.GetChild(2);

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < foodInfo.FoodSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = foodInfo.FoodSO.description;

        //Fix UI Height and position
        Content.transform.GetChild(3).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        InvDetails[3].transform.GetChild(0).GetComponent<FixHeight>().UpdateHeight();

        // Update rarity colors
        InvDetails[3].transform.GetChild(0).GetChild(3).GetComponent<Image>().color = DetailsBorderColor[foodInfo.FoodSO.rarity - 1];

        Transform BGMask = InvDetails[3].transform.GetChild(0).GetChild(1);
        foreach (Transform t in BGMask)
        {
            t.gameObject.GetComponent<Image>().color = DetailsSymboloColor[foodInfo.FoodSO.rarity - 1];
        }

        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[foodInfo.FoodSO.rarity - 1];
        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = DetailsTextColor[foodInfo.FoodSO.rarity - 1];
    }

    public void UpdateSpecialItemInvSlotDetails()
    {
        SpecialItemInfo specItemInfo = EventSystem.current.currentSelectedGameObject.GetComponent<SpecialItemInfo>();

        GameObject Content = InvDetails[4].transform.GetChild(0).GetChild(3).gameObject;

        Content.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = specItemInfo.SpecialItemSO.specialItemName;
        Content.transform.GetChild(1).GetComponent<Image>().sprite = specItemInfo.SpecialItemSO.icon;

        Transform rarity = Content.transform.GetChild(2);

        foreach (Transform t in rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < specItemInfo.SpecialItemSO.rarity; i++)
        {
            rarity.GetChild(i).gameObject.SetActive(true);
        }

        Content.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = specItemInfo.SpecialItemSO.description;

        //Fix UI Height and position
        Content.transform.GetChild(3).GetChild(0).GetComponent<FixHeight>().UpdateHeight();
        InvDetails[4].transform.GetChild(0).GetComponent<FixHeight>().UpdateHeight();
    }
}
