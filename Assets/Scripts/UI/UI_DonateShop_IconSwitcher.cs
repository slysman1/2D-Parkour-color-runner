using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DonateShop_IconSwitcher : MonoBehaviour
{
    Image image;
    float countdownTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer-= 1 * Time.deltaTime;

        if (countdownTimer < 0)
        {
            countdownTimer = 1;
            image.color = Random.ColorHSV();
        }
    }
}
