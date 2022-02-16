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

    public InventoryManager invMan;
    public DialogueManager dialMan;

    public GameObject InventoryObj;
    public GameObject GameUIObj;
    public GameObject MapObj;
    public GameObject EscMenuObj;

    public GameObject FreeLookCam;

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

    private PlayerTriggerCollision playerTrColl;
    public float scrollWheel;

    private void Awake()
    {
        GameUIObj.SetActive(true);
        InventoryObj.SetActive(true);
        MapObj.SetActive(true);
        EscMenuObj.SetActive(true);

        FreeLookCam.SetActive(true);

        playerControls = new PlayerControls();

        playerControls.Player.Enable();
        playerControls.Inventory.Enable();
        playerControls.Map.Enable();
        playerControls.EscMenu.Enable();
        playerControls.Dialogues.Enable();

        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.OpenInventory.performed += OpenInventory;
        playerControls.Player.OpenMap.performed += OpenMap;
        playerControls.Player.OpenEscMenu.performed += OpenEscMenu;

        playerControls.Inventory.CloseInventory.performed += CloseInventory;
        playerControls.Inventory.NextPage.performed += NextInvPage;
        playerControls.Inventory.PreviousPage.performed += PreviousInvPage;

        playerControls.Map.CloseMap.performed += CloseMap;

        playerControls.EscMenu.CloseEscMenu.performed += CloseEscMenu;

        playerControls.Player.Enable();
        playerControls.Inventory.Disable();
        playerControls.Map.Disable();
        playerControls.EscMenu.Disable();
        playerControls.Dialogues.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTrColl = GameObject.Find("PlayerTriggerCollider").GetComponent<PlayerTriggerCollision>();

        GameUIObj.SetActive(true);
        InventoryObj.SetActive(false);
        MapObj.SetActive(false);
        EscMenuObj.SetActive(false);
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

        //bool isRunning = Input.GetKey(KeyCode.LeftControl);
        //currentSpeed = ((isRunning) ? runningSpeed : walkingSpeed) * direction.magnitude;

        //animationSpeedPercent = ((isRunning) ? 1f : 0.5f) * direction.magnitude;
        //animator.SetFloat("SpeedPercent", animationSpeedPercent);

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

            playerTrColl.InteractableUIContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25 + (50 * playerTrColl.currentInteractableIndex));
        }
        #endregion

        if (dialMan.isWritingText && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)))
        {
            dialMan.canSkipText = true;
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

        FreeLookCam.SetActive(false);

        playerControls.Player.Disable();

        yield return new WaitForSeconds(openTime);

        invMan.GoToSpecificInventoryTab(0);
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
        //invMan.TabIconsAnim.Play("Close");

        ColorBlock colors1 = invMan.TabSections[invMan.currentActiveTab].GetComponent<Button>().colors;
        colors1.normalColor = Color.clear;
        invMan.TabSections[invMan.currentActiveTab].GetComponent<Button>().colors = colors1;

        invMan.TabIcons[invMan.currentActiveTab].GetComponent<Image>().color = invMan.TabInactiveColor;

        yield return new WaitForSeconds(closeTime);

        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        FreeLookCam.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void NextInvPage(InputAction.CallbackContext context)
    {
        invMan.GoToNextInventoryTab();
    }

    public void PreviousInvPage(InputAction.CallbackContext context)
    {
        invMan.GoToPreviousInvetoryTab();
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        playerControls.Map.Enable();
        MapObj.SetActive(true);
        GameUIObj.SetActive(false);

        FreeLookCam.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        FreeLookCam.SetActive(true);

        playerControls.Inventory.Disable();
    }

    public void OpenEscMenu(InputAction.CallbackContext context)
    {
        playerControls.EscMenu.Enable();
        EscMenuObj.SetActive(true);
        GameUIObj.SetActive(false);

        FreeLookCam.SetActive(false);

        playerControls.Player.Disable();
    }

    public void CloseEscMenu(InputAction.CallbackContext context)
    {
        StartCoroutine(CloseEscMenuCoroutine(55f/60f));
    }

    public IEnumerator CloseEscMenuCoroutine(float closeTime)
    {
        Animator anim = EscMenuObj.GetComponent<Animator>();

        anim.Play("Close");

        yield return new WaitForSeconds(closeTime);

        playerControls.Player.Enable();
        EscMenuObj.SetActive(false);
        GameUIObj.SetActive(true);

        FreeLookCam.SetActive(true);

        playerControls.EscMenu.Disable();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (playerTrColl.InRangeInteractables.Count > 0)
        {
            Interactable interactab = playerTrColl.InRangeInteractables[playerTrColl.currentInteractableIndex].GetComponent<Interactable>();

// ----------------------------------------------------------- PICK UP ------------------------------------------------------------------------------------
            if (interactab.type == InteractableType.PickUp)
            {
// -------------------------------------------------------------- WEAPON ----------------------------------------------------------------------------------
                if (interactab.pickUpType == PickUpType.Weapon)
                {
                    #region Set Inventory Slot
                    WeaponInfo objInfo = interactab.gameObject.GetComponent<WeaponInfo>();

                    // Instantiate Slot: WeaponInfo script
                    WeaponInfo newSlot = Instantiate<WeaponInfo>(invMan.WeaponInvSlot.GetComponent<WeaponInfo>(), invMan.WeaponInvWindow.transform);
                    invMan.EquipmentTab.Add(newSlot);
                    newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateWeaponInvSlotDetails);

                    newSlot.SO = objInfo.SO;

                    objInfo.SetAtkFromLevel();
                    newSlot.baseATK = objInfo.baseATK;

                    newSlot.currentXp = objInfo.currentXp;
                    objInfo.xpForNextLevel = objInfo.XpForNextLevel(objInfo.currentLevel);
                    newSlot.xpForNextLevel = objInfo.xpForNextLevel;

                    newSlot.currentLevel = objInfo.currentLevel;
                    newSlot.currentMaxLevel = objInfo.currentMaxLevel;
                    newSlot.ascensionLevel = objInfo.ascensionLevel;
                    newSlot.isLocked = objInfo.isLocked;

                    // Instantiate Slot: Slot UI
                    newSlot.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv. " + newSlot.currentLevel;
                    newSlot.transform.GetChild(1).GetComponent<Image>().sprite = newSlot.SO.icon;

                    GameObject Rarity = newSlot.transform.GetChild(3).gameObject;
                    foreach (Transform t in Rarity.transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < newSlot.SO.rarity; i++)
                    {
                        Rarity.transform.GetChild(i).gameObject.SetActive(true);
                    }

                    if (newSlot.isLocked)
                    {
                        newSlot.transform.GetChild(5).gameObject.SetActive(true);
                        newSlot.transform.GetChild(5).GetChild(0).GetComponent<Image>().color = invMan.closedPadlockColBG;
                        newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.PadlockClosed;
                        newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().color = invMan.closedPadlockCol;
                    }
                    else
                    {
                        newSlot.transform.GetChild(5).gameObject.SetActive(false);
                    }

                    newSlot.transform.GetChild(6).gameObject.SetActive(true);

                    #endregion
                }
// -------------------------------------------------------------- MATERIAL --------------------------------------------------------------------------------
                if (interactab.pickUpType == PickUpType.Material)
                {
                    #region Set Inventory Slot
                    MaterialInfo objInfo = interactab.gameObject.GetComponent<MaterialInfo>();

                    if (!invMan.MaterialsTabStr.Contains(objInfo.MaterialSO.materialName))
                    {
                        invMan.MaterialsTabStr.Add(objInfo.MaterialSO.materialName);
                        
                        // Instantiate Slot: WeaponInfo script
                        MaterialInfo newSlot = Instantiate<MaterialInfo>(invMan.MaterialInvSlot.GetComponent<MaterialInfo>(), invMan.MaterialInvWindow.transform);
                        invMan.MaterialsTab.Add(newSlot);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateMaterialInvSlotDetails);

                        newSlot.MaterialSO = objInfo.MaterialSO;
                        newSlot.count = objInfo.count;

                        // Instantiate Slot: Slot UI
                        newSlot.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = invMan.FromIntToRaritySprite(newSlot.MaterialSO.rarity);
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Image>().sprite = newSlot.MaterialSO.icon;

                        GameObject Rarity = newSlot.transform.GetChild(1).GetChild(0).GetChild(4).gameObject;
                        foreach (Transform t in Rarity.transform)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.MaterialSO.rarity; i++)
                        {
                            Rarity.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (Transform t in invMan.MaterialInvWindow.transform)
                        {
                            MaterialInfo info = t.GetComponent<MaterialInfo>();
                            if (info.MaterialSO.materialName == objInfo.MaterialSO.materialName)
                            {
                                int n = int.Parse(info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                                n += info.count;
                                info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = n.ToString();
                            }
                        }
                    }
                    #endregion
                }
// ---------------------------------------------------------------------- INGREDIENT ----------------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.Ingredient)
                {
                    #region Set Inventory Slot
                    IngredientInfo objInfo = interactab.gameObject.GetComponent<IngredientInfo>();

                    if (!invMan.IngredientsTabStr.Contains(objInfo.IngredientSO.ingredientName))
                    {
                        invMan.IngredientsTabStr.Add(objInfo.IngredientSO.ingredientName);

                        // Instantiate Slot: WeaponInfo script
                        IngredientInfo newSlot = Instantiate<IngredientInfo>(invMan.IngredientInvSlot.GetComponent<IngredientInfo>(), invMan.IngredientInvWindow.transform);
                        invMan.IngredientsTab.Add(newSlot);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateIngredientInvSlotDetails);

                        newSlot.IngredientSO = objInfo.IngredientSO;
                        newSlot.count = objInfo.count;

                        // Instantiate Slot: Slot UI
                        newSlot.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = invMan.FromIntToRaritySprite(newSlot.IngredientSO.rarity);
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Image>().sprite = newSlot.IngredientSO.icon;

                        GameObject Rarity = newSlot.transform.GetChild(1).GetChild(0).GetChild(4).gameObject;
                        foreach (Transform t in Rarity.transform)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.IngredientSO.rarity; i++)
                        {
                            Rarity.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (Transform t in invMan.IngredientInvWindow.transform)
                        {
                            IngredientInfo info = t.GetComponent<IngredientInfo>();
                            if (info.IngredientSO.ingredientName == objInfo.IngredientSO.ingredientName)
                            {
                                int n = int.Parse(info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                                n += info.count;
                                info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = n.ToString();
                            }
                        }
                    }
                    #endregion
                }
// ---------------------------------------------------------------------- FOOD ----------------------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.Food)
                {
                    #region Set Inventory Slot
                    FoodInfo objInfo = interactab.gameObject.GetComponent<FoodInfo>();

                    if (!invMan.FoodTabStr.Contains(objInfo.FoodSO.foodName))
                    {
                        invMan.FoodTabStr.Add(objInfo.FoodSO.foodName);

                        // Instantiate Slot: WeaponInfo script
                        FoodInfo newSlot = Instantiate<FoodInfo>(invMan.FoodInvSlot.GetComponent<FoodInfo>(), invMan.FoodInvWindow.transform);
                        invMan.FoodTab.Add(newSlot);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateFoodInvSlotDetails);

                        newSlot.FoodSO = objInfo.FoodSO;
                        newSlot.count = objInfo.count;

                        // Instantiate Slot: Slot UI
                        newSlot.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = invMan.FromIntToRaritySprite(newSlot.FoodSO.rarity);
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString(); ;
                        newSlot.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Image>().sprite = newSlot.FoodSO.icon;
                        newSlot.transform.GetChild(3).GetComponent<Image>().sprite = invMan.FromBuffStringToSprite(newSlot.FoodSO.FoodBuffTypeEnumToString(newSlot.FoodSO.buffType));

                        GameObject Rarity = newSlot.transform.GetChild(1).GetChild(0).GetChild(4).gameObject;
                        foreach (Transform t in Rarity.transform)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.FoodSO.rarity; i++)
                        {
                            Rarity.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (Transform t in invMan.IngredientInvWindow.transform)
                        {
                            FoodInfo info = t.GetComponent<FoodInfo>();
                            if (info.FoodSO.foodName == objInfo.FoodSO.foodName)
                            {
                                int n = int.Parse(info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                                n += info.count;
                                info.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = n.ToString();
                            }
                        }
                    }
                    #endregion
                }
// ---------------------------------------------------------------------- SPECIAL ITEM --------------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.SpecialItem)
                {

                }


                //Updates all indexes
                foreach (GameObject obj in playerTrColl.InRangeInteractables)
                {
                    if (obj.GetComponent<Interactable>().index > playerTrColl.currentInteractableIndex)
                    {
                        obj.GetComponent<Interactable>().index--;
                    }
                }
                foreach (Transform t in playerTrColl.InteractableUIContent.transform)
                {
                    if (t.gameObject.GetComponent<IDInteractableUI>().index > playerTrColl.currentInteractableIndex)
                    {
                        t.gameObject.GetComponent<IDInteractableUI>().index--;
                    }
                }

                // Fix the position of the UI
                Destroy(playerTrColl.InteractableUIContent.transform.GetChild(playerTrColl.currentInteractableIndex).gameObject);

                playerTrColl.AllIDs.Remove(interactab.ID);
                playerTrColl.InstantiatedInteractablesUI--;

                if (playerTrColl.InstantiatedInteractablesUI == playerTrColl.currentInteractableIndex && playerTrColl.currentInteractableIndex != 0)
                {
                    playerTrColl.currentInteractableIndex--;
                }

                playerTrColl.FixInteractableUIPos();

                if (playerTrColl.InstantiatedInteractablesUI == 0)
                {
                    playerTrColl.InteractableItemUI.SetActive(false);
                }

                //Destroy both the world object and the UI Interactable AT THE END
                playerTrColl.InRangeInteractables.Remove(interactab.gameObject);
                Destroy(interactab.gameObject);

                //We can eventually spawn some particles at interactab.gameObject.transform.position (world position)
            }
// ------------------------------------------------------------- NPC --------------------------------------------------------------------------------------
            else if (interactab.type == InteractableType.NPC)
            {
                playerControls.Player.Disable();
                playerControls.Dialogues.Enable();

                NPCTalk npc = interactab.gameObject.GetComponent<NPCTalk>();

                dialMan.StartStory(npc);
            }
// ------------------------------------------------------------ DOOR --------------------------------------------------------------------------------------
            else if (interactab.type == InteractableType.Door)
            {

            }
// ----------------------------------------------------------- CHEST --------------------------------------------------------------------------------------
            else if (interactab.type == InteractableType.Chest)
            {
                Chest chest = interactab.gameObject.GetComponent<Chest>();

                Destroy(chest.gameObject.GetComponent<Rigidbody>());

                Animator anim = interactab.gameObject.GetComponent<Animator>();

                anim.SetTrigger("Open");

                #region Fixes Post-Interaction
                //Updates all indexes
                foreach (GameObject obj in playerTrColl.InRangeInteractables)
                {
                    if (obj.GetComponent<Interactable>().index > playerTrColl.currentInteractableIndex)
                    {
                        obj.GetComponent<Interactable>().index--;
                    }
                }
                foreach (Transform t in playerTrColl.InteractableUIContent.transform)
                {
                    if (t.gameObject.GetComponent<IDInteractableUI>().index > playerTrColl.currentInteractableIndex)
                    {
                        t.gameObject.GetComponent<IDInteractableUI>().index--;
                    }
                }

                // Fix the position of the UI
                Destroy(playerTrColl.InteractableUIContent.transform.GetChild(playerTrColl.currentInteractableIndex).gameObject);

                playerTrColl.AllIDs.Remove(interactab.ID);
                playerTrColl.InstantiatedInteractablesUI--;

                if (playerTrColl.InstantiatedInteractablesUI == playerTrColl.currentInteractableIndex && playerTrColl.currentInteractableIndex != 0)
                {
                    playerTrColl.currentInteractableIndex--;
                }

                playerTrColl.FixInteractableUIPos();

                if (playerTrColl.InstantiatedInteractablesUI == 0)
                {
                    playerTrColl.InteractableItemUI.SetActive(false);
                }

                //Destroy only the UI Interactable AT THE END
                playerTrColl.InRangeInteractables.Remove(interactab.gameObject);

                //We can eventually spawn some particles at interactab.gameObject.transform.position (world position)
                #endregion

                chest.OpenChest();
            }
// ----------------------------------------------------------- DUNGEON ------------------------------------------------------------------------------------
            else if (interactab.type == InteractableType.Dungeon)
            {

            }
        }
    }

    public void ExitDialogueMode()
    {
        playerControls.Dialogues.Disable();
        playerControls.Player.Enable();

        GameUIObj.SetActive(true);
        FreeLookCam.SetActive(true);    
    }
}
