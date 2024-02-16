using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Item", menuName = "Item/Quest Item")]
public class QuestItemScriptableObject : ScriptableObject
{
    public string itemID;

    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    public Sprite itemIcon;

    public int buyCost;
    public int sellCost;
}
