using System.Collections;
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
    [SerializeField]
    protected bool isAttackOn;
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
    public Image hpImage;
    public Text charNameText;

    protected void Init_Func(GroupType _groupType)
    {
        groupType = _groupType;

        isAlive = true;

        // Init HP
        healthPoint_Recent = healthPoint_Max;
        hpImage.fillAmount = 1f;

        // Init Attack
        StartCoroutine(CheckAttackRate_Cor());
        StartCoroutine(CheckAttack_Cor());
        defenceValue_Calc = 1f - (defenceValue * 0.01f);

        // Init Renderer
        charNameText.text = charName;

        SetState_Func(CharacterState.Move);
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
            if (CheckTargetAlive_Func() == true)
            {
                if (CheckRange_Func() == true)
                {
                    if (charState != CharacterState.Attack)
                        SetState_Func(CharacterState.Attack);

                    if(isAttackOn == true)
                    {
                        isAttackOn = false;
                        attackRate_Recent = 0f;

                        float _attackValue_Calc = attackValue;
                        if(Random.Range(0f, 100f) < criticalPercent)
                        {
                            _attackValue_Calc *= criticalBonus;
                        }

                        targetClassList[0].Damaged_Func(_attackValue_Calc);
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
        isAttackOn = true;
        attackRate_Recent = 0f;

        while (isAlive == true)
        {
            if(isAttackOn == false)
            {
                if (attackRate_Recent < attackRate_Max)
                {
                    attackRate_Recent += 0.02f;
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    isAttackOn = true;
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
        bool isRangeOn = false;
        Character_Script _closeCharClass = null;
        float _closeCharValue = 0f;
        int _closeCharID = 0;

        for (int i = 0; i < targetClassList.Count; i++)
        {
            float _distanceValue = Vector3.Distance(this.transform.position, targetClassList[i].transform.position);

            if (_distanceValue < _closeCharValue || _closeCharClass == null)
            {
                _closeCharValue = _distanceValue;
                _closeCharClass = targetClassList[i];
                _closeCharID = i;
            }
        }

        if (_closeCharValue <= attackRange)
        {
            Character_Script _tempClass = targetClassList[0];
            targetClassList[0] = _closeCharClass;
            targetClassList[_closeCharID] = _tempClass;

            isRangeOn = true;
        }

        return isRangeOn;
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

    public void Damaged_Func(float _damageValue)
    {
        _damageValue *= defenceValue_Calc;

        healthPoint_Recent -= _damageValue;

        if (0 < healthPoint_Recent)
        {
            hpImage.fillAmount = healthPoint_Recent / healthPoint_Max;
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

        // 사망 연출

        ObjectPoolManager.Instance.Free_Func(this.gameObject);
    }
}
