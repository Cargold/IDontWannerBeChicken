using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSystem_Manager : MonoBehaviour
{
    public static SkillSystem_Manager Instance;

    public Skill_Parent[] skillClassArr;

    public Skill_Parent[] playerSkillClassArr;
    public SkillBtn_Script[] skillBtnClassArr;
    public Color skillBtnCoolTimeColor;

    public float mana_Max;
    public float mana_Recent;
    public float manaRegen;
    public bool isActive = false;

    public Image manaImage;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        for (int i = 0; i < skillClassArr.Length; i++)
        {
            skillClassArr[i].Init_Func();
        }

        for (int i = 0; i < 5; i++)
        {
            skillBtnClassArr[i].Init_Func(this, i);
        }

        yield break;
    }

    public void Active_Func()
    {
        isActive = true;

        for (int i = 0; i < 5; i++)
        {
            skillBtnClassArr[i].Active_Func(playerSkillClassArr[i]);
        }

        StartCoroutine("ManaRegen_Cor");
    }

    IEnumerator ManaRegen_Cor()
    {
        while(isActive == true)
        {
            SetMana_Func(manaRegen * 0.02f);
            
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }
    void SetMana_Func(float _regenValue, bool _isMinus = false)
    {
        if(_isMinus == false)
        {
            mana_Recent += _regenValue;
            if (mana_Max < mana_Recent)
            {
                mana_Recent = mana_Max;
            }
        }
        else if(_isMinus == true)
        {
            mana_Recent -= _regenValue;
            if (mana_Recent < 0f)
            {
                mana_Recent = 0f;
                Debug.LogError("Bug : 아니 근데 이런 상황이 나올 순 있는 거임? 앞에서 체크하는데?");
            }
        }
        
        manaImage.fillAmount = mana_Recent / mana_Max;
    }

    public bool CheckSkillUse_Func(int _slotID)
    {
        bool _isManaOn = false;
        float _manaCost = playerSkillClassArr[_slotID].manaCost;

        if (_manaCost < mana_Recent)
        {
            _isManaOn = true;
        }

        return _isManaOn;
    }
    public void UseSkill_Func(int _slotID)
    {
        float _manaCost = playerSkillClassArr[_slotID].manaCost;
        SetMana_Func(_manaCost, true);
    }

    public void Deactive_Func()
    {
        isActive = false;

        StopCoroutine("ManaRegen_Cor");

        for (int i = 0; i < 5; i++)
        {
            skillBtnClassArr[i].Deactive_Func();
        }
    }
}
