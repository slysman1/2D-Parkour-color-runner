using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour
{
    public bool isRewinding = false;

    public float recordTime;

    [SerializeField] ParticleSystem resurrectFireFX;
    [SerializeField] GameObject tapToStopRewindBTN;

    List<PointInTime> pointsInTime;
    Rigidbody2D rb;
    Player player;


    public Color playerOriginalColor;
    public Color playerDarkenColor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        pointsInTime = new List<PointInTime>();


        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        playerOriginalColor = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        playerDarkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRewind();
        }
        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    StopRewind();
        //}
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
             Record();           
        }
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    void Record()
    {
        if (!player.isDead)
        {
            if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        }

    }

    public void StartRewind()
    {
        if (GameManager.instance.amountOfRewinds > 0)
        {
            isRewinding = true;
            rb.isKinematic = true;
            RewindPlayer(isRewinding);
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        rb.gravityScale = 5;
        RewindPlayer(isRewinding);

        GameManager.instance.changeAmountOfRewinds(-1);
        AchievementManager.instance.SendAchiveUpdate(9, 1);
        AchievementManager.instance.DoIncrementalAchievemnt(GPGSIDs.achievement_i_would_rewind_that_rewind_if_i_could);
    }

    void RewindPlayer(bool isRewinding)
    {
        if (isRewinding)
        {
            if (player.isDead)
            {
                GameManager.instance.ResurrectPlayer();
            }

            player.CheckIfLedgeClimbFinished();
            resurrectFireFX.Play();
            player.sr.color = playerDarkenColor;
            player.canRun = false;
            KillAllEnemies();
            tapToStopRewindBTN.SetActive(true);


        }
        else
        {
            player.sr.color = playerOriginalColor;
            player.canRun = true;
            resurrectFireFX.Stop();
            tapToStopRewindBTN.SetActive(false);
        }
    }

    void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy iEnemy = enemies[i].GetComponent<Enemy>();
            iEnemy.SelfDestroy();
        }
    }
}
