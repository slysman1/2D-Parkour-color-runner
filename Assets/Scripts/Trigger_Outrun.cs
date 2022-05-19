using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Outrun : MonoBehaviour
{
    bool canBeTriggerd = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && canBeTriggerd)
        {
            canBeTriggerd = false;
            AchievementManager.instance.SendAchiveUpdate(12, 1);
        }
    }
}
