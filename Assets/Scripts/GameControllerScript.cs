using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public bool inBattle = false;
    public bool allowInput = true;

    public bool getInBattle(){
        return this.inBattle;
    }

    public void setInBattle(bool b){
        this.inBattle = b;
    }

    public bool getAllowInput(){
        return this.allowInput;
    }

    public void setAllowInput(bool b){
        this.allowInput = b;
    }
    
}
