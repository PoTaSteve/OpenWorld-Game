using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    public List<IDToNPC> NPC_ID = new List<IDToNPC>();
    //public List<IDToInteractables> OtherInteractablesID = new List<IDToInteractables>();
    //public List<IDToWeaponScrObj> WeaponsID = new List<IDToWeaponScrObj>();
    //public List<IDToMaterialScrObj> MaterialsID = new List<IDToMaterialScrObj>();
    //public List<IDToFoodScrObj> FoodID = new List<IDToFoodScrObj>();
    //public List<IDToSpecialItemScrObj> SpecialItemsID = new List<IDToSpecialItemScrObj>();

    public GameObject NPCFromID(string ID)
    {
        foreach (IDToNPC obj in NPC_ID)
        {
            if (obj.ID == ID)
            {
                Debug.Log("Found NPC: " + obj.NPC.name);

                return obj.NPC;
            }
        }

        Debug.Log("Error: NPC not found");
        return null;
    }
}

//#region classes for variables

[Serializable]
public class IDToNPC
{
    public string ID;
    public GameObject NPC;
}

//[Serializable]
//public class IDToInteractables
//{
//    public string ID;
//    public GameObject InteractableObj;
//}

//[Serializable]
//public class IDToWeaponScrObj
//{
//    public string ID;
//    public WeaponScriptableObject weapScrObj;
//}

//[Serializable]
//public class IDToMaterialScrObj
//{
//    public string ID;
//    public MaterialScriptableObject matScrObj;
//}

//[Serializable]
//public class IDToFoodScrObj
//{
//    public string ID;
//    public FoodScriptableObject foodScrObj;
//}

//[Serializable]
//public class IDToSpecialItemScrObj
//{
//    public string ID;
//    public SpecialItemScriptableObject specItemScrObj;
//}

//#endregion