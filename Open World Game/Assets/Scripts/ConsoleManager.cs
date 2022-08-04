using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public InventoryManager invMan;
    [SerializeField]
    private PlayerInputManager plInputMan;
    public GameObject Player;

    public TMP_InputField txtField;
    public Transform MsgParent;
    public GameObject infoObj;
    public GameObject TempConsoleDebugObj;

    public string cmd;
    public string[] funcParams;

    public void OnInputFieldDeselect()
    {
        txtField.Select();
    }

    public void Console()
    {
        string debug;

        string[] strs = txtField.text.Split(' ');
        cmd = strs[0];
        funcParams = new string[strs.Length - 1];

        for (int i = 1; i < strs.Length; i++)
        {
            funcParams[i - 1] = strs[i];
        }

        if (cmd == "/spawn")
        {
            debug = ConsoleSpawn(funcParams);
        }
        else if (cmd == "/give")
        {
            debug = ConsoleGive(funcParams);
        }
        else if (cmd == "/tp")
        {
            debug = ConsoleTp(funcParams);
        }
        else
        {            
            debug = "Error: Command not found";
        }
        
        DebugInfos(debug);

        txtField.text = "";

        TempConsoleDebugObj.SetActive(true);
        TempConsoleDebugObj.GetComponentInChildren<TextMeshProUGUI>().text = debug;
        StartCoroutine(DeactivateTempConsoleDebug(5.5f));
    }

    public IEnumerator DeactivateTempConsoleDebug(float time)
    {
        yield return new WaitForSeconds(time);
        TempConsoleDebugObj.SetActive(false);
    }

    public string ConsoleSpawn(string[] funcParams)
    {
        string debug = "No Item spawned";
        /*
        int count;
        string type;
        Vector3 pos;
        bool canSpawn;
        string objName;

        if (funcParams.Length == 5)
        {
            count = int.Parse(funcParams[0]);
            type = funcParams[1].Split(':')[0];
            objName = funcParams[1];
            pos = new Vector3(float.Parse(funcParams[2]), float.Parse(funcParams[3]), float.Parse(funcParams[4]));
            canSpawn = true;
        }
        else if (funcParams.Length == 4)
        {
            count = 1;
            type = funcParams[0].Split(':')[0];
            objName = funcParams[0];
            pos = new Vector3(float.Parse(funcParams[1]), float.Parse(funcParams[2]), float.Parse(funcParams[3]));
            canSpawn = true;
        }
        else
        {
            count = 0;
            type = "";
            objName = "";
            pos = Vector3.zero;
            canSpawn = false;

            debug = "Error: invalid number of parameters";
        }
        
        if (canSpawn)
        {
            if (type == "weapon")
            {
                foreach (WeaponScriptableObject obj in GameManager.Instance.itemMan.AllWeapons)
                {
                    if (obj.Console_Name == objName)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            GameObject newObj = Instantiate(obj, pos, Quaternion.identity, GameManager.Instance.itemMan.itemsParent);

                            debug = "Weapon spawned at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
                        }
                    }
                }
            }
            else if (type == "material")
            {
                foreach (GameObject obj in GameManager.Instance.itemMan.AllMaterials)
                {
                    if (obj.GetComponent<MaterialInfo>().MaterialSO.Console_Name == objName)
                    {
                        GameObject newObj = Instantiate(obj, pos, Quaternion.identity, GameManager.Instance.itemMan.itemsParent);

                        newObj.GetComponent<MaterialInfo>().count = count;

                        debug = "Material x" + count + " spawned at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
                    }
                }
            }
            else if (type == "ingredient")
            {
                foreach (GameObject obj in GameManager.Instance.itemMan.AllIngredients)
                {
                    if (obj.GetComponent<IngredientInfo>().IngredientSO.Console_Name == objName)
                    {
                        GameObject newObj = Instantiate(obj, pos, Quaternion.identity, GameManager.Instance.itemMan.itemsParent);

                        newObj.GetComponent<IngredientInfo>().count = count;

                        debug = "Ingredient x" + count + " spawned at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
                    }
                }
            }
            else if (type == "food")
            {
                foreach (GameObject obj in GameManager.Instance.itemMan.AllFood)
                {
                    if (obj.GetComponent<FoodInfo>().FoodSO.Console_Name == objName)
                    {
                        GameObject newObj = Instantiate(obj, pos, Quaternion.identity, GameManager.Instance.itemMan.itemsParent);

                        newObj.GetComponent<FoodInfo>().count = count;

                        debug = "Food x" + count + " spawned at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
                    }
                }
            }
            else
            {
                debug = "Error: invalid spawn type";
            }
        }
        */
        return debug;
    }

    public string ConsoleGive(string[] funcParams)
    {
        string debug;

        int count;
        string type;
        bool canGive;
        string itemName;

        // /give (count) itemType:item_name
        if (funcParams.Length == 1)
        {
            count = 1;
            type = funcParams[0].Split(':')[0];
            itemName = funcParams[0];

            canGive = true;
        }
        else if (funcParams.Length == 2)
        {
            count = int.Parse(funcParams[0]);
            type = funcParams[1].Split(':')[0];
            itemName = funcParams[1];

            canGive = true;
        }
        else
        {
            canGive = false;
            debug = "Error: invalid number of parameters";
        }

        if (canGive)
        {
            // give item to player
        }

        debug = "";

        return debug;
    }

    public string ConsoleTp(string[] funcParams)
    {
        string debug;

        if (funcParams.Length == 2)
        {
            Vector3 pos = new Vector3(float.Parse(funcParams[0]), 0, float.Parse(funcParams[1]));

            Player.GetComponent<CharacterController>().enabled = false;

            Player.transform.position = pos;

            Player.GetComponent<CharacterController>().enabled = true;

            debug = "Teleported at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        }
        else if (funcParams.Length == 3)
        {
            Vector3 pos = new Vector3(float.Parse(funcParams[0]), float.Parse(funcParams[1]), float.Parse(funcParams[2]));

            Player.GetComponent<CharacterController>().enabled = false;

            Player.transform.position = pos;

            Player.GetComponent<CharacterController>().enabled = true;

            debug = "Teleported at: (" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        }
        else
        {
            debug = "Tp error: Wrong number of parameters";
        }

        return debug;
    }

    public void DebugInfos(string info)
    {
        infoObj = Instantiate(infoObj, MsgParent);

        infoObj.GetComponent<TextMeshProUGUI>().text = info;

        infoObj.GetComponent<FixHeight>().UpdateHeight();
    }
}
