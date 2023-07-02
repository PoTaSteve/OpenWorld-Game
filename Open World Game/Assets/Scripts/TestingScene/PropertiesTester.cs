using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesTester : MonoBehaviour
{
    public int myVal;

    private int myVar = 0;
    public int MyVar
    {
        get 
        { 
            return myVar; 
        }
        set
        {
            if (myVar == value) 
                return;
            myVar = value;
            if (OnVariableChange != null)
                OnVariableChange(myVar);
        }
    }

    public delegate void OnVariableChangeDelegate(int newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    // Start is called before the first frame update
    void Start()
    {
        OnVariableChange += VariableChangeHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MyVar = myVal;
        }
    }

    private void VariableChangeHandler(int newVal)
    {
        Debug.Log("Changed value to: " + newVal);
    }
}
