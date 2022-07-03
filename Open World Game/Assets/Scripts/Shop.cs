using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : Interactable
{
    public List<GameObject> products = new List<GameObject>();
    public List<int> StartCounts = new List<int>();
    public List<int> CurrCounts = new List<int>();

    public override void Interact()
    {
        ShopManager shopMan = GameManager.Instance.shopMan;

        //shopMan.products.Clear();
        //shopMan.StartCounts.Clear();
        //shopMan.CurrCounts.Clear();

        shopMan.products = products;
        shopMan.StartCounts = StartCounts;
        shopMan.CurrCounts = CurrCounts;

        shopMan.currentShop = this;

        GameManager.Instance.plInMan.OpenShop();

        shopMan.CreateShop();
    }
}