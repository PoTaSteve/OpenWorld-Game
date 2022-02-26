using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhanceEquipmentManager : MonoBehaviour
{
    private WeaponInfo weapInfo;

    public GameObject weaponObj;

    [SerializeField]
    private GameObject EnhanceSection;
    [SerializeField]
    private GameObject AscendSection;
    [SerializeField]
    private GameObject MaxLevelReached;

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI addingXp;
    [SerializeField]
    private TextMeshProUGUI attackText;

    [SerializeField]
    private Slider toApplyXpSlider;
    [SerializeField]
    private Slider appliedXpSlider;

    public int currAddingXp;
    private int levelJump;
    public int currentMaxXp;
    private int spareXp;

    public bool hasReachedCurrMaxLvl;

    private void Start()
    {
        InitializeSection();
    }

    public void InitializeSection()
    {
        weapInfo = weaponObj.GetComponent<WeaponInfo>();

        // Set current info
        levelText.text = "Level: " + weapInfo.currentLevel;
        attackText.text = "Attack: " + (int)weapInfo.baseATK;

        // Set slider values (need slider component)
        appliedXpSlider.value = (float)weapInfo.currentXp / weapInfo.xpForNextLevel;

        // Reset enhancement values
        currAddingXp = 0;
        levelJump = 0;
        spareXp = 0;

        // Decide which section to set active
        if ((weapInfo.currentLevel == 20 && weapInfo.ascensionLevel == 0)
            || (weapInfo.currentLevel == 40 && weapInfo.ascensionLevel == 1)
            || (weapInfo.currentLevel == 60 && weapInfo.ascensionLevel == 2)
            || (weapInfo.currentLevel == 70 && weapInfo.ascensionLevel == 3)
            || (weapInfo.currentLevel == 80 && weapInfo.ascensionLevel == 4)
            || (weapInfo.currentLevel == 90 && weapInfo.ascensionLevel == 5))
        {
            AscendSection.SetActive(true);
            EnhanceSection.SetActive(false);
            MaxLevelReached.SetActive(false);

            hasReachedCurrMaxLvl = true;
        }
        else if (weapInfo.currentLevel == 100 && weapInfo.ascensionLevel == 6)
        {
            MaxLevelReached.SetActive(true);
            AscendSection.SetActive(false);
            EnhanceSection.SetActive(false);

            hasReachedCurrMaxLvl = true;
        }
        else
        {
            EnhanceSection.SetActive(true);
            AscendSection.SetActive(false);
            MaxLevelReached.SetActive(false);

            hasReachedCurrMaxLvl = false;
        }
    }

    public void Enhance()
    {

    }

    public void Ascend()
    {

    }

    public void Refine()
    {

    }
}
