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
    BUFFS_WINDOW,
    INVENTORY,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject LoadingScreen;

    public GameObject player;

    public State currentState;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalLoadProgress;

    
    public InventoryManager invMan;
    
    public ItemManager itemMan;
    
    public DialogueManager dialMan;
    
    public PlayerInputManager plInMan;
    
    public PlayerStats plStats;
    
    public ShopManager shopMan;
    
    public DeBuffManager debuffMan;
    
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
            
            SceneManager.LoadSceneAsync((int)SceneIndex.MAIN_MENU, LoadSceneMode.Additive);

            player.SetActive(false);
            plInMan.orbitCam.gameObject.SetActive(false);
            DeactivateUI();
            LoadingScreen.SetActive(false);

            currentState = State.MAIN_MENU;
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
        plInMan.orbitCam.gameObject.SetActive(true);

        SetUpManagersInOpenWorld();

        SetUI();

        //plStats.SetHealth();
    }

    public void DeactivateUI()
    {
        plInMan.GameUIObj.SetActive(false);
        plInMan.InventoryObj.SetActive(false);
        plInMan.MapObj.SetActive(false);
        plInMan.EscMenuObj.SetActive(false);
        plInMan.ConsoleObj.SetActive(false);
        plInMan.WeaponEnhanceObj.SetActive(false);
        plInMan.DialogueObj.SetActive(false);
        plInMan.TempConsoleDebugObj.SetActive(false);
        plInMan.ShopObj.SetActive(false);
        plInMan.ActiveBuffsWindow.SetActive(false);
    }

    public void SetUI()
    {
        plInMan.GameUIObj.SetActive(true);
        plInMan.InventoryObj.SetActive(false);
        plInMan.MapObj.SetActive(false);
        plInMan.EscMenuObj.SetActive(false);
        plInMan.ConsoleObj.SetActive(false);
        plInMan.WeaponEnhanceObj.SetActive(false);
        plInMan.DialogueObj.SetActive(false);
        plInMan.TempConsoleDebugObj.SetActive(false);
        plInMan.ShopObj.SetActive(false);
        plInMan.ActiveBuffsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetUpManagersInOpenWorld()
    {
        //#region DeBuff Manager
        //debuffMan.DeBuffParent = GameObject.FindGameObjectWithTag("DeBuffParent").transform;
        //debuffMan.ActiveDeBuffWindow = GameObject.FindGameObjectWithTag("DeBuffWindow");
        //debuffMan.ActiveDeBuffParent = debuffMan.ActiveDeBuffWindow.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        //#endregion

        //#region Shop Manager
        //GameObject shopWin = GameObject.FindGameObjectWithTag("ShopWindow");
        //shopMan.buyPopUp = shopWin.transform.GetChild(4).gameObject;
        //shopMan.ItemDetails = shopWin.transform.GetChild(2).GetChild(0).gameObject;
        //shopMan.ProductContent = shopWin.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        //#endregion

        //#region Dialogue Manager
        //GameObject dialogueWin = GameObject.FindGameObjectWithTag("DialogueWindow");
        //dialMan.DialogueWindow = dialogueWin;
        //dialMan.dialogue = dialogueWin.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //dialMan.speaker = dialogueWin.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        //dialMan.choiceParent = dialogueWin.transform.GetChild(4).GetComponent<RectTransform>();
        //#endregion

        //#region Console Manager
        //consoleMan.invMan = invMan;
        //GameObject console = GameObject.FindGameObjectWithTag("ConsoleWindow");
        //consoleMan.txtField = console.transform.GetChild(1).GetComponent<TMP_InputField>();
        //consoleMan.MsgParent = console.transform.GetChild(2).GetChild(0);
        //#endregion

        #region Item Manager
        itemMan.itemsParent = GameObject.FindGameObjectWithTag("ItemsParent").transform;
        #endregion
    }
}
