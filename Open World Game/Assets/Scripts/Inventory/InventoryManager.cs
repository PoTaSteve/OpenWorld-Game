using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using UnityEditor.PackageManager.UI;

public enum ItemType
{
    Weapon,
    Material,
    Food,
    SpecialItem
}

public class InventoryManager : MonoBehaviour
{
    #region Defines
    private const int SLOTS_PER_ROW = 9;
    private const int ITEM_TYPES = 4;

    private const int SLOT_DIM_X = 120;
    private const int SLOT_DIM_Y = 120;

    private const int SLOT_SPACING_X = 20;
    private const int SLOT_SPACING_Y = 20;

    private const int SLOT_PADDING_LEFT = 80;
    private const int SLOT_PADDING_RIGHT = 80;
    private const int SLOT_PADDING_TOP = 30;
    private const int SLOT_PADDING_BOTTOM = 30;


    // Corner boxes val defines
    private const int SLOT_ICON_CHILD_INDEX = 1;
    private const int SLOT_HIGHLIGHT_CHILD_INDEX = 2;

    private const int SLOT_COUNTER_CHILD_INDEX = 3; // Bottom Right
    private const int SLOT_DET_ICON_CHILD_INDEX = 4; // Top Left

    // Tab icons page num dots child
    private const int TAB_ICON_PAGE_NUM_CHILD_INDEX = 1;

    private const float WINDOW_CHANGE_SPEED = 50;

    private const float WINDOW_WIDTH = 840; // Width of Weapons_Window Rect transform
    private const float WINDOW_START_X = 0;
    private const float WINDOW_START_Y = 0;
    #endregion

    public GameObject InventoryObj;
    [Space]
    public GameObject SlotPrefab;
    public GameObject DotPrefab;
    [Space]
    public int[] itemsInTab = new int[ITEM_TYPES];
    public int[] maxItemsForTab = new int[ITEM_TYPES];
    [Space]
    public GameObject[] Windows = new GameObject[ITEM_TYPES];
    public GameObject[] TabIcons = new GameObject[ITEM_TYPES];
    [Space]
    public List<string> weaponsContainedIDs = new List<string>();
    public List<int> materialsContainedIDs = new List<int>();
    public List<int> foodContainedIDs = new List<int>();
    public List<int> specialItemsContainedIDs = new List<int>();
    [Space]
    public RectTransform WindowsParent;
    public RectTransform ContainerParent;
    [Space]
    public int currTab;
    [Space]
    public Color UsedSlotBGColor;
    public Color EmptySlotBGColor;
    [Space]
    public Color ActiveTabIconColor;
    public Color InactiveTabIconColor;
    [Space]
    public Color ActiveDotColor;
    public Color InactiveDotColor;
    [Space]
    public Vector2 ActiveDotScale;
    public Vector2 InactiveDotScale;
    [Space]
    public TextMeshProUGUI ItemNameDet;
    public TextMeshProUGUI ItemDecriptionDet;
    [Space]
    public GameObject CurrSelectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        SetUpInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function called when the game is launched to create the inventory
    public void SetUpInventory()
    {
        foreach (GameObject obj in TabIcons)
        {
            obj.transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;
        }

        Windows[0].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void SetInventoryActive(bool active)
    {
        //GameManager.Instance.plInputMan.GameUIObj.SetActive(!active);

        //GameManager.Instance.plInputMan.UIStateObject.SetActive(active);
        GameManager.Instance.plInputMan.CommonUIObject.SetActive(active);

        GameManager.Instance.plInputMan.InventoryObj.SetActive(active);
        GameManager.Instance.plInputMan.QuestsUIObject.SetActive(false);
        GameManager.Instance.plInputMan.SkillsUIObject.SetActive(false);
        GameManager.Instance.plInputMan.SystemUIObject.SetActive(false);        

        if (active)
        {
            foreach (GameObject obj in TabIcons)
            {
                obj.transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;
            }

            if (currTab < 0 || currTab >= ITEM_TYPES)
            {
                currTab = 0;
            }

            if (Windows[currTab].transform.GetChild(0).childCount > 0)
            {
                CurrSelectedSlot = Windows[currTab].transform.GetChild(0).GetChild(0).gameObject;

                CurrSelectedSlot.GetComponent<InventorySlot>().UpdateItemDetails();
            }

            ContainerParent.anchoredPosition = new Vector2(-currTab * WINDOW_WIDTH, 0);

            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;
        }
    }

    #region Inventory With Same Window
    public void AddItemToInventory(ItemType type, GameObject item, int count)
    {
        int typeInt;
        bool isNewItem = false;
        int itemID = 0;

        // Set typeInt based on the type of item
        switch (type)
        {
            case ItemType.Weapon:
                typeInt = 0;
                break;

            case ItemType.Material:
                typeInt = 1;
                itemID = item.GetComponent<MaterialInfo>().scrObj.ItemID;
                isNewItem = !materialsContainedIDs.Contains(itemID);
                break;

            case ItemType.Food:
                typeInt = 2;
                itemID = item.GetComponent<FoodInfo>().scrObj.ItemID;
                isNewItem = !foodContainedIDs.Contains(itemID);
                break;

            case ItemType.SpecialItem:
                typeInt = 3;
                itemID = item.GetComponent<SpecialItemInfo>().scrObj.ItemID;
                isNewItem = !specialItemsContainedIDs.Contains(itemID);
                break;

            default:
                Debug.Log("Error adding item in inventory");
                return;
        }

        // Check add slot or count
        // weapons always add slot
        // others add slot if material is new and there is space
        // or add count if material is not new

        if (typeInt == 0)
        {
            AddSlotST(0, item, count);
        }
        else
        {
            if (isNewItem)
            {
                if (itemsInTab[typeInt] < maxItemsForTab[typeInt])
                {
                    AddSlotST(typeInt, item, count);
                }
                else
                {
                    Debug.Log("Cannot add item. Inventory window of type " + typeInt + " is full.");
                }
            }
            else
            {
                // Add count
                AddCountST(itemID, count);
            }
        }        
    }

    public void AddSlotST(int typeInt, GameObject item, int count)
    {
        GameObject Slot = Instantiate(SlotPrefab, Windows[typeInt].transform.GetChild(0));

        // Set the gfx of the slot when picking up an item
        switch (typeInt)
        {
            case 0:
                WeaponInfo weap = item.GetComponent<WeaponInfo>();

                weaponsContainedIDs.Add(weap.GetIdentificationString());

                Slot.GetComponent<InventorySlot>().weap = weap;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(true);

                Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(weap.hasDetailIcon);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = weap.scrObj.icon;

                break;

            case 1:
                MaterialInfo mat = item.GetComponent<MaterialInfo>();

                materialsContainedIDs.Add(mat.scrObj.ItemID);

                Slot.GetComponent<InventorySlot>().mat = mat;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(mat.scrObj.isStackable);

                Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(mat.scrObj.hasDetailIcon);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).GetChild(2).GetComponent<TextMeshProUGUI>().text = count.ToString();

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = mat.scrObj.icon;

                break;

            case 2:
                FoodInfo food = item.GetComponent<FoodInfo>();

                foodContainedIDs.Add(food.scrObj.ItemID);

                Slot.GetComponent<InventorySlot>().food = food;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(food.scrObj.isStackable);

                Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(food.scrObj.hasDetailIcon);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).GetChild(2).GetComponent<TextMeshProUGUI>().text = count.ToString();

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = food.scrObj.icon;

                break;

            // Special Item
            case 3:
                SpecialItemInfo specItem = item.GetComponent<SpecialItemInfo>();

                specialItemsContainedIDs.Add(specItem.scrObj.ItemID);

                Slot.GetComponent<InventorySlot>().specItem = specItem;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(specItem.scrObj.isStackable);

                Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(specItem.scrObj.hasDetailIcon);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = specItem.scrObj.icon;

                break;

            default:
                Debug.Log("Error adding item");
                return;
        }

        itemsInTab[typeInt]++;
    }

    public void AddCountST(int ID, int count)
    {

    }

    // For the buttons to click on
    public void GoToInventoryTab(int tab)
    {

    }

    // Window of the same item type, tab is for different item types
    // Can only go up or down by 1
    public void GoToNextInventoryWindow()
    {
        // Check if can change tab
        if (currTab >= ITEM_TYPES - 1)
        {
            return;
        }

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

        // Edit curr values
        currTab++;

        // Activate tab icon
        TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;
        #endregion

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * currTab, 0);

        //Can animate trasition with coroutine
    }

    public void GoToPreviousInventoryWindow()
    {
        // Change to next tab if not the last
        if (currTab <= 0)
        {
            return;
        }

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

        // Edit curr values
        currTab--;

        // Activate tab icon
        TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;
        #endregion

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * currTab, 0);

        //Can animate trasition with coroutine
    }
    #endregion


    public void UpdateItemDetails()
    {
        // Based on item type add it to the respective tab
    }

    public void RemoveItemFromInventory()
    {
        // Called when an item is consumed or dropped
    }

    public void RemoveSlot()
    {
        // If removing the item makes its count reach 0
    }
}


//public class InventoryManagerST : MonoBehaviour
//{
//    #region Defines
//    private const int SLOTS_IN_WINDOW = 20;
//    private const int ITEM_TYPES = 4;

//    // Corner boxes val defines
//    private const int SLOT_ICON_CHILD_INDEX = 1;
//    private const int SLOT_HIGHLIGHT_CHILD_INDEX = 2;

//    private const int SLOT_VAL_1_CHILD_INDEX = 3; // Bottom Right
//    private const int SLOT_VAL_2_CHILD_INDEX = 4; // Top Left
//    private const int SLOT_VAL_3_CHILD_INDEX = 5; // Top Right

//    // Tab icons page num dots child
//    private const int TAB_ICON_PAGE_NUM_CHILD_INDEX = 1;

//    private const float WINDOW_CHANGE_SPEED = 50;
//    #endregion

//    public GameObject InventoryObj;
//    [Space]
//    public GameObject SlotsWindowPrefab;
//    public GameObject SlotPrefab;
//    public GameObject DotPrefab;
//    [Space]
//    public int[] itemUsedSlots = new int[ITEM_TYPES];
//    public int[] maxItemsForTab = new int[ITEM_TYPES];
//    public int[] windowsInTab = new int[ITEM_TYPES];
//    [Space]
//    public GameObject WindowsContainerST;
//    public GameObject[] TabIcons = new GameObject[ITEM_TYPES];
//    [Space]
//    public int currTab;
//    public int currWindowInTab;
//    [Tooltip("Index of the window that was left open when inventory was closed last time")]
//    public int openedWindowOnClose = -1;
//    [Space]
//    public Color UsedSlotBGColor;
//    public Color EmptySlotBGColor;
//    [Space]
//    public Color ActiveTabIconColor;
//    public Color InactiveTabIconColor;
//    [Space]
//    public Color ActiveDotColor;
//    public Color InactiveDotColor;
//    [Space]
//    public Vector2 ActiveDotScale;
//    public Vector2 InactiveDotScale;
//    [Space]
//    public TextMeshProUGUI ItemNameDet;
//    public TextMeshProUGUI ItemDecriptionDet;
//    [Space]
//    public GameObject CurrSelectedSlot;

//    // Start is called before the first frame update
//    void Start()
//    {
//        SetUpInventory();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    // Function called when the game is launched to create the inventory
//    public void SetUpInventory()
//    {
//        if (openedWindowOnClose <= -1 || openedWindowOnClose >= ITEM_TYPES)
//        {
//            openedWindowOnClose = 0;
//        }

//        // Reset all dots in tab icons 
//        foreach (GameObject obj in TabIcons)
//        {
//            foreach (Transform t in obj.transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX))
//            {
//                t.gameObject.GetComponent<Image>().color = InactiveDotColor;
//                t.localScale = InactiveDotScale;
//            }
//        }

//        foreach (GameObject obj in TabIcons)
//        {
//            obj.transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;
//        }

//        // Set the first window of the tab to active
//        TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).GetComponent<Image>().color = ActiveDotColor;
//        TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).localScale = ActiveDotScale;

//        TabIcons[openedWindowOnClose].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

//        currTab = openedWindowOnClose;
//        currWindowInTab = 0;

//        // Set position of the window container
//        int offset = 0;
//        for (int i = 0; i < currTab; i++)
//        {
//            offset += windowsInTab[i];
//        }

//        WindowsContainerST.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-offset * 840, 0);
//    }

//    public void SetInventoryActive(bool active)
//    {
//        GameManager.Instance.plInputMan.GameUIObj.SetActive(!active);

//        GameManager.Instance.plInputMan.UIStateObject.SetActive(active);
//        GameManager.Instance.plInputMan.CommonUIObject.SetActive(active);

//        GameManager.Instance.plInputMan.InventoryObj.SetActive(active);
//        GameManager.Instance.plInputMan.QuestsUIObject.SetActive(false);
//        GameManager.Instance.plInputMan.SkillsUIObject.SetActive(false);
//        GameManager.Instance.plInputMan.SystemUIObject.SetActive(false);

//        if (active)
//        {
//            if (openedWindowOnClose <= -1 ||  openedWindowOnClose >= ITEM_TYPES)
//            {
//                openedWindowOnClose = 0;
//            }

//            // Reset all dots in tab icons 
//            foreach (GameObject obj in TabIcons)
//            {
//                foreach (Transform t in obj.transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX))
//                {
//                    t.gameObject.GetComponent<Image>().color = InactiveDotColor;
//                    t.localScale = InactiveDotScale;
//                }
//            }

//            foreach (GameObject obj in TabIcons)
//            {
//                obj.transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;
//            }

//            // Set the first window of the tab to active
//            TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).GetComponent<Image>().color = ActiveDotColor;
//            TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).localScale= ActiveDotScale;

//            TabIcons[openedWindowOnClose].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

//            currTab = openedWindowOnClose;
//            currWindowInTab = 0;

//            // Set position of the window container
//            int offset = 0;
//            for (int i = 0; i < currTab; i++)
//            {
//                offset += windowsInTab[i];
//            }

//            WindowsContainerST.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-offset * 840, 0);
//        }
//        else
//        {
//            openedWindowOnClose = currTab;
//        }
//    }

//    #region Inventory With Same Tab (ST)
//    public void AddItemToInventoryST(ItemType type, GameObject item)
//    {
//        int typeInt;

//        // Set typeInt based on the type of item
//        switch (type)
//        {
//            case ItemType.Weapon:
//                typeInt = 0;
//                break;

//            case ItemType.Material:
//                typeInt = 1;
//                break;

//            case ItemType.Food:
//                typeInt = 2;
//                break;

//            case ItemType.SpecialItem:
//                typeInt = 3;
//                break;

//            default:
//                typeInt = -1;
//                Debug.Log("Error adding item in inventory");
//                return;
//        }

//        if (itemUsedSlots[typeInt] % SLOTS_IN_WINDOW == 0 && itemUsedSlots[typeInt] != 0 && itemUsedSlots[typeInt] < maxItemsForTab[typeInt])
//        {
//            // Add window
//            AddWindowST(typeInt);
//        }

//        // else set the slot 
//        SetSlotST(typeInt, item);
//    }

//    public void AddWindowST(int type)
//    {
//        TabIcons[type].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).gameObject.SetActive(true);

//        Instantiate(DotPrefab, TabIcons[type].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX));

//        GameObject window = Instantiate(SlotsWindowPrefab, WindowsContainerST.transform.GetChild(0));

//        int offset = 0;
//        for (int i = 0; i <= type; i++)
//        {
//            offset += windowsInTab[i];
//        }

//        window.transform.SetSiblingIndex(offset);

//        windowsInTab[type]++;
//    }

//    public void SetSlotST(int typeInt, GameObject item)
//    {
//        // Set to -1 because index is count - 1
//        int offset = -1;
//        for (int i = 0; i <= typeInt; i++)
//        {
//            offset += windowsInTab[i];
//        }

//        GameObject Slot = WindowsContainerST.transform. GetChild(0).GetChild(offset).GetChild(itemUsedSlots[typeInt] % SLOTS_IN_WINDOW).gameObject;

//        // Set the gfx of the slot when picking up an item
//        switch (typeInt)
//        {
//            case 0:
//                WeaponInfo weap = item.GetComponent<WeaponInfo>();

//                Slot.GetComponent<InventorySlot>().weap = weap;
//                Slot.GetComponent<InventorySlot>().itemType = typeInt;

//                #region Corner boxes
//                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(weap.hasBottomRightValue);

//                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(weap.hasTopLeftValue);

//                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(weap.hasTopRightValue);
//                #endregion

//                Slot.GetComponent<Image>().color = UsedSlotBGColor;

//                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = weap.scrObj.icon;

//                break;

//            case 1:
//                MaterialInfo mat = item.GetComponent<MaterialInfo>();

//                Slot.GetComponent<InventorySlot>().mat = mat;
//                Slot.GetComponent<InventorySlot>().itemType = typeInt;

//                #region Corner boxes
//                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(mat.hasBottomRightValue);

//                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(mat.hasTopLeftValue);

//                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(mat.hasTopRightValue);
//                #endregion

//                Slot.GetComponent<Image>().color = UsedSlotBGColor;

//                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = mat.scrObj.icon;

//                break;

//            case 2:
//                FoodInfo food = item.GetComponent<FoodInfo>();

//                Slot.GetComponent<InventorySlot>().food = food;
//                Slot.GetComponent<InventorySlot>().itemType = typeInt;

//                #region Corner boxes
//                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(food.hasBottomRightValue);

//                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(food.hasTopLeftValue);

//                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(food.hasTopRightValue);
//                #endregion

//                Slot.GetComponent<Image>().color = UsedSlotBGColor;

//                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = food.scrObj.icon;

//                break;

//            // Special Item
//            case 3:
//                SpecialItemInfo specItem = item.GetComponent<SpecialItemInfo>();

//                Slot.GetComponent<InventorySlot>().specItem = specItem;
//                Slot.GetComponent<InventorySlot>().itemType = typeInt;

//                #region Corner boxes
//                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(specItem.hasBottomRightValue);

//                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(specItem.hasTopLeftValue);

//                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(specItem.hasTopRightValue);
//                #endregion

//                Slot.GetComponent<Image>().color = UsedSlotBGColor;

//                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
//                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = specItem.scrObj.icon;

//                break;

//            default:
//                Debug.Log("Error adding item");
//                return;
//        }

//        itemUsedSlots[typeInt]++;
//    }

//    // For the buttons to click on
//    public void GoToInventoryTabST(int tab)
//    {
        
//    }

//    // Window of the same item type, tab is for different item types
//    // Can only go up or down by 1
//    public void GoToNextInvetoryWindowST()
//    {
//        // Check if it was the last window of the tab or not
//        if (currWindowInTab + 1 >= windowsInTab[currTab])
//        {
//            // Change to next tab if not the last
//            if (currTab >= ITEM_TYPES - 1)
//            {
//                return;
//            }

//            #region Handle Tab Icon Area
//            // Deactivate tab icon
//            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

//            // Deactivate tab icon dot
//            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
//            Dot.localScale = InactiveDotScale;


//            // Edit curr values
//            currTab++;
//            currWindowInTab = 0;


//            // Activate tab icon
//            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

//            // Activate tab icon dot
//            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
//            ActiveDot.localScale = ActiveDotScale;
//            #endregion

//            StartCoroutine(AnimateWindowChangeST(-1));
//        }
//        else
//        {            
//            // Stay in the same tab but changes window
//            StartCoroutine(AnimateWindowChangeST(-1));

//            // Deactivate tab icon dot
//            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
//            Dot.localScale = InactiveDotScale;


//            // Edit curr values
//            currWindowInTab++;


//            // Activate tab icon dot
//            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
//            ActiveDot.localScale = ActiveDotScale;
//        }
//    }

//    public void GoToPreviousInvetoryWindowST()
//    {
//        // Check if it was the last window of the tab or not
//        if (currWindowInTab - 1 < 0)
//        {
//            // Change to next tab if not the last
//            if (currTab <= 0)
//            {
//                return;
//            }

//            #region Handle Tab Icon Area
//            // Deactivate tab icon
//            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

//            // Deactivate tab icon dot
//            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
//            Dot.localScale = InactiveDotScale;


//            // Edit curr values
//            currTab--;
//            currWindowInTab = windowsInTab[currTab] - 1;


//            // Activate tab icon
//            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

//            // Activate tab icon dot
//            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
//            ActiveDot.localScale = ActiveDotScale;
//            #endregion

//            StartCoroutine(AnimateWindowChangeST(1));
//        }
//        else
//        {
//            // Animated transition
//            StartCoroutine(AnimateWindowChangeST(1));

//            // Deactivate tab icon dot
//            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
//            Dot.localScale = InactiveDotScale;


//            // Edit curr values
//            currWindowInTab--;


//            // Activate tab icon dot
//            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

//            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
//            ActiveDot.localScale = ActiveDotScale;
//        }
//    }

//    private IEnumerator AnimateWindowChangeST(int dir)
//    {
//        float progress = 0;
//        // delta 840 is the width of the window UI
//        while (progress < 840)
//        {
//            float delta;

//            if (progress + WINDOW_CHANGE_SPEED > 840)
//            {
//                delta = 840 - (progress + WINDOW_CHANGE_SPEED) + WINDOW_CHANGE_SPEED;
//            }
//            else
//            {
//                delta = WINDOW_CHANGE_SPEED;
//            }

//            WindowsContainerST.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition += new Vector2(delta * dir, 0);

//            progress += delta;

//            yield return null;
//        }
//    }
//    #endregion


//    public void UpdateItemDetails()
//    {
//        // Based on item type add it to the respective tab
//    }

//    public void RemoveItemFromInventory()
//    {
//        // Called when an item is consumed or dropped
//    }

//    public void RemoveSlot()
//    {
//        // If removing the item makes its count reach 0
//    }

//    public void RemoveWindow()
//    {
//        // If RemoveSlot was done on the last slot of a window
//    }
//}
