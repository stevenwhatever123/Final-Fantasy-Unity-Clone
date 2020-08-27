using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDetector : MonoBehaviour
{

    public GameControllerScript gameController;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            gameController.setInBattle(true);
            gameController.setEnemy(gameObject.transform.root.gameObject);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            gameController.setInBattle(false);  
            gameController.setEnemy(null);
        }
    }


}
