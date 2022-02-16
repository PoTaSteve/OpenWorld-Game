using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FixWidth : MonoBehaviour
{
    public string FixType;
    public int multiplier;

    public float[] offsets = new float[5];

    public RectTransform[] objects = new RectTransform[5];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FixType == "Only Text")
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<TextMeshProUGUI>().preferredWidth, gameObject.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (FixType == "Rect With Obj")
        {
            UpdateRectWithText();
        }
    }

    public void UpdateRectWithText()
    {
        float x = 0;
        for (int i = 0; i < multiplier; i++)
        {
            x += offsets[i] + objects[i].sizeDelta.x;
        }
        x += offsets[multiplier];

        float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        
        Vector2 dim = new Vector2(x, y);

        gameObject.GetComponent<RectTransform>().sizeDelta = dim;
    }
}
