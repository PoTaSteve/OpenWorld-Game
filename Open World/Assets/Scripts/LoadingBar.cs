using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public RectTransform[] Image = new RectTransform[5];
    [Range(0, 100)]
    public float value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // (100 / 6) * n -> 
        for (int i = 0; i < Image.Length; i++)
        {
            if (value >= (100 / 5) * (i + 1))
            {
                Image[i].anchoredPosition = Vector2.zero;
            }
            else if (value >= (100 / 5) * i && value < (100 / 5) * (i + 1))
            {
                float x = Mathf.Abs(value - (100 / 5) * (i + 1)) * 5;

                Image[i].anchoredPosition = new Vector2(-x, 0);
            }
            else
            {
                Image[i].anchoredPosition = new Vector2(-100, 0);
            }
        }
    }
}
