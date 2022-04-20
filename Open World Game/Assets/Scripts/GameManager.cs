using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public InventoryManager invMan;
    [HideInInspector]
    public ItemManager itemMan;
    [HideInInspector]
    public DialogueManager dialMan;
    [HideInInspector]
    public PlayerInputManager plInMan;
    [HideInInspector]
    public ShopManager shopMan;

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

        invMan = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        itemMan = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        dialMan = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        plInMan = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputManager>();
        shopMan = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();

        plInMan.GameUIObj.SetActive(true);
        plInMan.InventoryObj.SetActive(false);
        plInMan.MapObj.SetActive(false);
        plInMan.EscMenuObj.SetActive(false);
        plInMan.ConsoleObj.SetActive(false);
        plInMan.DebugModeObj.SetActive(false);
        plInMan.WeaponEnhanceObj.SetActive(false);
        plInMan.DialogueObj.SetActive(false);
        plInMan.TempConsoleDebugObj.SetActive(false);
        plInMan.ShopObj.SetActive(false);
    }
}
