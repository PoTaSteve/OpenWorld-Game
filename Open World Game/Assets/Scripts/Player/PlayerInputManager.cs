using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    private PlayerControls playerControls;

    [SerializeField]
    private ConsoleManager consoleMan;
    [SerializeField]
    private DebugModeManager debugModeMan;

    public GameObject InventoryObj;
    public GameObject GameUIObj;
    public GameObject MapObj;
    public GameObject EscMenuObj;
    public GameObject ConsoleObj;
    public GameObject DebugModeObj;
    public GameObject TempConsoleDebugObj;

    public float debugModeTimer;
    public bool isInDebugMode;

    public GameObject ThirdPersonCam;

    public Transform cam;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float gravity = -19.62f;
    public float jumpHeight = 1;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float speed = 5f;

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

        playerControls.Player.Jump.performed += Jump;
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
        
        playerControls.Player.Enable();
        playerControls.Inventory.Disable();
        playerControls.Map.Disable();
        playerControls.EscMenu.Disable();
        playerControls.Dialogues.Disable();
        playerControls.Console.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        debugModeTimer = 0f;
        isInDebugMode = false;


        //Fetch the Raycaster from the GameObject (the Canvas)
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

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 inputDir = playerControls.Player.Movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputDir.x, 0f, inputDir.y).normalized;

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir.normalized * speed * Time.deltaTime);
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

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        playerControls.Inventory.Enable();
        InventoryObj.SetActive(true);
        GameUIObj.SetActive(false);

        ThirdPersonCam.SetActive(false);

        playerControls.Player.Disable();

        InventoryManager.Instance.OpenInventory();
    }

    public void CloseInventory(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        ThirdPersonCam.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void CloseInventoryForButton()
    {
        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        ThirdPersonCam.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void NextInvPage(InputAction.CallbackContext context)
    {
        InventoryManager.Instance.GoToNextInvTab();
    }

    public void PreviousInvPage(InputAction.CallbackContext context)
    {
        InventoryManager.Instance.GoToPreviousInvTab();
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        playerControls.Map.Enable();

        MapObj.SetActive(true);
        GameUIObj.SetActive(false);

        ThirdPersonCam.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();

        MapObj.SetActive(false);
        GameUIObj.SetActive(true);

        ThirdPersonCam.SetActive(true);

        playerControls.Map.Disable();
    }

    public void OpenEscMenu(InputAction.CallbackContext context)
    {
        playerControls.EscMenu.Enable();

        EscMenuObj.SetActive(true);
        GameUIObj.SetActive(false);

        ThirdPersonCam.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseEscMenu(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();

        EscMenuObj.SetActive(false);
        GameUIObj.SetActive(true);

        ThirdPersonCam.SetActive(true);

        playerControls.EscMenu.Disable();
    }

    public void OpenConsole(InputAction.CallbackContext context)
    {
        playerControls.Console.Enable();

        GameUIObj.SetActive(false);
        ConsoleObj.SetActive(true);
        consoleMan.txtField.ActivateInputField();

        playerControls.Player.Disable();
    }

    public void ConfirmInput(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();

        GameUIObj.SetActive(true);
        consoleMan.Console();
        ConsoleObj.SetActive(false);

        playerControls.Console.Disable();
    }

    public void CloseConsole(InputAction.CallbackContext context)
    {
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

                Destroy(interactab.gameObject);
            }
        }
    }
}
