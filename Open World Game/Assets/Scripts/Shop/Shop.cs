using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour
{
    public List<ShopProductItem> Products = new List<ShopProductItem>();

    public void OpenShop()
    {
        ShopManager shopMan = GameManager.Instance.shopMan;

        shopMan.currentShop = this;

        GameManager.Instance.plInputMan.SetShopUI();

        shopMan.CreateShop();
    }
}

[Serializable]
public class ShopProductItem
{
    public ItemInfo item;

    public int startCount;

    public int currCount;
}