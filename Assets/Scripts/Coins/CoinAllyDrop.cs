using UnityEngine;

public class CoinAllyDrop : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector2 dropDirection;

    [SerializeField] private float dropCooldown;
    [SerializeField] private float lastDropTime;

    Player player;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastDropTime + dropCooldown)
        {
            lastDropTime = Time.time;

            DropCoin();
        }
    }

    private void DropCoin()
    {
        GameObject coin = Instantiate(objectToSpawn, transform.position, transform.rotation);
        DropController coinScript = coin.GetComponent<DropController>();

        coinScript.LaunchDropDirection(dropDirection);
    }
}
