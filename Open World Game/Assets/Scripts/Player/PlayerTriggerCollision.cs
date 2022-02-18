using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTriggerCollision : MonoBehaviour
{
    public List<int> AllIDs = new List<int>();
    [SerializeField]
    private int maxID = 1000;
    public GameObject InteractableItemUI;
    public GameObject InteractableUIPrefab;
    public Transform InteractableUIContent;
    public int InstantiatedInteractablesUI;
    public int currentInteractableIndex;
    [SerializeField]
    private PlayerInputManager PlInpMan;
    public List<GameObject> InRangeInteractables = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InteractableItemUI.SetActive(false);
        InstantiatedInteractablesUI = 0;
        currentInteractableIndex = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        
    }

    public int GenerateRandomID()
    {
        int n;

        n = Random.Range(1, maxID);

        while (AllIDs.Contains(n))
        {
            n = Random.Range(1, maxID);
        }

        AllIDs.Add(n);

        return n;
    }
}
