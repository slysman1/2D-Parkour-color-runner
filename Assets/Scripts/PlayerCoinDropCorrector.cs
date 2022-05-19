using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoinDropCorrector : MonoBehaviour
{
    Player player;
    bool isGrounded;
    public Rigidbody2D rb;
    public BoxCollider2D cd;
    public float moveSpeed = 5;
    [SerializeField] LayerMask whatIsGround;

    public bool canGoBackToPlayer;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, whatIsGround);

        if (isGrounded)
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (Vector2.Distance(player.transform.position, transform.position) > 10)
        {
            canGoBackToPlayer = true;
        }

        if (canGoBackToPlayer)
        {
            cd.isTrigger = true;
            rb.gravityScale = 0;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void LaunchDropDirection(Vector2 launchDirection)
    {
        rb.velocity = launchDirection;
    }
}