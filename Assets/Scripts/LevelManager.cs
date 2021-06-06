using Cinemachine;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] protected AudioClip _levelMusic;
    [SerializeField] protected float _musicVolume;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] float playerEndLevelSpeed = 30;
    [SerializeField] CheckPoint[] checkPoints;
    [SerializeField] AudioClip[] otherMusics;
    [SerializeField] string nextLevel;

    public AudioClip levelMusic { get { return _levelMusic; } }
    public float musicVolume { get { return _musicVolume; } }
    public CinemachineVirtualCamera camera { get { return _camera; } }
    public static LevelManager instance { get; protected set; }
    protected void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LevelStart();    
    }

    protected virtual void LevelStart()
    {
        SoundManager.instance.PlayBGM(_levelMusic, _musicVolume);
        if (GameManager.instance.hasCheckPoint)
            GameObject.FindGameObjectWithTag("Player").transform.position = GameManager.instance.spawnPosition;
    }

    public virtual void LevelEnd(LevelEnd levelEnd)
    {
        StartCoroutine(_LevelEnd(levelEnd));
    }

    private IEnumerator _LevelEnd(LevelEnd levelEnd)
    {
        SoundManager.instance.FadeBGM(.5f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<InputController>().enabled = false;
        player.GetComponent<HealthController>().EndLevel();
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, levelEnd.playerEndLevelStartPosition, playerEndLevelSpeed))
        {
            yield return null;
        }
        yield return null;
        player.GetComponent<Animator>().SetBool("Win", true);
        SoundManager.instance.PlaySFX(GameManager.instance.endLevelMusic, GameManager.instance.endLevelMusicVolume);
        yield return new WaitForSeconds(GameManager.instance.endLevelMusic.length);
        player.GetComponent<Animator>().SetBool("Win", false);

        StartCoroutine(ScreenFader.instance.FadeOut(1));
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, levelEnd.playerEndLevelEndPosition, playerEndLevelSpeed))
        {
            yield return null;
        }
        GameManager.instance.hasCheckPoint = false;
        Loading.LoadScene(nextLevel);
    }

    public void ChangeMusic(int id)
    {
        if (otherMusics == null || otherMusics.Length > id || id<0)
            return;

        SoundManager.instance.PlayBGM(otherMusics[id], SoundManager.instance.currentBGMVolume);
    }
}
