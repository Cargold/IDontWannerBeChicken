using System.Collections;
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

    public float attackRate_Init;
    public bool isAttackClear;

    public bool isMovable;

    public void BattleEnter_Func()
    {
        Instance = this;

        base.Init_Func(GroupType.Ally);
        InitPlayer_Func();

        isMovable = true;
    }

    void InitPlayer_Func()
    {
        // Set Renderer
        hpRend_Group.gameObject.SetActive(true);
        shadowRend.gameObject.SetActive(true);

        // Set Status
        isAlive = true;
        SetState_Func(CharacterState.Move);

        // Set Attack
        attackRate_Speed = attackRate_Init / attackRate_Max;
        StartCoroutine(base.CheckAttackRate_Cor());
        StartCoroutine(this.CheckAttack_Cor());
    }

    protected override void Idle_Func()
    {
        charState = CharacterState.Idle;

        isAttackClear = true;

        animator.speed = 1f;
        animator.Play("Idle");
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

        if(isMovable == true)
            MovePlayer_Func();
    }
    void MovePlayer_Func()
    {
        if (moveDir == MoveDir.Left)
        {
            SetState_Func(CharacterState.Move);

            float _calcMove = this.transform.position.x + (-1f * moveSpeed * Time.deltaTime);

            if (_calcMove < limitPos_Left)
                _calcMove = limitPos_Left;

            this.transform.position = Vector3.right * _calcMove;

            Enviroment_Manager.Instance.OnDevastated_Func(this.transform.position.x);
        }
        else if (moveDir == MoveDir.Right && CheckRange_Func(1f) == false)
        {
            SetState_Func(CharacterState.Move);

            float _calcMove = this.transform.position.x + (moveSpeed * Time.deltaTime);

            if (limitPos_Right < _calcMove)
                _calcMove = limitPos_Right;

            this.transform.position = Vector3.right * _calcMove;

            Enviroment_Manager.Instance.OnWoody_Func(this.transform.position.x);
        }
    }
    protected override void Move_Func()
    {
        charState = CharacterState.Move;

        animator.speed = 1f;
        animator.Play("Move");
    }

    protected override void Attack_Func()
    {
        charState = CharacterState.Attack;

        animator.speed = attackRate_Speed;
        animator.Play("Attack");
    }
    protected override IEnumerator CheckAttack_Cor()
    {
        isAttackClear = true;

        while (isAlive == true)
        {
            // 내가 살아있다면

            if(charState != CharacterState.Move)
            {
                if (CheckTargetAlive_Func() == true)
                {
                    // 목표대상이 살아있다면

                    if (CheckRange_Func() == true)
                    {
                        // 목표대상이 사정권 내에 있다면

                        if (animator.GetBool("AttackReady") == true && isAttackClear == true)
                        {
                            isAttackClear = false;

                            SetState_Func(CharacterState.Attack);
                        }
                    }
                }
            }
            else if (moveDir == MoveDir.None)
            {
                SetState_Func(CharacterState.Idle);
            }

            yield return null;
        }
    }
    public override void AniEvent_OnAttack_Func()
    {
        // Call : Ani Event

        if (CheckTargetAlive_Func() == true)
        {
            // 목표대상이 살아있다면

            if (CheckRange_Func() == true)
            {
                // 목표대상이 사정권 내에 있다면

                if (animator.GetBool("AttackReady") == true)
                {
                    animator.SetBool("AttackReady", false);
                    attackRate_Recent = 0f;

                    float _attackValue_Calc = attackValue;
                    if (Random.Range(0f, 100f) < criticalPercent)
                    {
                        _attackValue_Calc *= criticalBonus;
                    }

                    contactCharClassList[0].Damaged_Func(_attackValue_Calc);
                    isAttackClear = true;
                }
                else
                    SetState_Func(CharacterState.Idle);
            }
            else
                SetState_Func(CharacterState.Idle);
        }
        else
            SetState_Func(CharacterState.Idle);
    }

    public override void Die_Func(bool _isImmediate = false)
    {
        isAlive = false;
        charState = CharacterState.Die;
        contactCharClassList.Clear();

        hpRend_Group.gameObject.SetActive(false);

        Battle_Manager.Instance.GameOver_Func(false);
    }

    public void AniEvnet_AttackEnd_Func()
    {
        SetState_Func(CharacterState.Idle);
    }
}
