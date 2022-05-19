using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProccesController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            colorGrading.active = true;
        }
        else
        {
            colorGrading.active = false;
        }
    }
}
