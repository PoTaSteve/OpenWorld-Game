using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInfo : Interactable
{
    public abstract string GetID();

    public abstract string GetName();

    public abstract string GetDescription();

    public abstract Sprite GetIcon();

    public abstract int GetItemIntType();

    public abstract int GetBuyCost();

    public abstract int GetSellCost();
}

public abstract class EquipableItem : ItemInfo
{
    public abstract EquipType GetEquipType();

    public abstract void Equip();

    public abstract void Unequip();
}
