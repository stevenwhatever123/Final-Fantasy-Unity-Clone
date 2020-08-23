using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public GameControllerScript gameController;
    public int damage = 20;

    void OnTriggerEnter(Collider collision){
        if(collision.tag == "Enemy"){
            GameObject enemy = collision.gameObject;
            EnemyScript enemyData = (EnemyScript) collision.transform.GetComponent("EnemyScript");
            enemyData.damaged(damage);
            print("Health: " + enemyData.getHealth());
            if(enemyData.getHealth() == 0){
                MainCharacterMovement character = this.transform.root.GetComponent<MainCharacterMovement>();
                Destroy(enemy);
                gameController.setInBattle(false);
                character.setInBattle(false);
            }
        }

    }

}
