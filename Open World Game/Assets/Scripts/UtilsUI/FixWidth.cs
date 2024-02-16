using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WidthFixType
{
    TextOnly, RectWithObj
}

public class FixWidth : MonoBehaviour
{
    public WidthFixType FixType;
    public int multiplier;

    public float[] offsets = new float[5];

    public RectTransform[] objects = new RectTransform[5];

    public void UpdateWidth()
    {
        if (FixType == WidthFixType.TextOnly)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<TextMeshProUGUI>().preferredWidth + offsets[0], gameObject.GetComponent<RectTransform>().sizeDelta.x);
        }

        if (FixType == WidthFixType.RectWithObj)
        {
            UpdateRectWithObj();
        }
    }

    public void UpdateRectWithObj()
    {
        float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;

        float x = 0;
        for (int i = 0; i < multiplier; i++)
        {
            x += offsets[i] + objects[i].sizeDelta.x;
        }
        x += offsets[multiplier];

        Vector2 dim = new Vector2(x, y);

        gameObject.GetComponent<RectTransform>().sizeDelta = dim;
    }
}
