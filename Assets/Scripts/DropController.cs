using UnityEngine;

public class DropController : MonoBehaviour
{
    bool isGrounded;
    public Rigidbody2D rb;
    [SerializeField] LayerMask whatIsGround;

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, whatIsGround);

        if (isGrounded)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void LaunchDropDirection(Vector2 launchDirection)
    {
        rb.velocity = launchDirection;
    }
}
