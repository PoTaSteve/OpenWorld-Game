using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnhanceMaterialType
{
    Weapon,
    EnhanceMaterial
}

public class EnhanceMaterialInfo : MonoBehaviour
{
    [HideInInspector]
    public WeaponInfo weapInfo;
    [HideInInspector]
    public MaterialInfo matInfo;

    public int rarity;
    public int count;
    public EnhanceMaterialType type;
    public int selectedCount;
    public int Xp;
    public bool isSelected;
    public int id;
    public Sprite icon;
    public int level;
}
