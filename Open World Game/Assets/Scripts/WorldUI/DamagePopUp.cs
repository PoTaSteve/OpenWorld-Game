using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
