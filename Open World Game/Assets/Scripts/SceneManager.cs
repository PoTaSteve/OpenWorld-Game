using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject InventoryObj;
    public GameObject GameUIObj;
    public GameObject ConsoleObj;
    public GameObject DebugModeObj;
    public GameObject TempConsoleDebugObj;
    public GameObject WeaponInfoWindow;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        InventoryObj.SetActive(false);
        ConsoleObj.SetActive(false);
        DebugModeObj.SetActive(false);
        GameUIObj.SetActive(true);
        TempConsoleDebugObj.SetActive(false);
        WeaponInfoWindow.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
