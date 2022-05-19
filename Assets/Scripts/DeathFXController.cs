using UnityEngine;

public class DeathFXController : MonoBehaviour
{
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public void KillAnimation()
    {
        Destroy(this.gameObject);
    }
}
