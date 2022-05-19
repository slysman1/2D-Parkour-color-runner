using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_JumpOverTrap : MonoBehaviour
{
    bool canBeTriggerd = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && canBeTriggerd)
        {
            canBeTriggerd = false;
            AchievementManager.instance.SendAchiveUpdate(11, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_mario_hold_my_beer);
        }

    }
}
