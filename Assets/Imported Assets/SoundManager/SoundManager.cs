using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmAudioSource;

    [Header("Volume Settings")]
    [Range(0, 1)] [SerializeField] float maxBgmVolume = 1;
    [Range(0, 1)] public float sfxVolume = 1;

    static SoundManager _instance;

    public float bgmVolume { get { return maxBgmVolume; } }
    public static SoundManager instance { get { return _instance; } }
    public class MusicInQueue { public string bgmName; public bool saveInGameManager; }

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SetBGMVolume(maxBgmVolume);
    }

    /// <summary>
    /// Toca um Sound Effect
    /// </summary>
    /// <param name=""></param>
    public void PlaySFX(AudioClip clip,float volume = 1, float pitchVariation = 0)
    {
        GameObject goTemp = new GameObject();
        AudioSource audioTemp = goTemp.AddComponent<AudioSource>();
        
        audioTemp.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
        audioTemp.volume = sfxVolume* volume;
        audioTemp.clip = clip;
        audioTemp.Play();
        Destroy(goTemp, clip.length + 0.1f);
    }

    public void PlayBGM(AudioClip audioClip)
    {
        if (audioClip)
        {
            if (audioClip == bgmAudioSource.clip)
                return;
            bgmAudioSource.loop = true;
            bgmAudioSource.clip = audioClip;
            bgmAudioSource.Play();
        }
        else
        {
            bgmAudioSource.clip = null;
            bgmAudioSource.Stop();
        }

    }

    public void PlayBGMOnce(AudioClip audioClip)
    {
        if (audioClip)
        {
            if (audioClip == bgmAudioSource.clip)
                return;

            bgmAudioSource.loop = false;
            bgmAudioSource.clip = audioClip;
            bgmAudioSource.Play();
        }
        else
        {
            bgmAudioSource.clip = null;
            bgmAudioSource.Stop();
        }

    }

    /// <summary>
    /// Seta o volume máximo da BGM
    /// </summary>
    public void SetBGMVolume(float value)
    {
        maxBgmVolume = value;
        bgmAudioSource.volume =  maxBgmVolume;
    }

    
}
