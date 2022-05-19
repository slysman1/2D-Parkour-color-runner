using UnityEngine;

public class Trigger_DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            Player player = collision.GetComponent<Player>();

            if (!player.isDead || player.canDie)
            {
                player.KillPlayer();
                player.rb.gravityScale = 0;
                player.rb.velocity = new Vector2(0, 0);
            }
        }
    }
}
