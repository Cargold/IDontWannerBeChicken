using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : Character_Script
{
    public Transform spawnPos;
    public enum MoveDir
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

    [SerializeField]
    private int isControlOutCount_Player;

    public Transform shieldGroupTrf;
    public Transform shieldGaugeTrf;
    public float shieldValue_Max;
    public float shieldValue_Recent;

    public void BattleEnter_Func()
    {
        base.Init_Func(GroupType.Ally);
        InitPlayer_Func();

        isControlOutCount_Player = 0;
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
        if (isControlOutCount_Player == 0)
            moveDir = MoveDir.Left;
    }
    public void MoveRight_Func()
    {
        if (isControlOutCount_Player == 0)
            moveDir = MoveDir.Right;
    }
    public void MoveOver_Func()
    {
        if (isControlOutCount_Player == 0)
            moveDir = MoveDir.None;
    }
    void Update()
    {
        if(isControlOutCount_Player == 0)
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
        }

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
        else if (moveDir == MoveDir.Right && GetCollideCheck_Func(1f) == false)
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

                    if (GetCollideCheck_Func() == true)
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

            if (GetCollideCheck_Func() == true)
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

    public override void Damaged_Func(float _damageValue)
    {
        if(0f < shieldValue_Recent)
        {
            _damageValue *= defenceValue_Calc;

            shieldValue_Recent -= _damageValue;
            CalcShieldGauge_Func();
        }
        else
        {
            base.Damaged_Func(_damageValue);
        }
    }
    private void CalcShieldGauge_Func()
    {
        float _remainPer = shieldValue_Recent / shieldValue_Max;
        if (_remainPer <= 0f)
        {
            _remainPer = 0f;
            shieldGroupTrf.gameObject.SetActive(false);
        }

        float _remainPos = 0.35f * (_remainPer - 1f);

        shieldGaugeTrf.localPosition = new Vector2(_remainPos * -1f, shieldGaugeTrf.localPosition.y);
        shieldGaugeTrf.localScale = new Vector2(_remainPer, shieldGaugeTrf.localScale.y);
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

    #region Skill Group
    public void SetControlOut_Func(bool _isOn)
    {
        if (_isOn == true)
        {
            isControlOutCount_Player++;
        }
        else if (_isOn == false)
        {
            isControlOutCount_Player--;

            if(isControlOutCount_Player < 0)
            {
                Debug.LogError("Bug : 플레이어 상태가 비정상입니다.");
            }
        }
    }
    #region Rush Group
    public void SetShield_Func(float _maxValue)
    {
        if(0f < _maxValue)
        {
            shieldValue_Max = _maxValue;
            shieldValue_Recent = shieldValue_Max;

            shieldGaugeTrf.localScale = Vector2.one;
            shieldGroupTrf.gameObject.SetActive(true);
        }
        else if(_maxValue == 0f)
        {
            shieldValue_Recent = 0f;

            shieldGaugeTrf.localScale = Vector2.zero;
            shieldGroupTrf.gameObject.SetActive(false);
        }
    }
    public void SetMove_Func(MoveDir _moveDir, float _setSpeed)
    {
        moveDir = _moveDir;

        moveSpeed = _setSpeed;
    }
    #endregion
    #endregion
}
