using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDetector : MonoBehaviour
{

    public GameControllerScript gameController;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            gameController.setInBattle(true);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            gameController.setInBattle(false);
        }
    }
}
