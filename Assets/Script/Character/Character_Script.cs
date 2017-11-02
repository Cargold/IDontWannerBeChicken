﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Script : MonoBehaviour
{
    public int charId;
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
    public float attackRate_Max;
    [SerializeField]
    protected float attackRate_Recent;
    public float attackRange;
    public float moveSpeed;
    public float criticalPercent;
    public float criticalBonus;
    public ShootType shootType;
    public float shootSpeed;
    public float shootHeight;
    public AttackType attackType;
    [SerializeField]
    protected List<Character_Script> targetClassList = new List<Character_Script>();

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
    public SpriteRenderer hpRend;
    public Transform hpTrf;
    public Sprite unitSprite;
    public float feedImageSize;
    public float imagePivotAxisY;
    public Vector2 shadowSize;

    protected void Init_Func(GroupType _groupType)
    {
        groupType = _groupType;

        isAlive = true;

        // Init Renderer
        animator = this.GetComponent<Animator>();
        if (unitRend == null)
            unitRend = this.transform.Find("Image").GetComponent<SpriteRenderer>();
        unitRend.sprite = unitSprite;
        hpTrf = this.transform.Find("HP_Group").Find("Gauge");
        hpRend = hpTrf.GetComponent<SpriteRenderer>();
        SetState_Func(CharacterState.Move);

        // Init HP
        healthPoint_Recent = healthPoint_Max;
        CalcHP_Func();

        // Init Attack
        StartCoroutine(CheckAttackRate_Cor());
        StartCoroutine(CheckAttack_Cor());
        defenceValue_Calc = 1f - (defenceValue * 0.01f);
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
        charState = CharacterState.Idle;
    }
    protected virtual void Move_Func()
    {
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

            if(targetClassList.Contains(_charClass) == true)
            {
                targetClassList.Remove(_charClass);
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
                if(targetClassList.Contains(_charClass) == false)
                {
                    targetClassList.Add(col.GetComponent<Character_Script>());
                }
            }
        }
    }

    protected virtual void Attack_Func()
    {
        if (charState != CharacterState.Attack)
            charState = CharacterState.Attack;
    }
    IEnumerator CheckAttack_Cor()
    {
        while(isAlive == true)
        {
            // 내가 살아있다면

            if (CheckTargetAlive_Func() == true)
            {
                // 목표대상이 살아있다면

                if (CheckRange_Func() == true)
                {
                    // 목표대상이 사정권 내에 있다면

                    if (charState != CharacterState.Attack)
                    {
                        animator.SetBool("OnContact", true);
                        SetState_Func(CharacterState.Attack);
                    }
                }
                else if (charState == CharacterState.Attack)
                    SetState_Func(CharacterState.Move);
            }
            else
            {
                if (charState == CharacterState.Attack)
                    SetState_Func(CharacterState.Move);
            }

            yield return null;
        }
    }
    IEnumerator CheckAttackRate_Cor()
    {
        animator.SetBool("AttackReady", true);
        attackRate_Recent = 0f;

        while (isAlive == true)
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
            else
            {
                yield return null;
            }
        }
    }
    bool CheckRange_Func()
    {
        Character_Script _closerCharClass = null;
        float _closerCharDistance = 0f;
        int _closeCharID = 0;

        for (int i = 0; i < targetClassList.Count; i++)
        {
            float _distanceValue = Vector3.Distance(this.transform.position, targetClassList[i].transform.position);

            if (_distanceValue < _closerCharDistance || _closerCharClass == null)
            {
                _closerCharDistance = _distanceValue;
                _closerCharClass = targetClassList[i];
                _closeCharID = i;
            }
        }

        if (_closerCharDistance <= attackRange && _closerCharClass != null)
        {
            Character_Script _tempClass = targetClassList[0];
            targetClassList[0] = _closerCharClass;
            targetClassList[_closeCharID] = _tempClass;

            return true;
        }

        return false;
    }
    protected bool CheckTargetAlive_Func()
    {
        bool isTargetOn = false;

        while (0 < targetClassList.Count)
        {
            if (targetClassList[0].isAlive == false)
            {
                targetClassList.Remove(targetClassList[0]);
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
        float _remainPos = 0.35f * (_remainPer - 1f);

        hpTrf.localPosition = new Vector2(_remainPos * -1f, hpTrf.localPosition.y);
        hpTrf.localScale = new Vector2(_remainPer, hpTrf.localScale.y);
    }
    public void Damaged_Func(float _damageValue)
    {
        _damageValue *= defenceValue_Calc;

        healthPoint_Recent -= _damageValue;

        if (0 < healthPoint_Recent)
        {
            CalcHP_Func();
        }
        else
        {
            SetState_Func(CharacterState.Die);
        }
    }
    protected virtual void Die_Func()
    {
        isAlive = false;
        charState = CharacterState.Die;
        targetClassList.Clear();

        // 사망 연출

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }

    #region Animation Group
    public GameObject effectObj;
    public virtual void AniEvent_OnAttack_Func()
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

                    targetClassList[0].Damaged_Func(_attackValue_Calc);

                    effectObj.SetActive(true);
                }
            }
        }
    }
    #endregion
}
