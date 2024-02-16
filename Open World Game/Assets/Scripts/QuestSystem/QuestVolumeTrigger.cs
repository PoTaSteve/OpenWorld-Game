using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestVolumeTrigger : MonoBehaviour
{
    public string questID;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            GameManager.Instance.QuestsMan.CheckQuestProgress(questID);

            Destroy(gameObject);
        }
    }
}
