using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
{
    public PlayerInputManager plInputMan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAttackDetection()
    {
        plInputMan.canPrepareNextAttack = true;
    }

    public void ResetAttackCount()
    {
        plInputMan.attackCount = 0;
        plInputMan.canPrepareNextAttack = true;
    }

    public void Attack()
    {
        plInputMan.CheckAndDamageEnemy();
    }
}
