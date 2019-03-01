using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public int monsterID;       //怪物ID
    public int HP;              //总血量
    public int currentHP;       //当前血量
    public float moveSpeed;     //当前移动速度
    public float initMoveSpeed; //初始移动速度
    public int prize;           //奖励

    private Animator animator;
    //public RuntimeAnimatorController runtimeAnimatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void GetMonsterProperty()
    {
        animator.runtimeAnimatorController = GameController.Instance.controllers[monsterID - 1];
    }
}
