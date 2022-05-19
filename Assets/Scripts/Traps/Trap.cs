using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float chanceToSpawn;

    private Player player;
    [SerializeField] private bool achiveTriggerOn;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GetComponentInChildren<BoxCollider2D>().gameObject.SetActive(achiveTriggerOn);

        if (Random.Range(1, 100) > chanceToSpawn)
        {
            Destroy(this.gameObject);
        }   // chance to spawn the trap
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!player.unlockedSkillNumber[3])
            {
                    if (player.moveSpeed >= player.moveSpeedNeededToSurive)
                    {
                        player.Knockback();
                    }
                    else
                    {
                        player.KillPlayer();
                    }
            }

        }
    }
}
