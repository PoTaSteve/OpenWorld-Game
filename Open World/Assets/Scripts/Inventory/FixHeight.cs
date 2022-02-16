using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FixHeight : MonoBehaviour
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
        
    }

    public void UpdateHeight()
    {
        if (FixType == "Only Text")
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, gameObject.GetComponent<TextMeshProUGUI>().preferredHeight);
        }

        if (FixType == "Rect With Obj")
        {
            UpdaterectWithObj();
        }
    }

    public void UpdaterectWithObj()
    {
        float x = gameObject.GetComponent<RectTransform>().sizeDelta.x;

        float y = 0;
        for (int i = 0; i < multiplier; i++)
        {
            y += offsets[i] + objects[i].sizeDelta.y;
        }
        y += offsets[multiplier];

        Vector2 dim = new Vector2(x, y);

        gameObject.GetComponent<RectTransform>().sizeDelta = dim;
    }
}
