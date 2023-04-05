using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Special Item", menuName = "Special Item")]
public class SpecialItemScriptableObject : ScriptableObject
{
    public int ItemID;

    //Top

    public string specialItemName;

    public Sprite icon;

    //Middle

    [TextArea(3, 10)]
    public string description;

    public string Console_Name;

    public int buyCost;
}
