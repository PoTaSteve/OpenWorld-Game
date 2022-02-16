using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Special Item", menuName = "Special Item")]
public class SpecialItemScriptableObj : ScriptableObject
{
    //Top

    public string specialItemName;
    public string specialItemType;
    public int rarity;

    public Sprite icon;

    //Middle

    [TextArea(3, 10)]
    public string description;
    public int numberOfSources;
    public string[] sources;
}
