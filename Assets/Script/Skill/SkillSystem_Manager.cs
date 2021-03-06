﻿using System.Collections;
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

    public bool isActive = false;
    public float mana_Max;
    public float mana_Recent;

    public Image manaImage;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        for (int i = 0; i < skillClassArr.Length; i++)
        {
            skillClassArr[i] = Player_Data.Instance.skillDataArr[i].skillParentClass;
            skillClassArr[i].transform.SetParent(this.transform);
            skillClassArr[i].Init_Func();
        }

        for (int i = 0; i < 5; i++)
        {
            skillBtnClassArr[i].Init_Func(this, i);
        }

        yield break;
    }
    
    public void BattleEnter_Func()
    {
        isActive = true;

        for (int i = 0; i < 5; i++)
        {
            int _selectSkillID = Player_Data.Instance.selectSkillIDArr[i];

            if (0 <= _selectSkillID)
            {
                playerSkillClassArr[i] = skillClassArr[_selectSkillID];
            }
            else
            {
                playerSkillClassArr[i] = null;
            }

            if (playerSkillClassArr[i] != null)
            {
                playerSkillClassArr[i].BattleEnter_Func();
            }

            skillBtnClassArr[i].Active_Func(playerSkillClassArr[i]);
        }
    }

    public void ManaRegen_Func()
    {
        StartCoroutine("ManaRegen_Cor");
    }
    IEnumerator ManaRegen_Cor()
    {
        mana_Recent = Player_Data.Instance.heroClass.manaStart;
        float _mana_RegenValue = Player_Data.Instance.heroClass.manaRegen;

        if (Player_Data.Instance.CheckDrinkUse_Func(DrinkType.Mana) == true)
        {
            float _drinkEffectValue = DataBase_Manager.Instance.drinkDataArr[(int)DrinkType.Mana].effectValue;
            _mana_RegenValue = _mana_RegenValue * _drinkEffectValue;
        }
        
        while (isActive == true)
        {
            SetMana_Func(_mana_RegenValue * 0.02f);
            
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

        playerSkillClassArr[_slotID].UseSkill_Func();
    }

    public void Deactive_Func()
    {
        isActive = false;

        SetMana_Func(100, true);
        StopCoroutine("ManaRegen_Cor");

        for (int i = 0; i < 5; i++)
        {
            skillBtnClassArr[i].Deactive_Func();
        }

        if (Player_Data.Instance.CheckDrinkUse_Func(DrinkType.Mana) == true)
        {
            Player_Data.Instance.UseDrink_Func(DrinkType.Mana);
        }
    }
}
