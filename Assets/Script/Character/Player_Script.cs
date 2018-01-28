using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player_Script : Character_Script
{
    [Header("Hero Data")]
    public bool isInit = false;
    public int heroLevel;
    public Transform spawnPos;
    public GameObject rendGroupObj;
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
    
    [SerializeField]
    private int isControlOutCount_Player;

    public Transform shieldGroupTrf;
    public Transform shieldGaugeTrf;
    public float shieldValue_Max;
    public float shieldValue_Recent;

    public float manaStart;
    public float manaRegen;

    // Rendering Var
    public Vector2 feedImagePos;
    public float feedImageSize;

    public void SetData_Func(Hero_Data _heroData)
    {
        // 풀링 될 때의 첫 데이터 세팅

        healthPoint_Max = _heroData.healthPoint;
        healthPoint_Recent = _heroData.healthPoint;
        defenceValue = _heroData.defenceValue;
        attackValue = _heroData.attackValue;
        attackRate_Max = _heroData.attackRate;
        attackRange = _heroData.attackRange;
        moveSpeed = _heroData.moveSpeed;
        criticalPercent = _heroData.criticalPercent;
        criticalBonus = _heroData.criticalBonus;
        shootType = _heroData.shootType;
        shootTime = _heroData.shootSpeed;
        shootHeight = _heroData.shootHeight;
        attackType = _heroData.attackType;

        manaStart = _heroData.manaStart;
        manaRegen = _heroData.manaRegen;
    }
    public void LobbyEnter_Func()
    {
        hpRend_Group.gameObject.SetActive(false);
        rendGroupObj.SetActive(true);
        shadowRend.gameObject.SetActive(true);

        moveDir = MoveDir.None;
        SetState_Func(CharacterState.Move);
        isControlOutCount_Player = 0;
    }
    public void BattleEnter_Func()
    {
        InitPlayer_Func();
    }
    void InitPlayer_Func()
    {
        isInit = true;

        base.Init_Func(GroupType.Ally);

        // Set Renderer
        rendGroupObj.SetActive(true);
        hpRend_Group.gameObject.SetActive(true);
        shadowRend.gameObject.SetActive(true);

        // Set Status
        isAlive = true;
        sphereCol.enabled = true;
        moveDir = MoveDir.None;
        SetState_Func(CharacterState.Move);
        isControlOutCount_Player = 0;

        // Set Attack
        attackRate_AniSpeed = 1f;
        StartCoroutine(base.CheckAttackRate_Cor());
        StartCoroutine(this.CheckAttack_Cor());
    }

    protected override void Idle_Func()
    {
        charState = CharacterState.Idle;
        
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
        if (isInit == false) return;

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
        if (Battle_Manager.Instance.battleState == Battle_Manager.BattleState.Result) return;

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

        animator.speed = attackRate_AniSpeed;
        animator.Play("Attack");
    }
    protected override IEnumerator CheckAttack_Cor()
    {
        while (isAlive == true)
        {
            // 내가 살아있다면

            if(charState != CharacterState.Move)
            {
                if (GetCollideCheck_Func() == true)
                {
                    // 목표대상이 사정권 내에 있다면

                    if (animator.GetBool("AttackReady") == true)
                    {
                        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
                            SetState_Func(CharacterState.Attack);
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
    //public override void AniEvent_OnAttack_Func()
    //{
    //    // Call : Ani Event

    //    if (GetCollideCheck_Func() == true)
    //    {
    //        // 목표대상이 사정권 내에 있다면

    //        if (animator.GetBool("AttackReady") == true)
    //        {
    //            animator.SetBool("AttackReady", false);
    //            attackRate_Recent = 0f;

    //            attackValue_Calc = attackValue;
    //            if (Random.Range(0f, 100f) < criticalPercent)
    //            {
    //                attackValue_Calc *= criticalBonus;
    //            }

    //            effectData_AttackAniOn.ActiveEffect_Func();

    //            // 범위 공격이라는 전제...
    //            Vector3 _contactCharPos = contactCharClassList[0].transform.position;
    //            targetPos = new Vector3(_contactCharPos.x, 0f, 0f);

    //            GameObject _spawnShellObj = ObjectPool_Manager.Instance.Get_Func(shellObj.name);
    //            _spawnShellObj.transform.position = targetPos;

    //            spawnShellClass = _spawnShellObj.GetComponent<Shell_Script>();
    //            int _sortingOrder = (int)(this.transform.position.y * -100f) + 209;
    //            spawnShellClass.Init_Func(this, _sortingOrder);

    //            spawnShellClass.OnAttack_Func();

    //            SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Fire);
    //        }
    //        else
    //            SetState_Func(CharacterState.Idle);
    //    }
    //    else
    //        SetState_Func(CharacterState.Idle);
    //}
    public override void AniEvent_OnAttack_Func()
    {
        // Call : Ani Event

        if (GetCollideCheck_Func() == true)
        {
            // 목표대상이 사정권 내에 있다면

            if (animator.GetBool("AttackReady") == true)
            {
                animator.SetBool("AttackReady", false);
                attackRate_Recent = 0f;

                attackValue_Calc = attackValue;
                if (Random.Range(0f, 100f) < criticalPercent)
                {
                    attackValue_Calc *= criticalBonus;
                }

                effectData_AttackAniOn.ActiveEffect_Func();

                // 범위 공격이라는 전제...
                targetPos = new Vector3(shellPivotTrf.position.x, 0f, 0f);

                GameObject _spawnShellObj = ObjectPool_Manager.Instance.Get_Func(shellObj.name);
                _spawnShellObj.transform.position = targetPos;

                spawnShellClass = _spawnShellObj.GetComponent<Shell_Script>();
                int _sortingOrder = (int)(this.transform.position.y * -100f) + 209;
                spawnShellClass.Init_Func(this, _sortingOrder);

                spawnShellClass.OnAttack_Func();

                SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Fire);
            }
            else
                SetState_Func(CharacterState.Idle);
        }
        else
            SetState_Func(CharacterState.Idle);
    }

    public override void Damaged_Func(float _damageValue)
    {
        if (charState == CharacterState.Die) return;

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
        sphereCol.enabled = false;
        charState = CharacterState.Die;
        contactCharClassList.Clear();

        hpRend_Group.gameObject.SetActive(false);
        rendGroupObj.SetActive(false);
        
        StartCoroutine(DieEffect_Cor());

        Battle_Manager.Instance.GameOver_Func(false);
    }
    IEnumerator DieEffect_Cor()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 _effectPos = new Vector3
                (
                    this.transform.position.x + Random.Range(-1f, 1f),
                    this.transform.position.y + Random.Range(-1f, 1f),
                    0f
                );

            effectData_Die.SetEffectPos(_effectPos);
            effectData_Die.ActiveEffect_Func();

            yield return null;
        }
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
            
            shieldGroupTrf.gameObject.SetActive(true);
        }
        else if(_maxValue == 0f)
        {
            shieldValue_Recent = 0f;
            
            shieldGroupTrf.gameObject.SetActive(false);
        }

        CalcShieldGauge_Func();
    }
    public void SetMove_Func(MoveDir _moveDir, float _setSpeed)
    {
        moveDir = _moveDir;

        moveSpeed = _setSpeed;
    }
    #endregion
    #endregion
}
