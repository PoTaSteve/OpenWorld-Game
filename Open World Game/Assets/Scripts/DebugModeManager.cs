using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugModeManager : MonoBehaviour
{
    public GameObject DebugModeObj;
    public bool isInDebugMode;

    public Camera cam;
    public GameObject Player;
    public LayerMask GroundMask;

    public TextMeshProUGUI posTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDebugMode)
        {
            DebugPos();
        }
    }

    public void DebugPos()
    {
        Vector3 pos = Player.transform.position;

        // Crop text to first or second decimal digit

        posTxt.text = "Player => X / Y / Z : " + pos.x + " / " + pos.y + " / " + pos.z;
    }
}
