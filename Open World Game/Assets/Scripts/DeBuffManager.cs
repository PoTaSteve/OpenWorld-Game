using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeBuffManager : MonoBehaviour
{
    public GameObject DeBuffPrefab;
    public GameObject MoreDeBuffPrefab;
    public Transform DeBuffParent;

    public GameObject DeBuffPrefabForWindow;
    public Transform ActiveDeBuffParent;

    public GameObject ActiveDeBuffWindow;

    public List<DeBuff> activeDeBuffs = new List<DeBuff>();

    public int DeBuffsNum;

    //private const float yOffset = 44f;
    private const float xOffset = 39f;
    private const float halfXOffset = xOffset / 2f;
    private const int maxRowBuff = 7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddDeBuff();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RemoveDeBuff();
        }
    }

    public void AddDeBuff()
    {        
        DeBuffsNum++;

        GameObject newDeBuff = Instantiate(DeBuffPrefabForWindow, ActiveDeBuffParent);
        activeDeBuffs.Add(newDeBuff.GetComponent<DeBuff>());
        ActiveDeBuffParent.GetComponent<RectTransform>().sizeDelta = new Vector3(1920f, 150 * Mathf.Ceil(DeBuffsNum / 3f) + 50);

        FixDeBuffPos();
    }

    public void RemoveDeBuff()
    {
        DeBuffsNum--;

        FixDeBuffPos();
    }

    public void FixDeBuffPos()
    {
        int fixedDeBuffs = 0;

        if (DeBuffsNum <= 0)
        {
            return;
        }

        if (DeBuffsNum <= 7)
        {
            GameObject newDeBuff = Instantiate(DeBuffPrefab, DeBuffParent);
            newDeBuff.name = "NewDeBuff" + (DeBuffsNum + 1).ToString();

            newDeBuff.GetComponentInChildren<Button>().onClick.AddListener(delegate { GameManager.Instance.plInMan.OpenActiveBuffsWindow(); });

            if ((DeBuffsNum) % 2 == 0)
            {
                int leftBorder = DeBuffsNum / 2 - 1;
                int rightBorder = DeBuffsNum / 2;
                
                DeBuffParent.GetChild(leftBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(-halfXOffset, 22f);
                DeBuffParent.GetChild(rightBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(halfXOffset, 0f);
                
                fixedDeBuffs = 2;

                if (fixedDeBuffs < DeBuffsNum)
                {
                    leftBorder--;
                    int iters = 1;

                    while (leftBorder >= 0)
                    {
                        int y;

                        if (iters % 2 == 0)
                        {
                            y = 22;
                        }
                        else
                        {
                            y = 0;
                        }
                        DeBuffParent.GetChild(leftBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(-halfXOffset - (xOffset * iters), y);
                        
                        leftBorder--;
                        iters++;
                    }

                    rightBorder++;
                    iters = 1;

                    while (rightBorder < DeBuffsNum)
                    {
                        int y;

                        if (iters % 2 == 0)
                        {
                            y = 0;
                        }
                        else
                        {
                            y = 22;
                        }
                        DeBuffParent.GetChild(rightBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(halfXOffset + (xOffset * iters), y);
                        
                        rightBorder++;
                        iters++;
                    }
                }
            }
            else
            {
                int leftBorder = DeBuffsNum / 2;
                
                DeBuffParent.GetChild(leftBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                fixedDeBuffs = 1;

                if (fixedDeBuffs < DeBuffsNum)
                {
                    int rightBorder = leftBorder;

                    leftBorder--;
                    rightBorder++;
                    int iters = 1;

                    while (leftBorder >= 0)
                    {
                        int y;

                        if (iters % 2 == 0)
                        {
                            y = 0;
                        }
                        else
                        {
                            y = 22;
                        }

                        DeBuffParent.GetChild(leftBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(-xOffset * iters, y);
                        DeBuffParent.GetChild(rightBorder).GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset * iters, y);
                        
                        leftBorder--;
                        rightBorder++;
                        iters++;
                    }
                }
            }
        }
        else if (DeBuffsNum == 8)
        {
            Vector2 pos = DeBuffParent.GetChild(6).GetComponent<RectTransform>().anchoredPosition;
            Destroy(DeBuffParent.GetChild(6).gameObject);

            GameObject moreBuffs = Instantiate(MoreDeBuffPrefab, DeBuffParent);
            moreBuffs.name = "MoreBuffs";
            moreBuffs.GetComponent<RectTransform>().anchoredPosition = pos;
            moreBuffs.GetComponentInChildren<TextMeshProUGUI>().text = "+2";
            moreBuffs.GetComponentInChildren<Button>().onClick.AddListener(delegate { GameManager.Instance.plInMan.OpenActiveBuffsWindow(); });
        }
        else
        {
            DeBuffParent.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = "+" + (DeBuffsNum - 6);
        }
    }
}
