using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Quest : MonoBehaviour
{
    public string questId;
    public string questName;
    public string questGiver;
    public string questPlace;
    public string questDescription;

    private Animator anim;

    public bool isTracking;
    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isTracking", isTracking);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickQuestButton()
    {
        isTracking = !isTracking;

        anim.SetBool("isTracking", isTracking);

        if (!isSelected )
        {
            isSelected = true;

            // Change things in description
        }
    }
}
