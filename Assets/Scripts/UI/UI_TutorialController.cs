using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialController : MonoBehaviour
{
    Player player;

    [SerializeField] GameObject tapToJumpTutorial;
    [SerializeField] GameObject tapToDoubleJumpTutorial;
    [SerializeField] GameObject tapToSlideTutorial;
    [SerializeField] Text slideTutorialText;

    bool tapToJumpWasShown;
    bool tapToDoubleJumpWasShown;
    bool tutorialCompleted;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();


        tutorialCompleted = PlayerPrefs.GetInt("tutorialCompleted") == 1;


        if (!tutorialCompleted)
        {
            tapToSlideTutorial.SetActive(false);
            tapToDoubleJumpTutorial.SetActive(false);
            tapToJumpTutorial.SetActive(true);
        }
    }

    private void Update()
    {
        if (!tutorialCompleted)
        {
            if (player.rb.velocity.y > 0 && !tapToJumpWasShown)
            {
                tapToJumpTutorial.SetActive(false);            
                tapToJumpWasShown = true;
            }

            if (tapToJumpWasShown && player.rb.velocity.y < 0 && !tapToDoubleJumpWasShown)
            {
                Time.timeScale = 0;
                tapToDoubleJumpTutorial.SetActive(true);
            }
        }

    }


    public void TurnOffDoubleJumpTutorial()
    {
            Time.timeScale = 0.7f;
            player.JumpButton();
            tapToDoubleJumpTutorial.SetActive(false);
            tapToDoubleJumpWasShown = true;
            StartCoroutine(TurnOnSlideTutorialRoutine());
    }

    public void TurnOffSlideTutorial()
    {
        if (!player.canSlide && player.isWallDetected)
        {
            slideTutorialText.text = "You must move to slide";
        }
        else
        {
            tapToSlideTutorial.SetActive(false);
            player.SlideButton();
            Time.timeScale = 1.1f;
            PlayerPrefs.SetInt("tutorialCompleted", 1);
        }

    }

    IEnumerator TurnOnSlideTutorialRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 0.6f;
        tapToSlideTutorial.SetActive(true);
    }
}
