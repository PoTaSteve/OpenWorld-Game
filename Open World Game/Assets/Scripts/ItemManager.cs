using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Transform itemsParent;
    public List<GameObject> AllWeapons = new List<GameObject>();
    public List<GameObject> AllMaterials = new List<GameObject>();
    public List<GameObject> AllIngredients = new List<GameObject>();
    public List<GameObject> AllFood = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
