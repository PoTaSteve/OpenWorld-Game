using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public List<GameObject> Loot = new List<GameObject>();
    public int Gems;
    public bool spawnInWorld;
    [SerializeField]
    private Transform LootSpawnPoint;
    private Vector3 spawnPoint;

    public override void Interact()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("Open");

        // Need to add gem count

        if (spawnInWorld)
        {
            StartCoroutine(SpawnLootInWorld());
        }
        else
        {
            // Directly add items to player inventory
        }

        StartCoroutine(DestroyChest());
    }

    public IEnumerator SpawnLootInWorld()
    {
        spawnPoint = LootSpawnPoint.position;

        foreach (GameObject obj in Loot)
        {
            Instantiate(obj, spawnPoint, Quaternion.identity, GameManager.Instance.itemMan.itemsParent);

            spawnPoint += Vector3.up * 0.1f; 

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator DestroyChest()
    {
        // Play animation of dissolvence

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
