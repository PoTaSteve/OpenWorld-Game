using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector]
    public WeaponInfo weap;
    [HideInInspector]
    public MaterialInfo mat;
    [HideInInspector]
    public FoodInfo food;
    [HideInInspector]
    public SpecialItemInfo specItem;

    public int itemType = -1;

    public void UpdateItemDetails()
    {
        if (GameManager.Instance.invMan.CurrSelectedSlot == null)
        {
            GameManager.Instance.invMan.CurrSelectedSlot = gameObject;
        }

        GameManager.Instance.invMan.CurrSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);

        transform.GetChild(2).gameObject.SetActive(true);

        GameManager.Instance.invMan.CurrSelectedSlot = gameObject;

        switch (itemType)
        {
            case 0:
                GameManager.Instance.invMan.ItemNameDet.text = weap.scrObj.weaponName;
                GameManager.Instance.invMan.ItemDecriptionDet.text = weap.scrObj.description;

                break;
            
            case 1:
                GameManager.Instance.invMan.ItemNameDet.text = mat.scrObj.materialName;
                GameManager.Instance.invMan.ItemDecriptionDet.text = mat.scrObj.description;

                break;

            case 2:
                GameManager.Instance.invMan.ItemNameDet.text = food.scrObj.foodName;
                GameManager.Instance.invMan.ItemDecriptionDet.text = food.scrObj.description;

                break;

            case 3:
                GameManager.Instance.invMan.ItemNameDet.text = specItem.scrObj.specialItemName;
                GameManager.Instance.invMan.ItemDecriptionDet.text = specItem.scrObj.description;

                break;

            default:
                Debug.Log("Error updating item details: item type not defined");
                return;
        }

        Debug.Log("Updating item of type " + itemType);
    }
}
