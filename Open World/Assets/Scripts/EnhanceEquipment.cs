using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhanceEquipment : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        InitializeSection();

        for (int i = 1; i < 20; i++)
        {
            Debug.Log("From " + i + " to " + (i + 1) + ": " + weapInfo.XpFromAToB(i, i + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeSection()
    {
        weapInfo = weaponObj.GetComponent<WeaponInfo>();

        levelText.text = "Level: " + weapInfo.currentLevel;
        attackText.text = "Attack: " + (int)weapInfo.baseATK;

        appliedXpSlider.value = (float)weapInfo.currentXp / weapInfo.xpForNextLevel;

        currAddingXp = 0;
        levelJump = 0;
        spareXp = 0;

        // Decide if is Enhance or Ascend section
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

    public void AddXp(int val)
    {
        // Is the max xp that can be added
        currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

        if (currAddingXp < currentMaxXp)
        {
            // Can add Xp
            if (currAddingXp + val < currentMaxXp) // Have not exceeded current max xp
            {
                currAddingXp += val;

                toApplyXpSlider.value = (float)(weapInfo.currentXp + currAddingXp) / weapInfo.xpForNextLevel;

                addingXp.text = "+" + currAddingXp.ToString();
            }
            else
            {
                spareXp = currAddingXp + val - currentMaxXp;

                currAddingXp += val;

                toApplyXpSlider.value = 1f;

                addingXp.text = "+" + currentMaxXp.ToString();

                hasReachedCurrMaxLvl = true;
            }
        }

        if (weapInfo.currentXp + currAddingXp > weapInfo.XpForNextLevel(weapInfo.currentLevel + 1)) // It is not in the same level
        {
            levelJump = 1;

            while (weapInfo.currentXp + currAddingXp >= weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + levelJump))
            {
                levelJump++;
            }

            levelJump--;

            levelText.text = "Level: " +  weapInfo.currentLevel + " <color=#FF9500>+ " + levelJump.ToString() + "</color>";
        }
        else
        {
            levelText.text = "Level: " + weapInfo.currentLevel;
        }
    }

    public void RemoveAddedXp(int val)
    {
        currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

        if (currAddingXp > 0)
        {
            currAddingXp -= val;
            if (currAddingXp > 0)
            {
                if (currAddingXp < currentMaxXp)
                {
                    addingXp.text = "+" + currAddingXp.ToString();

                    hasReachedCurrMaxLvl = false;
                }
                else
                {
                    addingXp.text = "+" + currentMaxXp.ToString();
                }
            }
            else
            {
                addingXp.text = "";
            }

            toApplyXpSlider.value = (float)(weapInfo.currentXp + currAddingXp) / weapInfo.xpForNextLevel;

            if (weapInfo.currentXp + currAddingXp > weapInfo.XpForNextLevel(weapInfo.currentLevel + 1)) // It is not in the same level
            {
                levelJump = 1;

                while (weapInfo.currentXp + currAddingXp >= weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + levelJump))
                {
                    levelJump++;
                }

                levelJump--;

                levelText.text = "Level: " + weapInfo.currentLevel + " <color=#FF9500>+ " + levelJump.ToString() + "</color>";
            }
            else
            {
                levelText.text = "Level: " + weapInfo.currentLevel;
            }
        }
    }

    public void Enhance()
    {
        if (currAddingXp > 0)
        {            
            spareXp = weapInfo.currentXp + currAddingXp - weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + levelJump);

            if (!hasReachedCurrMaxLvl)
            {
                weapInfo.currentXp = spareXp;
            }
            else
            {
                weapInfo.currentXp = 0;
            }

            weapInfo.currentLevel += levelJump;
            levelText.text = "Level: " + weapInfo.currentLevel.ToString();
            weapInfo.SetAtkFromLevel();
            attackText.text = "Attack: " + ((int)weapInfo.baseATK).ToString();
            weapInfo.xpForNextLevel = weapInfo.XpForNextLevel(weapInfo.currentLevel + 1);

            if (weapInfo.currentLevel == 20 || weapInfo.currentLevel == 40 || weapInfo.currentLevel == 60
                 || weapInfo.currentLevel == 70 || weapInfo.currentLevel == 80 || weapInfo.currentLevel == 90)
            {
                appliedXpSlider.value = 0f;
                toApplyXpSlider.value = 0f;
            }
            else if (weapInfo.currentLevel == 100)
            {
                appliedXpSlider.value = 1f;
                toApplyXpSlider.value = 1f;
            }
            else
            {
                appliedXpSlider.value = (float)weapInfo.currentXp / weapInfo.xpForNextLevel;
                toApplyXpSlider.value = (float)weapInfo.currentXp / weapInfo.xpForNextLevel;
            }            

            levelJump = 0;
            currAddingXp = 0;
            spareXp = 0;
            addingXp.text = "+0";

            InitializeSection();
        }        
    }

    public void Ascend()
    {
        if (weapInfo.currentLevel == 20)
        {
            weapInfo.currentMaxLevel = 40;
        }
        else if (weapInfo.currentLevel == 40)
        {
            weapInfo.currentMaxLevel = 60;
        }
        else if (weapInfo.currentLevel == 60)
        {
            weapInfo.currentMaxLevel = 70;
        }
        else if (weapInfo.currentLevel == 70)
        {
            weapInfo.currentMaxLevel = 80;
        }
        else if (weapInfo.currentLevel == 80)
        {
            weapInfo.currentMaxLevel = 90;
        }
        else if (weapInfo.currentLevel == 90)
        {
            weapInfo.currentMaxLevel = 100;
        }

        weapInfo.ascensionLevel++;

        weapInfo.SetAtkFromLevel();

        InitializeSection();
    }
}
