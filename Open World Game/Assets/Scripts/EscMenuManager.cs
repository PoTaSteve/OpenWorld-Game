using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuManager : MonoBehaviour
{
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalLoadProgress;

    public void QuitGame()
    {
        GameManager.Instance.LoadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.OPEN_WORLD));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.MAIN_MENU, LoadSceneMode.Additive));

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

                GameManager.Instance.LoadingScreen.GetComponentInChildren<LoadingBar>().progress = totalLoadProgress;

                yield return null;
            }
        }

        GameManager.Instance.LoadingScreen.SetActive(false);
    }
}
