using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public ItemInfo item;

    public void UpdateItemDetails()
    {
        if (GameManager.Instance.invMan.CurrSelectedSlot == null)
        {
            GameManager.Instance.invMan.CurrSelectedSlot = gameObject;
        }

        GameManager.Instance.invMan.CurrSelectedSlot.transform.GetChild(2).gameObject.SetActive(false);

        transform.GetChild(2).gameObject.SetActive(true);

        GameManager.Instance.invMan.CurrSelectedSlot = gameObject;

        GameManager.Instance.invMan.ItemNameDetailTxt.text = item.GetName();
        GameManager.Instance.invMan.ItemDecriptionDetailTxt.text = item.GetDescription();
    }
}
