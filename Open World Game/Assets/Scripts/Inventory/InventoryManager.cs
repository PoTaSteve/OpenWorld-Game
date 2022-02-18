using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int currInvTab;
    [Space]
    public GameObject[] Windows = new GameObject[5]; // Windows containing the items of the tab
    public GameObject[] Tabs = new GameObject[5]; // Buttons to go to a specific tab (use for animations)

    // Start is called before the first frame update
    void Start()
    {
        currInvTab = 0;
        Windows[0].SetActive(true);
        Tabs[0].GetComponent<Animator>().SetTrigger("Selected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNextInvTab()
    {
        Windows[currInvTab].SetActive(false);

        if (currInvTab == 4)
        {
            currInvTab = 0;
        }
        else
        {
            currInvTab++;
        }

        Windows[currInvTab].SetActive(true);
    }

    public void GoToPreviousInvTab()
    {
        Windows[currInvTab].SetActive(false);

        if (currInvTab == 0)
        {
            currInvTab = 4;
        }
        else
        {
            currInvTab--;
        }

        Windows[currInvTab].SetActive(true);
    }

    public void GoToSpecificInvTab(int tab)
    {
        if (tab!= currInvTab)
        {
            Windows[currInvTab].SetActive(false);
            Tabs[currInvTab].GetComponent<Animator>().SetTrigger("BackToNormal");

            Windows[tab].SetActive(true);
            Tabs[tab].GetComponent<Animator>().SetTrigger("Selected");
            currInvTab = tab;
        }
    }
}
