using UnityEngine;

public class UI_NotificationKiller : MonoBehaviour
{
    public void destroyNotification()
    {
        Destroy(this.gameObject);
    }
}
