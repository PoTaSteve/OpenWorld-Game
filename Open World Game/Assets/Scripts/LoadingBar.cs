using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [Range(0, 1)]
    public float progress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        int icons = transform.childCount;
        float delta = 1f / icons;
        float dimX = GetComponent<GridLayoutGroup>().cellSize.x;

        int fullIcons = (int)(progress * icons);

        for (int i = 0; i < icons; i++)
        {
            if (i < fullIcons)
            {
                transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else if (i == fullIcons)
            {
                float x = (((progress - (fullIcons * delta)) / delta) * dimX) - dimX;

                transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
            else
            {
                transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-150f, 0);
            }
        }
    }
}
