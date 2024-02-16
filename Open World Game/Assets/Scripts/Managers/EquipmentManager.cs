using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region Defines
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

    public TextMeshProUGUI currMaxHP; // BaseHP * (1 + percHPBuff) + flatHPBuff

    public TextMeshProUGUI currATK; // BaseATK * (1 + percATKBuff) + flatATKBuff

    public TextMeshProUGUI currDEF; // BaseDEF * (1 + percDEFBuff) + flatDEFBuff

    [Space]
    public EquipmentInfo currHead;
    public EquipmentInfo currChest;
    public EquipmentInfo currLegs;
    public WeaponInfo currWeapon;

    [Space]
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
    //public Dictionary<string, int>[] containedIDLists = new Dictionary<string, int>[ITEM_TYPES];
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
    public GameObject CurrSelectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerStatsUI()
    {
        GameManager.Instance.plStats.UpdateAllCurrStats();

        currMaxHP.text = GameManager.Instance.plStats.GetCurrMaxHP().ToString();
        currATK.text = GameManager.Instance.plStats.GetCurrATK().ToString();
        currDEF.text = GameManager.Instance.plStats.GetCurrDEF().ToString();
    }

    public void AddItemToEquipmentInventory(ItemInfo item)
    {        
        AddSlot(item, true, true);
    }

    public void AddSlot(ItemInfo item, bool hasCounter, bool hasDetailIcon)
    {
        int typeInt = item.GetItemIntType();

        GameObject Slot = Instantiate(SlotPrefab, WindowsContent[typeInt].transform);

        //containedIDLists[typeInt].Add(item.GetID(), 1);

        // Slot.GetComponent<InventorySlot>().weapScrObj = item.scrObj;
        Slot.GetComponent<InventorySlot>().item = item;

        Slot.transform.GetChild(SLOT_COUNTER_CHILD_INDEX).gameObject.SetActive(hasCounter);

        Slot.transform.GetChild(SLOT_DET_ICON_CHILD_INDEX).gameObject.SetActive(hasDetailIcon);

        Slot.GetComponent<Image>().color = UsedSlotBGColor;

        Slot.transform.GetChild(SLOT_HIGHLIGHT_CHILD_INDEX).gameObject.SetActive(false);

        Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).gameObject.SetActive(true);
        Slot.transform.GetChild(SLOT_ICON_CHILD_INDEX).GetComponent<Image>().sprite = item.GetIcon();

        itemsInTab[typeInt]++;
    }

    public void GoToEquipmentTab(int tab)
    {
        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab = tab;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * currTab, 0);
    }

    // Window of the same item type, tab is for different item types
    // Can only go up or down by 1
    public void GoToNextEquipmentWindow()
    {
        // Check if can change tab
        if (currTab >= ITEM_TYPES - 1)
        {
            return;
        }

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab++;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * currTab, 0);

        //Can animate trasition with coroutine
    }

    public void GoToPreviousEquipmentWindow()
    {
        // Change to next tab if not the last
        if (currTab <= 0)
        {
            return;
        }

        #region Handle Tab Icon Area
        // Deactivate tab icon
        TabImage[currTab].color = InactiveTabIconColor;

        // Edit curr values
        currTab--;

        // Activate tab icon
        TabImage[currTab].color = ActiveTabIconColor;
        #endregion

        ContainerParent.anchoredPosition = new Vector2(-WINDOW_WIDTH * currTab, 0);

        //Can animate trasition with coroutine
    }

    public void SelectSlot()
    {
        Debug.Log("Selected a slot");
    }

    public void EquipButton()
    {
        EquipableItem newEquipable = CurrSelectedSlot.GetComponent<EquipableItem>();

        EquipType newEquipType = newEquipable.GetEquipType();

        // Unequip old equipment of right type
        switch (newEquipType)
        {
            case EquipType.NONE:
                Debug.LogError("Unequipping equipment of type: NONE");
                return;

            case EquipType.Weapon:

                currWeapon.Unequip();

                break;

            case EquipType.Head_Equip:

                currHead.Unequip();

                break;

            case EquipType.Chest_Equip:

                currChest.Unequip();

                break;

            case EquipType.Legs_Equip:

                currLegs.Unequip();

                break;

            default:
                Debug.LogError("Tryig to unequip equipment type not set");
                return;
        }

        newEquipable.Equip();
    }
}
