using UnityEngine;

public class Trigger_PlatformColorChange : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D cd;
    private Player player;

    private Color defaultColor;
    [SerializeField] private Color nextColor;
    [SerializeField] GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponentInParent<SpriteRenderer>();
        cd = GetComponent<BoxCollider2D>();

        //change size of this box collider according to sprite size of the parent
        cd.size = new Vector2(sr.size.x, sr.size.y + 0.001f);
        cd.offset = new Vector2(0, 0);

        defaultColor = player.purchasedCollor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && player.canColorPlatform)
        {
            nextColor = collision.GetComponent<Player>().purchasedCollor;
            AchievementManager.instance.SendAchiveUpdate(13, 1);
            
            if (nextColor != defaultColor)
            {
            }


            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform child = transform.parent.GetChild(i);
                if (child.tag == "PlatformHeader")
                {
                    SpriteRenderer newSr = child.GetComponent<SpriteRenderer>();
                    newSr.color = nextColor;


                    //  sr.color = nextColor;

                }

            }

            //sr.color = nextColor;
            //if(ColorUtility.TryParseHtmlString("#8A1E11",out nextColor)) {}
        }
    }
}
