﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerScript : MonoBehaviour
{
    public GameControllerScript GameController;
    public MusicControllerScript musicController;
    public MainCharacterMovement characterMovement;
    public GameObject AnimationEvent;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Cursor.lockState = CursorLockMode.Locked;
            AnimationEvent.SetActive(true);
            characterMovement.allowWalk = false;
            GameController.setAllowInput(false);
            characterMovement.setIsRunning(false);
            characterMovement.setIsWalking(false);
            StartCoroutine(timeDelay());
        }
    }

    IEnumerator timeDelay(){
        yield return new WaitForSeconds(10.5f);
        Cursor.lockState = CursorLockMode.None;
        characterMovement.allowWalk = true;
        GameController.setAllowInput(true);
        Destroy(AnimationEvent);
        Object.Destroy(this.gameObject);
    }
}
