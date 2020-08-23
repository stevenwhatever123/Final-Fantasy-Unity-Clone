using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{

    public int damage = 20;

    void OnTriggerEnter(Collider collision){
        if(collision.tag == "Enemy"){
            GameObject enemy = collision.gameObject;
            EnemyScript enemyData = (EnemyScript) collision.transform.GetComponent("EnemyScript");
            enemyData.damaged(damage);
            print("Health: " + enemyData.getHealth());
            if(enemyData.getHealth() == 0){
                Destroy(enemy);
            }
        }
    }

}
