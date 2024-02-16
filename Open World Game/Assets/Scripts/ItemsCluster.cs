using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCluster : Interactable
{
    public List<ScriptableObject> invItems;
    public List<ScriptableObject> equipItems;

    public override void Interact()
    {
        foreach (ScriptableObject item in invItems)
        {
            
        }

        foreach (ScriptableObject item in equipItems)
        {
            if (item is WeaponScriptableObject)
            {

            }
            else if (item is EquipmentScriptableObject)
            {
                EquipmentInfo eq = new EquipmentInfo(item as EquipmentScriptableObject);
            }
            else
            {
                Debug.LogError("Item cluster trying to add invalid equipable");
            }
        }
    }
}
