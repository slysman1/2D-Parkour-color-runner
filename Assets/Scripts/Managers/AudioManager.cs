using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sfx;
    public AudioSource[] bgm;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       // InvokeRepeating("DoCheckIfMusicPlaying", 10, 15);
    }
    private void DoCheckIfMusicPlaying()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (!bgm[i].isPlaying)
            {
                int bgmNumber = Random.Range(0, bgm.Length);
                PlayBGM(bgmNumber);
                return;
            }
        }
    }

    public void PlaySFX(int soundToPlay)
    {
        sfx[soundToPlay].pitch = Random.Range(0.9f, 1.1f);

        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void StopSFX(int sountToStop)
    {
        if (sountToStop < sfx.Length)
        {
            sfx[sountToStop].Stop();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        StopBGM();

        if (musicToPlay < bgm.Length)
        {
            bgm[musicToPlay].Play();
        }
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
