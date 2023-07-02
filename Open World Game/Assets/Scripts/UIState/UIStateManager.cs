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
    

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUISection(int section)
    {
        if (section < 0 || section > 3 || section == currSectionTab)
        {
            return;
        }

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
}
