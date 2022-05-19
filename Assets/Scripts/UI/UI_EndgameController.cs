using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndgameController : MonoBehaviour
{
    public static UI_EndgameController instance;

    private Player player;
    private RewindController rewindController;

    [SerializeField] Text earnCoins;
    [SerializeField] Text amountOfRewinds;
    [SerializeField] Text rewindButtonText;

    public bool rewindAdWatched;
    public bool coinsAdWatched;

    [SerializeField] GameObject coinsBTN;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rewindController = player.GetComponent<RewindController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.amountOfRewinds <= 0 || !rewindAdWatched)
        {
            rewindButtonText.gameObject.SetActive(true);
        }
        else
        {
            rewindButtonText.gameObject.SetActive(false);
        }
        
    }

    public void DoRewind()
    {

        bool canRewind = GameManager.instance.amountOfRewinds > 0;
            Debug.Log("DoRewind Clicked");

        if (canRewind)
        {
            Debug.Log("Trying to rewind");
            rewindController.StartRewind();
        }
        else if(!rewindAdWatched)
        {
            WatchADToGetRewind();
        }
        else
        {
            rewindButtonText.text = "1 time per run. Sorry";
        }
    }

    public void GetTripleCoins()
    {
        if (!coinsAdWatched)
        {
            WatchAdToGetTripleCoins();
            coinsBTN.SetActive(false);
        }

    }

    public void WatchADToGetRewind()
    {
        AdMobRewarded.instance.ShowRewardedAdForRewinds();
        rewindAdWatched = true;
    }

    public void WatchAdToGetTripleCoins()
    {
        AdMobRewarded.instance.ShowRewardedAdForCoins();
        coinsAdWatched = true;
    }
}
