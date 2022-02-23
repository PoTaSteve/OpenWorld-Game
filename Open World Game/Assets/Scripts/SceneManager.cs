using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject InventoryObj;
    public GameObject GameUIObj;

    // Start is called before the first frame update
    void Start()
    {
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
