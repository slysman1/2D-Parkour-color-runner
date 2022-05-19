using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public UI_AbilityShop ui_AbilityShop;
    private string[] skillName;

    private string randomColorOnJump = "com.alexdev.parkourcolorrunner.random_color_on_jump";
    private string endlessRewind = "com.alexdev.parkourcolorrunner.endless_rewind";
    private string removeAds = "com.alexdev.parkourcolorrunner.remove_ads";
    private string coins20k = "com.alexdev.parkourcolorrunner.coins20k";
    private string rewinds20 = "com.alexdev.parkourcolorrunner.rewinds20";
    private string noTrapsDamage = "com.alexdev.parkourcolorrunner.no_traps";

    private void Start()
    {
        GetSkillNames();
    }

    private void GetSkillNames()
    {

        skillName = new string[ui_AbilityShop.skillName.Length];

        for (int i = 0; i < ui_AbilityShop.skillName.Length; i++)
        {
            skillName[i] = ui_AbilityShop.skillName[i];
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == noTrapsDamage)
        {
            PlayerPrefs.SetInt(skillName[3], 1);
        }

        if (product.definition.id == randomColorOnJump)
        {
            PlayerPrefs.SetInt(skillName[4], 1);
        }

        if (product.definition.id == endlessRewind)
        {
            PlayerPrefs.SetInt(skillName[5], 1);
        }

        if (product.definition.id == rewinds20)
        {
            int amountOfRewinds = PlayerPrefs.GetInt("rewinds");
            PlayerPrefs.SetInt("rewinds", amountOfRewinds + 20);
            GameManager.instance.amountOfRewinds = PlayerPrefs.GetInt("rewinds");
        }


        if (product.definition.id == coins20k)
        {

            Debug.Log("Hhere is your coin brother!");
            int coinsInBankBefore = PlayerPrefs.GetInt("coinsInBank");
            PlayerPrefs.SetInt("coinsInBank", coinsInBankBefore + 20000);

            GameManager.instance.coinsInBank = PlayerPrefs.GetInt("coinsInBank");

        }

        if (product.definition.id == removeAds)
        {
            PlayerPrefs.SetInt("NoAds", 1);
        }

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + " failed because" + failureReason);
    }
}
