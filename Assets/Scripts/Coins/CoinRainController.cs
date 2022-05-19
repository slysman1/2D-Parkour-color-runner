using UnityEngine;

public class CoinRainController : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;

    [SerializeField] float minCooldown;
    [SerializeField] float maxCooldown;
    float dropCooldown;
    float lastRainDrop;

    [SerializeField] float rainTime;
    float rainBegun;
    bool canRain;



    private void Update()
    {
        if (Time.time > rainBegun + rainTime)
        {
            canRain = false;
        }

        if (Time.time > lastRainDrop + dropCooldown && canRain)
        {
            dropCooldown = Random.Range(minCooldown, maxCooldown);
            lastRainDrop = Time.time;
            GenerateGoldenRainCoins();
        }


    }

    public void GenerateGoldenRainCoins()
    {
        int spawnPosIndex = -15;


        for (int i = 0; i < 40; i++)
        {
            spawnPosIndex++;
            GameObject coin = Instantiate(coinPrefab, new Vector3(transform.position.x + spawnPosIndex, transform.position.y, transform.position.z), coinPrefab.transform.rotation);
            DropController CoinDropCorrector = coin.GetComponent<DropController>();

            CoinDropCorrector.rb.gravityScale = Random.Range(5, 8); // so they all drop with diffrent gravity(speed);
            CoinDropCorrector.LaunchDropDirection(new Vector2(15, 15));
        }

        spawnPosIndex = -15;
    }


    public void StartRainCoin()
    {
        rainBegun = Time.time;
        canRain = true;
    }


}
