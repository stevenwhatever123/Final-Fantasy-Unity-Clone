using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int maxHealth;
    public int health = 100;
    public bool hit = false;
    public int hitCount = 0;
    public bool dead;
    Animator animator;
    CharacterController characterController;
    
    void Start(){
        maxHealth = health;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        impact();
        deadAnimation();
    }

    public CharacterController getCharacterController(){
        return this.characterController;
    }

    public void damaged(int damage){
        this.health -= damage;
        hitCount++;
        animator.SetInteger("health", health);
        if(health < 1){
            dead = true;
            characterController.enabled = false;
            animator.SetBool("dead", dead);
        }
        hit = true;
        animator.SetBool("hit", hit);
    }

    public int getMaxHealth(){
        return this.maxHealth;
    }

    public int getHealth(){
        return this.health;
    }

    void impact(){
        if(hit){
            animator.SetBool("hit", hit);
            if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Impact")){
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                    hit = false;
                    animator.SetBool("hit", hit);
                }
            }
        }
    }

    void deadAnimation(){
        if(dead){
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                Object.Destroy(this.gameObject);
            }
        }
    }

}
