using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStateManager : MonoBehaviour
{
    [Header("Common UI Section")]
    public GameObject HealthObj;
    [Space]
    public TextMeshProUGUI MoneyText;
    [Space]
    public TextMeshProUGUI currSectionNameTxt;
    [Space]
    public GameObject LeftButton;
    public GameObject RightButton;
    public TextMeshProUGUI LeftKeyText;
    public TextMeshProUGUI RightKeyText;
    [Space]
    public TextMeshProUGUI LeftSectionText;
    public TextMeshProUGUI RightSectionText;
    [Space]
    public string[] SectionTextStrings = new string[4];
    [Space]
    public Image[] Dots = new Image[4];
    [Space]
    public Color DotActiveColor;
    public Color DotInactiveColor;
    [Space]
    public Vector3 DotActiveScale;
    public Vector3 DotInactiveScale;
    [Space]
    public GameObject[] SectionTabs = new GameObject[4];
    [Space]
    public int currSectionTab;

    [Header("Quests Section")]
    public TextMeshProUGUI QuestNameTxt;
    public TextMeshProUGUI QuestGiverTxt;
    public TextMeshProUGUI QuestPlaceTxt;
    public TextMeshProUGUI QuestDescriptionTxt;
    [Space]
    public Color questIconDisabledColor;
    public Color questIconEnabledColor;
    [Space]
    public Image[] questTypeIcon = new Image[4];
    [Space]
    public GameObject[] questTypeSelected = new GameObject[4];
    [Space]
    public GameObject[] questTypeTextObj = new GameObject[4];
    [Space]
    public GameObject[] questTab = new GameObject[4];
    [Space]
    public int currQuestTab;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeToQuestTab(currQuestTab - 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeToQuestTab(currQuestTab + 1);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeUISection(currSectionTab - 1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeUISection(currSectionTab + 1);
        }
    }

    public void ChangeUISection(int section)
    {
        LeftButton.SetActive(true);
        RightButton.SetActive(true);

        SectionTabs[currSectionTab].SetActive(false);
        Dots[currSectionTab].color = DotInactiveColor;
        Dots[currSectionTab].gameObject.transform.localScale = DotInactiveScale;

        currSectionTab = section;

        SectionTabs[currSectionTab].SetActive(true);
        Dots[currSectionTab].color = DotActiveColor;
        Dots[currSectionTab].gameObject.transform.localScale = DotActiveScale;

        currSectionNameTxt.text = SectionTextStrings[section];

        if (section == 0)
        {
            LeftButton.SetActive(false);
            RightSectionText.text = SectionTextStrings[1];
        }            
        else if (section == 3)
        {
            RightButton.SetActive(false);
            LeftSectionText.text = SectionTextStrings[2];
        }
        else
        {
            LeftSectionText.text = SectionTextStrings[section - 1];
            RightSectionText.text = SectionTextStrings[section + 1];
        }
    }

    public void ChangeToQuestTab(int tab)
    {
        if (tab > 3 || tab < 0)
            return;

        questTypeIcon[currQuestTab].color = questIconDisabledColor;
        questTypeSelected[currQuestTab].SetActive(false);
        questTypeTextObj[currQuestTab].SetActive(false);
        questTab[currQuestTab].SetActive(false);

        currQuestTab = tab;

        questTypeIcon[currQuestTab].color = questIconEnabledColor;
        questTypeSelected[currQuestTab].SetActive(true);
        questTypeTextObj[currQuestTab].SetActive(true);
        questTab[currQuestTab].SetActive(true);

        // Update the quest description on the right
    }

    public void AddItemToInventory()
    {
        // First page already instantiated, if there is not enough space add a new window
        // Then take the first available slot, activate and set icon image
        // Activate the value icons based on the type of item
    }
}
