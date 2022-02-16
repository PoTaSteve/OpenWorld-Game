using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject QuitMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);

        //Load data from save file
    }

    public void OpenQuitMenu()
    {
        QuitMenu.SetActive(true);
    }

    public void Cancel()
    {
        QuitMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
