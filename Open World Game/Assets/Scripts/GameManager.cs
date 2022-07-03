using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    PERSISTENT_SCENE = 0,
    MAIN_MENU = 1,
    OPEN_WORLD = 2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject LoadingScreen;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalLoadProgress;

    [HideInInspector]
    public InventoryManager invMan;
    [HideInInspector]
    public ItemManager itemMan;
    [HideInInspector]
    public DialogueManager dialMan;
    [HideInInspector]
    public PlayerInputManager plInMan;
    [HideInInspector]
    public PlayerStats plStats;
    [HideInInspector]
    public ShopManager shopMan;

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
        }
    }

    public void SetUpManagers()
    {
        GameObject.FindGameObjectWithTag("InventoryManager").TryGetComponent(out invMan);
        GameObject.FindGameObjectWithTag("ItemManager").TryGetComponent(out itemMan);
        GameObject.FindGameObjectWithTag("DialogueManager").TryGetComponent(out dialMan);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out plInMan);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out plStats);
        GameObject.FindGameObjectWithTag("ShopManager").TryGetComponent(out shopMan);
    }

    public void LoadOpenWorld()
    {
        LoadingScreen.SetActive(true);
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

        SetUpManagers();

        SetUI();

        plStats.SetHealth();
    }

    public void SetUI()
    {
        plInMan.GameUIObj.SetActive(true);
        plInMan.InventoryObj.SetActive(false);
        plInMan.MapObj.SetActive(false);
        plInMan.EscMenuObj.SetActive(false);
        plInMan.ConsoleObj.SetActive(false);
        plInMan.DebugModeObj.SetActive(false);
        plInMan.WeaponEnhanceObj.SetActive(false);
        plInMan.DialogueObj.SetActive(false);
        plInMan.TempConsoleDebugObj.SetActive(false);
        plInMan.ShopObj.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
