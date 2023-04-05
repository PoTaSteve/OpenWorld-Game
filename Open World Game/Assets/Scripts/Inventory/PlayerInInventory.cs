using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInInventory : MonoBehaviour
{
    public GameObject PlayerGFX;
    public float multiplier;

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RotatePlayer();
        }
    }

    public void RotatePlayer()
    {
        float deltaY = Input.GetAxis("Mouse X");

        PlayerGFX.transform.Rotate(new Vector3(0, deltaY, 0) * multiplier);
    }
}
