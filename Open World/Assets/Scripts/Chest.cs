using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Material DissolveMat;
    [Tooltip("Rarer chests have higher chances of giving more and batter loot"), Range(1, 3)]
    public int chestRarity;
    [Tooltip("Special chests have a preset of spawned loot")]
    public bool isSpecialChest;
    [SerializeField]
    private List<GameObject> SpecialLoot;
    private bool canTimeFlow;
    private float t;
    private Transform lootSpawnPoint;
    [Space]
    [SerializeField]
    private List<GameObject> CommonWeapons;
    [SerializeField]
    private List<GameObject> UncommonWeapons;
    [SerializeField]
    private List<GameObject> RareWeapons;
    [SerializeField]
    private GameObject CommonWeaponEnhance;
    [SerializeField]
    private GameObject UncommonWeaponEnhance;
    [SerializeField]
    private GameObject RareWeaponEnhance;
    [SerializeField]
    private List<GameObject> CommonAccessories;
    [SerializeField]
    private List<GameObject> UncommonAccessories;
    [SerializeField]
    private List<GameObject> RareAccessories;
    [SerializeField]
    private List<GameObject> EpicAccessories;

    private List<string> commonDroppables = new List<string>() {"Weapon", "Enhance", "Accessory" };
    private List<string> uncommonDroppables = new List<string>() { "Weapon", "Weapon", "Enhance", "Enhance", "Accessory", "Accessory" };
    private List<string> rareDroppables = new List<string>() { "Weapon", "Weapon", "Enhance", "Enhance", "Accessory", "Accessory" };

    // Start is called before the first frame update
    void Start()
    {
        canTimeFlow = false;
        lootSpawnPoint = gameObject.transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (canTimeFlow)
        {
            t += Time.deltaTime;
            DissolveMat.SetFloat("Time_", t);

            if (t >= 1)
            {
                StopCoroutine(StartDissolving(2));

                Destroy(gameObject);
            }
        }
    }

    public void OpenChest()
    {
        gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = DissolveMat;
        
        t = -1;
        DissolveMat.SetFloat("Time_", t);

        StartCoroutine(SpawnLoot());

        StartCoroutine(StartDissolving(2));
    }

    IEnumerator StartDissolving(float time)
    {
        yield return new WaitForSeconds(time);

        canTimeFlow = true;
    }

    IEnumerator SpawnLoot()
    {
        yield return new WaitForSeconds(0.5f);

        if (isSpecialChest)
        {
            foreach (GameObject o in SpecialLoot)
            {
                Instantiate(o, lootSpawnPoint.position, Quaternion.Euler(90, 0, 0));

                yield return new WaitForSeconds(0.1f);
            }
        }
        else // Random generated loot
        {
            List<GameObject> loot = new List<GameObject>();

            int itemCount = GenerateItemsCount(chestRarity);

            for (int i = 0; i < itemCount; i++)
            {
                int lootRarity = GenerateLootRarity(chestRarity);

                loot.Add(GenerateLootItem(lootRarity, chestRarity));
            }

            foreach (GameObject o in loot)
            {
                Instantiate(o, lootSpawnPoint.position, Quaternion.Euler(90, 0, 0));

                yield return new WaitForSeconds(0.1f);
            }
        }        
    }

    public int GenerateItemsCount(int chestRarity)
    {
        int count;

        if (chestRarity == 1)
        {
            count = Random.Range(1, 4);
        }
        else if (chestRarity == 2)
        {
            count = Random.Range(2, 5);
        }
        else
        {
            count = Random.Range(4, 6);
        }

        return count;
    }

    public int GenerateLootRarity(int chestRarity)
    {
        if (chestRarity == 1) // Common
        {
            int rand = Random.Range(1, 101);

            if (rand <= 75)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else if (chestRarity == 2) // Uncommon
        {
            int rand = Random.Range(1, 101);

            if (rand <= 39)
            {
                return 1;
            }
            else if (rand > 39 && rand <= 84)
            {
                return 2;
            }
            else if (rand > 84 && rand <= 99)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else // Rare
        {
            int rand = Random.Range(1, 101);

            if (rand <= 35)
            {
                return 2;
            }
            else if (rand > 35 && rand <= 85)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
    }

    public GameObject GenerateLootItem(int itemRarity, int chestRarity)
    {
        if (itemRarity == 4)
        {
            int randI = Random.Range(0, EpicAccessories.Count);

            return EpicAccessories[randI];
        }
        else
        {
            if (chestRarity == 1)
            {
                int type = Random.Range(0, commonDroppables.Count);

                string typeS = commonDroppables[type];

                if (typeS == "Weapon")
                {
                    if (itemRarity == 1)
                    {
                        commonDroppables.Remove("Weapon");

                        int rand = Random.Range(0, CommonWeapons.Count);

                        return CommonWeapons[rand];
                    }
                    else
                    {
                        commonDroppables.Remove("Weapon");

                        int rand = Random.Range(0, UncommonWeapons.Count);

                        return UncommonWeapons[rand];
                    }
                }
                else if (typeS == "Enhance")
                {
                    if (itemRarity == 1)
                    {
                        commonDroppables.Remove("Enhance");

                        return CommonWeaponEnhance;
                    }
                    else
                    {
                        commonDroppables.Remove("Enhance");

                        return UncommonWeaponEnhance;
                    }
                }
                else
                {
                    if (itemRarity == 1)
                    {
                        commonDroppables.Remove("Accessory");

                        int rand = Random.Range(0, CommonAccessories.Count);

                        return CommonAccessories[rand];
                    }
                    else
                    {
                        commonDroppables.Remove("Accessory");

                        int rand = Random.Range(0, UncommonAccessories.Count);

                        return UncommonAccessories[rand];
                    }
                }
            }
            else if (chestRarity == 2)
            {
                int type = Random.Range(0, uncommonDroppables.Count);

                string typeS = uncommonDroppables[type];

                if (typeS == "Weapon")
                {
                    if (itemRarity == 1)
                    {
                        uncommonDroppables.Remove("Weapon");

                        int rand = Random.Range(0, CommonWeapons.Count);

                        return CommonWeapons[rand];
                    }
                    else if (itemRarity == 2)
                    {
                        uncommonDroppables.Remove("Weapon");

                        int rand = Random.Range(0, UncommonWeapons.Count);

                        return UncommonWeapons[rand];
                    }
                    else
                    {
                        uncommonDroppables.Remove("Weapon");

                        int rand = Random.Range(0, RareWeapons.Count);

                        return RareWeapons[rand];
                    }
                }
                else if (typeS == "Enhance")
                {
                    if (itemRarity == 1)
                    {
                        uncommonDroppables.Remove("Enhance");

                        return CommonWeaponEnhance;
                    }
                    else if (itemRarity == 2)
                    {
                        uncommonDroppables.Remove("Enhance");

                        return UncommonWeaponEnhance;
                    }
                    else
                    {
                        uncommonDroppables.Remove("Enhance");

                        return RareWeaponEnhance;
                    }
                }
                else
                {
                    if (itemRarity == 1)
                    {
                        uncommonDroppables.Remove("Accessory");

                        int rand = Random.Range(0, CommonAccessories.Count);

                        return CommonAccessories[rand];
                    }
                    else if (itemRarity == 2)
                    {
                        uncommonDroppables.Remove("Accessory");

                        int rand = Random.Range(0, UncommonAccessories.Count);

                        return UncommonAccessories[rand];
                    }
                    else
                    {
                        uncommonDroppables.Remove("Accessory");

                        int rand = Random.Range(0, RareAccessories.Count);

                        return RareAccessories[rand];
                    }
                }
            }
            else
            {
                int type = Random.Range(0, rareDroppables.Count);

                string typeS = rareDroppables[type];

                if (typeS == "Weapon")
                {
                    if (itemRarity == 2)
                    {
                        rareDroppables.Remove("Weapon");

                        int rand = Random.Range(0, UncommonWeapons.Count);

                        return UncommonWeapons[rand];
                    }
                    else
                    {
                        rareDroppables.Remove("Weapon");

                        int rand = Random.Range(0, RareWeapons.Count);

                        return RareWeapons[rand];
                    }
                }
                else if (typeS == "Enhance")
                {
                    if (itemRarity == 2)
                    {
                        rareDroppables.Remove("Enhance");

                        return UncommonWeaponEnhance;
                    }
                    else
                    {
                        rareDroppables.Remove("Enhance");

                        return RareWeaponEnhance;
                    }
                }
                else
                {
                    if (itemRarity == 2)
                    {
                        rareDroppables.Remove("Accessory");

                        int rand = Random.Range(0, UncommonAccessories.Count);

                        return UncommonAccessories[rand];
                    }
                    else
                    {
                        rareDroppables.Remove("Accessory");

                        int rand = Random.Range(0, RareAccessories.Count);

                        return RareAccessories[rand];
                    }
                }
            }
        }
    }
}
