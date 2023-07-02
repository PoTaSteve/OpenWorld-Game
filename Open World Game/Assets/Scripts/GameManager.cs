using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum SceneIndex
{
    PERSISTENT_SCENE = 0,
    MAIN_MENU = 1,
    OPEN_WORLD = 2
}

public enum State
{
    MAIN_MENU,
    LOADING_SCREEN,
    OPEN_WORLD,
    QUESTS,
    INVENTORY,
    SKILLS,
    SYSTEM,
    MAP,
    DIALOGUE,
    SHOP
}

public enum InputMethod
{
    MOUSE_AND_KEYBOARD,
    CONTROLLER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject LoadingScreen;

    public GameObject player;

    public State currentState;
    public InputMethod inputMethod;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalLoadProgress;

    
    public ItemManager itemMan;
    
    public DialogueManager dialMan;
    
    public PlayerInputManager plInputMan;

    public PlayerInteractionManager plInteractMan;
    
    public PlayerStats plStats;

    public UIStateManager UIStateMan;
    
    public QuestsManager QuestsMan;

    public InventoryManager invMan;

    public SkillsManager skillsMan;

    public SystemManager systemMan;
    
    public ShopManager shopMan;
    
    public ConsoleManager consoleMan;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            
            if (currentState != State.OPEN_WORLD)
            {
                SceneManager.LoadSceneAsync((int)SceneIndex.MAIN_MENU, LoadSceneMode.Additive);

                player.SetActive(false);
                plInputMan.cam.gameObject.SetActive(false);
                DeactivateUI();
                LoadingScreen.SetActive(false);

                currentState = State.MAIN_MENU;
            }
            else
            {
                SetUI();
            }
        }
    }

    public void LoadOpenWorld()
    {
        LoadingScreen.SetActive(true);
        currentState = State.LOADING_SCREEN;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.MAIN_MENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.OPEN_WORLD, LoadSceneMode.Additive));

        StartCoroutine(GetLoadProgress());
    }

    public IEnumerator GetLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalLoadProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalLoadProgress += operation.progress;
                }

                totalLoadProgress = totalLoadProgress / scenesLoading.Count;

                LoadingScreen.GetComponentInChildren<LoadingBar>().progress = totalLoadProgress;

                yield return null;
            }
        }

        LoadingScreen.SetActive(false);

        SetUpOpenWorld();
    }

    public void SetUpOpenWorld()
    {
        currentState = State.OPEN_WORLD;

        player.SetActive(true);
        plInputMan.cam.gameObject.SetActive(true);

        SetItemParent();

        SetUI();

        //plStats.SetHealth();
    }

    public void DeactivateUI()
    {
        plInputMan.UIStateObject.SetActive(false);
        plInputMan.GameUIObj.SetActive(false);
        plInputMan.MapObj.SetActive(false);
        plInputMan.ConsoleObj.SetActive(false);
        plInputMan.DialogueObj.SetActive(false);
        plInputMan.TempConsoleDebugObj.SetActive(false);
        plInputMan.ShopObj.SetActive(false);
    }

    public void SetUI()
    {
        plInputMan.UIStateObject.SetActive(false);
        plInputMan.GameUIObj.SetActive(true);
        plInputMan.MapObj.SetActive(false);
        plInputMan.ConsoleObj.SetActive(false);
        plInputMan.DialogueObj.SetActive(false);
        plInputMan.TempConsoleDebugObj.SetActive(false);
        plInputMan.ShopObj.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetItemParent()
    {
        itemMan.itemsParent = GameObject.FindGameObjectWithTag("ItemsParent").transform;
    }
}
