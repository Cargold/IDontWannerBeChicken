using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHouse_Script : Character_Script
{
    public void Init_Func(GroupType _groupType)
    {
        base.Init_Func(_groupType);
    }

    public override void Damaged_Func(float _damageValue)
    {
        base.Damaged_Func(_damageValue);

        float _remainPer = healthPoint_Recent / healthPoint_Max;
        if(_remainPer < 0.25f)
        {
            Battle_Manager.Instance.SetMonsterSpawnBonus_Func(2);
        }
        else if(_remainPer < 0.5f)
        {
            Battle_Manager.Instance.SetMonsterSpawnBonus_Func(1);
        }
        else if(_remainPer < 0.75f)
        {
            Battle_Manager.Instance.SetMonsterSpawnBonus_Func(0);
        }
    }

    public override void Die_Func(bool _isImmediate = false)
    {
        isAlive = false;
        charState = CharacterState.Die;
        contactCharClassList.Clear();

        Battle_Manager.Instance.GameClear_Func();
    }
}
