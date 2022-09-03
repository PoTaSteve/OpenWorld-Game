using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlowerMat : MonoBehaviour
{
    [SerializeField]
    private Color[] NormalPetals = new Color[3];
    [SerializeField]
    private Color[] RedPetals = new Color[3];

    private SkinnedMeshRenderer meshRend;

    public float LerpTime;
    private bool isLerpingToRed;
    private bool isLerpingToWhite;
    private float LerpTimer;

    // Start is called before the first frame update
    void Start()
    {
        meshRend = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        meshRend.material.SetColor("_BaseColor", NormalPetals[0]);
        meshRend.material.SetColor("_1st_ShadeColor", NormalPetals[1]);
        meshRend.material.SetColor("_2nd_ShadeColor", NormalPetals[2]);

        float rand = Random.Range(0f, 1f);

        gameObject.GetComponent<Animator>().Play("Idle", 0, rand);
    }

    private void Update()
    {
        if (isLerpingToRed)
        {
            LerpTimer += Time.deltaTime / LerpTime;

            if (LerpTimer >= 1)
            {
                isLerpingToRed = false;
                LerpTimer = 1f;
            }

            meshRend.material.SetColor("_BaseColor", Color.Lerp(NormalPetals[0], RedPetals[0], LerpTimer));
            meshRend.material.SetColor("_1st_ShadeColor", Color.Lerp(NormalPetals[1], RedPetals[1], LerpTimer));
            meshRend.material.SetColor("_2nd_ShadeColor", Color.Lerp(NormalPetals[2], RedPetals[2], LerpTimer));

            if (!isLerpingToRed)
            {
                LerpTimer = 0f;
            }
        }
        else if (isLerpingToWhite)
        {
            LerpTimer += Time.deltaTime / LerpTime;

            if (LerpTimer >= 1)
            {
                isLerpingToWhite = false;
                LerpTimer = 1f;
            }

            meshRend.material.SetColor("_BaseColor", Color.Lerp(RedPetals[0], NormalPetals[0], LerpTimer));
            meshRend.material.SetColor("_1st_ShadeColor", Color.Lerp(RedPetals[1], NormalPetals[0], LerpTimer));
            meshRend.material.SetColor("_2nd_ShadeColor", Color.Lerp(RedPetals[2], NormalPetals[0], LerpTimer));

            if (!isLerpingToWhite)
            {
                LerpTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isLerpingToRed = true;
            isLerpingToWhite = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isLerpingToWhite = true;
            isLerpingToRed = false;
        }
    }
}
