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

    private void Start()
    {
        InteractableItemUI.SetActive(false);
        InstantiatedInteractablesUI = 0;
        currentInteractableIndex = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactab))
        {
            InRangeInteractables.Add(interactab.gameObject);

            if (InstantiatedInteractablesUI == 0)
            {
                InteractableItemUI.SetActive(true);
            }

            // Generate ID and index for the object
            interactab.ID = GenerateRandomID();
            interactab.index = InstantiatedInteractablesUI;

            // Spawn UI for interacting with objects
            GameObject InteractabUI = Instantiate(InteractableUIPrefab, InteractableUIContent);

            InteractabUI.transform.GetChild(2).GetComponent<Image>().sprite = interactab.icon;
            InteractabUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = interactab.interactionType;
            
            InteractabUI.GetComponent<IDInteractableUI>().ID = interactab.ID;
            InteractabUI.GetComponent<IDInteractableUI>().index = InstantiatedInteractablesUI;

            InstantiatedInteractablesUI++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable interactab))
        {
            //Updates all indexes
            foreach (GameObject obj in InRangeInteractables)
            {
                if (obj.GetComponent<Interactable>().index > currentInteractableIndex)
                {
                    obj.GetComponent<Interactable>().index--;
                }
            }
            foreach (Transform t in InteractableUIContent.transform)
            {
                if (t.gameObject.GetComponent<IDInteractableUI>().index > currentInteractableIndex)
                {
                    t.gameObject.GetComponent<IDInteractableUI>().index--;
                }
            }

            //Destroy
            foreach (Transform t in InteractableUIContent.transform)
            {
                if (t.gameObject.GetComponent<IDInteractableUI>().ID == interactab.ID)
                {
                    AllIDs.Remove(interactab.ID);

                    Destroy(t.gameObject);
                }
            }

            InstantiatedInteractablesUI--;

            if (InstantiatedInteractablesUI == currentInteractableIndex && currentInteractableIndex != 0)
            {
                currentInteractableIndex--;
            }

            FixInteractableUIPos();

            if (InstantiatedInteractablesUI == 0)
            {
                InteractableItemUI.SetActive(false);
            }

            InRangeInteractables.Remove(interactab.gameObject);
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
        if (InteractableUIContent.GetComponent<RectTransform>().anchoredPosition.y > 25 + 50 * (InstantiatedInteractablesUI - 1))
        {
            if (InstantiatedInteractablesUI > 0)
            {
                InteractableUIContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25 + 50 * (InstantiatedInteractablesUI - 1));
            }
            else
            {
                InteractableUIContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
            }
        }
    }
}
