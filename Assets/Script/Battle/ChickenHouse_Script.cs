using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenHouse_Script : Character_Script
{
    public void Init_Func(GroupType _groupType)
    {
        base.Init_Func(_groupType);
    }

    public override void Die_Func()
    {
        isAlive = false;
        charState = CharacterState.Die;
        targetClassList.Clear();

        Battle_Manager.Instance.GameClear_Func();
    }
}
