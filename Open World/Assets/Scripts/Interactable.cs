using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    PickUp,
    NPC, 
    Door,
    Chest,
    Dungeon
}

public enum PickUpType
{
    Weapon, 
    Material, 
    Ingredient, 
    Food, 
    SpecialItem
}

public class Interactable : MonoBehaviour
{
    public InteractableType type;
    public int ID;
    public int index;
    public PickUpType pickUpType;
    public Sprite icon;
    [Tooltip("String to display on the UI to interact with the item")]
    public string interactionType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
