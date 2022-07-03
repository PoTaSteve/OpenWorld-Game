using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class EnhanceEquipmentManager : MonoBehaviour
{
    private WeaponInfo weapInfo;
    private GameObject weaponSlot;

    [Space]
    public GameObject Enh_Asc_MaxLvlSection;
    public TextMeshProUGUI Enh_Asc_MaxLvlText;

    [Space]
    [Header("Details Section")]
    public Details_Section det;

    [Space]
    [Header("Enhance Section")]
    public Enhance_Section enh;

    [Space]
    [Header("Ascend Section")]
    public Ascend_Section asc;

    [Space]
    [Header("Max level Section")]
    public MaxLevel_Section maxLvl;

    [Space]
    [Header("Refine Section")]
    public Refine_Section refine;

    public void OpenWeaponDetails()
    {
        weaponSlot = GameManager.Instance.invMan.LastSelectedSlot;
        weapInfo = weaponSlot.GetComponent<WeaponInfo>();

        if (weapInfo.currentLevel == weapInfo.currentMaxLevel)
        {
            Enh_Asc_MaxLvlText.text = "Ascend";
        }
        else if (weapInfo.currentLevel == 100 && weapInfo.ascensionLevel == 7)
        {
            Enh_Asc_MaxLvlText.text = "Max Level";
        }
        else
        {
            Enh_Asc_MaxLvlText.text = "Enhance";
        }

        ActivateDetailsSection();
    }

    public void CloseWeaponDetails()
    {
        EventSystem.current.SetSelectedGameObject(weaponSlot);
        GameManager.Instance.invMan.UpdateWeaponInvSlotDetails(weaponSlot);  // ??  GameManager.Instance.invMan.TabsContent[GameManager.Instance.invMan.currInvTab].transform.GetChild(0).gameObject
        
        // Need to update the slot UI (level)
        weaponSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + weapInfo.currentLevel;
        weaponSlot.transform.GetChild(5).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = weapInfo.refinementLevel.ToString();
    }

    public void ActivateDetailsSection()
    {
        det.DetailsSection.SetActive(true);
        Enh_Asc_MaxLvlSection.SetActive(false);
        refine.RefineSection.SetActive(false);

        //Need to show the weapon details
        foreach (Transform t in det.Rarity)
        {
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < weapInfo.scrObj.rarity; i++)
        {
            det.Rarity.GetChild(i).gameObject.SetActive(true);
        }

        foreach (Image image in det.SymbolIcons)
        {
            image.color = GameManager.Instance.invMan.DetailsSymboloColor[weapInfo.scrObj.rarity - 1];
        }

        det.Border.color = GameManager.Instance.invMan.DetailsBorderColor[weapInfo.scrObj.rarity - 1];

        det.icon.sprite = weapInfo.scrObj.icon;

        det.weaponNameText.text = weapInfo.scrObj.weaponName;
        det.weaponNameText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.weaponTypeText.text = weapInfo.StringWeaponType();
        det.weaponTypeText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.baseAtkText.text = Mathf.RoundToInt(weapInfo.baseATK).ToString();
        det.baseAtkText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.substatTypeText.text = weapInfo.scrObj.subStatType;
        det.substatTypeText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        if (weapInfo.scrObj.isSubStatPercentage)
        {
            det.substatText.text = (weapInfo.currentSubstat * 100).ToString() + "%";
        }
        else
        {
            det.substatText.text = weapInfo.currentSubstat.ToString();
        }        
        det.substatText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.currentLevelText.text = "Lv. " + weapInfo.currentLevel.ToString() + "/" + weapInfo.currentMaxLevel;
        det.currentLevelText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.refinementLevelText.text = "Refinement level: " + weapInfo.refinementLevel.ToString();
        det.refinementLevelText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        det.descriptionText.text = "<b>Effect</b>\n<size=14> </size>\n" + weapInfo.scrObj.effectName + "\n" + weapInfo.scrObj.effect + "\n\n<b>Description</b>\n<size=14> </size>\n" + weapInfo.scrObj.description; ;
        det.descriptionText.color = GameManager.Instance.invMan.DetailsTextColor[weapInfo.scrObj.rarity - 1];

        // Need to fix UI position and height
        det.descriptionText.gameObject.GetComponent<FixHeight>().UpdateHeight();
        det.ContentObj.GetComponent<FixHeight>().UpdateHeight();
    }

    public void ActivateRefineSection()
    {
        refine.RefineSection.SetActive(true);
        det.DetailsSection.SetActive(false);
        Enh_Asc_MaxLvlSection.SetActive(false);
    }

    public void InitializeEnh_Asc_MaxLvlSection()
    {
        Enh_Asc_MaxLvlSection.SetActive(true);
        det.DetailsSection.SetActive(false);
        refine.RefineSection.SetActive(false);
        // Decide which section to set active
        if (weapInfo.currentLevel == weapInfo.currentMaxLevel)
        {
            ActivateAscensionSection();
        }
        else if (weapInfo.currentLevel == 100 && weapInfo.ascensionLevel == 7)
        {
            // Activate Max level section
            maxLvl.MaxLevelSection.SetActive(true);
            asc.AscendSection.SetActive(false);
            enh.EnhanceSection.SetActive(false);

            enh.hasReachedCurrMaxLvl = true;
        }
        else
        {
            ActivateEnhanceSection();
        }
    }

    public void ActivateEnhanceSection()
    {
        enh.EnhanceSection.SetActive(true);
        asc.AscendSection.SetActive(false);
        maxLvl.MaxLevelSection.SetActive(false);

        enh.hasReachedCurrMaxLvl = false;

        foreach (Transform t in enh.SelectedEnhMatsParent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject NewSlot = Instantiate(enh.RightPanelSlotPrefab, enh.SelectedEnhMatsParent);
            NewSlot.transform.GetChild(2).gameObject.SetActive(false);
            NewSlot.GetComponent<Button>().onClick.AddListener(CreateEnhanceMatTab);
        }

        enh.Enh_occupiedSlots = 0;
        enh.Enh_selectedMaterials.Clear();

        enh.Enh_UpdatedStats.SetActive(false);
        enh.EnhanceMatWindow.SetActive(false);

        // Set current info
        enh.Enh_currentLevelText.text = "Lv. " + weapInfo.currentLevel;
        enh.Enh_currentAttackText.text = (Mathf.RoundToInt(weapInfo.baseATK)).ToString();
        enh.Enh_currentXpText.text = weapInfo.currentXp.ToString() + " / " + weapInfo.xpForNextLevel.ToString();
        enh.Enh_substatTypeText.text = weapInfo.scrObj.subStatType;

        enh.Enh_addingXpText.text = "";
        enh.Enh_addingLevelText.text = "";

        if (weapInfo.scrObj.rarity > 2)
        {
            if (weapInfo.scrObj.isSubStatPercentage)
            {
                enh.Enh_currentSubstatText.text = (weapInfo.currentSubstat * 100).ToString() + "%";
            }
            else
            {
                enh.Enh_currentSubstatText.text = weapInfo.currentSubstat.ToString();
            }
        }
        else
        {
            enh.Enh_substatTypeText.text = "";
            enh.Enh_currentSubstatText.text = "";
            enh.Enh_enhancedSubstatText.text = "";
        }


        // Set slider values (need slider component)
        enh.Enh_currentXpSlider.value = (float)weapInfo.currentXp / weapInfo.xpForNextLevel;
        enh.Enh_addingXpSlider.value = 0;

        // Reset enhancement values
        enh.addingXp = 0;
        enh.levelJump = 0;
        enh.spareXp = 0;
    }

    public void ActivateAscensionSection()
    {
        asc.AscendSection.SetActive(true);
        enh.EnhanceSection.SetActive(false);
        maxLvl.MaxLevelSection.SetActive(false);

        enh.hasReachedCurrMaxLvl = true;

        asc.newMaxLevel = weapInfo.NextMaxLevel(weapInfo.currentLevel);

        asc.Asc_currentLevelText.text = "Lv. " + weapInfo.currentLevel + "/" + weapInfo.currentMaxLevel;
        asc.Asc_enhancedLevelText.text = "Lv. " + weapInfo.currentLevel + "/" + asc.newMaxLevel;

        foreach (Transform t in asc.CurrentAscension)
        {
            t.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }

        foreach (Transform t in asc.EnhancedAscension)
        {
            t.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < weapInfo.ascensionLevel; i++)
        {
            asc.CurrentAscension.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < weapInfo.ascensionLevel + 1; i++)
        {
            asc.EnhancedAscension.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
        }

        asc.enhancedAttack = weapInfo.scrObj.BaseATKs[weapInfo.ascensionLevel + 1];
        asc.Asc_currentAttackText.text = Mathf.RoundToInt(weapInfo.baseATK).ToString();
        asc.Asc_enhacedAttackText.text = Mathf.RoundToInt(asc.enhancedAttack).ToString();

        asc.Asc_substatTypeText.text = weapInfo.scrObj.subStatType;
        if (weapInfo.scrObj.isSubStatPercentage)
        {
            asc.Asc_currentSubstatText.text = (weapInfo.currentSubstat * 100).ToString() + "%";
        }
        else
        {
            asc.Asc_currentSubstatText.text = weapInfo.currentSubstat.ToString();
        }

        // Set the Ascension materials
    }

    public void ActivateMaxLevelSection()
    {
        maxLvl.MaxLevelSection.SetActive(true);
        enh.EnhanceSection.SetActive(false);
        asc.AscendSection.SetActive(false);
    }

    public void Enhance()
    {
        if (enh.hasReachedCurrMaxLvl)
        {
            enh.spareXp = 0;
        }
        else
        {
            enh.spareXp = weapInfo.currentXp + enh.addingXp - weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + enh.levelJump);
        }

        enh.xpForNextLevel = weapInfo.XpForNextLevel(weapInfo.currentLevel + enh.levelJump);
        enh.Enh_currentXpSlider.value = (float)enh.spareXp / enh.xpForNextLevel;
        enh.Enh_addingXpSlider.value = 0f;

        // Edit weap info
        weapInfo.currentLevel += enh.levelJump;
        enh.Enh_currentLevelText.text = "Lv. " + weapInfo.currentLevel;

        weapInfo.baseATK = weapInfo.SetAtkFromLevel(weapInfo.currentLevel);
        enh.Enh_currentAttackText.text = (Mathf.RoundToInt(weapInfo.baseATK)).ToString();

        weapInfo.currentXp = enh.spareXp;
        enh.Enh_currentXpText.text = enh.spareXp.ToString() + "/" + enh.xpForNextLevel.ToString();

        weapInfo.currentSubstat = weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel);
        if (weapInfo.scrObj.isSubStatPercentage)
        {
            enh.Enh_currentSubstatText.text = (weapInfo.currentSubstat * 100).ToString() + "%";
        }
        else
        {
            enh.Enh_currentSubstatText.text = weapInfo.currentSubstat.ToString();
        }

        enh.Enh_UpdatedStats.SetActive(false);

        enh.addingXp = 0;
        enh.Enh_addingXpText.text = "";
        enh.levelJump = 0;
        enh.Enh_addingLevelText.text = "";

        enh.GeneratedIDs.Clear();

        foreach (EnhanceMaterialInfo enhMatInfo in enh.Enh_selectedMaterials)
        {
            if (enhMatInfo.type == EnhanceMaterialType.EnhanceMaterial)
            {
                if (enhMatInfo.selectedCount >= enhMatInfo.count)
                {
                    GameManager.Instance.invMan.MaterialsTab.Remove(enhMatInfo.matInfo.scrObj.ItemID);

                    Destroy(enhMatInfo.matInfo.gameObject);
                }
                else
                {
                    enhMatInfo.matInfo.count = enhMatInfo.count - enhMatInfo.selectedCount;
                }
            }
            else if (enhMatInfo.type == EnhanceMaterialType.Weapon)
            {
                GameManager.Instance.invMan.MaterialsTab.Remove(enhMatInfo.weapInfo.scrObj.ItemID);

                Destroy(enhMatInfo.weapInfo.gameObject);
            }
        }

        enh.Enh_selectedMaterials.Clear();

        enh.Enh_occupiedSlots = 0;

        enh.currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

        foreach (Transform t in enh.SelectedEnhMatsParent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject NewSlot = Instantiate(enh.RightPanelSlotPrefab, enh.SelectedEnhMatsParent);
            NewSlot.transform.GetChild(2).gameObject.SetActive(false);
            NewSlot.GetComponent<Button>().onClick.AddListener(CreateEnhanceMatTab);
        }


        foreach (Transform t in enh.EnhanceMatContent)
        {
            Destroy(t.gameObject);
        }

        enh.EnhanceMatWindow.SetActive(false);

        if (enh.hasReachedCurrMaxLvl)
        {
            ActivateAscensionSection();
        }
    }

    public void AddEnhanceMat(GameObject slot)
    {
        enh.currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

        if (enh.addingXp >= enh.currentMaxXp)
        {
            enh.hasReachedCurrMaxLvl = true;
        }
        else
        {
            enh.hasReachedCurrMaxLvl = false;
        }

        if (!enh.hasReachedCurrMaxLvl)
        {
            enh.currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

            EnhanceMaterialInfo enhMatInfo = slot.GetComponent<EnhanceMaterialInfo>();

            if (enh.Enh_occupiedSlots < 10 || (enhMatInfo.type == EnhanceMaterialType.EnhanceMaterial && enh.Enh_selectedMaterials.Contains(enhMatInfo)))
            {
                if (enhMatInfo.count > enhMatInfo.selectedCount)
                {
                    enhMatInfo.selectedCount++;

                    if (!enh.Enh_selectedMaterials.Contains(enhMatInfo))
                    {
                        Destroy(enh.SelectedEnhMatsParent.GetChild(9).gameObject);

                        GameObject NewSlot = Instantiate(enh.RightPanelSlotPrefab, enh.SelectedEnhMatsParent);
                        NewSlot.transform.GetChild(2).gameObject.SetActive(true);
                        NewSlot.transform.SetSiblingIndex(enh.Enh_occupiedSlots);
                        NewSlot.GetComponent<ID>().id = GenerateRandomID();
                        NewSlot.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = enhMatInfo.icon;

                        if (enhMatInfo.type == EnhanceMaterialType.Weapon)
                        {
                            NewSlot.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv. " + enhMatInfo.level;
                        }
                        else if (enhMatInfo.type == EnhanceMaterialType.EnhanceMaterial)
                        {
                            NewSlot.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "1/" + enhMatInfo.count;
                        }

                        enh.Enh_selectedMaterials.Add(enhMatInfo);
                        enhMatInfo.isSelected = true;
                        enhMatInfo.id = NewSlot.GetComponent<ID>().id;
                        enh.Enh_occupiedSlots++;

                        slot.transform.GetChild(3).gameObject.SetActive(true);

                        slot.transform.GetChild(6).gameObject.SetActive(true);
                    }
                    else
                    {
                        foreach (Transform t in enh.SelectedEnhMatsParent)
                        {
                            if (slot.GetComponent<EnhanceMaterialInfo>().id == t.gameObject.GetComponent<ID>().id)
                            {
                                t.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = enhMatInfo.selectedCount + "/" + enhMatInfo.count;
                            }
                        }
                    }

                    if (enhMatInfo.type == EnhanceMaterialType.EnhanceMaterial)
                    {
                        slot.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = (enhMatInfo.selectedCount).ToString();
                    }

                    enh.addingXp += enhMatInfo.Xp;
                    enh.Enh_addingXpText.text = "+" + enh.addingXp.ToString();
                    weapInfo.xpForNextLevel = weapInfo.XpForNextLevel(weapInfo.currentLevel + enh.levelJump);

                    float addingXpSliderValue = (float)(weapInfo.currentXp + enh.addingXp) / weapInfo.xpForNextLevel;

                    if (addingXpSliderValue > 1)
                    {
                        enh.Enh_addingXpSlider.value = 1;
                    }
                    else
                    {
                        enh.Enh_addingXpSlider.value = addingXpSliderValue;
                    }

                    if (weapInfo.currentXp + enh.addingXp >= weapInfo.XpForNextLevel(weapInfo.currentLevel))
                    {
                        enh.Enh_UpdatedStats.SetActive(true);

                        // Check how many levels is the increment
                        enh.levelJump = 1;

                        while (weapInfo.currentXp + enh.addingXp >= weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + enh.levelJump))
                        {
                            enh.levelJump++;
                        }

                        enh.levelJump--;

                        // Update the UI
                        enh.Enh_enhancedAttackText.text = Mathf.RoundToInt(weapInfo.SetAtkFromLevel(weapInfo.currentLevel + enh.levelJump)).ToString();
                        enh.Enh_addingLevelText.text = "+" + enh.levelJump.ToString();

                        if (weapInfo.scrObj.isSubStatPercentage)
                        {
                            enh.Enh_enhancedSubstatText.text = (weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel + enh.levelJump) * 100).ToString() + "%";
                        }
                        else
                        {
                            enh.Enh_enhancedSubstatText.text = weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel + enh.levelJump).ToString();
                        }
                    }
                }
            }

            if (enh.addingXp >= enh.currentMaxXp)
            {
                enh.hasReachedCurrMaxLvl = true;
            }
            else
            {
                enh.hasReachedCurrMaxLvl = false;
            }
        }
    }

    public void RemoveEnhanceMat(GameObject slot)
    {
        EnhanceMaterialInfo enhMatInfo = slot.GetComponent<EnhanceMaterialInfo>();

        if (enhMatInfo.type == EnhanceMaterialType.Weapon)
        {
            enh.addingXp -= enhMatInfo.Xp;
            enhMatInfo.selectedCount--;
            enh.Enh_occupiedSlots--;
            enh.Enh_selectedMaterials.Remove(enhMatInfo);

            foreach (Transform t in enh.SelectedEnhMatsParent.transform)
            {
                if (t.GetComponent<ID>().id == enhMatInfo.id)
                {
                    Destroy(t.gameObject);

                    GameObject NewSlot = Instantiate(enh.RightPanelSlotPrefab, enh.SelectedEnhMatsParent);
                    NewSlot.transform.GetChild(2).gameObject.SetActive(false);
                    NewSlot.GetComponent<Button>().onClick.AddListener(CreateEnhanceMatTab);
                }
            }

            // Update UI
            enhMatInfo.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            enhMatInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);
        }
        else if (enhMatInfo.type == EnhanceMaterialType.EnhanceMaterial)
        {
            enh.addingXp -= enhMatInfo.Xp;
            enhMatInfo.selectedCount--;

            foreach (Transform t in enh.SelectedEnhMatsParent.transform)
            {
                if (t.GetComponent<ID>().id == enhMatInfo.id)
                {
                    if (enhMatInfo.selectedCount <= 0)
                    {
                        Destroy(t.gameObject);

                        GameObject NewSlot = Instantiate(enh.RightPanelSlotPrefab, enh.SelectedEnhMatsParent);
                        NewSlot.transform.GetChild(2).gameObject.SetActive(false);
                        NewSlot.GetComponent<Button>().onClick.AddListener(CreateEnhanceMatTab);

                        enh.Enh_occupiedSlots--;
                        enh.Enh_selectedMaterials.Remove(enhMatInfo);
                    }
                    else
                    {
                        t.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = enhMatInfo.selectedCount + "/" + enhMatInfo.count;
                    }
                }
            }

            // Update UI
            if (enhMatInfo.selectedCount == 0)
            {
                enhMatInfo.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                enhMatInfo.gameObject.transform.GetChild(6).gameObject.SetActive(false);
            }
            else
            {
                enhMatInfo.gameObject.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = enhMatInfo.selectedCount.ToString();
            }
        }

        if (enh.levelJump == 0)
        {
            enh.Enh_UpdatedStats.SetActive(false);

            enh.Enh_addingLevelText.text = "";
        }
        else if (enh.levelJump == 1)
        {
            if (weapInfo.currentXp + enh.addingXp < weapInfo.XpForNextLevel(weapInfo.currentLevel))
            {
                enh.levelJump = 0;

                enh.Enh_UpdatedStats.SetActive(false);

                enh.Enh_addingLevelText.text = "";
            }
        }
        else
        {
            while (weapInfo.currentXp + enh.addingXp < weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentLevel + enh.levelJump))
            {
                enh.levelJump--;
            }

            if (enh.levelJump == 0)
            {
                enh.Enh_UpdatedStats.SetActive(false);

                enh.Enh_addingLevelText.text = "";
            }
            else
            {
                enh.Enh_addingLevelText.text = "+" + enh.levelJump;

                enh.Enh_enhancedAttackText.text = Mathf.RoundToInt(weapInfo.SetAtkFromLevel(weapInfo.currentLevel + enh.levelJump)).ToString();

                if (weapInfo.scrObj.isSubStatPercentage)
                {
                    enh.Enh_enhancedSubstatText.text = (weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel + enh.levelJump) * 100).ToString() + "%";
                }
                else
                {
                    enh.Enh_enhancedSubstatText.text = weapInfo.SetSecondaryStatFromLevel(weapInfo.currentLevel + enh.levelJump).ToString();
                }
            }
        }

        enh.Enh_addingXpText.text = "+" + enh.addingXp.ToString();

        weapInfo.xpForNextLevel = weapInfo.XpForNextLevel(weapInfo.currentLevel + enh.levelJump);
        float addingXpSliderValue = (float)(weapInfo.currentXp + enh.addingXp) / weapInfo.xpForNextLevel;

        if (addingXpSliderValue > 1)
        {
            enh.Enh_addingXpSlider.value = 1;
        }
        else
        {
            enh.Enh_addingXpSlider.value = addingXpSliderValue;
        }

        enh.currentMaxXp = weapInfo.XpFromAToB(weapInfo.currentLevel, weapInfo.currentMaxLevel) - weapInfo.currentXp;

        if (enh.addingXp >= enh.currentMaxXp)
        {
            enh.hasReachedCurrMaxLvl = true;
        }
        else
        {
            enh.hasReachedCurrMaxLvl = false;
        }
    }

    public void CreateEnhanceMatTab()
    {
        enh.enhancedLevel = weapInfo.currentLevel;

        enh.EnhanceMatWindow.SetActive(true);

        //Clear the window
        foreach (Transform t in enh.EnhanceMatContent)
        {
            Destroy(t.gameObject);
        }

        // Go through these two inventory tabs and search for the slots satifying the requirements
        foreach (Transform t in GameManager.Instance.invMan.TabsContent[1].transform)
        {
            MaterialInfo matInfo = t.gameObject.GetComponent<MaterialInfo>();
            if (matInfo.scrObj.materialType == MaterialTypeEnum.EnhanceMaterial)
            {
                GameObject newSlot = Instantiate(enh.Mat_EnhanceMatSlotPrefab, enh.EnhanceMatContent);

                newSlot.GetComponent<EnhanceMaterialInfo>().matInfo = matInfo;

                newSlot.GetComponent<EnhanceMaterialInfo>().selectedCount = 0;
                newSlot.GetComponent<EnhanceMaterialInfo>().count = matInfo.count;
                newSlot.GetComponent<EnhanceMaterialInfo>().rarity = matInfo.scrObj.rarity;
                newSlot.GetComponent<EnhanceMaterialInfo>().Xp = matInfo.scrObj.enhanceXp;
                newSlot.GetComponent<EnhanceMaterialInfo>().icon = matInfo.scrObj.icon;

                newSlot.GetComponent<EnhanceMaterialInfo>().type = EnhanceMaterialType.EnhanceMaterial;

                newSlot.GetComponent<Button>().onClick.AddListener(delegate { AddEnhanceMat(newSlot); });
                newSlot.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate { RemoveEnhanceMat(newSlot); });

                newSlot.transform.GetChild(3).gameObject.SetActive(false);
                newSlot.transform.GetChild(6).gameObject.SetActive(false);

                newSlot.transform.GetChild(1).GetComponent<Image>().sprite = matInfo.scrObj.icon;

                Transform rarity = newSlot.transform.GetChild(4);
                foreach (Transform tr in rarity)
                {
                    tr.gameObject.SetActive(false);
                }
                for (int i = 0; i < matInfo.scrObj.rarity; i++)
                {
                    rarity.GetChild(i).gameObject.SetActive(true);
                }

                newSlot.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = matInfo.count.ToString();
            }
        }

        foreach (Transform t in GameManager.Instance.invMan.TabsContent[0].transform)
        {
            WeaponInfo weapInfo = t.gameObject.GetComponent<WeaponInfo>();
            if (!weapInfo.isLocked)
            {
                GameObject newSlot = Instantiate(enh.Weap_EnhanceMatSlotPrefab, enh.EnhanceMatContent);

                newSlot.GetComponent<EnhanceMaterialInfo>().weapInfo = weapInfo;

                newSlot.GetComponent<EnhanceMaterialInfo>().selectedCount = 0;
                newSlot.GetComponent<EnhanceMaterialInfo>().count = 1;
                newSlot.GetComponent<EnhanceMaterialInfo>().rarity = weapInfo.scrObj.rarity;
                newSlot.GetComponent<EnhanceMaterialInfo>().Xp = weapInfo.scrObj.enhanceXp;
                newSlot.GetComponent<EnhanceMaterialInfo>().icon = weapInfo.scrObj.icon;
                newSlot.GetComponent<EnhanceMaterialInfo>().level = weapInfo.currentLevel;

                newSlot.GetComponent<EnhanceMaterialInfo>().type = EnhanceMaterialType.Weapon;

                newSlot.GetComponent<Button>().onClick.AddListener(delegate { AddEnhanceMat(newSlot); });
                newSlot.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate { RemoveEnhanceMat(newSlot); });

                newSlot.transform.GetChild(3).gameObject.SetActive(false);
                newSlot.transform.GetChild(6).gameObject.SetActive(false);

                newSlot.transform.GetChild(1).GetComponent<Image>().sprite = weapInfo.scrObj.icon;

                Transform rarity = newSlot.transform.GetChild(4);
                foreach (Transform tr in rarity)
                {
                    tr.gameObject.SetActive(false);
                }
                for (int i = 0; i < weapInfo.scrObj.rarity; i++)
                {
                    rarity.GetChild(i).gameObject.SetActive(true);
                }

                newSlot.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + weapInfo.currentLevel.ToString();
            }
        }

        // Fix Slot UI
        // Fix UI height
    }

    public void Ascend()
    {
        weapInfo.ascensionLevel++;
        weapInfo.baseATK = asc.enhancedAttack;

        if (weapInfo.currentLevel != 100)
        {
            weapInfo.currentMaxLevel = asc.newMaxLevel;

            enh.hasReachedCurrMaxLvl = false;

            ActivateEnhanceSection();
        }
        else
        {
            enh.hasReachedCurrMaxLvl = true;

            ActivateMaxLevelSection();
        }
    }

    public void Refine()
    {

    }

    public int GenerateRandomID()
    {
        int n;

        n = Random.Range(1, 20);

        while (enh.GeneratedIDs.Contains(n))
        {
            n = Random.Range(1, 20);
        }

        return n;
    }
}

[System.Serializable]
public class Details_Section
{
    public GameObject DetailsSection;
    public Transform Rarity;
    public GameObject ContentObj;
    [Space]
    public Image[] SymbolIcons = new Image[2];
    public Image Border;
    public Image icon;
    [Space]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponTypeText;
    public TextMeshProUGUI baseAtkText;
    public TextMeshProUGUI substatTypeText;
    public TextMeshProUGUI substatText;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI refinementLevelText;
    public TextMeshProUGUI descriptionText;
}

[System.Serializable]
public class Enhance_Section
{
    public GameObject EnhanceSection;
    public GameObject EnhanceMatWindow;
    public Transform EnhanceMatContent;
    public GameObject RightPanelSlotPrefab;
    public Transform SelectedEnhMatsParent;
    public GameObject Enh_UpdatedStats;
    public GameObject Mat_EnhanceMatSlotPrefab;
    public GameObject Weap_EnhanceMatSlotPrefab;

    [Space]
    public TextMeshProUGUI Enh_currentXpText;
    public TextMeshProUGUI Enh_addingXpText;
    public TextMeshProUGUI Enh_currentLevelText;
    public TextMeshProUGUI Enh_addingLevelText;

    [Space]
    public TextMeshProUGUI Enh_currentAttackText;
    public TextMeshProUGUI Enh_enhancedAttackText;
    public TextMeshProUGUI Enh_substatTypeText;
    public TextMeshProUGUI Enh_currentSubstatText;
    public TextMeshProUGUI Enh_enhancedSubstatText;

    [Space]
    public Slider Enh_addingXpSlider;
    public Slider Enh_currentXpSlider;

    public int addingXp;
    public int levelJump;
    public int currentMaxXp;
    public int spareXp;
    public int enhancedLevel;

    public bool hasReachedCurrMaxLvl;
    public int Enh_occupiedSlots;
    public int xpForNextLevel;

    public List<EnhanceMaterialInfo> Enh_selectedMaterials = new List<EnhanceMaterialInfo>();
    public List<int> GeneratedIDs = new List<int>();
}

[System.Serializable]
public class Ascend_Section
{
    public GameObject AscendSection;
    public Transform AscensionMatContent;
    public Transform CurrentAscension;
    public Transform EnhancedAscension;

    [Space]
    public TextMeshProUGUI Asc_currentLevelText;
    public TextMeshProUGUI Asc_enhancedLevelText;
    public TextMeshProUGUI Asc_currentAttackText;
    public TextMeshProUGUI Asc_enhacedAttackText;
    public TextMeshProUGUI Asc_substatTypeText;
    public TextMeshProUGUI Asc_currentSubstatText;

    [Space]
    public float enhancedAttack;
    public int newMaxLevel;
}

[System.Serializable]
public class MaxLevel_Section
{
    public GameObject MaxLevelSection;
}

[System.Serializable]
public class Refine_Section
{
    public GameObject RefineSection;
}
