using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickSkillPanel : MonoBehaviour
{
    UI ui;
    Player player;
    

    [SerializeField] int killAllPrice;
    [SerializeField] int summonAllyPrice;
    [SerializeField] int getComboPrice;
    [SerializeField] int getRewindPrice;
    [SerializeField] Text logText;
    [SerializeField] Text maxSpeedTextPrice;
    [SerializeField] Text rewindPrice;


    [SerializeField] GameObject comboCounterController;
    private bool haveEndlessRewind;
    private bool haveFreeSpeedBoost;

    public float countdownTime;
    public float notCountdown;

    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        ui = GameObject.Find("Canvas").GetComponent<UI>();

        if (player.unlockedSkillNumber[2])
        {
            maxSpeedTextPrice.text = "0 " ;
        }

        haveEndlessRewind = player.unlockedSkillNumber[5];
        haveFreeSpeedBoost = player.unlockedSkillNumber[2];
    }


    private void Update()
    {

        countdownTime -= 1 * Time.deltaTime;
        notCountdown -= 1 * Time.deltaTime;

        if (notCountdown <= 0)
        {
            logText.text = "Bank: " + GameManager.instance.coinsInBank.ToString("#,#");
        }

        if (countdownTime <= 0)
        {
            ui.CloseQuickSkillPanelBTN();
            logText.text = GameManager.instance.coinsInBank.ToString("#,#");
            notCountdown = 0;
        }

         if (player.unlockedSkillNumber[5] || GameManager.instance.amountOfRewinds > 0)
        {
            rewindPrice.text = "0 ";
        }
       else
        {
            rewindPrice.text = getRewindPrice.ToString("#,#");
        }
    }

    public void KillAllEnemies()
    {
        bool enoughMoney = GameManager.instance.coinsInBank > killAllPrice;
        countdownTime++;

        if (enoughMoney || haveEndlessRewind)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy iEnemy = enemies[i].GetComponent<Enemy>();
                iEnemy.SelfDestroy();
            }

            ui.CloseQuickSkillPanelBTN();
            GameManager.instance.SpendMoney(killAllPrice);
        }
        else
        {
            NotEnoughMoney();
        }

    }

    public void GetMaxSpeed()
    {
        bool enoughMoney = GameManager.instance.coinsInBank > summonAllyPrice;
        countdownTime++;

        if (!haveFreeSpeedBoost) 
        {
            if (enoughMoney)
            {
                player.moveSpeed = 20;
                GameManager.instance.SpendMoney(summonAllyPrice);
                ui.CloseQuickSkillPanelBTN();
            }
            else
            {
                NotEnoughMoney();
            }

        }
        else
        {
            player.moveSpeed = 20;
            ui.CloseQuickSkillPanelBTN();
        }
    }

    public void GetCombo()
    {
        ComboControllerUI comboScript = comboCounterController.GetComponent<ComboControllerUI>();
        countdownTime++;



        if (comboScript.comboCounter >= 3)
        {
            ui.CloseQuickSkillPanelBTN();
            return;
        }

        bool enoughMoney = GameManager.instance.coinsInBank > killAllPrice;

        if (enoughMoney)
        {
            comboCounterController.SetActive(true);
            comboScript.ComboCounterPlus();

            GameManager.instance.SpendMoney(getComboPrice);
        }
        else
        {
            NotEnoughMoney();
        }
        
    }

    public void GetRewind()
    {
        bool enoughMoney = GameManager.instance.coinsInBank > getRewindPrice;
        bool haveRewind = GameManager.instance.amountOfRewinds > 0;


        if (!haveEndlessRewind) // skilNumber 5 is endlress rewind
        {
            if (haveRewind)
                {
                    player.GetComponent<RewindController>().StartRewind();
                    //GameManager.instance.SpendMoney(getRewindPrice);
                    ui.CloseQuickSkillPanelBTN();
                }
            else if (enoughMoney)
                {
                    GameManager.instance.changeAmountOfRewinds(+1);
                    player.GetComponent<RewindController>().StartRewind();
                    GameManager.instance.SpendMoney(getRewindPrice);
                    ui.CloseQuickSkillPanelBTN();
                }
            else
                {
                    NotEnoughMoney();
                }
        }
        else
        {
                player.GetComponent<RewindController>().StartRewind();
                ui.CloseQuickSkillPanelBTN();
        }

          


    }

    void NotEnoughMoney()
    {
        notCountdown = 1f;
        logText.text = "Not enough";
    }

}
