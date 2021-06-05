using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmAudioSource;

    //[Header("BGM Clips")]
    //[SerializeField] AudioClip[] bgmAudioClips;

    [Header("Volume Settings")]
    [Range(0, 1)] [SerializeField] float maxBgmVolume = 1;
    [Range(0, 1)] public float sfxVolume = 1;

    Coroutine fadeBGM;
    static SoundManager _instance;

    public float bgmVolume { get { return maxBgmVolume; } }
    public static SoundManager instance { get { return _instance; } }
    //public class MusicInQueue { public string bgmName; public bool saveInGameManager; }

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        SetBGMVolume(maxBgmVolume);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Toca um Sound Effect
    /// </summary>
    /// <param name=""></param>
    public void PlaySFX(AudioClip clip,float volume = 1, float pitch = 1)
    {
        GameObject goTemp = new GameObject();
        AudioSource audioTemp = goTemp.AddComponent<AudioSource>();
        
        audioTemp.pitch = pitch;
        audioTemp.volume = sfxVolume* volume;
        audioTemp.clip = clip;
        audioTemp.Play();
        Destroy(goTemp, clip.length + 0.1f);
    }

    public void PlayBGM(AudioClip audioClip, float volume = 1, float pitch = 1, bool loop = true)
    {
        if (fadeBGM != null)
            StopCoroutine(fadeBGM);

        if (audioClip)
        {
            //if (audioClip == bgmAudioSource.clip)
            //    return;

            bgmAudioSource.volume = maxBgmVolume * volume;
            bgmAudioSource.pitch = pitch;
            bgmAudioSource.loop = loop;
            bgmAudioSource.clip = audioClip;
            bgmAudioSource.Play();
        }
        else
        {
            bgmAudioSource.clip = null;
            bgmAudioSource.Stop();
        }

    }

    internal void FadeBGM(float time)
    {
        if (fadeBGM != null)
            StopCoroutine(fadeBGM);
        fadeBGM = StartCoroutine(_FadeBGM(time));
    }

    private IEnumerator _FadeBGM(float time)
    {
        float decurredTime = 0;
        while(decurredTime < time)
        {
            bgmAudioSource.volume *= 0.95f;
            decurredTime += Time.deltaTime;
            yield return null;
        }
        bgmAudioSource.volume = 0;
    }

    /// <summary>
    /// Seta o volume máximo da BGM
    /// </summary>
    public void SetBGMVolume(float value)
    {
        maxBgmVolume = value;
        bgmAudioSource.volume =  maxBgmVolume;
    }

    //public void CheckCurrentBGM()
    //{
    //    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
    //    {
    //        PlayBGM(bgmAudioClips[(int)BGM_List.Tutorial]);
    //    }
    //    else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
    //    {
    //        PlayBGM(bgmAudioClips[(int) BGM_List.Castle]);
    //    }
           
    //}
    
    //private enum BGM_List
    //{
    //    Tutorial,
    //    Forest,
    //    Mountain,
    //    Cave,
    //    Castle
    //}
}
