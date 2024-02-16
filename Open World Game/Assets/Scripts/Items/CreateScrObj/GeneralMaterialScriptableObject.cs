using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New General Material", menuName = "Item/General Material")]
public class GeneralMaterialScriptableObject : ScriptableObject
{
    public string itemID;

    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    public Sprite itemIcon;

    public int buyCost;
    public int sellCost;
}
