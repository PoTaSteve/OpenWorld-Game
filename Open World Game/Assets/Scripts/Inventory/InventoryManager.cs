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
    private const int SLOTS_IN_WINDOW = 20;
    private const int ITEM_TYPES = 4;

    // Corner boxes val defines
    private const int SLOT_ICON_CHILD_INDEX = 1;
    private const int SLOT_HIGHLIGHT_CHILD_INDEX = 2;

    private const int SLOT_VAL_1_CHILD_INDEX = 3; // Bottom Right
    private const int SLOT_VAL_2_CHILD_INDEX = 4; // Top Left
    private const int SLOT_VAL_3_CHILD_INDEX = 5; // Top Right

    // Tab icons page num dots child
    private const int TAB_ICON_PAGE_NUM_CHILD_INDEX = 1;

    private const float WINDOW_CHANGE_SPEED = 50;
    #endregion

    public GameObject InventoryObj;
    [Space]
    public GameObject SlotsWindowPrefab;
    public GameObject SlotPrefab;
    public GameObject DotPrefab;
    [Space]
    public int[] itemUsedSlots = new int[ITEM_TYPES];
    public int[] maxItemsForTab = new int[ITEM_TYPES];
    public int[] windowsInTab = new int[ITEM_TYPES];
    [Space]
    public GameObject WindowsContainerST;
    public GameObject[] TabIcons = new GameObject[ITEM_TYPES];
    [Space]
    public int currTab;
    public int currWindowInTab;
    [Tooltip("Index of the window that was left open when inventory was closed last time")]
    public int openedWindowOnClose = -1;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoToPreviousInvetoryWindowST();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoToNextInvetoryWindowST();
        }
    }

    // Function called when the game is launched to create the inventory
    public void CreateInventory()
    {

    }

    public void SetInventoryActive(bool active)
    {
        GameManager.Instance.plInMan.GameUIObj.SetActive(!active);

        GameManager.Instance.plInMan.UIStateObject.SetActive(active);
        GameManager.Instance.plInMan.CommonUIObject.SetActive(active);

        GameManager.Instance.plInMan.InventoryObj.SetActive(active);
        GameManager.Instance.plInMan.QuestsUIObject.SetActive(false);
        GameManager.Instance.plInMan.SkillsUIObject.SetActive(false);
        GameManager.Instance.plInMan.SystemUIObject.SetActive(false);

        if (active)
        {
            if (openedWindowOnClose <= -1 ||  openedWindowOnClose >= ITEM_TYPES)
            {
                openedWindowOnClose = 0;
            }

            // Reset all dots in tab icons 
            foreach (GameObject obj in TabIcons)
            {
                foreach (Transform t in obj.transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX))
                {
                    t.gameObject.GetComponent<Image>().color = InactiveDotColor;
                    t.localScale = InactiveDotScale;
                }
            }

            foreach (GameObject obj in TabIcons)
            {
                obj.transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;
            }

            // Set the first window of the tab to active
            TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).GetComponent<Image>().color = ActiveDotColor;
            TabIcons[openedWindowOnClose].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(0).localScale= ActiveDotScale;

            TabIcons[openedWindowOnClose].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

            currTab = openedWindowOnClose;
            currWindowInTab = 0;

            // Set position of the window container
            int offset = 0;
            for (int i = 0; i < currTab; i++)
            {
                offset += windowsInTab[i];
            }

            WindowsContainerST.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-offset * 840, 0);
        }
        else
        {
            openedWindowOnClose = currTab;
        }
    }

    #region Inventory With Same Tab (ST)
    public void AddItemToInventoryST(ItemType type, GameObject item)
    {
        int typeInt;

        // Set typeInt based on the type of item
        switch (type)
        {
            case ItemType.Weapon:
                typeInt = 0;
                break;

            case ItemType.Material:
                typeInt = 1;
                break;

            case ItemType.Food:
                typeInt = 2;
                break;

            case ItemType.SpecialItem:
                typeInt = 3;
                break;

            default:
                typeInt = -1;
                Debug.Log("Error adding item in inventory");
                return;
        }

        if (itemUsedSlots[typeInt] % SLOTS_IN_WINDOW == 0 && itemUsedSlots[typeInt] != 0 && itemUsedSlots[typeInt] < maxItemsForTab[typeInt])
        {
            // Add window
            AddWindowST(typeInt);
        }

        // else set the slot 
        SetSlotST(typeInt, item);
    }

    public void AddWindowST(int type)
    {
        TabIcons[type].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).gameObject.SetActive(true);

        Instantiate(DotPrefab, TabIcons[type].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX));

        GameObject window = Instantiate(SlotsWindowPrefab, WindowsContainerST.transform.GetChild(0));

        int offset = 0;
        for (int i = 0; i <= type; i++)
        {
            offset += windowsInTab[i];
        }

        window.transform.SetSiblingIndex(offset);

        windowsInTab[type]++;
    }

    public void SetSlotST(int typeInt, GameObject item)
    {
        // Set to -1 because index is count - 1
        int offset = -1;
        for (int i = 0; i <= typeInt; i++)
        {
            offset += windowsInTab[i];
        }

        GameObject Slot = WindowsContainerST.transform. GetChild(0).GetChild(offset).GetChild(itemUsedSlots[typeInt] % SLOTS_IN_WINDOW).gameObject;

        // Set the gfx of the slot when picking up an item
        switch (typeInt)
        {
            case 0:
                WeaponInfo weap = item.GetComponent<WeaponInfo>();

                Slot.GetComponent<InventorySlot>().info = weap;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(weap.hasBottomRightValue);

                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(weap.hasTopLeftValue);

                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(weap.hasTopRightValue);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = weap.scrObj.icon;

                break;

            case 1:
                MaterialInfo mat = item.GetComponent<MaterialInfo>();

                Slot.GetComponent<InventorySlot>().info = mat;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(mat.hasBottomRightValue);

                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(mat.hasTopLeftValue);

                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(mat.hasTopRightValue);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = mat.scrObj.icon;

                break;

            case 2:
                FoodInfo food = item.GetComponent<FoodInfo>();

                Slot.GetComponent<InventorySlot>().info = food;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(food.hasBottomRightValue);

                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(food.hasTopLeftValue);

                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(food.hasTopRightValue);
                #endregion

                Slot.GetComponent<Image>().color = UsedSlotBGColor;

                Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
                Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = food.scrObj.icon;

                break;

            // Special Item
            case 3:
                SpecialItemInfo specItem = item.GetComponent<SpecialItemInfo>();

                Slot.GetComponent<InventorySlot>().info = specItem;
                Slot.GetComponent<InventorySlot>().itemType = typeInt;

                #region Corner boxes
                Slot.transform.GetChild(SLOT_VAL_1_CHILD_INDEX).gameObject.SetActive(specItem.hasBottomRightValue);

                Slot.transform.GetChild(SLOT_VAL_2_CHILD_INDEX).gameObject.SetActive(specItem.hasTopLeftValue);

                Slot.transform.GetChild(SLOT_VAL_3_CHILD_INDEX).gameObject.SetActive(specItem.hasTopRightValue);
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

        itemUsedSlots[typeInt]++;
    }

    // For the buttons to click on
    public void GoToInventoryTabST(int tab)
    {
        
    }

    // Window of the same item type, tab is for different item types
    // Can only go up or down by 1
    public void GoToNextInvetoryWindowST()
    {
        // Check if it was the last window of the tab or not
        if (currWindowInTab + 1 >= windowsInTab[currTab])
        {
            // Change to next tab if not the last
            if (currTab >= ITEM_TYPES - 1)
            {
                return;
            }

            #region Handle Tab Icon Area
            // Deactivate tab icon
            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

            // Deactivate tab icon dot
            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
            Dot.localScale = InactiveDotScale;


            // Edit curr values
            currTab++;
            currWindowInTab = 0;


            // Activate tab icon
            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

            // Activate tab icon dot
            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
            ActiveDot.localScale = ActiveDotScale;
            #endregion

            StartCoroutine(AnimateWindowChangeST(-1));
        }
        else
        {            
            // Stay in the same tab but changes window
            StartCoroutine(AnimateWindowChangeST(-1));

            // Deactivate tab icon dot
            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
            Dot.localScale = InactiveDotScale;


            // Edit curr values
            currWindowInTab++;


            // Activate tab icon dot
            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
            ActiveDot.localScale = ActiveDotScale;
        }
    }

    public void GoToPreviousInvetoryWindowST()
    {
        // Check if it was the last window of the tab or not
        if (currWindowInTab - 1 < 0)
        {
            // Change to next tab if not the last
            if (currTab <= 0)
            {
                return;
            }

            #region Handle Tab Icon Area
            // Deactivate tab icon
            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = InactiveTabIconColor;

            // Deactivate tab icon dot
            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
            Dot.localScale = InactiveDotScale;


            // Edit curr values
            currTab--;
            currWindowInTab = windowsInTab[currTab] - 1;


            // Activate tab icon
            TabIcons[currTab].transform.GetChild(0).GetComponent<Image>().color = ActiveTabIconColor;

            // Activate tab icon dot
            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
            ActiveDot.localScale = ActiveDotScale;
            #endregion

            StartCoroutine(AnimateWindowChangeST(1));
        }
        else
        {
            // Animated transition
            StartCoroutine(AnimateWindowChangeST(1));

            // Deactivate tab icon dot
            Transform Dot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            Dot.gameObject.GetComponent<Image>().color = InactiveDotColor;
            Dot.localScale = InactiveDotScale;


            // Edit curr values
            currWindowInTab--;


            // Activate tab icon dot
            Transform ActiveDot = TabIcons[currTab].transform.GetChild(TAB_ICON_PAGE_NUM_CHILD_INDEX).GetChild(currWindowInTab);

            ActiveDot.gameObject.GetComponent<Image>().color = ActiveDotColor;
            ActiveDot.localScale = ActiveDotScale;
        }
    }

    private IEnumerator AnimateWindowChangeST(int dir)
    {
        float progress = 0;
        // delta 840 is the width of the window UI
        while (progress < 840)
        {
            float delta;

            if (progress + WINDOW_CHANGE_SPEED > 840)
            {
                delta = 840 - (progress + WINDOW_CHANGE_SPEED) + WINDOW_CHANGE_SPEED;
            }
            else
            {
                delta = WINDOW_CHANGE_SPEED;
            }

            WindowsContainerST.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition += new Vector2(delta * dir, 0);

            progress += delta;

            yield return null;
        }
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

    public void RemoveWindow()
    {
        // If RemoveSlot was done on the last slot of a window
    }
}
