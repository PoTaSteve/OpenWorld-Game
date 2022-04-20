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
    [Space]
    public GameObject InteractablesUI; // Whole Gameobject
    public GameObject InteractableUIPrefab; // Prefab to instantiate
    public GameObject InteractableUIContent; // Transform to instantiate prefab and also gameobject to move to fix index
    [Space]
    public int InstantiatedInteractablesUI;
    public int currentInteractableIndex;
    public List<GameObject> InRangeInteractables = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InteractablesUI.SetActive(false);
        InstantiatedInteractablesUI = 0;
        currentInteractableIndex = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactab))
        {
            InteractablesUI.SetActive(true);

            InRangeInteractables.Add(other.gameObject);

            interactab.ID = GenerateRandomID();

            GameObject InteractabUIObj = Instantiate(InteractableUIPrefab, InteractableUIContent.transform);

            InteractabUIObj.GetComponent<Interactable>().ID = interactab.ID;

            InteractabUIObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = interactab.interactionType;
            InteractabUIObj.transform.GetChild(4).GetComponent<Image>().sprite = interactab.icon;

            InstantiatedInteractablesUI++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (InRangeInteractables.Contains(other.gameObject))
        {
            RemoveInteractabUI(other.gameObject);
        }
    }

    public void RemoveInteractabUI(GameObject obj)
    {
        InRangeInteractables.Remove(obj);

        int interactabID = obj.GetComponent<Interactable>().ID;

        AllIDs.Remove(interactabID);

        foreach (Transform t in InteractableUIContent.transform)
        {
            if (t.gameObject.GetComponent<Interactable>().ID == interactabID)
            {
                Destroy(t.gameObject);
            }
        }

        if (currentInteractableIndex == (InstantiatedInteractablesUI - 1))
        {
            if (currentInteractableIndex == 0)
            {
                InteractableUIContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -85);
                InteractablesUI.SetActive(false);
            }
            else
            {
                currentInteractableIndex--;

                FixInteractableUIPos();
            }
        }

        if (InstantiatedInteractablesUI != 0)
        {
            InstantiatedInteractablesUI--;
        }
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

    public void FixInteractableUIPos()
    {
        Vector2 pos = new Vector2(0, (55 * currentInteractableIndex) - 85);

        InteractableUIContent.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
