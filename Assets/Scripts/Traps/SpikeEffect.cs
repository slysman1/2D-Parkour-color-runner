using UnityEngine;

public class SpikeEffect : MonoBehaviour
{
    private Player player;
    [SerializeField] LayerMask whatIsGround;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.unlockedSkillNumber[3])
        {
            if (collision.tag == "Player")
            {
                player.SpeedReset();
            }
        }
            if (collision.tag == "Ground")
            {
                AudioManager.instance.PlaySFX(8);
            }
    }

}
