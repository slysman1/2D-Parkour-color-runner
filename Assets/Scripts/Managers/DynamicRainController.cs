using UnityEngine;

public class DynamicRainController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float currentRanIntensity;
    private float targetRainIntenisty;

    private bool valueCanBeChanged;
    private bool rainIncreaseing;
    [SerializeField] private float rateChangePerSecond;



    [Header("Rain intensity info")]
    [SerializeField] private float chanceToMakeRain;

    [SerializeField] private float minRainIntensity;
    [SerializeField] private float maxRainIntensity;

    [SerializeField] private float rainCheckCooldown;
    private float lastTimeCheckedForRain;
    // Update is called once per frame
    void Update()
    {

        CheckIfCanRain();
        CheckIfIncreaseing();
        CheckIfIncreaseingDone();

    }

    private void CheckIfIncreaseingDone()
    {
        if (rainIncreaseing)
        {
            if (currentRanIntensity >= targetRainIntenisty)
            {
                valueCanBeChanged = false;
                currentRanIntensity = targetRainIntenisty;
            }
        }
        else
        {
            if (currentRanIntensity <= targetRainIntenisty)
            {
                valueCanBeChanged = false;
                currentRanIntensity = targetRainIntenisty;
            }
        }
    }
    private void CheckIfIncreaseing()
    {
        if (currentRanIntensity < targetRainIntenisty)
        {
            rainIncreaseing = true;

            if (valueCanBeChanged)
            {
                currentRanIntensity += rateChangePerSecond * Time.deltaTime;
            }
        }
        else
        {
            rainIncreaseing = false;

            if (valueCanBeChanged)
            {
                currentRanIntensity -= rateChangePerSecond * Time.deltaTime;
            }
        }
    }

    private void CheckIfCanRain()
    {
        if (Time.time > lastTimeCheckedForRain + rainCheckCooldown)
        {
            MakeRain();
        }
    }
    private void MakeRain()
    {
        float choosenIntensity = Random.Range(minRainIntensity, maxRainIntensity);
        bool luckyRollCanRain = Random.Range(0, 100) <= chanceToMakeRain ;

        if (luckyRollCanRain)
        {
            targetRainIntenisty = choosenIntensity;
        }
        else
        {
            targetRainIntenisty = 0;
        }

        valueCanBeChanged = true;
    }


}
