﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : Character_Script
{
    public static Player_Script Instance;
    public Transform spawnPos;
    enum MoveDir
    {
        None,
        Left,
        Right,
    }
    [SerializeField]
    private MoveDir moveDir;

    public float limitPos_Left;
    public float limitPos_Right;

    public void BattleEnter_Func()
    {
        Instance = this;

        base.Init_Func(GroupType.Ally);
    }

    protected override void Idle_Func()
    {
        //if (CheckTargetAlive_Func() == false)
        //    charState = CharacterState.Idle;
        //else
        //    SetState_Func(CharacterState.Attack);
    }

    public void MoveLeft_Func()
    {
        moveDir = MoveDir.Left;
    }
    public void MoveRight_Func()
    {
        moveDir = MoveDir.Right;
    }
    public void MoveOver_Func()
    {
        moveDir = MoveDir.None;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A) == true)
        {
            moveDir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            moveDir = MoveDir.Right;
        }
        else if (Input.GetKeyUp(KeyCode.A) == true || Input.GetKeyUp(KeyCode.D) == true)
        {
            moveDir = MoveDir.None;
        }

        MovePlayer_Func();
    }
    void MovePlayer_Func()
    {
        if (moveDir == MoveDir.Left)
        {
            if (charState != CharacterState.Move)
            {
                SetState_Func(CharacterState.Move);
            }

            float _calcMove = this.transform.position.x + (-1f * moveSpeed * Time.deltaTime);

            if (_calcMove < limitPos_Left)
                _calcMove = limitPos_Left;

            this.transform.position = Vector3.right * _calcMove;
        }
        else if (moveDir == MoveDir.Right)
        {
            if (charState != CharacterState.Move)
            {
                SetState_Func(CharacterState.Move);
            }

            float _calcMove = this.transform.position.x + (moveSpeed * Time.deltaTime);

            if (limitPos_Right < _calcMove)
                _calcMove = limitPos_Right;

            this.transform.position = Vector3.right * _calcMove;
        }
        else
        {
            if (charState == CharacterState.Move)
                SetState_Func(CharacterState.Idle);
        }
    }
    protected override void Move_Func()
    {
        charState = CharacterState.Move;
    }

    protected override void Attack_Func()
    {
        charState = CharacterState.Attack;

        animator.SetBool("AttackReady", false);
        attackRate_Recent = 0f;

        float _attackValue_Calc = attackValue;
        if (Random.Range(0f, 100f) < criticalPercent)
        {
            _attackValue_Calc *= criticalBonus;
        }

        targetClassList[0].Damaged_Func(_attackValue_Calc);
    }

    protected override void Die_Func()
    {
        isAlive = false;
        charState = CharacterState.Die;
        targetClassList.Clear();

        Battle_Manager.Instance.GameOver_Func(false);
    }
}
