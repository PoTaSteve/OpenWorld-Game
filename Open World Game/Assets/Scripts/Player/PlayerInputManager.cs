using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    private PlayerControls playerControls;

    public InventoryManager invMan;

    public GameObject InventoryObj;
    public GameObject GameUIObj;

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

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Enable();
        playerControls.Inventory.Enable();
        playerControls.Map.Enable();
        playerControls.EscMenu.Enable();
        playerControls.Dialogues.Enable();

        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.OpenInventory.performed += OpenInventory;
        /*
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.OpenMap.performed += OpenMap;
        playerControls.Player.OpenEscMenu.performed += OpenEscMenu;
        
        playerControls.Inventory.CloseInventory.performed += CloseInventory;
        */
        playerControls.Inventory.NextInvPage.performed += NextInvPage;
        playerControls.Inventory.PreviousInvPage.performed += PreviousInvPage;
        /*
        playerControls.Map.CloseMap.performed += CloseMap;

        playerControls.EscMenu.CloseEscMenu.performed += CloseEscMenu;
        */

        playerControls.Player.Enable();
        playerControls.Inventory.Disable();
        playerControls.Map.Disable();
        playerControls.EscMenu.Disable();
        playerControls.Dialogues.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        StartCoroutine(OpenInventoryCoroutine(1f));
    }

    public IEnumerator OpenInventoryCoroutine(float openTime)
    {
        playerControls.Inventory.Enable();
        InventoryObj.SetActive(true);
        GameUIObj.SetActive(false);

        ThirdPersonCam.SetActive(false);

        playerControls.Player.Disable();

        yield return new WaitForSeconds(openTime);

        invMan.GoToSpecificInvTab(0);
    }

    public void CloseInventory(InputAction.CallbackContext context)
    {
        StartCoroutine(CloseInventoryCoroutine(5f / 6f));
    }

    public void CloseInventoryForButton()
    {
        StartCoroutine(CloseInventoryCoroutine(5f / 6f));
    }

    public IEnumerator CloseInventoryCoroutine(float closeTime)
    {
        yield return new WaitForSeconds(closeTime);
    }

    public void NextInvPage(InputAction.CallbackContext context)
    {
        invMan.GoToNextInvTab();
    }

    public void PreviousInvPage(InputAction.CallbackContext context)
    {
        invMan.GoToPreviousInvTab();
    }
}
