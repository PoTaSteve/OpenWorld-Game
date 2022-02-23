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

    [SerializeField]
    private InventoryManager invMan;

    public GameObject InventoryObj;
    public GameObject GameUIObj;
    public GameObject MapObj;
    public GameObject EscMenuObj;

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
        playerControls.Player.Interact.performed += Interact;
        playerControls.Player.OpenMap.performed += OpenMap;
        playerControls.Player.OpenEscMenu.performed += OpenEscMenu;
        
        playerControls.Inventory.CloseInventory.performed += CloseInventory;
        playerControls.Inventory.NextInvPage.performed += NextInvPage;
        playerControls.Inventory.PreviousInvPage.performed += PreviousInvPage;
        
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
        playerControls.Player.Enable();
        InventoryObj.SetActive(false);
        GameUIObj.SetActive(true);

        ThirdPersonCam.SetActive(true);

        playerControls.Inventory.Disable();

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

    public void Interact(InputAction.CallbackContext context)
    {
        if (playerTrColl.InRangeInteractables.Count > 0)
        {
            Interactable interactab = playerTrColl.InRangeInteractables[playerTrColl.currentInteractableIndex].GetComponent<Interactable>();

//---------------------------------------------------------------------- PICK UP -----------------------------------------------------------------
            if (interactab.type == InteractableType.PickUp)
            {
//---------------------------------------------------------------------- WEAPON -----------------------------------------------------------------
                if (interactab.pickUpType == PickUpType.Weapon)
                {
                    #region Set Inventory Slot
                    WeaponInfo objInfo = interactab.gameObject.GetComponent<WeaponInfo>();

                    // Instantiate Slot: WeaponInfo script
                    WeaponInfo newSlot = Instantiate<WeaponInfo>(invMan.WeaponInvSlot.GetComponent<WeaponInfo>(), invMan.TabsContent[0]);
                    invMan.WeaponsTab.Add(newSlot);
                    invMan.WeaponTabStr.Add(objInfo.SO.weaponName);
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
                    newSlot.transform.GetChild(2).gameObject.SetActive(false);
                    newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + newSlot.currentLevel;
                    newSlot.transform.GetChild(1).GetComponent<Image>().sprite = newSlot.SO.icon;

                    Transform Rarity = newSlot.transform.GetChild(3);
                    foreach (Transform t in Rarity)
                    {
                        t.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < newSlot.SO.rarity; i++)
                    {
                        Rarity.GetChild(i).gameObject.SetActive(true);
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
// ------------------------------------------------------------------------ MATERIAL ---------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.Material)
                {
                    MaterialInfo objInfo = interactab.gameObject.GetComponent<MaterialInfo>();

                    // Instantiate Slot: MaterialInfo script
                    
                    if (invMan.MaterialsTabStr.Contains(objInfo.MaterialSO.materialName))
                    {
                        // Update the count 
                        foreach (Transform t in invMan.TabsContent[1].transform)
                        {
                            MaterialInfo info = t.GetComponent<MaterialInfo>();
                            if (info.MaterialSO.materialName == objInfo.MaterialSO.materialName)
                            {
                                info.count += objInfo.count;
                                info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                            }
                        }
                    }
                    else
                    {
                        MaterialInfo newSlot = Instantiate<MaterialInfo>(invMan.MaterialInvSlot.GetComponent<MaterialInfo>(), invMan.TabsContent[1]);

                        invMan.MaterialsTab.Add(newSlot);
                        invMan.MaterialsTabStr.Add(objInfo.MaterialSO.materialName);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateMaterialInvSlotDetails);

                        newSlot.MaterialSO = objInfo.MaterialSO;
                        newSlot.count = objInfo.count;

                        newSlot.transform.GetChild(2).gameObject.SetActive(false);

                        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = objInfo.MaterialSO.icon;

                        Transform rarity = newSlot.transform.GetChild(3);
                        foreach (Transform t in rarity)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.MaterialSO.rarity; i++)
                        {
                            rarity.GetChild(i).gameObject.SetActive(true);
                        }

                        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

                        if (newSlot.MaterialSO.materialType == MaterialTypeEnum.CrafingIngredient)
                        {
                            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.CraftingIngredientIcon;
                        }
                        else
                        {
                            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.CraftingResultIcon;
                        }

                        newSlot.transform.GetChild(6).gameObject.SetActive(true);
                    }
                }
// ------------------------------------------------------------------------------ INGREDIENT -----------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.Ingredient)
                {
                    IngredientInfo objInfo = interactab.gameObject.GetComponent<IngredientInfo>();

                    // Instantiate Slot: IngredientInfo script
                    if (invMan.IngredientsTabStr.Contains(objInfo.IngredientSO.ingredientName))
                    {
                        // Add the count
                        foreach (Transform t in invMan.TabsContent[2].transform)
                        {
                            IngredientInfo info = t.GetComponent<IngredientInfo>();
                            if (info.IngredientSO.ingredientName== objInfo.IngredientSO.ingredientName)
                            {
                                info.count += objInfo.count;
                                info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                            }
                        }
                    }
                    else
                    {
                        // Instantiate slot
                        IngredientInfo newSlot = Instantiate<IngredientInfo>(invMan.IngredientInvSlot.GetComponent<IngredientInfo>(), invMan.TabsContent[2]);
                        
                        invMan.IngredientsTab.Add(newSlot);
                        invMan.IngredientsTabStr.Add(objInfo.IngredientSO.ingredientName);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateIngredientInvSlotDetails);

                        newSlot.IngredientSO = objInfo.IngredientSO;
                        newSlot.count = objInfo.count;

                        newSlot.transform.GetChild(2).gameObject.SetActive(false);

                        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = objInfo.IngredientSO.icon;

                        Transform rarity = newSlot.transform.GetChild(3);
                        foreach (Transform t in rarity)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.IngredientSO.rarity; i++)
                        {
                            rarity.GetChild(i).gameObject.SetActive(true);
                        }

                        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

                        if (newSlot.IngredientSO.ingredientType == IngredientType.Base)
                        {
                            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.BaseIngredientIcon;
                        }
                        else if (newSlot.IngredientSO.ingredientType == IngredientType.Specific)
                        {
                            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.SpecificIngredientIcon;
                        }

                        newSlot.transform.GetChild(6).gameObject.SetActive(true);
                    }
                }
// ------------------------------------------------------------------------- FOOD ------------------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.Food)
                {
                    FoodInfo objInfo = interactab.gameObject.GetComponent<FoodInfo>();

                    // Instantiate Slot: FoodInfo script
                    if (invMan.FoodTabStr.Contains(objInfo.FoodSO.foodName))
                    {
                        // Add the count
                        foreach (Transform t in invMan.TabsContent[3].transform)
                        {
                            FoodInfo info = t.GetComponent<FoodInfo>();
                            if (info.FoodSO.foodName== objInfo.FoodSO.foodName)
                            {
                                info.count += objInfo.count;
                                info.gameObject.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = info.count.ToString();
                            }
                        }
                    }
                    else
                    {
                        // Instantiate slot
                        FoodInfo newSlot = Instantiate<FoodInfo>(invMan.FoodInvSlot.GetComponent<FoodInfo>(), invMan.TabsContent[3]);

                        invMan.FoodTab.Add(newSlot);
                        invMan.FoodTabStr.Add(objInfo.FoodSO.foodName);
                        newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateFoodInvSlotDetails);

                        newSlot.FoodSO = objInfo.FoodSO;
                        newSlot.count = objInfo.count;

                        newSlot.transform.GetChild(2).gameObject.SetActive(false);

                        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = objInfo.FoodSO.icon;

                        Transform rarity = newSlot.transform.GetChild(3);
                        foreach (Transform t in rarity)
                        {
                            t.gameObject.SetActive(false);
                        }
                        for (int i = 0; i < newSlot.FoodSO.rarity; i++)
                        {
                            rarity.GetChild(i).gameObject.SetActive(true);
                        }

                        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.count.ToString();

                        if (newSlot.FoodSO.foodType == FoodType.Potion)
                        {
                            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.PotionFoodTypeIcon;
                        }
                        else if (newSlot.FoodSO.foodType == FoodType.Food)
                        {
                            if (newSlot.FoodSO.buffType == FoodBuffType.Attack)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.AttackFoodBuffIcon;
                            }
                            else if (newSlot.FoodSO.buffType == FoodBuffType.Defence)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.DefenceFoodBuffIcon;
                            }
                            else if (newSlot.FoodSO.buffType == FoodBuffType.Heal)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.HealFoodBuffIcon;
                            }
                            else if (newSlot.FoodSO.buffType == FoodBuffType.Regen)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.RegenFoodBuffIcon;
                            }
                            else if (newSlot.FoodSO.buffType == FoodBuffType.Speed)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.SpeedFoodBuffIcon;
                            }
                            else if (newSlot.FoodSO.buffType == FoodBuffType.StaminaConsumption)
                            {
                                newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = invMan.StaminaConsumptionFoodBuffIcon;
                            }
                        }

                        newSlot.transform.GetChild(6).gameObject.SetActive(true);
                    }
                }
// ----------------------------------------------------------------------- SPECIAL ITEM --------------------------------------------------------------
                else if (interactab.pickUpType == PickUpType.SpecialItem)
                {
                    SpecialItemInfo objInfo = interactab.gameObject.GetComponent<SpecialItemInfo>();

                    // Instantiate Slot: IngredientInfo script
                    SpecialItemInfo newSlot = Instantiate<SpecialItemInfo>(invMan.IngredientInvSlot.GetComponent<SpecialItemInfo>(), invMan.TabsContent[4]);

                    invMan.SpecialItemsTab.Add(newSlot);
                    invMan.SpecialItemsTabStr.Add(objInfo.SpecialItemSO.specialItemName);
                    newSlot.GetComponent<Button>().onClick.AddListener(invMan.UpdateSpecialItemInvSlotDetails);

                    newSlot.SpecialItemSO = objInfo.SpecialItemSO;

                    newSlot.transform.GetChild(2).gameObject.SetActive(false);

                    newSlot.transform.GetChild(1).GetComponent<Image>().sprite = objInfo.SpecialItemSO.icon;

                    Transform rarity = newSlot.transform.GetChild(3);
                    foreach (Transform t in rarity)
                    {
                        t.gameObject.SetActive(false);
                    }
                    for (int i = 0; i < newSlot.SpecialItemSO.rarity; i++)
                    {
                        rarity.GetChild(i).gameObject.SetActive(true);
                    }

                    newSlot.transform.GetChild(4).gameObject.SetActive(true);
                }

                playerTrColl.RemoveInteractabUI(interactab.gameObject);

                Destroy(interactab.gameObject);
            }
        }
    }
}
