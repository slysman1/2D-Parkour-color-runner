using UnityEngine;

public class Trigger_SlowMo : MonoBehaviour
{
    [SerializeField] int chanceToWork = 35;
    [SerializeField] int sfxToPlay;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool luckyRoll = Random.Range(1, 100) < chanceToWork;
            if (luckyRoll)
            {
                AchievementManager.instance.SendAchiveUpdate(10, 1);
                AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_trinity_help);

                Time.timeScale = 0.4f;

                if (sfxToPlay != 0)
                {
                    AudioManager.instance.PlaySFX(sfxToPlay);
                }

                // set timeScale back to 1 on landing in Player's script
            }
        }
    }
}
