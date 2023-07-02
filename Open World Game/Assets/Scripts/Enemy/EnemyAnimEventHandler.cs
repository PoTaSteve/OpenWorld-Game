using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventHandler : MonoBehaviour
{
    public Enemy enemy;

    public void EndAttackSequence()
    {
        StartCoroutine(EndAttackSequenceCor());
    }

    public IEnumerator EndAttackSequenceCor()
    {
        enemy.anim.SetInteger("MoveSpeed", 0);

        yield return new WaitForSeconds(1);

        enemy.isAttacking = false;
    }

    public void CheckForPlayerToDamage()
    {
        enemy.CheckForPlayerToDamage();
    }
}
