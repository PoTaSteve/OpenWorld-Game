using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour
{
    public List<GameObject> products = new List<GameObject>();
    public List<int> StartCounts = new List<int>();
    public List<int> CurrCounts = new List<int>();

    public void OpenShop()
    {
        ShopManager shopMan = GameManager.Instance.shopMan;

        //shopMan.products.Clear();
        //shopMan.StartCounts.Clear();
        //shopMan.CurrCounts.Clear();

        //shopMan.products = products;
        //shopMan.StartCounts = StartCounts;
        //shopMan.CurrCounts = CurrCounts;

        shopMan.currentShop = this;

        GameManager.Instance.plInputMan.SetShopUI();

        shopMan.CreateShop();
    }
}