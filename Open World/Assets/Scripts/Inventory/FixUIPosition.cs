using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixUIPosition : MonoBehaviour
{
    public string AxisFix;

    public RectTransform previousObj;
    public float offsetX;
    public float offsetY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePosition()
    {
        Vector2 pos;

        float x = 0f;
        float y = 0f;

        if (AxisFix == "X+")
        {
            x = previousObj.anchoredPosition.x + previousObj.sizeDelta.x + offsetX;
            y = previousObj.anchoredPosition.y + offsetY;
        }
        else if (AxisFix == "Y+")
        {
            x = previousObj.anchoredPosition.x + offsetX;
            y = previousObj.anchoredPosition.y + previousObj.sizeDelta.y + offsetY;
        }
        else if (AxisFix == "X-")
        {
            x = previousObj.anchoredPosition.x - previousObj.sizeDelta.x - offsetX;
            y = previousObj.anchoredPosition.y - offsetY;
        }
        else if (AxisFix == "Y-")
        {
            x = previousObj.anchoredPosition.x - offsetX;
            y = previousObj.anchoredPosition.y - previousObj.sizeDelta.y - offsetY;
        }
        else if (AxisFix == "X&Y")
        {
            x = previousObj.anchoredPosition.x + previousObj.sizeDelta.x + offsetX;
            y = previousObj.anchoredPosition.y + previousObj.sizeDelta.y + offsetY;
        }

        pos = new Vector2(x, y);

        gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
