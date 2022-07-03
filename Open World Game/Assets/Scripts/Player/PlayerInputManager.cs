using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] PlayerControllerRB plContrRB;
    private PlayerControls playerControls;

    [SerializeField]
    private ConsoleManager consoleMan;
    [SerializeField]
    private DebugModeManager debugModeMan;

    public GameObject InventoryObj = default;
    public GameObject GameUIObj = default;
    public GameObject MapObj = default;
    public GameObject EscMenuObj = default;
    public GameObject ConsoleObj = default;
    public GameObject DebugModeObj = default;
    public GameObject TempConsoleDebugObj = default;
    public GameObject DialogueObj = default;
    public GameObject WeaponEnhanceObj = default;
    public GameObject ShopObj = default;

    public float debugModeTimer;
    public bool isInDebugMode;

    public OrbitCamera orbitCam;

    public float animationSpeedPercent;

    [SerializeField]
    private PlayerTriggerCollision playerTrColl;
    public float scrollWheel;


    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public Canvas canvas;
    public List<GameObject> HitObjs;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Enable();
        playerControls.Inventory.Enable();
        playerControls.Map.Enable();
        playerControls.EscMenu.Enable();
        playerControls.Dialogues.Enable();
        playerControls.Console.Enable();
        playerControls.WeaponEnhance.Enable();
        playerControls.Shop.Enable();

        playerControls.Player.OpenInventory.performed += OpenInventory;
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.OpenMap.performed += OpenMap;
        playerControls.Player.OpenEscMenu.performed += OpenEscMenu;
        playerControls.Player.OpenConsole.performed += OpenConsole;
        playerControls.Player.EnterDebugMode.performed += EnterDebugMode;
        playerControls.Player.ExitDebugMode.performed += ExitDebugMode;

        playerControls.Inventory.CloseInventory.performed += CloseInventory;
        playerControls.Inventory.NextInvPage.performed += NextInvPage;
        playerControls.Inventory.PreviousInvPage.performed += PreviousInvPage;

        playerControls.Map.CloseMap.performed += CloseMap;

        playerControls.EscMenu.CloseEscMenu.performed += CloseEscMenu;

        playerControls.Console.ConfirmInput.performed += ConfirmInput;
        playerControls.Console.CloseConsole.performed += CloseConsole;

        playerControls.WeaponEnhance.CloseWeapDetails.performed += CloseWeapDetails;

        playerControls.Dialogues.Continue.performed += ContinueDialogue;

        playerControls.Shop.CloseShop.performed += CloseShop;

        playerControls.Player.Enable();
        playerControls.Inventory.Disable();
        playerControls.Map.Disable();
        playerControls.EscMenu.Disable();
        playerControls.Dialogues.Disable();
        playerControls.Console.Disable();
        playerControls.WeaponEnhance.Disable();
        playerControls.Shop.Disable();


        ToWorldState();
    }    

    // Start is called before the first frame update
    void Start()
    {
        debugModeTimer = 0f;
        isInDebugMode = false;
        orbitCam.canUpdateCam = true;

        //Fetch the Raycaster from the GameObject(the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDebugMode && debugModeTimer <= 1)
        {
            debugModeTimer += Time.deltaTime;
        }

        #region Read ScrollWheel Input
        scrollWheel = playerControls.Player.ScrollWheel.ReadValue<float>(); // it can be 120 or -120

        if (playerTrColl.InstantiatedInteractablesUI > 1 && scrollWheel != 0)
        {
            if (scrollWheel > 0 && playerTrColl.currentInteractableIndex > 0)
            {
                playerTrColl.currentInteractableIndex--;
            }
            else if (scrollWheel < 0 && playerTrColl.currentInteractableIndex < (playerTrColl.InstantiatedInteractablesUI - 1))
            {
                playerTrColl.currentInteractableIndex++;
            }

            playerTrColl.FixInteractableUIPos();

        }
        #endregion

        // Raycast UI test
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            HitObjs.Clear();
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                HitObjs.Add(result.gameObject);
            }
        }
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        ToUIState();

        playerControls.Inventory.Enable();
        InventoryObj.SetActive(true);
        GameUIObj.SetActive(false);

        playerControls.Player.Disable();

        GameManager.Instance.invMan.OpenInventory();
    }

    public void CloseInventory(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void CloseInventoryForButton()
    {
        ToWorldState();

        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void NextInvPage(InputAction.CallbackContext context)
    {
        GameManager.Instance.invMan.GoToNextInvTab();
    }

    public void PreviousInvPage(InputAction.CallbackContext context)
    {
        GameManager.Instance.invMan.GoToPreviousInvTab();
    }

    public void CloseWeapDetails(InputAction.CallbackContext context)
    {
        playerControls.Inventory.Enable();

        WeaponEnhanceObj.GetComponent<EnhanceEquipmentManager>().CloseWeaponDetails();

        InventoryObj.SetActive(true);
        WeaponEnhanceObj.SetActive(false);

        playerControls.WeaponEnhance.Disable();
    }

    public void CloseWeapDetailsForButton()
    {
        playerControls.Inventory.Enable();

        WeaponEnhanceObj.GetComponent<EnhanceEquipmentManager>().CloseWeaponDetails();

        InventoryObj.SetActive(true);
        WeaponEnhanceObj.SetActive(false);

        playerControls.WeaponEnhance.Disable();
    }

    public void OpenWeapDetails()
    {
        playerControls.WeaponEnhance.Enable();

        WeaponEnhanceObj.GetComponent<EnhanceEquipmentManager>().OpenWeaponDetails();

        WeaponEnhanceObj.SetActive(true);
        InventoryObj.SetActive(false);

        playerControls.Inventory.Disable();
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        ToUIState();

        playerControls.Map.Enable();

        MapObj.SetActive(true);
        GameUIObj.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        MapObj.SetActive(false);
        GameUIObj.SetActive(true);

        playerControls.Map.Disable();
    }

    public void OpenEscMenu(InputAction.CallbackContext context)
    {
        ToUIState();

        playerControls.EscMenu.Enable();

        EscMenuObj.SetActive(true);
        GameUIObj.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseEscMenu(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        EscMenuObj.SetActive(false);
        GameUIObj.SetActive(true);

        playerControls.EscMenu.Disable();
    }

    public void OpenConsole(InputAction.CallbackContext context)
    {
        ToUIState();

        playerControls.Console.Enable();

        GameUIObj.SetActive(false);
        ConsoleObj.SetActive(true);
        consoleMan.txtField.ActivateInputField();

        playerControls.Player.Disable();
    }

    public void ConfirmInput(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        GameUIObj.SetActive(true);
        consoleMan.Console();
        ConsoleObj.SetActive(false);

        playerControls.Console.Disable();
    }

    public void CloseConsole(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        GameUIObj.SetActive(true);
        consoleMan.txtField.text = "";
        ConsoleObj.SetActive(false);

        playerControls.Console.Disable();
    }

    public void EnterDebugMode(InputAction.CallbackContext context)
    {
        Debug.Log("Trying To acces debug mode");
        if (!isInDebugMode)
        {
            Debug.Log("In debug mode");
            debugModeMan.isInDebugMode = true;
            isInDebugMode = true;
            debugModeTimer = 0f;
            GameUIObj.SetActive(false);
            DebugModeObj.SetActive(true);
        }
    }

    public void ExitDebugMode(InputAction.CallbackContext context)
    {
        Debug.Log("Trying to exit debug mode");
        if (isInDebugMode && debugModeTimer >= 1)
        {
            Debug.Log("Out of debug mode");
            debugModeMan.isInDebugMode = false;
            isInDebugMode = false;
            debugModeTimer = 0f;
            DebugModeObj.SetActive(false);
            GameUIObj.SetActive(true);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (playerTrColl.InRangeInteractables.Count > 0)
        {
            Interactable interactab = playerTrColl.InRangeInteractables[playerTrColl.currentInteractableIndex].GetComponent<Interactable>();

            interactab.Interact();

            if (interactab.doesInteractionDestory)
            {
                playerTrColl.RemoveInteractabUI(interactab.gameObject);

                // Destroy(interactab.gameObject); Destroy handled in the object class
            }
        }
    }

    public void StartDialogue()
    {
        ToUIState();

        playerControls.Dialogues.Enable();

        DialogueObj.SetActive(true);
        GameUIObj.SetActive(false);

        playerControls.Player.Disable();
    }

    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        GameManager.Instance.dialMan.ContinueDialogue();
    }

    public void ExitDialogueSequence()
    {
        ToWorldState();

        playerControls.Player.Enable();

        GameUIObj.SetActive(true);
        DialogueObj.SetActive(false);

        playerControls.Dialogues.Disable();
    }

    public void OpenShop()
    {
        ToUIState();

        playerControls.Shop.Enable();

        ShopObj.SetActive(true);
        GameUIObj.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseShop(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        GameManager.Instance.shopMan.currentShop = null;

        GameUIObj.SetActive(true);
        ShopObj.SetActive(false);

        playerControls.Shop.Disable();
    }


    public void ToWorldState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        orbitCam.canUpdateCam = true;
        plContrRB.canUpdateMovement = true;
    }

    public void ToUIState()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        orbitCam.canUpdateCam = false;
        plContrRB.canUpdateMovement = false;
    }
}
