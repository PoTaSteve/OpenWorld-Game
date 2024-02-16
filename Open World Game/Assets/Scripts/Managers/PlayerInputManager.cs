using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Cinemachine;

public enum InputMethod
{
    MOUSE_AND_KEYBOARD,
    CONTROLLER
}

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] PlayerStats stats;
    public PlayerControls playerControls { get; private set; }

    public GameEvent OnEnemyHit;

    private InputMethod inputMethod;

    [SerializeField]
    private ConsoleManager consoleMan;

    public GameObject UIStateObject;
    public GameObject CommonUIObject;
    public GameObject QuestsUIObject;
    public GameObject InventoryObj;
    public GameObject SkillsUIObject;
    public GameObject SystemUIObject;
    public GameObject GameUIObj;
    public GameObject MapObj;
    public GameObject ConsoleObj;
    public GameObject TempConsoleDebugObj;
    public GameObject DialogueObj;
    public GameObject ShopObj;

    public ThirdPersonCam cam;

    [SerializeField]
    private PlayerInteractionManager plInterMan;
    public float scrollWheel;

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

    public Animator anim;

    public bool canPrepareNextAttack;
    public int attackCount;

    public Transform attackPos;
    public Vector3 attackBoxHalfExtens;
    public LayerMask whatIsEnemy;


    private void Awake()
    {
        InitializeControls();

        ToWorldState();
    }

    public void InitializeControls()
    {
        playerControls = new PlayerControls();

        #region Mouse And Keyboard
        // Detect Controller
        playerControls.DetectController.Enable();
        playerControls.DetectController.SwitchToController.performed += ChangeToController;
        // playerControls.DetectController.Disable();

        // Keyboard Player
        playerControls.Keyboard_Player.Enable();
        playerControls.Keyboard_Player.Jump.performed += Jump;
        playerControls.Keyboard_Player.Sprint.performed += Sprint;
        playerControls.Keyboard_Player.Sprint.canceled += StopSprint;
        playerControls.Keyboard_Player.Crouch.performed += Crouch;
        playerControls.Keyboard_Player.Crouch.canceled += ReleaseCrouch;
        playerControls.Keyboard_Player.Interact.performed += Interact;
        playerControls.Keyboard_Player.NormalAttack.performed += NormalAttack;
        playerControls.Keyboard_Player.ChargedAttack.performed += ChargedAttack;
        playerControls.Keyboard_Player.Skill0.performed += Skill0;
        playerControls.Keyboard_Player.Skill1.performed += Skill1;
        playerControls.Keyboard_Player.Skill2.performed += Skill2;
        playerControls.Keyboard_Player.OpenUIState.performed += OpenUIState;
        playerControls.Keyboard_Player.OpenMap.performed += OpenMap;
        playerControls.Keyboard_Player.OpenConsole.performed += OpenConsole;
        playerControls.Keyboard_Player.TurnCursorOnOff.performed += TurnCursorOn;
        playerControls.Keyboard_Player.TurnCursorOnOff.canceled += TurnCursorOff;
        // playerControls.Keyboard_Player.Disable();

        // Keyboard UI State
        playerControls.Keyboard_UIState.Enable();
        playerControls.Keyboard_UIState.CloseUIState.performed += CloseUIState;
        playerControls.Keyboard_UIState.NextUIState.performed += NextUIState;
        playerControls.Keyboard_UIState.PreviousUIState.performed += PreviousUIState;
        playerControls.Keyboard_UIState.Disable();

        // Keyboard Quests UI
        playerControls.Keyboard_QuestsUI.Enable();
        playerControls.Keyboard_QuestsUI.NextQuestPage.performed += NextQuestPage;
        playerControls.Keyboard_QuestsUI.PreviousQuestPage.performed += PreviousQuestPage;
        playerControls.Keyboard_QuestsUI.Disable();

        // Keyboard Inventory UI
        playerControls.Keyboard_InventoryUI.Enable();
        playerControls.Keyboard_InventoryUI.NextInventoryPage.performed += NextInventoryPage;
        playerControls.Keyboard_InventoryUI.PreviousInventoryPage.performed += PreviousInventoryPage;
        playerControls.Keyboard_InventoryUI.NextInventorySlot.performed += NextInventorySlot;
        playerControls.Keyboard_InventoryUI.PreviousInventorySlot.performed += PreviousInventorySlot;
        playerControls.Keyboard_InventoryUI.Disable();

        // Keyboard Skills UI
        playerControls.Keyboard_EquipmentUI.Enable();
        playerControls.Keyboard_EquipmentUI.NextInventoryPage.performed += NextEquipmentPage;
        playerControls.Keyboard_EquipmentUI.PreviousInventoryPage.performed += PreviousEquipmentPage;
        playerControls.Keyboard_EquipmentUI.NextInventorySlot.performed += NextEquipmentSlot;
        playerControls.Keyboard_EquipmentUI.PreviousInventorySlot.performed += PreviousEquipmentSlot;
        playerControls.Keyboard_EquipmentUI.Disable();

        // Keyboard System UI
        playerControls.Keyboard_SystemUI.Enable();
        //playerControls.Keyboard_SystemUI.
        playerControls.Keyboard_SystemUI.Disable();

        // Keyboard Map
        playerControls.Keyboard_Map.Enable();
        playerControls.Keyboard_Map.CloseMap.performed += CloseMap;
        playerControls.Keyboard_Map.Disable();

        // Keyboard Dialogues
        playerControls.Keyboard_Dialogues.Enable();
        playerControls.Keyboard_Dialogues.Continue.performed += ContinueDialogue;
        playerControls.Keyboard_Dialogues.Disable();

        // Keyboard Shop
        playerControls.Keyboard_Shop.Enable();
        playerControls.Keyboard_Shop.CloseShop.performed += CloseShop;
        playerControls.Keyboard_Shop.Disable();

        // Keyboard Console
        playerControls.Keyboard_Console.Enable();
        playerControls.Keyboard_Console.ConfirmInput.performed += ConfirmInput;
        playerControls.Keyboard_Console.CloseConsole.performed += CloseConsole;
        playerControls.Keyboard_Console.Disable();

        #endregion



        #region Controller
        // Detect Mouse And Keyboard
        playerControls.DetectMouseKeyboard.Enable();
        playerControls.DetectMouseKeyboard.SwitchToMouseKeyboard.performed += ChangeToMouseAndKeyboard;
        playerControls.DetectMouseKeyboard.Disable();

        // Controller Player
        playerControls.Controller_Player.Enable();
        playerControls.Controller_Player.Jump.performed += Jump;
        playerControls.Controller_Player.Sprint.performed += Sprint;
        playerControls.Controller_Player.Sprint.canceled += StopSprint;
        playerControls.Controller_Player.Crouch.performed += Crouch;
        playerControls.Controller_Player.Crouch.canceled += ReleaseCrouch;
        playerControls.Controller_Player.Interact.performed += Interact;
        playerControls.Controller_Player.NormalAttack.performed += NormalAttack;
        playerControls.Controller_Player.ChargedAttack.performed += ChargedAttack;
        playerControls.Controller_Player.OpenUIState.performed += OpenUIState;
        playerControls.Controller_Player.OpenMap.performed += OpenMap;
        playerControls.Controller_Player.Disable();

        // Controller UI State
        playerControls.Controller_UIState.Enable();
        playerControls.Controller_UIState.CloseUIState.performed += CloseUIState;
        playerControls.Controller_UIState.NextUIState.performed += NextUIState;
        playerControls.Controller_UIState.PreviousUIState.performed += PreviousUIState;
        playerControls.Controller_UIState.Disable();

        // Controller Quests UI
        playerControls.Controller_QuestsUI.Enable();
        playerControls.Controller_QuestsUI.NextQuestPage.performed += NextQuestPage;
        playerControls.Controller_QuestsUI.PreviousQuestPage.performed += PreviousQuestPage;
        playerControls.Controller_QuestsUI.Disable();

        // Controller Inventory UI
        playerControls.Controller_InventoryUI.Enable();
        playerControls.Controller_InventoryUI.NextInventoryPage.performed += NextInventoryPage;
        playerControls.Controller_InventoryUI.PreviousInventoryPage.performed += PreviousInventoryPage;
        playerControls.Controller_InventoryUI.Disable();

        // Controller Skills UI
        playerControls.Controller_EquipmentUI.Enable();
        //playerControls.Controller_EquipmentUI.
        playerControls.Controller_EquipmentUI.Disable();

        // Controller System UI
        playerControls.Controller_SystemUI.Enable();
        //playerControls.Controller_SystemUI.
        playerControls.Controller_SystemUI.Disable();

        // Controller Map
        playerControls.Controller_Map.Enable();
        playerControls.Controller_Map.CloseMap.performed += CloseMap;
        playerControls.Controller_Map.Disable();

        // Controller Dialogues
        playerControls.Controller_Dialogues.Enable();
        playerControls.Controller_Dialogues.Continue.performed += ContinueDialogue;
        playerControls.Controller_Dialogues.Disable();

        // Controller Shop
        playerControls.Controller_Shop.Enable();
        playerControls.Controller_Shop.CloseShop.performed += CloseShop;
        playerControls.Controller_Shop.Disable();

        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadAWSDInput();
    }

    public void ReadAWSDInput()
    {
        Vector2 inputVector;

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            inputVector = playerControls.Keyboard_Player.Movement.ReadValue<Vector2>();
        }
        else
        {
            inputVector = playerControls.Controller_Player.Movement.ReadValue<Vector2>();
        }

        horizontalInput = inputVector.x;
        verticalInput = inputVector.y;
    }

    public void ChangeToController(InputAction.CallbackContext context)
    {
        inputMethod = InputMethod.CONTROLLER;

        playerControls.DetectMouseKeyboard.Enable();
        playerControls.DetectController.Disable();

        switch (GameManager.Instance.gameState)
        {
            case State.MAIN_MENU:
                
                break;

            case State.LOADING_SCREEN:

                break;

            case State.OPEN_WORLD:
                playerControls.Keyboard_Player.Disable();

                playerControls.Controller_Player.Enable();
                break;

            case State.QUESTS:
                playerControls.Keyboard_QuestsUI.Disable();
                playerControls.Keyboard_UIState.Disable();

                playerControls.Controller_QuestsUI.Enable();
                playerControls.Controller_UIState.Enable();
                break;

            case State.INVENTORY:
                playerControls.Keyboard_InventoryUI.Disable();
                playerControls.Keyboard_UIState.Disable();

                playerControls.Controller_InventoryUI.Enable();
                playerControls.Controller_UIState.Enable();
                break;

            case State.SKILLS:
                playerControls.Keyboard_EquipmentUI.Disable();
                playerControls.Keyboard_UIState.Disable();

                playerControls.Controller_EquipmentUI.Enable();
                playerControls.Controller_UIState.Enable();
                break;

            case State.SYSTEM:
                playerControls.Keyboard_SystemUI.Disable();
                playerControls.Keyboard_UIState.Disable();

                playerControls.Controller_SystemUI.Enable();
                playerControls.Controller_UIState.Enable();
                break;

            case State.MAP:
                playerControls.Keyboard_Map.Disable();

                playerControls.Controller_Map.Enable();
                break;

            case State.DIALOGUE:
                playerControls.Keyboard_Dialogues.Disable();

                playerControls.Controller_Dialogues.Enable();
                break;

            case State.SHOP:
                playerControls.Keyboard_Shop.Disable();

                playerControls.Controller_Shop.Enable();
                break;

            default:
                break;
        }
    }

    public void ChangeToMouseAndKeyboard(InputAction.CallbackContext context)
    {
        inputMethod = InputMethod.MOUSE_AND_KEYBOARD;

        playerControls.DetectController.Enable();
        playerControls.DetectMouseKeyboard.Disable();

        switch (GameManager.Instance.gameState)
        {
            case State.MAIN_MENU:

                break;

            case State.LOADING_SCREEN:

                break;

            case State.OPEN_WORLD:
                playerControls.Keyboard_Player.Enable();

                playerControls.Controller_Player.Disable();
                break;

            case State.QUESTS:
                playerControls.Keyboard_QuestsUI.Enable();
                playerControls.Keyboard_UIState.Enable();

                playerControls.Controller_QuestsUI.Disable();
                playerControls.Controller_UIState.Disable();
                break;

            case State.INVENTORY:
                playerControls.Keyboard_InventoryUI.Enable();
                playerControls.Keyboard_UIState.Enable();

                playerControls.Controller_InventoryUI.Disable();
                playerControls.Controller_UIState.Disable();
                break;

            case State.SKILLS:
                playerControls.Keyboard_EquipmentUI.Enable();
                playerControls.Keyboard_UIState.Enable();

                playerControls.Controller_EquipmentUI.Disable();
                playerControls.Controller_UIState.Disable();
                break;

            case State.SYSTEM:
                playerControls.Keyboard_SystemUI.Enable();
                playerControls.Keyboard_UIState.Enable();

                playerControls.Controller_SystemUI.Disable();
                playerControls.Controller_UIState.Disable();
                break;

            case State.MAP:
                playerControls.Keyboard_Map.Enable();

                playerControls.Controller_Map.Disable();
                break;

            case State.DIALOGUE:
                playerControls.Keyboard_Dialogues.Enable();

                playerControls.Controller_Dialogues.Disable();
                break;

            case State.SHOP:
                playerControls.Keyboard_Shop.Enable();

                playerControls.Controller_Shop.Disable();
                break;

            default:
                break;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (pm.isGrounded)
        {
            pm.Jump();
        }
        else if (pm.state == MovementState.CLIMBING)
        {
            pm.climbingScript.ClimbJump();
        }
        else if (pm.state == MovementState.WALLRUNNING)
        {
            pm.wallRunningScript.WallJump();
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        pm.holdingSprintKey = true;
    }

    public void StopSprint(InputAction.CallbackContext context)
    {
        pm.holdingSprintKey = false;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        pm.PressedCrouchKey();
    }

    public void ReleaseCrouch(InputAction.CallbackContext context)
    {
        if (pm.sliding)
        {
            pm.slidingScript.StopSlide();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (plInterMan.InteractableLookingAt != null)
        {
            plInterMan.InteractableLookingAt.GetComponent<Interactable>().Interact();
        }
    }

    public void NormalAttack(InputAction.CallbackContext context)
    {
        if (!pm.isSprinting && !pm.isCrouched && canPrepareNextAttack)
        {
            canPrepareNextAttack = false;

            if (attackCount < 0 || attackCount > 2)
            {
                attackCount = 0;
            }

            if (attackCount == 0)
            {
                attackCount = 1;

                // CheckAndDamageEnemy();

                anim.SetTrigger("ATK1");
            }
            else if (attackCount == 1)
            {
                attackCount = 2;

                // CheckAndDamageEnemy();

                anim.SetTrigger("ATK2");
            }
            else if (attackCount == 2)
            {
                attackCount = 0;

                // CheckAndDamageEnemy();

                anim.SetTrigger("ATK3");
            }
        }
    }

    public void CheckAndDamageEnemy()
    {
        Collider[] enemiesToDamage = Physics.OverlapBox(attackPos.position, attackBoxHalfExtens, Quaternion.identity, whatIsEnemy);

        if (enemiesToDamage.Length > 0)
        {
            OnEnemyHit.Invoke(null, null);
        }

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(stats.GetCurrATK());

            Debug.Log("Damaging enemy");
        }
    }

    public void ChargedAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Charged attack");
    }

    public void Skill0(InputAction.CallbackContext context)
    {
        
    }

    public void Skill1(InputAction.CallbackContext context)
    {
        
    }

    public void Skill2(InputAction.CallbackContext context)
    {
        
    }

    public void OpenUIState(InputAction.CallbackContext context)
    {
        ToUIState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_UIState.Enable();

            playerControls.Keyboard_Player.Disable();
        }
        else
        {
            playerControls.Controller_UIState.Enable();

            playerControls.Controller_Player.Disable();
        }

        switch (GameManager.Instance.UIStateMan.currSectionTab)
        {
            case 0:
                GameManager.Instance.gameState = State.QUESTS;

                GameManager.Instance.QuestUIMan.SetQuestUIActive();
                break;

            case 1:
                GameManager.Instance.gameState = State.INVENTORY;

                GameManager.Instance.invMan.SetInventoryActive();

                break;

            case 2:
                GameManager.Instance.gameState = State.SKILLS;

                break;

            case 3:
                GameManager.Instance.gameState = State.SYSTEM;

                break;

            default:
                break;
        }

        CheckUIState();

        GameManager.Instance.equipmentMan.UpdatePlayerStatsUI();

        GameUIObj.SetActive(false);

        UIStateObject.SetActive(true);
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        ToUIState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Map.Enable();

            playerControls.Keyboard_Player.Disable();
        }
        else
        {
            playerControls.Controller_Map.Enable();

            playerControls.Controller_Player.Disable();
        }

        GameManager.Instance.gameState = State.MAP;

        MapObj.SetActive(true);
        GameUIObj.SetActive(false);
    }

    public void OpenConsole(InputAction.CallbackContext context)
    {
        ToUIState();

        playerControls.Keyboard_Console.Enable();

        playerControls.Keyboard_Player.Disable();

        GameUIObj.SetActive(false);
        ConsoleObj.SetActive(true);
        consoleMan.txtField.ActivateInputField();
    }

    public void TurnCursorOn(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TurnCursorOff(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CloseUIState(InputAction.CallbackContext context)
    {
        ToWorldState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Player.Enable();

            playerControls.Keyboard_UIState.Disable();

            playerControls.Keyboard_QuestsUI.Disable();
            playerControls.Keyboard_InventoryUI.Disable();
            playerControls.Keyboard_EquipmentUI.Disable();
            playerControls.Keyboard_SystemUI.Disable();
        }
        else
        {
            playerControls.Controller_Player.Enable();

            playerControls.Controller_UIState.Disable();

            playerControls.Controller_QuestsUI.Disable();
            playerControls.Controller_InventoryUI.Disable();
            playerControls.Controller_EquipmentUI.Disable();
            playerControls.Controller_SystemUI.Disable();
        }        

        GameManager.Instance.gameState = State.OPEN_WORLD;

        GameManager.Instance.plInputMan.GameUIObj.SetActive(true);

        GameManager.Instance.plInputMan.UIStateObject.SetActive(false);
    }

    public void NextUIState(InputAction.CallbackContext context)
    {
        GameManager.Instance.UIStateMan.ChangeUISection(GameManager.Instance.UIStateMan.currSectionTab + 1);

        CheckUIState();
    }

    public void PreviousUIState(InputAction.CallbackContext context)
    {
        GameManager.Instance.UIStateMan.ChangeUISection(GameManager.Instance.UIStateMan.currSectionTab - 1);

        CheckUIState();
    }

    public void CheckUIState()
    {
        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            switch (GameManager.Instance.UIStateMan.currSectionTab)
            {
                case 0:
                    playerControls.Keyboard_QuestsUI.Enable();
                    playerControls.Keyboard_InventoryUI.Disable();
                    playerControls.Keyboard_EquipmentUI.Disable();
                    playerControls.Keyboard_SystemUI.Disable();
                    break;

                case 1:
                    playerControls.Keyboard_QuestsUI.Disable();
                    playerControls.Keyboard_InventoryUI.Enable();
                    playerControls.Keyboard_EquipmentUI.Disable();
                    playerControls.Keyboard_SystemUI.Disable();
                    break;

                case 2:
                    playerControls.Keyboard_QuestsUI.Disable();
                    playerControls.Keyboard_InventoryUI.Disable();
                    playerControls.Keyboard_EquipmentUI.Enable();
                    playerControls.Keyboard_SystemUI.Disable();
                    break;

                case 3:
                    playerControls.Keyboard_QuestsUI.Disable();
                    playerControls.Keyboard_InventoryUI.Disable();
                    playerControls.Keyboard_EquipmentUI.Disable();
                    playerControls.Keyboard_SystemUI.Enable();
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (GameManager.Instance.UIStateMan.currSectionTab)
            {
                case 0:
                    playerControls.Controller_QuestsUI.Enable();
                    playerControls.Controller_InventoryUI.Disable();
                    playerControls.Controller_EquipmentUI.Disable();
                    playerControls.Controller_SystemUI.Disable();
                    break;

                case 1:
                    playerControls.Controller_QuestsUI.Disable();
                    playerControls.Controller_InventoryUI.Enable();
                    playerControls.Controller_EquipmentUI.Disable();
                    playerControls.Controller_SystemUI.Disable();
                    break;

                case 2:
                    playerControls.Controller_QuestsUI.Disable();
                    playerControls.Controller_InventoryUI.Disable();
                    playerControls.Controller_EquipmentUI.Enable();
                    playerControls.Controller_SystemUI.Disable();
                    break;

                case 3:
                    playerControls.Controller_QuestsUI.Disable();
                    playerControls.Controller_InventoryUI.Disable();
                    playerControls.Controller_EquipmentUI.Disable();
                    playerControls.Controller_SystemUI.Enable();
                    break;

                default:
                    break;
            }
        }
    }

    public void NextQuestPage(InputAction.CallbackContext context)
    {
        GameManager.Instance.QuestUIMan.ChangeToQuestTab(GameManager.Instance.QuestUIMan.currQuestTab + 1);
    }

    public void PreviousQuestPage(InputAction.CallbackContext context)
    {
        GameManager.Instance.QuestUIMan.ChangeToQuestTab(GameManager.Instance.QuestUIMan.currQuestTab - 1);
    }

    public void NextInventoryPage(InputAction.CallbackContext context)
    {
        Debug.Log("Next page");

        GameManager.Instance.invMan.GoToNextInventoryWindow();
    }

    public void PreviousInventoryPage(InputAction.CallbackContext context)
    {
        Debug.Log("Previous page");

        GameManager.Instance.invMan.GoToPreviousInventoryWindow();
    }

    public void NextInventorySlot(InputAction.CallbackContext context)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Next slot");
        }
    }

    public void PreviousInventorySlot(InputAction.CallbackContext context)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Previous slot");
        }
    }

    public void NextEquipmentPage(InputAction.CallbackContext context)
    {
        Debug.Log("Next page");

        GameManager.Instance.equipmentMan.GoToNextEquipmentWindow();
    }

    public void PreviousEquipmentPage(InputAction.CallbackContext context)
    {
        Debug.Log("Previous page");

        GameManager.Instance.equipmentMan.GoToPreviousEquipmentWindow();
    }

    public void NextEquipmentSlot(InputAction.CallbackContext context)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Next slot");
        }
    }

    public void PreviousEquipmentSlot(InputAction.CallbackContext context)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Previous slot");
        }
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        ToWorldState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Player.Enable();

            playerControls.Keyboard_Map.Disable();
        }
        else
        {
            playerControls.Controller_Player.Enable();

            playerControls.Controller_Map.Disable();
        }

        GameManager.Instance.gameState = State.OPEN_WORLD;

        MapObj.SetActive(false);
        GameUIObj.SetActive(true);
    }

    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        GameManager.Instance.dialMan.ContinueDialogue();
    }

    public void SetDialogueUI()
    {
        ToUIState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Dialogues.Enable();

            playerControls.Keyboard_Player.Disable();
        }
        else
        {
            playerControls.Controller_Dialogues.Enable();

            playerControls.Controller_Player.Disable();
        }

        GameManager.Instance.gameState = State.DIALOGUE;

        DialogueObj.SetActive(true);
        GameUIObj.SetActive(false);
    }

    public void ExitDialogueSequence()
    {
        ToWorldState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Player.Enable();

            playerControls.Keyboard_Dialogues.Disable();
        }
        else
        {
            playerControls.Controller_Player.Enable();

            playerControls.Controller_Dialogues.Disable();
        }

        GameManager.Instance.gameState = State.OPEN_WORLD;

        GameUIObj.SetActive(true);
        DialogueObj.SetActive(false);
    }

    public void SetShopUI()
    {
        ToUIState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Shop.Enable();

            playerControls.Keyboard_Player.Disable();
        }
        else
        {
            playerControls.Controller_Shop.Enable();

            playerControls.Controller_Player.Disable();
        }

        GameManager.Instance.gameState = State.SHOP;

        DialogueObj.SetActive(false);
        ShopObj.SetActive(true);
        GameUIObj.SetActive(false);
    }

    public void CloseShop(InputAction.CallbackContext context)
    {
        ToWorldState();

        if (inputMethod == InputMethod.MOUSE_AND_KEYBOARD)
        {
            playerControls.Keyboard_Player.Enable();

            playerControls.Keyboard_Shop.Disable();
        }
        else
        {
            playerControls.Controller_Player.Enable();

            playerControls.Controller_Shop.Disable();
        }

        GameManager.Instance.gameState = State.OPEN_WORLD;

        GameManager.Instance.shopMan.currentShop = null;

        GameUIObj.SetActive(true);
        ShopObj.SetActive(false);
    }

    public void ConfirmInput(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Keyboard_Player.Enable();

        GameUIObj.SetActive(true);
        consoleMan.Console();
        ConsoleObj.SetActive(false);

        playerControls.Keyboard_Console.Disable();
    }

    public void CloseConsole(InputAction.CallbackContext context)
    {
        ToWorldState();

        playerControls.Keyboard_Player.Enable();

        GameUIObj.SetActive(true);
        consoleMan.txtField.text = "";
        ConsoleObj.SetActive(false);

        playerControls.Keyboard_Console.Disable();
    }    

    public void ToWorldState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam.gameObject.GetComponent<CinemachineBrain>().enabled = true;
        pm.canRotate = true;
        pm.freeze = false;
    }

    public void ToUIState()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cam.gameObject.GetComponent<CinemachineBrain>().enabled = false;
        pm.canRotate = false;
        pm.freeze = true;
    }
}
