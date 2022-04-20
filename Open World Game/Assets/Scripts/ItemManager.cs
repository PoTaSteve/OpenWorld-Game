using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform itemsParent;

    //public List<WeaponScriptableObject> AllWeapons = new List<WeaponScriptableObject>();
    //public List<MaterialScriptableObject> AllMaterials = new List<MaterialScriptableObject>();
    //public List<IngredientScriptableObject> AllIngredients = new List<IngredientScriptableObject>();
    //public List<FoodScriptableObject> AllFood = new List<FoodScriptableObject>();
    //public List<SpecialItemScriptableObject> AllSpecialItem = new List<SpecialItemScriptableObject>();

    public List<GameObject> AllWeaponsObj = new List<GameObject>();
    public List<GameObject> AllMaterialsObj = new List<GameObject>();
    public List<GameObject> AllIngredientsObj = new List<GameObject>();
    public List<GameObject> AllFoodObj = new List<GameObject>();
    public List<GameObject> AllSpecialItemObj = new List<GameObject>();


    // Every 100000 the ID change area of intrest of the item
    public GameObject GetObjFromID(int ID)
    {
        if (ID >= 400000 && ID < 500000)
        {
            foreach (GameObject item in AllWeaponsObj)
            {
                if (item.GetComponent<WeaponInfo>().scrObj.TypeID == ID)
                {
                    return item;
                }
            }
        }        
        else if (ID >= 300000 && ID < 400000)
        {
            foreach (GameObject item in AllMaterialsObj)
            {
                if (item.GetComponent<MaterialInfo>().scrObj.TypeID == ID)
                {
                    return item;
                }
            }
        }
        else if (ID >= 200000 && ID < 300000)
        {
            foreach (GameObject item in AllIngredientsObj)
            {
                if (item.GetComponent<IngredientInfo>().scrObj.TypeID == ID)
                {
                    return item;
                }
            }
        }
        else if (ID >= 100000 && ID < 200000)
        {
            foreach (GameObject item in AllFoodObj)
            {
                if (item.GetComponent<FoodInfo>().scrObj.TypeID == ID)
                {
                    return item;
                }
            }
        }
        else if (ID >= 0 && ID < 100000)
        {
            foreach (GameObject item in AllSpecialItemObj)
            {
                if (item.GetComponent<SpecialItemInfo>().scrObj.TypeID == ID)
                {
                    return item;
                }
            }
        }

        return null;
    }
}
