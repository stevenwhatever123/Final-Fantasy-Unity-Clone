using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleAnimationController : MonoBehaviour
{
    public Animator animator;
    Random rnd = new Random();
    int luckyNumber;

    // Update is called once per frame
    void FixedUpdate()
    {
        CounterUpdate();
    }

    void CounterUpdate(){
        luckyNumber = (int) Random.Range(0f, 1000f);
        animator.SetInteger("IdleCounter", luckyNumber);
    }
}
