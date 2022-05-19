using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchievementContent : MonoBehaviour
{
    [SerializeField] private string Tittle;
    [SerializeField] private string Description;
    [SerializeField] private int progress;
    [SerializeField] private int reward;

    Text tittleText;
    Text desicriptionText;
    int progressText;
    int rewardText;

    Slider progressSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
