using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Components

    private CapsuleCollider2D cd;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private Player player;

    #endregion

    [SerializeField] private GameObject enemySpawnFX;

    [Header(" Spwan info")]
    [SerializeField] private float distanceToBeat;
    private bool justSpawned = true;
    [Tooltip("Used to set direction of enemy's jump after he appears")]
    [SerializeField] private Vector2 spawnLaunchDirection;

    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int amountOfSpikes;
    [SerializeField] private Vector2 startPos;

    [Header("Movement info")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private bool isRunning;
    public bool canRun;
    private bool canJump;
    private bool canDoubleJump;



    [Header("Ledge Climb Info")]

    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ledgeWallCheck;

    private bool isTouchingLedge;
    private bool isLedgeDetected;
    private bool isLedgeWallDetected;
    private bool canClimbLedge;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1; // position to hold the player before animations ends
    private Vector2 ledgePos2; // position where to move the player after animations ends

    public float ledgeClimb_Xoffset1 = 0f;
    public float ledgeClimb_Yoffset1 = 0f;
    public float ledgeClimb_Xoffset2 = 0f;
    public float ledgeClimb_Yoffset2 = 0f;


    [Header("Collision checks")]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheckAhead;
    [SerializeField] private Transform wallCheck;

    private bool isGrounded;
    private bool isGroundAhead;
    private bool isWallDetected;


    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckAheadDistance;
    [SerializeField] private float ledgeWallCheckDistance;
    [SerializeField] private float wallCheckRadius;

    [SerializeField] private LayerMask whatIsGround;


    [Header("Name List")]
    [SerializeField] Text myName;
    [SerializeField] string[] nameList;
    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();


        rb.velocity = spawnLaunchDirection;
        startPos = transform.position;

        myName.text = nameList[Random.Range(0, nameList.Length)];

        distanceToBeat = Random.Range(100, 250);

        GameObject fxDrop = Instantiate(enemySpawnFX, transform.position, transform.rotation);
        //fxDrop.GetComponent<SpriteRenderer>().color = sr.color;
    }

    // Update is called once per frame
    void Update()
    {

        EnemySpawnLogic();

        CheckIfShouldStop();
        CheckForMoveSpeed();
        CheckForMove();
        CheckForCollisions();
        CheckForLedgeClimb();

        CheckForAnim();
    }

    #region spawn state info
    public void DropSpikes()
    {
        AudioManager.instance.PlaySFX(9);

        for (int i = 0; i < amountOfSpikes; i++)
        {
            GameObject spike = Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation, transform.parent);
            DropController spikeScript = spike.GetComponent<DropController>();

            spikeScript.LaunchDropDirection(new Vector2(-3 + i, 10) * 2);
        }


    }
    public void EnemyCanMove()
    {
        justSpawned = false;
        canRun = true;
    }
    private void EnemySpawnLogic()
    {
        if (justSpawned)
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = 10;
                Time.timeScale = 1;
            }

            if (isGrounded)
            {
                StopMovement();
                rb.gravityScale = 5;
            }
        }
    }

    #endregion



    public void SelfDestroy()
    {
        Destroy(this.gameObject);
        GameObject fxDrop = Instantiate(enemySpawnFX, transform.position, transform.rotation);
        //fxDrop.GetComponent<SpriteRenderer>().color = sr.color;
    }

    private void CheckForAnim()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("justSpawned", justSpawned);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("canClimbLedge", canClimbLedge);

        if (rb.velocity.x != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }


    private void CheckForMoveSpeed()
    {
        if (player.transform.position.x > transform.position.x)
        {
            moveSpeed = 16;
        }
        else
        {
            moveSpeed = 13;
        }
    }
    private void CheckIfShouldStop()
    {
        if (Vector2.Distance(startPos, transform.position) > distanceToBeat && isGrounded)
        {
            StopMovement();
            canRun = false;

            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (Vector2.Distance(transform.position, player.transform.position) > 5)
            {
                SelfDestroy();
            }
        }
    }
    private void CheckForMove()
    {
        if (canRun)
        {
            if (isWallDetected & canJump)
            {
                Jump();
            }
            else if (isGrounded && !isGroundAhead)
            {
                Jump();
            }
            else if (canDoubleJump && !isGroundAhead && rb.velocity.y < -10)
            {
                canDoubleJump = false;
                Jump();
            }


            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (isGrounded)
            {
                canJump = true;
                canDoubleJump = true;
            }
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void StopMovement()
    {
        rb.velocity = new Vector2(0, 0);
    }

    private void CheckForLedgeClimb()
    {
        if (isLedgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            ledgePos1 = new Vector2((ledgePosBot.x + ledgeWallCheckDistance) + ledgeClimb_Xoffset1, (ledgePosBot.y) + ledgeClimb_Yoffset1);
            ledgePos2 = new Vector2(ledgePosBot.x + ledgeWallCheckDistance + ledgeClimb_Xoffset2, (ledgePosBot.y) + ledgeClimb_Yoffset2);

            canRun = false;
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
            canJump = false;
            StopMovement();

        }
    }

    private void CheckIfLedgeClimbFinished()
    {
        transform.position = ledgePos2;
        canClimbLedge = false;
        canRun = true;
        isLedgeDetected = false;
    }

    private void CheckForCollisions()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool isGroundNotFar = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance + 2, whatIsGround);
        isGroundAhead = Physics2D.Raycast(groundCheckAhead.position, Vector2.down, groundCheckAheadDistance, whatIsGround);

        if (isGroundNotFar)
        {
         //   cd.isTrigger = false;
        }

        if (isGrounded)
        {
            isWallDetected = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsGround);
        }

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.right, ledgeWallCheckDistance, whatIsGround);
        isLedgeWallDetected = Physics2D.Raycast(ledgeWallCheck.position, Vector2.right, ledgeWallCheckDistance, whatIsGround);

        #region ledgeCheckBugFix

        if (rb.velocity.y < 0)
        {
            ledgeClimb_Yoffset1 = -0.81f;
        }
        else
        {
            ledgeClimb_Yoffset1 = -0.77f;
        }

        #endregion

        if (isLedgeWallDetected && !isTouchingLedge && !isLedgeDetected)
        {
            isLedgeDetected = true;
            ledgePosBot = ledgeWallCheck.position;
            StopMovement();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, groundCheck.position.z));
        Gizmos.DrawLine(groundCheckAhead.position, new Vector3(groundCheckAhead.position.x, groundCheckAhead.position.y - groundCheckAheadDistance, groundCheckAhead.position.z));
        Gizmos.DrawLine(ledgeWallCheck.position, new Vector3(ledgeWallCheck.position.x + ledgeWallCheckDistance, ledgeWallCheck.position.y, ledgeWallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeWallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);


        //Gizmos.DrawLine(bottomWallCheck.position, new Vector3(bottomWallCheck.position.x + wallCheckDistance, bottomWallCheck.position.y, bottomWallCheck.position.z));
        //Gizmos.DrawLine(ceillingCheck.position, new Vector3(ceillingCheck.position.x, ceillingCheck.position.y + wallCheckDistance + 0.5f, ceillingCheck.position.z));
    }

}
