using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int spawnVariant;
    [SerializeField] private int coinSpawnChancePercent;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        int amountToSpawn = Random.Range(3, 10);

        for (int i = 0; i < amountToSpawn; i++)
        {
            spawnVariant++;
            if (Random.Range(1, 100) <= coinSpawnChancePercent)
            {
                Instantiate(coinPrefab, new Vector3(transform.position.x + spawnVariant, transform.position.y, transform.position.z), transform.rotation, transform.parent);
            }

        }
    }
}
