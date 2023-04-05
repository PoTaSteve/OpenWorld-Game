using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Cinemachine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    private PlayerControls playerControls;

    [SerializeField]
    private ConsoleManager consoleMan;

    public GameObject UIStateObject = default;
    public GameObject CommonUIObject = default;
    public GameObject QuestsUIObject = default;
    public GameObject InventoryObj = default;
    public GameObject SkillsUIObject = default;
    public GameObject SystemUIObject = default;
    public GameObject GameUIObj = default;
    public GameObject MapObj = default;
    public GameObject EscMenuObj = default;
    public GameObject ConsoleObj = default;
    public GameObject TempConsoleDebugObj = default;
    public GameObject DialogueObj = default;
    public GameObject ShopObj = default;
    public GameObject ActiveBuffsWindow = default;

    public ThirdPersonCam cam;

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
        playerControls.ActiveBuffs.Enable();

        playerControls.Player.OpenInventory.performed += OpenInventory;
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.OpenMap.performed += OpenMap;
        playerControls.Player.PressEsc.performed += PressEsc;
        playerControls.Player.OpenConsole.performed += OpenConsole;

        playerControls.Inventory.CloseInventory.performed += CloseInventory;

        playerControls.Map.CloseMap.performed += CloseMap;

        playerControls.EscMenu.CloseEscMenu.performed += CloseEscMenu;

        playerControls.Console.ConfirmInput.performed += ConfirmInput;
        playerControls.Console.CloseConsole.performed += CloseConsole;

        playerControls.Dialogues.Continue.performed += ContinueDialogue;

        playerControls.Shop.CloseShop.performed += CloseShop;

        playerControls.ActiveBuffs.ExitMenu.performed += CloseActiveBuffsWindow;

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
        //orbitCam.canUpdateCam = true;

        //Fetch the Raycaster from the GameObject(the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && GameManager.Instance.currentState == State.OPEN_WORLD)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt) && GameManager.Instance.currentState == State.OPEN_WORLD)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

        playerControls.Player.Disable();

        GameManager.Instance.invMan.SetInventoryActive(true);
    }

    public void CloseInventory(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Player.Enable();

        playerControls.Inventory.Disable();

        GameManager.Instance.invMan.SetInventoryActive(false);
    }

    public void CloseInventoryForButton()
    {
        ToWorldState();

        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

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

    public void PressEsc(InputAction.CallbackContext context)
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

    public void Interact(InputAction.CallbackContext context)
    {
        if (playerTrColl.InRangeInteractables.Count > 0)
        {
            Interactable interactab = playerTrColl.InRangeInteractables[playerTrColl.currentInteractableIndex].GetComponent<Interactable>();

            interactab.Interact();

            playerTrColl.RemoveInteractabUI(interactab.gameObject);
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

    public void OpenActiveBuffsWindow()
    {
        playerControls.ActiveBuffs.Enable();
        playerControls.Player.Disable();

        ActiveBuffsWindow.SetActive(true);

        GameManager.Instance.currentState = State.BUFFS_WINDOW;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseActiveBuffsWindow(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();
        playerControls.ActiveBuffs.Disable();

        ActiveBuffsWindow.SetActive(false);

        GameManager.Instance.currentState = State.OPEN_WORLD;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToWorldState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //cam.canUpdate = true;
        cam.gameObject.GetComponent<CinemachineBrain>().enabled = true;
        pm.canRotate = true;
        pm.freeze = false;
    }

    public void ToUIState()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //cam.canUpdate = false;
        cam.gameObject.GetComponent<CinemachineBrain>().enabled = false;
        pm.canRotate = false;
        pm.freeze = true;
    }
}
