using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public bool inBattle = false;

    public bool getInBattle(){
        return this.inBattle;
    }

    public void setInBattle(bool b){
        this.inBattle = b;
    }
    
}
