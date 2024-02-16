using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    NONE = -1,
    General_Material = 0,
    Upgrade_Material = 1,
    Food = 2,
    Quest_Item = 3,
    Valuable_Item = 4
}

public enum EquipType
{
    NONE = -1,
    Weapon = 0,
    Head_Equip = 1,
    Chest_Equip = 2,
    Legs_Equip = 3
}

public class InventoryManager : MonoBehaviour
{
    #region Defines
    private const int ITEM_TYPES = 5;

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
    private const float WINDOW_CHANGE_TIME = 0.25f;

    private const float WINDOW_WIDTH = 840; // Width of Weapons_Window Rect transform
    private const float WINDOW_START_X = 0;
    private const float WINDOW_START_Y = 0;
    #endregion

    public AnimationCurve WindowChangePos;

    public GameEvent onItemCollect;

    [Space]
    public GameObject SlotPrefab;
    [Space]
    public int[] itemsInTab = new int[ITEM_TYPES];
    public int[] maxItemsForTab = new int[ITEM_TYPES];
    [Space]
    public GameObject[] WindowsContent = new GameObject[ITEM_TYPES];
    public Image[] TabImage = new Image[ITEM_TYPES];
    [Space]
    public Dictionary<string, int>[] containedIDLists = new Dictionary<string, int>[ITEM_TYPES];
    [Space]
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
    public TextMeshProUGUI ItemNameDetailTxt;
    public TextMeshProUGUI ItemDecriptionDetailTxt;
    [Space]
    public GameObject CurrSelectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        SetUpInventory();

        for (int i = 0; i < ITEM_TYPES; i++)
        {
            containedIDLists[i] = new Dictionary<string, int>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function called when the game is launched to create the inventory
    public void SetUpInventory()
    {
        foreach (Image img in TabImage)
        {
            img.color = InactiveTabIconColor;
        }

        WindowsContent[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void SetInventoryActive()
    {
        GameManager.Instance.plInputMan.CommonUIObject.SetActive(true);

        GameManager.Instance.plInputMan.InventoryObj.SetActive(true);
        GameManager.Instance.plInputMan.QuestsUIObject.SetActive(false);
        GameManager.Instance.plInputMan.SkillsUIObject.SetActive(false);
        GameManager.Instance.plInputMan.SystemUIObject.SetActive(false);

        foreach (Image img in TabImage)
        {
            img.color = InactiveTabIconColor;
        }

        if (currTab < 0 || currTab >= ITEM_TYPES)
        {
            currTab = 0;
        }

        if (WindowsContent[currTab].transform.childCount > 0)
        {
            CurrSelectedSlot = WindowsContent[currTab].transform.GetChild(0).gameObject;

            CurrSelectedSlot.GetComponent<InventorySlot>().UpdateItemDetails();
        }

        ContainerParent.anchoredPosition = new Vector2(-currTab * WINDOW_WIDTH, 0);

        TabImage[currTab].color = ActiveTabIconColor;
    }

    public void AddItemToInventory(ItemInfo item, int count)
    {
        string itemID = item.GetID();

        int typeInt = item.GetItemIntType();

        bool isNewItem = !containedIDLists[typeInt].ContainsKey(itemID);

        // Check add slot or count
        // weapons always add slot
        // others add slot if material is new and there is space
        // or add count if material is not new

        if (isNewItem)
        {
            if (itemsInTab[typeInt] < maxItemsForTab[typeInt])
            {
                AddSlot(item, count, true, true);
            }
            else
            {
                Debug.Log("Cannot add item. Inventory window of type " + typeInt + " is full.");
            }
        }
        else
        {
            // Add count
            AddCount(itemID, count);
        }
    }

    public void AddCount(string ID, int count)
    {

    }

    public void AddSlot(ItemInfo item, int count, bool hasCounter, bool hasDetailIcon)
    {
        int typeInt = item.GetItemIntType();

        GameObject Slot = Instantiate(SlotPrefab, WindowsContent[typeInt].transform);

        containedIDLists[typeInt].Add(item.GetID(), 1);

        // Slot.GetComponent<InventorySlot>().weapScrObj = item.scrObj;
        Slot.GetComponent<InventorySlot>().item = item;

        Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(hasCounter);

        Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(hasDetailIcon);

        Slot.GetComponent<Image>().color = UsedSlotBGColor;

        Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

        if (hasCounter)
        {
            Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).GetChild(2).GetComponent<TextMeshProUGUI>().text = count.ToString();
        }

        Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
        Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = item.GetIcon();

        itemsInTab[typeInt]++;
    }

    // For the buttons to click on
    public void GoToInventoryTab(int tab)
    {
        if (tab == currTab)
        {
            return;
        }

        StartCoroutine(AnimateTabCurve(currTab, tab, WINDOW_CHANGE_TIME));

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab = tab;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion
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

        StartCoroutine(AnimateTabCurve(currTab, currTab + 1, WINDOW_CHANGE_TIME));

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab++;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion

        //Can animate trasition with coroutine
    }

    public void GoToPreviousInventoryWindow()
    {
        // Change to next tab if not the last
        if (currTab <= 0)
        {
            return;
        }

        StartCoroutine(AnimateTabCurve(currTab, currTab - 1, WINDOW_CHANGE_TIME));

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab--;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion

        //Can animate trasition with coroutine
    }

    public void RemoveItemFromInventory()
    {
        // Called when an item is consumed or dropped
    }

    public void RemoveSlot()
    {
        // If removing the item makes its count reach 0
    }

    public IEnumerator AnimateTabCurve(int start, int target, float time)
    {
        int multiplier = 1;

        if (target > start)
        {
            multiplier = -1;
        }

        float timer = 0f;

        float startPosX = ContainerParent.anchoredPosition.x;
        float startPosY = ContainerParent.anchoredPosition.y;

        float totalDelta = Mathf.Abs(-WINDOW_WIDTH * target - startPosX);

        while (timer < 1f)
        {
            ContainerParent.anchoredPosition = new Vector2(startPosX + multiplier * totalDelta * WindowChangePos.Evaluate(timer), startPosY);

            timer += Time.deltaTime / time;

            yield return null;
        }

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * target, 0);
    }
}
