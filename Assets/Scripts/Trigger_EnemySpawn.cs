using UnityEngine;

public class Trigger_EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private GameObject allyToSpawn;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private int chanceToSpawn;
    [SerializeField] private int chanceToSpawnAlly;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool enemyCanSpawn = Random.Range(0, 100) < chanceToSpawn;

            if (enemyCanSpawn)
            {
                bool luckyRoll = Random.Range(0, 100) < chanceToSpawnAlly;

                if (luckyRoll)
                {
                    Instantiate(allyToSpawn, spawnLocation.position, transform.rotation);
                }
                else
                {
                    Instantiate(enemyToSpawn, spawnLocation.position, transform.rotation);
                }
                Time.timeScale = 0.6f;
                AudioManager.instance.PlaySFX(5);
            }
            Destroy(this.gameObject);

        }
    }

}
