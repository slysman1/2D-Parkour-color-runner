using UnityEngine;

public class UI_SetParentTo : MonoBehaviour
{
    [SerializeField] private Transform myFutureParent;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(myFutureParent);
    }
}
