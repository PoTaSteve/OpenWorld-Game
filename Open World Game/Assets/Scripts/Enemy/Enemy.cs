using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyScrObj enemyScrObj;
    public float detectionRange;
    public float minPlayerDistance;
    public LayerMask PlayerLayer;
    public Animator anim;

    public bool isAttacking;

    private float smoothVel;
    private float turnSmoothTime = 0.1f;

    public Transform AttackOrigin;
    public Vector3 AttackBoxDimension;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, PlayerLayer) && !isAttacking)
        {
            if (Vector3.Distance(GameManager.Instance.player.transform.position, transform.position) > minPlayerDistance)
            {
                SetRotation();

                transform.position += transform.forward * enemyScrObj.speed * Time.deltaTime;

                anim.SetInteger("MoveSpeed", 1);
            }
            else
            {
                anim.SetTrigger("Attack");

                isAttacking = true;
            }
        }
        else
        {
            anim.SetInteger("MoveSpeed", 0);
        }
    }

    public void SetRotation()
    {
        Vector3 plPos = GameManager.Instance.player.transform.position;

        float targetAngle = Mathf.Atan2(plPos.x - transform.position.x, plPos.z - transform.position.z) * Mathf.Rad2Deg;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void CheckForPlayerToDamage()
    {
        GameObject player = Physics.OverlapBox(AttackOrigin.position, AttackBoxDimension, transform.rotation, PlayerLayer)[0].gameObject;

        if (player != null)
        {
            player.GetComponent<PlayerStats>().GetDamage(enemyScrObj.attack);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
