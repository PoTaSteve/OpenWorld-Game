using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Interactable info;

    public int itemType = -1;

    public void UpdateItemDetails()
    {
        switch (itemType)
        {
            case 0:
                

                break;
            
            case 1:


                break;

            case 2:


                break;

            case 3:


                break;

            default:
                Debug.Log("Error updating item details: item type not defined");
                return;
        }

        Debug.Log("Updating item of type " + itemType);
    }
}
