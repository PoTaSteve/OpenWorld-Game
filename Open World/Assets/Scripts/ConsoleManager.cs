using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public GameObject Console;
    public TMP_InputField inputField;
    public GameObject ChoicePrefab;
    public GameObject ChoicesObj;
    public Transform ChoicesContent;

    public GameObject CmdFormat;
    public TextMeshProUGUI CmdFormatText;

    public string currText;
    public int txtLen;

    public bool hasFoundCmd;
    public string currCmd;
    public int cmdIndex;

    private bool hasCmdBeenExecuted;

    public int instantiatedCmds;
    public int currentSelectedIndex = 0;

    public float maxCmdWidth = 0;

    public TextMeshProUGUI selectedText;

    public List<string> History = new List<string>();

    [TextArea(1, 3)]
    public Dictionary<string, string> CmdToFormat = new Dictionary<string, string>()
    {
        {"/spawn ", "<ammount> <entity> <position>"},
        {"/quit ", ""},
        {"/give ", "<ammount> <item> <position>"},
        {"/print ", "<msg>"}
    };

    // Start is called before the first frame update
    void Start()
    {
        hasFoundCmd = false;
        hasCmdBeenExecuted = false;
        instantiatedCmds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !Console.activeSelf)
        {
            Console.SetActive(true);

            foreach (Transform t in ChoicesContent.transform)
            {
                Destroy(t.gameObject);
            }

            inputField.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Console.activeSelf)
        {
            inputField.text = "";
            inputField.DeactivateInputField();

            Console.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Return) && Console.activeSelf)
        {
            if (inputField.text.StartsWith("/"))
            {
                ExecuteCommand();
            }
            else
            {
                DebugInConsole(currText);
            }            

            inputField.text = "";
            inputField.DeactivateInputField();

            Console.SetActive(false);
        }

        if (instantiatedCmds > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputField.caretPosition = currText.Length;

                if (currentSelectedIndex > 0)
                {
                    ChoicesContent.transform.GetChild(currentSelectedIndex).GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

                    currentSelectedIndex--;

                    selectedText = ChoicesContent.transform.GetChild(currentSelectedIndex).GetComponentInChildren<TextMeshProUGUI>();

                    selectedText.color = Color.yellow;
                }                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentSelectedIndex < instantiatedCmds - 1)
                {
                    ChoicesContent.transform.GetChild(currentSelectedIndex).GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

                    currentSelectedIndex++;

                    selectedText = ChoicesContent.transform.GetChild(currentSelectedIndex).GetComponentInChildren<TextMeshProUGUI>();

                    selectedText.color = Color.yellow;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                inputField.text = selectedText.text;

                inputField.caretPosition = inputField.text.Length;
            }
        }
    }

    public void UpdateSearch()
    {
        currText = inputField.text;
        txtLen = currText.Length;

        if (!hasFoundCmd)
        {
            UpdateChoices();         
        }
        else
        {
            if (!currText.Contains(" "))
            {
                hasFoundCmd = false;
                currCmd = "";
                CmdFormat.SetActive(false);

                UpdateChoices();
            }
        }
    }

    public void UpdateChoices()
    {
        currentSelectedIndex = 0;

        if (currText != "")
        {
            foreach (Transform t in ChoicesContent)
            {
                Destroy(t.gameObject);
            }

            instantiatedCmds = 0;

            int i = 0;

            foreach (string s in CmdToFormat.Keys)
            {
                if (s.StartsWith(currText))
                {
                    GameObject Choice = Instantiate(ChoicePrefab, ChoicesContent);
                    instantiatedCmds++;

                    Choice.GetComponentInChildren<TextMeshProUGUI>().text = s.Remove(s.Length - 1);

                    float currWidth = Choice.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

                    if (currWidth > maxCmdWidth)
                    {
                        maxCmdWidth = currWidth;
                    }

                    if (s == currText)
                    {
                        cmdIndex = i;
                        currCmd = currText;
                        currCmd = currCmd.Remove(currCmd.Length - 1);
                        instantiatedCmds = 0;
                        hasFoundCmd = true;

                        foreach (Transform t in ChoicesContent)
                        {
                            Destroy(t.gameObject);
                        }

                        //Instantiate format for current command
                        CmdFormat.SetActive(true);

                        float posX = inputField.preferredWidth + 40;
                        float posY = 100f;

                        Vector2 pos = new Vector2(posX, posY);

                        CmdFormat.GetComponent<RectTransform>().anchoredPosition = pos;

                        // set dimensions
                        if (CmdToFormat.TryGetValue(s, out string text))
                        {
                            if (text != "")
                            {
                                CmdFormatText.text = text;

                                float dimX = CmdFormatText.preferredWidth;
                                float dimY = 100f;

                                Vector2 dim = new Vector2(dimX, dimY);

                                CmdFormat.GetComponent<RectTransform>().sizeDelta = dim;
                            }
                            else
                            {
                                CmdFormat.SetActive(false);
                            }
                        }                        
                    }
                }

                i++;
            }

            if (instantiatedCmds > 0)
            {
                StartCoroutine(TurnYellow());
            }

            ChoicesObj.GetComponent<RectTransform>().sizeDelta = new Vector2(maxCmdWidth + 40, ChoicesObj.GetComponent<RectTransform>().sizeDelta.y);

            if (instantiatedCmds > 4)
            {
                Vector2 dims = new Vector2(ChoicesContent.GetComponent<RectTransform>().sizeDelta.x, 100 * instantiatedCmds);

                ChoicesContent.GetComponent<RectTransform>().sizeDelta = dims;
            }
        }
        else
        {
            foreach (Transform t in ChoicesContent)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public void ExecuteCommand()
    {
        if (hasFoundCmd)
        {
            switch (currCmd)
            {
                case "/print":
                    hasCmdBeenExecuted = PrintCmd();
                    break;
                case "/quit":
                    hasCmdBeenExecuted = QuitCmd();
                    break;
                case "/spawn":
                    hasCmdBeenExecuted = SpawnCmd();
                    break;
                case "/give":
                    hasCmdBeenExecuted = GiveCmd();
                    break;
                default:
                    break;
            }
        }
        else
        {
            DebugInConsole("Command not found");
        }

        if (!hasCmdBeenExecuted)
        {
            DebugInConsole("Error during command execution");
        }
    }

    public bool PrintCmd()
    {
        bool res = true;

        string[] cmdParams = currText.Split(' ');

        Debug.Log(cmdParams[1]);

        return res;
    }

    public bool QuitCmd()
    {
        bool res = true;



        return res;
    }

    public bool SpawnCmd()
    {
        bool res = true;



        return res;
    }

    public bool GiveCmd()
    {
        bool res = true;



        return res;
    }

    public void DebugInConsole(string msg)
    {
        Debug.Log(msg);
    }

    public IEnumerator TurnYellow()
    {
        yield return new WaitForEndOfFrame();

        selectedText = ChoicesContent.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        selectedText.color = Color.yellow;
    }
}
