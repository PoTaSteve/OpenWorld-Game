using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Material", menuName = "Item/Upgrade Material")]
public class UpgradeMaterialScriptableObject : ScriptableObject
{
    public string itemID;

    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    public Sprite itemIcon;

    public int buyCost;
    public int sellCost;
}
