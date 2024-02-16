using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class EquipmentScriptableObject : ScriptableObject
{
    public EquipType equipType;

    public string itemID;

    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    public Sprite itemIcon;

    public List<BuffValueMap> buffs;

    public int buyCost;
    public int sellCost;
}

[System.Serializable]
public class BuffValueMap
{
    public StatType buff;
    public float value;
}