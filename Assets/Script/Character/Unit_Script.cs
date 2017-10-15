using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : Character_Script
{
    [SerializeField]
    private Vector3 moveDir;

    public int spawnNum;
    public float spawnInterval;

    public void SetData_Func(Character_Data _charData)
    {
        
    }

    public void Init_Func(GroupType _groupType)
    {
        base.Init_Func(_groupType);

        InitMove_Func();
    }

    void InitMove_Func()
    {
        moveDir = Vector3.zero;
        if (groupType == GroupType.Ally)
        {
            moveDir = Vector3.right;
        }
        else if (groupType == GroupType.Enemy)
        {
            moveDir = Vector3.left;
        }
    }

    protected override void Move_Func()
    {
        if(charState != CharacterState.Move)
        {
            StopCoroutine("Move_Cor");
            StartCoroutine("Move_Cor");
        }
    }

    IEnumerator Move_Cor()
    {
        charState = CharacterState.Move;

        while (charState == CharacterState.Move)
        {
            this.transform.position += moveDir * moveSpeed * 0.01f;

            yield return new WaitForFixedUpdate();
        }
    }
}
