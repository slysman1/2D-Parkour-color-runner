using UnityEngine;
using UnityEngine.UI;

public class UI_HeartIconSwitcher : MonoBehaviour
{
    private Player player;
    private Image heartIcon;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        heartIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.moveSpeed > player.moveSpeedNeededToSurive)
        {
            heartIcon.enabled = true;
        }
        else
        {
            heartIcon.enabled = false;
        }
    }
}
