using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D cd;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cd.offset = new Vector2(0, 0);
            cd.size = sr.size / 2;
            rb.gravityScale = 15;
            cd.isTrigger = false;

        }


    }
}