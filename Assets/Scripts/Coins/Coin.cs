using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject comboCounter = GameObject.Find("ComboCounter");
        //ComboCounterController comboScript = comboCounter.GetComponent<ComboCounterController>();

        
        if (collision.tag == "Player")
        {
            GameManager.instance.AddCoin();
            
            //comboScript.comboCounter++;
            //if (comboScript.comboOn)
            //{

            //    GameManager.instance.coins += 3;
            //}
            //else
            //{
            //}


            AudioManager.instance.PlaySFX(0);
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
