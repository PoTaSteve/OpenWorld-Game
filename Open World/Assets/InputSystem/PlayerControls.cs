// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""03b3ec15-c769-4ab8-87a7-415f3b35841b"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b16f4c73-df05-4c2a-9013-a9fd0cfa5e0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""0bdd92ea-0ea5-44c5-a050-3ea8d4c329d8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""2e6ce5f5-8e46-4163-aef7-b8521fc7de3c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""823eda98-2f4a-46f8-9d68-e5742ea904fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""b065d5ae-f7f4-4e79-b1cc-71b2bc4d87a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkillHold"",
                    ""type"": ""Button"",
                    ""id"": ""099cb733-236f-413f-936d-317409783156"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Burst"",
                    ""type"": ""Button"",
                    ""id"": ""910bfa26-29fe-4873-9fbe-a074cf25e53b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickMenu"",
                    ""type"": ""Button"",
                    ""id"": ""d71cd8a7-a963-408f-b2ee-0da20f307fa9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NormalAttack"",
                    ""type"": ""Button"",
                    ""id"": ""932ef013-7493-4321-a658-fe3185955c5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChargedAttack"",
                    ""type"": ""Button"",
                    ""id"": ""6ab6b254-8fff-4dc5-b91a-11a432e1c71d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenInventory"",
                    ""type"": ""Button"",
                    ""id"": ""f9077331-2ef2-4b8a-9612-873938908aa2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenMap"",
                    ""type"": ""Button"",
                    ""id"": ""effb5bf6-51ab-4ee8-8364-be681fd94c7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenEscMenu"",
                    ""type"": ""Button"",
                    ""id"": ""cd152140-1a5a-4e36-8d7b-970b5baa1f40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""Value"",
                    ""id"": ""0aa76714-4921-4184-b6f7-c002a9d01ba1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""46da3726-4a78-4df0-bf1f-44985d8fb87f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1463dfd3-0d85-4303-9828-326be33fa4a7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7e9e9384-cbf0-4195-bc5b-2065586626bb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""29698627-4c5d-4c98-ae34-e7dbc6f664b2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5a6ccb30-39e0-4127-aaa8-d2860a6f7904"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ef2c1489-4d26-44f0-94d4-85a0f2572f15"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c35175a-7db0-482c-b405-7ced2543874b"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50644746-ca3d-473e-8f38-fd85c1a2d8b5"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1b0a662-8007-4da1-ba7b-ad2f3358b9f7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28443db4-3f11-4f4d-80ff-7d036dd6ab5d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkillHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4ca08a1-54f9-41d5-9c4e-c36b6a6bcd04"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Burst"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1c16aaa-4cfa-48ae-ace4-5f5fb9337ff4"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbaefdf0-b0a4-4828-963d-9c2338037a0f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NormalAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""796ebec6-d741-49e6-aac0-0434e6b79b7e"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ba7a505-9521-4044-a592-cb43978e606a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChargedAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5095781a-a3e8-4109-aa09-d85c0175a321"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3c345a6-d04a-441e-a8ac-d5e218bfbca5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEscMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4b03e52-80b1-4d0a-ab91-1b3f5851ce15"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""76ac9b0d-9584-4ab1-b22a-ad3b5d536882"",
            ""actions"": [
                {
                    ""name"": ""CloseInventory"",
                    ""type"": ""Button"",
                    ""id"": ""84fff91e-5e30-465e-a2ee-51c01e73fedf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NextPage"",
                    ""type"": ""Button"",
                    ""id"": ""6cdd1716-79a8-4026-b7b6-072e3dc2668c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PreviousPage"",
                    ""type"": ""Button"",
                    ""id"": ""22f90fd6-d4b4-41bb-be16-c38d02fcb2fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""731599d6-e6de-4730-b60d-3881edc25ed6"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53b26bed-122f-4e38-8194-e5d907648d12"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17f7e3ce-d199-4270-87b7-0f5ca9122002"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""434a2485-09ce-4b98-a7f9-63fded44d822"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Map"",
            ""id"": ""0276d16e-07ff-4d2d-99f4-e8f7727346d1"",
            ""actions"": [
                {
                    ""name"": ""CloseMap"",
                    ""type"": ""Button"",
                    ""id"": ""51d3e19b-300c-407d-8359-e68c7f1d530e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d39575dd-6fc0-4b5e-893b-056b04dbd19e"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b801901-41c9-4542-846f-261489d133b2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""EscMenu"",
            ""id"": ""6feb0245-8c6f-45a3-99b0-5309e35de091"",
            ""actions"": [
                {
                    ""name"": ""CloseEscMenu"",
                    ""type"": ""Button"",
                    ""id"": ""8188cfaa-14f2-47ec-a8af-df989e5a0982"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1e856256-4cc2-4f47-b983-2aa26673a135"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseEscMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Dialogues"",
            ""id"": ""0d6c659c-46b0-47b7-b208-95997caca2c4"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Skill = m_Player.FindAction("Skill", throwIfNotFound: true);
        m_Player_SkillHold = m_Player.FindAction("SkillHold", throwIfNotFound: true);
        m_Player_Burst = m_Player.FindAction("Burst", throwIfNotFound: true);
        m_Player_QuickMenu = m_Player.FindAction("QuickMenu", throwIfNotFound: true);
        m_Player_NormalAttack = m_Player.FindAction("NormalAttack", throwIfNotFound: true);
        m_Player_ChargedAttack = m_Player.FindAction("ChargedAttack", throwIfNotFound: true);
        m_Player_OpenInventory = m_Player.FindAction("OpenInventory", throwIfNotFound: true);
        m_Player_OpenMap = m_Player.FindAction("OpenMap", throwIfNotFound: true);
        m_Player_OpenEscMenu = m_Player.FindAction("OpenEscMenu", throwIfNotFound: true);
        m_Player_ScrollWheel = m_Player.FindAction("ScrollWheel", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_CloseInventory = m_Inventory.FindAction("CloseInventory", throwIfNotFound: true);
        m_Inventory_NextPage = m_Inventory.FindAction("NextPage", throwIfNotFound: true);
        m_Inventory_PreviousPage = m_Inventory.FindAction("PreviousPage", throwIfNotFound: true);
        // Map
        m_Map = asset.FindActionMap("Map", throwIfNotFound: true);
        m_Map_CloseMap = m_Map.FindAction("CloseMap", throwIfNotFound: true);
        // EscMenu
        m_EscMenu = asset.FindActionMap("EscMenu", throwIfNotFound: true);
        m_EscMenu_CloseEscMenu = m_EscMenu.FindAction("CloseEscMenu", throwIfNotFound: true);
        // Dialogues
        m_Dialogues = asset.FindActionMap("Dialogues", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Skill;
    private readonly InputAction m_Player_SkillHold;
    private readonly InputAction m_Player_Burst;
    private readonly InputAction m_Player_QuickMenu;
    private readonly InputAction m_Player_NormalAttack;
    private readonly InputAction m_Player_ChargedAttack;
    private readonly InputAction m_Player_OpenInventory;
    private readonly InputAction m_Player_OpenMap;
    private readonly InputAction m_Player_OpenEscMenu;
    private readonly InputAction m_Player_ScrollWheel;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Skill => m_Wrapper.m_Player_Skill;
        public InputAction @SkillHold => m_Wrapper.m_Player_SkillHold;
        public InputAction @Burst => m_Wrapper.m_Player_Burst;
        public InputAction @QuickMenu => m_Wrapper.m_Player_QuickMenu;
        public InputAction @NormalAttack => m_Wrapper.m_Player_NormalAttack;
        public InputAction @ChargedAttack => m_Wrapper.m_Player_ChargedAttack;
        public InputAction @OpenInventory => m_Wrapper.m_Player_OpenInventory;
        public InputAction @OpenMap => m_Wrapper.m_Player_OpenMap;
        public InputAction @OpenEscMenu => m_Wrapper.m_Player_OpenEscMenu;
        public InputAction @ScrollWheel => m_Wrapper.m_Player_ScrollWheel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Skill.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill;
                @Skill.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill;
                @Skill.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill;
                @SkillHold.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkillHold;
                @SkillHold.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkillHold;
                @SkillHold.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkillHold;
                @Burst.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBurst;
                @Burst.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBurst;
                @Burst.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBurst;
                @QuickMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickMenu;
                @QuickMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickMenu;
                @QuickMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickMenu;
                @NormalAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAttack;
                @NormalAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAttack;
                @NormalAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAttack;
                @ChargedAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargedAttack;
                @ChargedAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargedAttack;
                @ChargedAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChargedAttack;
                @OpenInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenMap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMap;
                @OpenMap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMap;
                @OpenMap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMap;
                @OpenEscMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenEscMenu;
                @OpenEscMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenEscMenu;
                @OpenEscMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenEscMenu;
                @ScrollWheel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Skill.started += instance.OnSkill;
                @Skill.performed += instance.OnSkill;
                @Skill.canceled += instance.OnSkill;
                @SkillHold.started += instance.OnSkillHold;
                @SkillHold.performed += instance.OnSkillHold;
                @SkillHold.canceled += instance.OnSkillHold;
                @Burst.started += instance.OnBurst;
                @Burst.performed += instance.OnBurst;
                @Burst.canceled += instance.OnBurst;
                @QuickMenu.started += instance.OnQuickMenu;
                @QuickMenu.performed += instance.OnQuickMenu;
                @QuickMenu.canceled += instance.OnQuickMenu;
                @NormalAttack.started += instance.OnNormalAttack;
                @NormalAttack.performed += instance.OnNormalAttack;
                @NormalAttack.canceled += instance.OnNormalAttack;
                @ChargedAttack.started += instance.OnChargedAttack;
                @ChargedAttack.performed += instance.OnChargedAttack;
                @ChargedAttack.canceled += instance.OnChargedAttack;
                @OpenInventory.started += instance.OnOpenInventory;
                @OpenInventory.performed += instance.OnOpenInventory;
                @OpenInventory.canceled += instance.OnOpenInventory;
                @OpenMap.started += instance.OnOpenMap;
                @OpenMap.performed += instance.OnOpenMap;
                @OpenMap.canceled += instance.OnOpenMap;
                @OpenEscMenu.started += instance.OnOpenEscMenu;
                @OpenEscMenu.performed += instance.OnOpenEscMenu;
                @OpenEscMenu.canceled += instance.OnOpenEscMenu;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_CloseInventory;
    private readonly InputAction m_Inventory_NextPage;
    private readonly InputAction m_Inventory_PreviousPage;
    public struct InventoryActions
    {
        private @PlayerControls m_Wrapper;
        public InventoryActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CloseInventory => m_Wrapper.m_Inventory_CloseInventory;
        public InputAction @NextPage => m_Wrapper.m_Inventory_NextPage;
        public InputAction @PreviousPage => m_Wrapper.m_Inventory_PreviousPage;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @CloseInventory.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnCloseInventory;
                @CloseInventory.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnCloseInventory;
                @CloseInventory.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnCloseInventory;
                @NextPage.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnNextPage;
                @NextPage.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnNextPage;
                @NextPage.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnNextPage;
                @PreviousPage.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnPreviousPage;
                @PreviousPage.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnPreviousPage;
                @PreviousPage.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnPreviousPage;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CloseInventory.started += instance.OnCloseInventory;
                @CloseInventory.performed += instance.OnCloseInventory;
                @CloseInventory.canceled += instance.OnCloseInventory;
                @NextPage.started += instance.OnNextPage;
                @NextPage.performed += instance.OnNextPage;
                @NextPage.canceled += instance.OnNextPage;
                @PreviousPage.started += instance.OnPreviousPage;
                @PreviousPage.performed += instance.OnPreviousPage;
                @PreviousPage.canceled += instance.OnPreviousPage;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);

    // Map
    private readonly InputActionMap m_Map;
    private IMapActions m_MapActionsCallbackInterface;
    private readonly InputAction m_Map_CloseMap;
    public struct MapActions
    {
        private @PlayerControls m_Wrapper;
        public MapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CloseMap => m_Wrapper.m_Map_CloseMap;
        public InputActionMap Get() { return m_Wrapper.m_Map; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapActions set) { return set.Get(); }
        public void SetCallbacks(IMapActions instance)
        {
            if (m_Wrapper.m_MapActionsCallbackInterface != null)
            {
                @CloseMap.started -= m_Wrapper.m_MapActionsCallbackInterface.OnCloseMap;
                @CloseMap.performed -= m_Wrapper.m_MapActionsCallbackInterface.OnCloseMap;
                @CloseMap.canceled -= m_Wrapper.m_MapActionsCallbackInterface.OnCloseMap;
            }
            m_Wrapper.m_MapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CloseMap.started += instance.OnCloseMap;
                @CloseMap.performed += instance.OnCloseMap;
                @CloseMap.canceled += instance.OnCloseMap;
            }
        }
    }
    public MapActions @Map => new MapActions(this);

    // EscMenu
    private readonly InputActionMap m_EscMenu;
    private IEscMenuActions m_EscMenuActionsCallbackInterface;
    private readonly InputAction m_EscMenu_CloseEscMenu;
    public struct EscMenuActions
    {
        private @PlayerControls m_Wrapper;
        public EscMenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CloseEscMenu => m_Wrapper.m_EscMenu_CloseEscMenu;
        public InputActionMap Get() { return m_Wrapper.m_EscMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EscMenuActions set) { return set.Get(); }
        public void SetCallbacks(IEscMenuActions instance)
        {
            if (m_Wrapper.m_EscMenuActionsCallbackInterface != null)
            {
                @CloseEscMenu.started -= m_Wrapper.m_EscMenuActionsCallbackInterface.OnCloseEscMenu;
                @CloseEscMenu.performed -= m_Wrapper.m_EscMenuActionsCallbackInterface.OnCloseEscMenu;
                @CloseEscMenu.canceled -= m_Wrapper.m_EscMenuActionsCallbackInterface.OnCloseEscMenu;
            }
            m_Wrapper.m_EscMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CloseEscMenu.started += instance.OnCloseEscMenu;
                @CloseEscMenu.performed += instance.OnCloseEscMenu;
                @CloseEscMenu.canceled += instance.OnCloseEscMenu;
            }
        }
    }
    public EscMenuActions @EscMenu => new EscMenuActions(this);

    // Dialogues
    private readonly InputActionMap m_Dialogues;
    private IDialoguesActions m_DialoguesActionsCallbackInterface;
    public struct DialoguesActions
    {
        private @PlayerControls m_Wrapper;
        public DialoguesActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_Dialogues; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialoguesActions set) { return set.Get(); }
        public void SetCallbacks(IDialoguesActions instance)
        {
            if (m_Wrapper.m_DialoguesActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_DialoguesActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public DialoguesActions @Dialogues => new DialoguesActions(this);
    public interface IPlayerActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSkill(InputAction.CallbackContext context);
        void OnSkillHold(InputAction.CallbackContext context);
        void OnBurst(InputAction.CallbackContext context);
        void OnQuickMenu(InputAction.CallbackContext context);
        void OnNormalAttack(InputAction.CallbackContext context);
        void OnChargedAttack(InputAction.CallbackContext context);
        void OnOpenInventory(InputAction.CallbackContext context);
        void OnOpenMap(InputAction.CallbackContext context);
        void OnOpenEscMenu(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnCloseInventory(InputAction.CallbackContext context);
        void OnNextPage(InputAction.CallbackContext context);
        void OnPreviousPage(InputAction.CallbackContext context);
    }
    public interface IMapActions
    {
        void OnCloseMap(InputAction.CallbackContext context);
    }
    public interface IEscMenuActions
    {
        void OnCloseEscMenu(InputAction.CallbackContext context);
    }
    public interface IDialoguesActions
    {
    }
}
