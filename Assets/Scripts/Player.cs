using System.Collections;
using UnityEngine;
using UI_InputSystem.Base;



public class Player : MonoBehaviour
{
    [SerializeField] GameObject UI_Inputs;

    #region components
    public Rigidbody2D rb; // rb - rigid body
    public SpriteRenderer sr;
    private Animator anim;
    private Coroutine hurtAnimCoroutine;
    private UI UI;
    private RewindController rewindController;

    public UI_AbilityShop abilityShop;
    public ComboControllerUI comboController;
    #endregion

    public Color purchasedCollor;
    [HideInInspector] public bool canColorPlatform;

    [Header("Inventory")]
    public bool[] unlockedSkillNumber;
    [SerializeField] private GameObject coinToDrop;
    [SerializeField] Transform dropPos;

    #region Movemnt info region

    [Header("Movement Info")]
    public float moveSpeedNeededToSurive;
    public float moveSpeed;
    public float maxMoveSpeed;
    private float defaultMoveSpeed;
    private bool isRunning;
    public bool canRun = false; // bool boolean true || false


    private float speedMilestone;
    [SerializeField] private float speedMultipler;
    [SerializeField] private float speedIncreaseMilestone;
    private float defaultSpeedIncreaseMilestone;


    public bool canRoll;
    private bool justLanded;

    [Header("Jump info")]
    public float jumpForce;
    public float doubleJumpForce;

    private float defaultJumpForce;
    private bool canDoubleJump;
    private bool canTrippleJump;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float knockbackPower;

    private bool canBeKnocked = true;
    public bool isKnocked;

    [Header("Slide info")]
    public float slideSpeedMultipler;
    private bool isSliding;

     public bool canSlide;

    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingTime;
    private float slidingBegun;


    [Header("Die info")]
     public bool canDie = true;
    public bool isDead;
    private bool dieNotificationPlayed;

    [Header("Collision detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform bottomWallCheck;
    [SerializeField] private Transform ceillingCheck;


    public float wallCheckDistance;
    public float groundCheckRadius;
    public LayerMask whatIsGround;


    private bool isGrounded;
    [HideInInspector] public bool isWallDetected;
    private bool isBottomWallDetected;
    private bool isCeillingDetected;

    [Header("Ledge Climb Info")]

    [SerializeField] private Transform ledgeCheck;

    private bool isTouchingLedge;
    private bool isLedgeDetected;
    private bool canClimbLedge;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1; // position to hold the player before animations ends
    private Vector2 ledgePos2; // position where to move the player after animations ends

    public float ledgeClimb_Xoffset1 = 0f;
    public float ledgeClimb_Yoffset1 = 0f;
    public float ledgeClimb_Xoffset2 = 0f;
    public float ledgeClimb_Yoffset2 = 0f;
    #endregion


    [Header("Particels")]
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem bloodSplatter;

    private void Awake()
    {
        Instantiate(UI_Inputs, transform.parent);
    }

    #region Simple Inputs

    private void OnEnable()
    {
        UIInputSystem.ME.AddOnClickEvent(ButtonAction.SlideClickTrigger, NoFunction);
        UIInputSystem.ME.AddOnTouchEvent(ButtonAction.JumpTouchTrigger, NoFunction);
        //UIInputSystem.ME.AddOnTouchEvent(ButtonAction.RewindHoldTrigger, NoFunction);
    }

    private void OnDisable()
    {
        UIInputSystem.ME.RemoveOnClickEvent(ButtonAction.SlideClickTrigger, NoFunction);
        UIInputSystem.ME.RemoveOnTouchEvent(ButtonAction.JumpTouchTrigger, NoFunction);
        //UIInputSystem.ME.RemoveOnTouchEvent(ButtonAction.RewindHoldTrigger, NoFunction);
    }
    void NoFunction()
    {

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        UI = GameObject.Find("Canvas").GetComponent<UI>();
        rewindController = GetComponent<RewindController>();

        CheckWhatIsPurchased();
        SettingDefaultValues();
    }


    // Update is called once per frame // if you have 60fps -  60 times per second
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            JumpButton();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlideButton();



        



        if (UIInputSystem.ME.GetButton(ButtonAction.SlideClickTrigger))
            SlideButton();
        if (UIInputSystem.ME.GetButton(ButtonAction.JumpTouchTrigger))
            JumpButton();
        rewindController = GetComponent<RewindController>();

        if (!rewindController.isRewinding)
        {
            CheckForRun();
            CheckForDoubleJump();
            CheckForSlide();
            CheckForSpeedingUp();
            CheckForLedgeClimb();
        }


        CheckForCollisions();
        AnimationControllers();
    }

   
    public void CheckWhatIsPurchased()
    { 
        #region check if color purchased
        string myColorCode = PlayerPrefs.GetString("myColorCode");
        Color nextColor;
        if (myColorCode != null)
        {
            if (ColorUtility.TryParseHtmlString(myColorCode, out nextColor))
            {
                sr.color = nextColor;
            }
        }
        #endregion

        //UI_AbbilityShopUI abilityShop = GameObject.Find("abbilityShopUI").GetComponent<UI_AbbilityShopUI>();
        string[] skillSet = abilityShop.skillName;

        for (int i = 0; i < skillSet.Length; i++)
        {
            if (PlayerPrefs.GetInt(skillSet[i]) == 1)
            {
                unlockedSkillNumber[i] = true;
            }
        }
    }
    private void SettingDefaultValues()
    {
        

        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
        defaultSpeedIncreaseMilestone = speedIncreaseMilestone;
        speedMilestone = defaultSpeedIncreaseMilestone;


        #region position for tutorial
        Transform posBeforeTutorial = GameObject.Find("playerPositionBeforeTutorial").transform;
        //Transform posAfterTutorial = GameObject.Find("playerPositionAfterTutorial").transform;

        if (PlayerPrefs.GetInt("firstLogin") == 0)
        {
            PlayerPrefs.SetInt("firstLogin", 1);
            GameManager.instance.changeAmountOfRewinds(+2);
        }

        if (PlayerPrefs.GetInt("tutorialCompleted") == 0)
        {
            transform.position = posBeforeTutorial.position;
        }


        #endregion
    }

    #region animation info

    private void AnimationControllers()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimbLedge", canClimbLedge);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("canRoll", canRoll);
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isDead", isDead);

        if (canClimbLedge)
        {
            canRoll = false;
        }
        else if (rb.velocity.y < -25)
        {
            canRoll = true;
        }
    }
    private void RollAnimationFinished()
    {
        canRoll = false;

        AchievementManager.instance.SendAchiveUpdate(3,1);
        AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_limp_bizkit_would_be_proud);
    }
    private void KnockbackAnimationFinished()
    {
        if (isDead && !canDie)
        {
            isDead = false;
            canDie = true;
        }

        isKnocked = false;
        canRun = true;

        
    }
    private void PlayerDieAnimationFinished()
    {
        if (isKnocked)
        {
            ResurectPlayer();
        }

        GameManager.instance.GameEnds();
        rb.velocity = new Vector2(0, 0);
    }

    #endregion

    public void ResurectPlayer()
    {
        isDead = false;
        canDie = true;
        CheckIfLedgeClimbFinished();
        canRun = true;
    }

    public void Knockback()
    {
        if (!rewindController.isRewinding)
        {
            if (canBeKnocked)
            {
                Debug.Log("got knocked");
                isKnocked = true;
                HurtVFX();
                CreateBloodVFX();
                SpeedReset();
            }
        }

    }

    public void CheckIfDead()
    {
        if (isDead && !canDie && isKnocked)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        if (!rewindController.isRewinding)
        {
            //rb.velocity = new Vector2(0, 0);
            isDead = true;
            CreateBloodVFX();
            Time.timeScale = 0.4f;

            if (!dieNotificationPlayed)
            {
                AudioManager.instance.PlaySFX(4);
                dieNotificationPlayed = true;
            }

            AchievementManager.instance.SendAchiveUpdate(8, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_i_can_do_this_all_day);
        }

    }


    private void CheckForRun()
    {
        if (isDead & canDie)
        {
            canDie = false;
            canRun = false;
            rb.velocity = knockbackDirection * knockbackPower;
        }
        else if (isKnocked & canBeKnocked)
        {
            canBeKnocked = false;
            canRun = false;
            rb.velocity = knockbackDirection * knockbackPower;
        }

        if (canRun)
        {
            if (isBottomWallDetected || isWallDetected && !isSliding)
            {
                SpeedReset();
            }
            else if (isSliding)
            {
                rb.velocity = new Vector2(moveSpeed * slideSpeedMultipler, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }


        if (rb.velocity.x > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }


    #region jump button and functions
    public void JumpButton()
    {
        if (isGrounded && !isKnocked)
        {
            Jump();
            canRoll = false;

            AchievementManager.instance.SendAchiveUpdate(0, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_everybody_flarking_jump);
        }
        else if (canDoubleJump)
        {

            canDoubleJump = false;

            jumpForce = doubleJumpForce;
            Jump();

            
            AchievementManager.instance.SendAchiveUpdate(2, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_im_not_a_sonic_but);
        }
    }

    private void Jump()
    {
        CreateDustVFX();

        TriggerOnJump();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AudioManager.instance.PlaySFX(Random.Range(2, 3));
    }
    private void CheckForDoubleJump()
    {
        if (isGrounded)
        {
            jumpForce = defaultJumpForce;
            canDoubleJump = true;

        }
        else
        {
            justLanded = false;
        }

        if (isGrounded & !justLanded)
        {
            justLanded = true;
            Time.timeScale = 1;
            CreateDustVFX();


        }
    }
    private void TriggerOnJump()
    {
        

        bool luckyRollToDropCoin = Random.Range(0, 100) < 50;
        bool luckyRollComboAdd = Random.Range(0, 100) < 15;
        bool canIncreaseCombo = comboController.comboCounter < 3;

        if (unlockedSkillNumber[1] && luckyRollToDropCoin)
        {
            // drop coin

            GameObject newCoin = Instantiate(coinToDrop, dropPos.position, transform.rotation);
            PlayerCoinDropCorrector newCoinScript = newCoin.GetComponent<PlayerCoinDropCorrector>();
            newCoinScript.LaunchDropDirection(new Vector2(-10, 20));
        }

        if (unlockedSkillNumber[0])
        {
            if (luckyRollComboAdd && canIncreaseCombo)
            {
                comboController.gameObject.SetActive(true);
                comboController.ComboCounterPlus();
            }
        }    

        if (unlockedSkillNumber[4])
        {
            sr.color = Random.ColorHSV(); // used for random color
        }
    }

    #endregion
    #region slide region
    public void SlideButton()
    {
        if (canSlide && isGrounded && !isWallDetected)
        {
            CreateDustVFX();

            isSliding = true;
            canSlide = false;
            slidingBegun = Time.time;

            AchievementManager.instance.SendAchiveUpdate(1, 1);
            AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_wet_floor);
        }
    }
    private void CheckForSlide()
    {


        if (Time.time > slidingBegun + slidingTime && !isCeillingDetected)
        {
            isSliding = false;
        }

        if (Time.time > slidingBegun + slidingCooldown)
        {
            canSlide = true;
        }
    }

    #endregion
    #region speed controll
    private void CheckForSpeedingUp()
    {
        if (transform.position.x > speedMilestone)
        {
            speedMilestone = speedMilestone + speedIncreaseMilestone;


            moveSpeed = moveSpeed * speedMultipler;
            speedIncreaseMilestone = speedIncreaseMilestone * speedMultipler;

            if (moveSpeed > maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
            }
        }
    }
    public void SpeedReset()
    {

        moveSpeed = defaultMoveSpeed;
        speedIncreaseMilestone = defaultSpeedIncreaseMilestone;
    }


    #endregion
    #region ledgeClimb
    private void CheckForLedgeClimb()
    {
        if (isLedgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            ledgePos1 = new Vector2((ledgePosBot.x + wallCheckDistance) + ledgeClimb_Xoffset1, (ledgePosBot.y) + ledgeClimb_Yoffset1);
            ledgePos2 = new Vector2(ledgePosBot.x + wallCheckDistance + ledgeClimb_Xoffset2, (ledgePosBot.y) + ledgeClimb_Yoffset2);

            canRun = false;
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;


        }
    }

    public void CheckIfLedgeClimbFinished()
    {
        transform.position = ledgePos2;
        canClimbLedge = false;
        canRun = true;
        isLedgeDetected = false;
    }

    #endregion
    #region collisions and gizmos
    private void CheckForCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isBottomWallDetected = Physics2D.Raycast(bottomWallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
        isCeillingDetected = Physics2D.Raycast(ceillingCheck.position, Vector2.up, wallCheckDistance + 0.5f, whatIsGround);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        #region ledgeCheckBugFix

        if (rb.velocity.y < 0)
        {
            ledgeClimb_Yoffset1 = -1.15f;
        }
        else
        {
            ledgeClimb_Yoffset1 = -0.75f;
        }

        #endregion

        if (isWallDetected && !isTouchingLedge && !isLedgeDetected && !rewindController.isRewinding)
        {
            isLedgeDetected = true;
            ledgePosBot = wallCheck.position;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        Gizmos.DrawLine(bottomWallCheck.position, new Vector3(bottomWallCheck.position.x + wallCheckDistance, bottomWallCheck.position.y, bottomWallCheck.position.z));
        Gizmos.DrawLine(ceillingCheck.position, new Vector3(ceillingCheck.position.x, ceillingCheck.position.y + wallCheckDistance + 0.5f, ceillingCheck.position.z));
    }

    #endregion
    #region VX logic
    private IEnumerator HurtVFXRoutine()
    {
        Color originalColor = sr.color;
        Color darkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);

        sr.color = darkenColor;

        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;

        yield return new WaitForSeconds(0.2f);
        sr.color = darkenColor;

        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;

        yield return new WaitForSeconds(0.2f);
        sr.color = darkenColor;

        yield return new WaitForSeconds(0.3f);
        sr.color = originalColor;

        yield return new WaitForSeconds(0.3f);
        sr.color = darkenColor;

        yield return new WaitForSeconds(0.4f);
        sr.color = originalColor;

        yield return new WaitForSeconds(0.2f);

        canBeKnocked = true; // makes player valnurable again



        hurtAnimCoroutine = null; // stops coroutine 
    }

    public void HurtVFX()
    {
        // stops actve coroutine before activating new one
        if (hurtAnimCoroutine != null)
        {
            StopCoroutine(hurtAnimCoroutine);
        }

        // starts coroutine with reference to it
        hurtAnimCoroutine = StartCoroutine(HurtVFXRoutine());
    }

    private void CreateDustVFX()
    {
        dust.Play();
    }

    private void CreateBloodVFX()
    {
        bloodSplatter.Play();
    }
    #endregion


  
}
