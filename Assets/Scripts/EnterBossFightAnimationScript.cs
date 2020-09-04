using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossFightAnimationScript : MonoBehaviour
{
    public EnemyScript character;
    public GameControllerScript GameController;
    public MainCharacterMovement characterMovement;
    public GameObject AnimationEvent;

    void Update()
    {
        if(character.getHealth() <= character.getMaxHealth()/2){
            playAnimation();
        }
    }

    void playAnimation(){
        Cursor.lockState = CursorLockMode.Locked;
        AnimationEvent.SetActive(true);
        characterMovement.allowWalk = false;
        GameController.setAllowInput(false);
        characterMovement.setIsRunning(false);
        characterMovement.setIsWalking(false);
        StartCoroutine(timeDelay());
    }

    IEnumerator timeDelay(){
        yield return new WaitForSeconds(4.75f);
        Cursor.lockState = CursorLockMode.None;
        characterMovement.allowWalk = true;
        GameController.setAllowInput(true);
        Destroy(AnimationEvent);
        Vector3 newPosition = new Vector3(-1.598f, 0.891f, 63.2f);
        character.gameObject.transform.position = newPosition;
        Object.Destroy(this.gameObject);
    }
}
