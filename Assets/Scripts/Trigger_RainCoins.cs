using UnityEngine;

public class Trigger_RainCoins : MonoBehaviour
{
    [SerializeField] float chanceToSpawn;

    CoinRainController coinRainController;
    // Start is called before the first frame update
    void Start()
    {
        coinRainController = GameObject.Find("CoinRainController").GetComponent<CoinRainController>();

        bool luckyRollToSpawn = Random.Range(1, 100) <= chanceToSpawn;
        if (!luckyRollToSpawn)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AchievementManager.instance.SendAchiveUpdate(14, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_its_raining_cats_and_dogs);


            coinRainController.StartRainCoin();
            AudioManager.instance.PlaySFX(6);
            Destroy(this.gameObject);
        }
    }

}
