using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // All the attributes to be saved
    //Inventory



    //Player

    public int level;
    public int health;
    public float[] position;

    public SaveData(PlayerStats player, InventoryManager inv)
    {
        level = player.level;
        health = player.health;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
