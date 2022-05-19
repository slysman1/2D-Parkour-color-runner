using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_ComboCounter : MonoBehaviour
{
    public GameObject comboCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            //GameObject comboCounter = GameObject.Find("ComboCounter");
            ComboControllerUI comboScript = comboCounter.GetComponent<ComboControllerUI>();
        if (collision.tag == "Player")
        {


            if (!comboScript.comboOn)
            {
                comboCounter.SetActive(true);
            }

            comboScript.ComboCounterPlus();
            Destroy(this.gameObject);
        }

    }
}
