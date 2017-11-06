using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character_Script : MonoBehaviour
{
    public int unitID;
    public string charName;
    public string charDesc;

    public bool isAlive = false;
    public float healthPoint_Max;
    [SerializeField]
    protected float healthPoint_Recent;
    [Range(0f, 99.9f)]
    public float defenceValue;
    [SerializeField]
    protected float defenceValue_Calc;
    public float attackValue;
    [SerializeField]
    private float attackValue_Calc;
    public float attackRate_Speed;
    public float attackRate_Max;
    [SerializeField]
    protected float attackRate_Recent;
    public float attackRange;
    public float moveSpeed;
    public float criticalPercent;
    public float criticalBonus;
    public ShootType shootType;
    public float shootTime;
    public float shootHeight;
    public GameObject shellObj;
    public Transform shellPivotTrf;
    public Shell_Script spawnShellClass;
    public AttackType attackType;
    public bool isContactAttackTiming;
    public float pluralRange;
    private Vector3 targetPos;
    [SerializeField]
    protected List<Character_Script> contactCharClassList = new List<Character_Script>();

    public enum CharacterState
    {
        None = -1,
        Idle,
        Move,
        Attack,
        Die,
    }
    public CharacterState charState;

    // Info Data
    public GroupType groupType;

    // Rendering Data
    public Animator animator;
    public SpriteRenderer unitRend;
    public SpriteRenderer shadowRend;
    public Transform hpRend_Group;
    public Transform hpTrf;
    public Sprite unitSprite;
    public float imagePivotAxisY;
    public Vector2 shadowSize;

    public bool isPlayer;
    public bool isHouse;

    public CharEffectData effectData_AttackStart;
    public CharEffectData effectData_AttackAniOn;
    public CharEffectData effectData_Die;

    public bool isControlOut = false;

    [System.Serializable]
    public struct KnockBackData
    {
        public bool isHave;
        public float power;
        public float height;
        public float time;
        public Character_Script hitterClass;
        public Vector3 knockingStartPos;
        public bool isKnockBackState;
    }
    public KnockBackData knockBackData;
    [System.Serializable]
    public struct StunData
    {
        public bool isHave;
        public float time;
        public Character_Script hitterClass;
        public bool isStunState;
    }
    public StunData stunData;
    private Character_Script hitterClass;

    protected void Init_Func(GroupType _groupType)
    {
        groupType = _groupType;
        
        // Init Renderer
        animator = this.GetComponent<Animator>();
        hpTrf = this.transform.Find("Pivot").Find("HP_Group").Find("Gauge");
        hpRend_Group = this.transform.Find("Pivot").Find("HP_Group");
        hpRend_Group.gameObject.SetActive(false);
        shadowRend = this.transform.Find("Pivot").Find("Shadow").GetComponent<SpriteRenderer>();
        shadowRend.gameObject.SetActive(false);

        // Init HP
        healthPoint_Recent = healthPoint_Max;
        CalcHP_Func();
        
        // Init Attack
        defenceValue_Calc = 1f - (defenceValue * 0.01f);

        // Init Var
        RestoreControl_Func();
    }
    public void OnLanding_Func()
    {
        if (this.gameObject.activeSelf == false) return;

        // Set Renderer
        hpRend_Group.gameObject.SetActive(true);
        shadowRend.gameObject.SetActive(true);
        if(isHouse == false && isPlayer == false)
            unitRend.sortingOrder = (int)(this.transform.position.y * -100f) + 210;
        if (isHouse == false && isPlayer == false)
            shadowRend.sortingOrder = (int)(this.transform.position.y * -100f) + 200;

        // Set Status
        isAlive = true;
        SetState_Func(CharacterState.Move);

        // Set Attack
        StartCoroutine(CheckAttackRate_Cor());
        if(isPlayer == false)
            StartCoroutine(CheckAttack_Cor());
    }

    protected void SetState_Func(CharacterState _charState)
    {
        switch (_charState)
        {
            case CharacterState.Idle:
                Idle_Func();
                break;
            case CharacterState.Move:
                Move_Func();
                break;
            case CharacterState.Attack:
                Attack_Func();
                break;
            case CharacterState.Die:
                Die_Func();
                break;
        }
    }

    protected virtual void Idle_Func()
    {
        animator.speed = 1f;
        charState = CharacterState.Idle;
    }
    protected virtual void Move_Func()
    {
        animator.speed = 1f;
        charState = CharacterState.Move;
    }
    
    void OnTriggerEnter(Collider col)
    {
        OnContact_Func(col);
    }
    void OnTriggerExit(Collider col)
    {
        OnRunAway_Func(col);
    }

    void OnRunAway_Func(Collider col)
    {
        if(col.tag == "Character")
        {
            Character_Script _charClass = col.GetComponent<Character_Script>();

            if(contactCharClassList.Contains(_charClass) == true)
            {
                contactCharClassList.Remove(_charClass);
            }
        }
    }
    void OnContact_Func(Collider col)
    {
        if (col.tag == "Character")
        {
            Character_Script _charClass = col.GetComponent<Character_Script>();

            if (_charClass.groupType != this.groupType)
            {
                if(contactCharClassList.Contains(_charClass) == false)
                {
                    contactCharClassList.Add(col.GetComponent<Character_Script>());
                }
            }
        }
    }

    protected virtual void Attack_Func()
    {
        if (charState != CharacterState.Attack)
        {
            animator.speed = attackRate_Speed;
            charState = CharacterState.Attack;
        }
    }
    protected virtual IEnumerator CheckAttack_Cor()
    {
        while(isAlive == true)
        {
            // 내가 살아있다면

            if (isControlOut == false)
            {
                if (CheckTargetAlive_Func() == true)
                {
                    // 목표대상이 살아있다면

                    if (CheckRange_Func() == true)
                    {
                        // 목표대상이 사정권 내에 있다면

                        if (charState != CharacterState.Attack)
                        {
                            attackValue_Calc = attackValue;
                            if (Random.Range(0f, 100f) < criticalPercent)
                            {
                                attackValue_Calc *= criticalBonus;
                            }

                            animator.SetBool("OnContact", true);
                            SetState_Func(CharacterState.Attack);

                            if (effectData_AttackStart.isEffectOn == true)
                            {
                                GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_AttackStart.effectObj);
                                _effectObj.transform.position = effectData_AttackStart.effectPos.position;

                                if (effectData_AttackStart.isSetParentTrf == true)
                                {
                                    _effectObj.transform.SetParent(effectData_AttackStart.effectPos);
                                    _effectObj.transform.rotation = effectData_AttackStart.effectPos.rotation;
                                }
                            }
                        }
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
                        SetState_Func(CharacterState.Move);
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
                        SetState_Func(CharacterState.Move);
                }
            }
            else if(isControlOut == true)
            {
                SetState_Func(CharacterState.Idle);
            }

            yield return null;
        }
    }
    protected IEnumerator CheckAttackRate_Cor()
    {
        animator.SetBool("AttackReady", true);
        attackRate_Recent = 0f;

        while (isAlive == true)
        {
            if(isControlOut == false)
            {
                if(animator.GetBool("AttackReady") == false)
                {
                    if (attackRate_Recent < attackRate_Max)
                    {
                        attackRate_Recent += 0.02f;
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        animator.SetBool("AttackReady", true);
                        attackRate_Recent = 0f;
                    }
                }
            }

            yield return null;
        }
    }
    protected bool CheckRange_Func(float _checkValue = -1f)
    {
        if(_checkValue == -1f)
        {
            _checkValue = attackRange;
        }

        Character_Script _closerCharClass = null;
        float _closerCharDistance = 0f;
        int _closeCharID = 0;

        for (int i = 0; i < contactCharClassList.Count; i++)
        {
            // 지나친 대상일 경우 무시
            if(groupType == GroupType.Ally)
            {
                if(contactCharClassList[i].transform.position.x < this.transform.position.x)
                {
                    continue;
                }
            }
            else if(groupType == GroupType.Enemy)
            {
                if (this.transform.position.x < contactCharClassList[i].transform.position.x)
                {
                    continue;
                }
            }

            // 거리 체크
            float _distanceValue = Vector3.Distance(this.transform.position, contactCharClassList[i].transform.position);

            // 처음 체크하는 대상이거나, 기존 최단 근접 대상보다 가까운 경우
            if (_closerCharClass == null || _distanceValue < _closerCharDistance)
            {
                _closerCharDistance = _distanceValue;
                _closerCharClass = contactCharClassList[i];
                _closeCharID = i;

                // 체크된 대상이 공격사거리(또는 인자값)보다 짧은 경우
                if (_distanceValue <= _checkValue)
                {
                    break;
                }
            }
        }

        // 체크된 대상이 있으며, 체크된 대상이 공격사거리(또는 인자값)보다 짧은 경우
        if (_closerCharClass != null && _closerCharDistance <= _checkValue)
        {
            Character_Script _tempClass = contactCharClassList[0];
            contactCharClassList[0] = _closerCharClass;
            contactCharClassList[_closeCharID] = _tempClass;

            return true;
        }

        return false;
    }
    protected bool CheckTargetAlive_Func()
    {
        bool isTargetOn = false;

        while (0 < contactCharClassList.Count)
        {
            if (contactCharClassList[0].isAlive == false)
            {
                contactCharClassList.Remove(contactCharClassList[0]);
            }
            else
            {
                isTargetOn = true;
                break;
            }
        }

        return isTargetOn;
    }

    private void CalcHP_Func()
    {
        float _remainPer = healthPoint_Recent / healthPoint_Max;
        if (_remainPer <= 0f)
            _remainPer = 0f;

        float _remainPos = 0.35f * (_remainPer - 1f);

        hpTrf.localPosition = new Vector2(_remainPos * -1f, hpTrf.localPosition.y);
        hpTrf.localScale = new Vector2(_remainPer, hpTrf.localScale.y);
    }
    public virtual void Damaged_Func(float _damageValue)
    {
        _damageValue *= defenceValue_Calc;

        healthPoint_Recent -= _damageValue;

        if (0 < healthPoint_Recent)
        {
            CalcHP_Func();
        }
        else
        {
            CalcHP_Func();
            SetState_Func(CharacterState.Die);
        }
    }
    public virtual void Die_Func(bool _isImmediate = false)
    {
        isAlive = false;
        charState = CharacterState.Die;
        contactCharClassList.Clear();

        StopCoroutine("Move_Cor");

        hpRend_Group.gameObject.SetActive(false);

        if(_isImmediate == false)
        {
            if (effectData_Die.isEffectOn == true)
            {
                GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_Die.effectObj);
                _effectObj.transform.position = effectData_Die.effectPos.position;
                _effectObj.transform.eulerAngles = new Vector3(270f, 0f, 0f);
            }
        }

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);

        if(groupType == GroupType.Enemy)
            Battle_Manager.Instance.CountKillMonster_Func(unitID);
    }

    #region Animation Group
    public virtual void AniEvent_OnAttack_Func()
    {
        // Call : Ani Event

        if (animator.GetBool("AttackReady") == true)
        {
            // 공격이 준비된 상태라면

            animator.SetBool("AttackReady", false);
            attackRate_Recent = 0f;

            if (effectData_AttackAniOn.isEffectOn == true)
            {
                GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_AttackAniOn.effectObj);
                _effectObj.transform.position = effectData_AttackAniOn.effectPos.position;

                if (effectData_AttackAniOn.isSetParentTrf == true)
                {
                    _effectObj.transform.SetParent(effectData_AttackAniOn.effectPos);
                    _effectObj.transform.rotation = effectData_AttackAniOn.effectPos.rotation;
                }
            }

            if (shootType == ShootType.RelativeAnimation)
            {
                if (CheckTargetAlive_Func() == true)
                {
                    // 목표대상이 살아있다면

                    if (CheckRange_Func() == true)
                    {
                        // 목표대상이 사정권 내에 있다면

                        OnAttack_Func();
                    }
                }
            }
            else if(shootType == ShootType.Projectile)
            {
                GameObject _spawnShellObj = ObjectPool_Manager.Instance.Get_Func(shellObj.name);
                _spawnShellObj.transform.SetParent(shellPivotTrf);
                _spawnShellObj.transform.position = shellPivotTrf.position;
                _spawnShellObj.transform.rotation = shellPivotTrf.rotation;

                spawnShellClass = _spawnShellObj.GetComponent<Shell_Script>();
                int _sortingOrder = (int)(this.transform.position.y * -100f) + 209;
                spawnShellClass.Init_Func(this, _sortingOrder);

                //float _shootTime = shootTime;
                //if (shootHeight == 0)
                //{
                //    float _distance = Vector3.Distance(this.transform.position, contactCharClassList[0].transform.position);
                //    _shootTime *= _distance / attackRange;
                //}
                
                if(isContactAttackTiming == false)
                {
                    targetPos = contactCharClassList[0].transform.position;
                }
                else if(isContactAttackTiming == true)
                {
                    float _attackRange = attackRange;
                    if (groupType == GroupType.Enemy)
                        _attackRange *= -1f;

                    targetPos = new Vector3(shellPivotTrf.position.x + _attackRange, shellPivotTrf.position.y, shellPivotTrf.position.z);
                }

                if (attackType == AttackType.Single)
                {
                    if (isContactAttackTiming == false)
                    {
                        bool _isRealShot = false;

                        if (CheckTargetAlive_Func() == true)
                        {
                            // 목표대상이 살아있다면

                            if (CheckRange_Func() == true)
                            {
                                // 목표대상이 사정권 내에 있다면

                                _isRealShot = true;
                            }

                            if (_isRealShot == true)
                            {
                                _spawnShellObj.transform.DOJump(targetPos, shootHeight, 1, shootTime).SetEase(Ease.Linear).OnComplete(OnAttack_Func);
                            }
                            else if (_isRealShot == false)
                            {
                                _spawnShellObj.transform.DOJump(targetPos, shootHeight, 1, shootTime).SetEase(Ease.Linear).OnComplete(spawnShellClass.Deactive_Func);
                            }
                        }
                    }
                    else if(isContactAttackTiming == true)
                    {
                        _spawnShellObj.transform.DOJump(targetPos, shootHeight, 1, shootTime).SetEase(Ease.Linear).OnComplete(spawnShellClass.Deactive_Func);
                    }
                }
                else if (attackType == AttackType.Plural)
                {
                    if (isContactAttackTiming == false)
                    {
                        _spawnShellObj.transform.DOJump(targetPos, shootHeight, 1, shootTime).SetEase(Ease.Linear).OnComplete(spawnShellClass.OnAttack_Func);
                    }
                    else if (isContactAttackTiming == true)
                    {
                        _spawnShellObj.transform.DOJump(targetPos, shootHeight, 1, shootTime).SetEase(Ease.Linear).OnComplete(spawnShellClass.Deactive_Func);
                    }
                }
            }
            else if(shootType == ShootType.FixedRange)
            {
                if (attackType == AttackType.Plural)
                {
                    GameObject _spawnShellObj = ObjectPool_Manager.Instance.Get_Func(shellObj.name);
                    _spawnShellObj.transform.position = shellPivotTrf.position;
                    _spawnShellObj.transform.rotation = shellPivotTrf.rotation;

                    spawnShellClass = _spawnShellObj.GetComponent<Shell_Script>();
                    spawnShellClass.Init_Func(this, 0);
                            
                    spawnShellClass.OnAttack_Func();
                }
                else
                {
                    Debug.LogError("Bug : 고정된 사거리 방식인 경우, 광역 공격만 가능합니다.");
                }
            }
        }
    }
    private void OnAttack_Func()
    {
        contactCharClassList[0].Damaged_Func(attackValue_Calc);

        knockBackData.hitterClass = this;
        contactCharClassList[0].CheckKnockBack_Func(knockBackData);
    }
    public void OnAttackPlural_Func(Character_Script _targetCharClass, bool _isDistanceClear = false)
    {
        if(shootType == ShootType.Projectile)
        {
            if(_isDistanceClear == false)
            {
                Vector3 _targetPos = _targetCharClass.transform.position;
                _targetPos = new Vector3(_targetPos.x, 0f, 0f);
                float _distanceValue = Vector3.Distance(targetPos, _targetPos);

                if (_distanceValue < pluralRange)
                {
                    _targetCharClass.Damaged_Func(attackValue_Calc);
                }
            }
            else if(_isDistanceClear == true)
            {
                _targetCharClass.Damaged_Func(attackValue_Calc);
            }
        }
        else if(shootType == ShootType.FixedRange)
        {
            Vector3 _pivotPos = shellPivotTrf.position;
            Vector3 _targetPos = _targetCharClass.transform.position;
            _targetPos = new Vector3(_targetPos.x, 0f, 0f);

            float _distanceValue = Vector3.Distance(_pivotPos, _targetPos);

            if(_distanceValue < pluralRange)
            {
                _targetCharClass.Damaged_Func(attackValue_Calc);

                knockBackData.hitterClass = this;
                _targetCharClass.CheckKnockBack_Func(knockBackData);
            }
        }
    }
    public void SelfDie_Func()
    {
        Die_Func(true);
    }
    #endregion
    #region Skill Group
    void CheckKnockBack_Func(KnockBackData _knockbackData)
    {
        if(isPlayer == false && isHouse == false)
        {
            if (_knockbackData.isHave == true)
            {
                this.KnockBack_Func(_knockbackData);
            }
        }
    }
    public void KnockBack_Func(KnockBackData _knockbackData)
    {
        isControlOut = true;
        animator.SetBool("OnContact", false);
        animator.SetBool("AttackReady", false);
        attackRate_Recent = 0f;
        knockBackData.isKnockBackState = true;

        hitterClass = _knockbackData.hitterClass;

        float _power = _knockbackData.power;
        if (groupType == GroupType.Enemy)
            _power *= 1f;

        Vector3 _powerPos = this.transform.position;
        if(knockBackData.isKnockBackState == false)
        {
            knockBackData.knockingStartPos = this.transform.position;
            _powerPos = new Vector3(_powerPos.x + _power, _powerPos.y, _powerPos.z);
        }
        else
        {
            float _startPosY = this.transform.position.y - knockBackData.knockingStartPos.y;
            _powerPos = new Vector3(_powerPos.x + _power, _powerPos.y - _startPosY, _powerPos.z);
        }
        
        this.transform.DOJump(_powerPos, _knockbackData.height, 1, _knockbackData.time).OnComplete(CheckStun_Func);
    }
    void CheckStun_Func()
    {
        if (isPlayer == false && isHouse == false)
        {
            StunData _stunData = hitterClass.stunData;

            if (_stunData.isHave == true)
            {
                if(this.gameObject.activeInHierarchy == true)
                {
                    StopCoroutine("StunTime_Cor");
                    StartCoroutine("StunTime_Cor", _stunData.time);
                }
            }
            else
            {
                knockBackData.isKnockBackState = false;
                RestoreControl_Func();
            }
        }
        else
        {
            knockBackData.isKnockBackState = false;
            RestoreControl_Func();
        }
    }
    IEnumerator StunTime_Cor(float _stunTime)
    {
        yield return new WaitForSeconds(_stunTime);

        stunData.isStunState = false;
        RestoreControl_Func();
    }
    public void Stun_Func()
    {
        isControlOut = true;
        animator.SetBool("OnContact", false);
        animator.SetBool("AttackReady", false);
        attackRate_Recent = 0f;

        stunData.isStunState = true;
    }
    public void RestoreControl_Func()
    {
        if(knockBackData.isKnockBackState == false && stunData.isStunState == false)
        {
            isControlOut = false;
            animator.SetBool("OnContact", true);
        }
    }
    #endregion
}
