using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : Interactable
{
    public List<GameObject> products = new List<GameObject>();
    public List<int> counts = new List<int>();

    public override void Interact()
    {
        ShopManager shopMan = GameManager.Instance.shopMan;

        shopMan.currentShop = this;

        shopMan.products.Clear();
        shopMan.counts.Clear();

        shopMan.products = products;
        shopMan.counts = counts;

        GameManager.Instance.plInMan.OpenShop();

        shopMan.CreateShop();
    }
}