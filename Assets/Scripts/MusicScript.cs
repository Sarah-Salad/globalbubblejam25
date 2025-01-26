using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicScript : MonoBehaviour
{
    public GameObject fishMusicObject;
    public GameObject seaMusicObject;
    private AudioSource fishMusic;
    private AudioSource seaMusic;

    private bool isPlayingFishMusic = false;
    private bool isPlayingSeaMusic = false;

    public string[] fishMusicScenes;
    public string[] seaMusicScenes;

    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        fishMusic = fishMusicObject.GetComponent<AudioSource>();
        seaMusic = seaMusicObject.GetComponent<AudioSource>();
        PlayMusic(fishMusic);
        isPlayingFishMusic = true;
    }

    public void PlayMusic(AudioSource music)
    {
        if (!music.isPlaying)
        {
        music.Play();
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StopMusic()
    {
        Debug.Log("stog music");
        if (isPlayingFishMusic == true)
        {
            fishMusic.Stop();
            isPlayingFishMusic = false;
        } else {
            seaMusic.Stop();
            isPlayingSeaMusic = false;
        }
    }

    public void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log("Scene changed");
        if (previousScene.name != newScene.name) 
        {
            if (((IList)fishMusicScenes).Contains(newScene.name) && isPlayingFishMusic == false)
            {
                StopMusic();
                PlayMusic(fishMusic);
                isPlayingFishMusic = true;
            } else if (isPlayingSeaMusic == false && !((IList)fishMusicScenes).Contains(newScene.name)) {
                StopMusic();
                PlayMusic(seaMusic);
                isPlayingSeaMusic = true;
            }
        }
    }
}
