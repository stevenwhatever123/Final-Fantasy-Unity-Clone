using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicControllerScript : MonoBehaviour
{

    [Header("Audio Source")]
    public AudioSource audio;
    public AudioSource victoryAudio;

    [Header("Audio Clip")]
    public AudioClip normalClip;
    public AudioClip battleClip;
    public AudioClip victoryClip;
    float playBackTime;
    
    [Header("Game Variables")]
    public GameControllerScript gameController;
    public bool inBattle;
    public bool musicChanged;
    public bool climaxClipMusicChanged;
    public bool inClimax;

    // Start is called before the first frame update
    void Start()
    {
        audio.clip = normalClip;
        audio.Play();
    }

    void Update(){
        updateWithGameController();
        switchBattleMusic();
    }

    void updateWithGameController(){
        inBattle = gameController.getInBattle();
    }

    void switchBattleMusic(){
        if(inBattle && !musicChanged){
            playBackTime = audio.time;
            musicChanged = true;
            audio.clip = battleClip;
            audio.time = 0f;
            audio.Play();
        } else if(!inBattle && musicChanged){
            audio.clip = normalClip;
            audio.time = playBackTime;
            musicChanged = false;
            audio.Play();
        }
    }

    public void playVictoryClip(){
        victoryAudio.clip = victoryClip;
        victoryAudio.time = 0f;
        victoryAudio.Play();
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime(){
        yield return new WaitForSeconds(5.5f);
        victoryAudio.Stop();
    }

    public void setInClimax(bool b){
        this.inClimax = b;
    }

    public bool getInClimax(){
        return this.inClimax;
    }
}
