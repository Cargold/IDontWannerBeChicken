using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush_Script : Skill_Parent
{
    public Player_Script playerClass;
    public Transform playerTrf;

    [SerializeField]
    private SkillVar shieldTimeData;
    [SerializeField]
    private SkillVar shieldValueData;
    [SerializeField]
    private float rushSpeed;
    [SerializeField]
    private float rushMoveTime;
    [SerializeField]
    private float collideDistance;
    [SerializeField]
    private float playerSpeedOriginal;
    private bool isMove;
    private bool isTime;

    public override void Init_Func()
    {
        playerClass = Player_Data.Instance.playerClass;
        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        shieldTimeData = skillVarArr[0];
        shieldValueData = skillVarArr[1];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        SetMove_Func();
        SetTime_Func();
    }
    void SetMove_Func()
    {
        isMove = true;
        
        playerClass.SetControlOut_Func(true);

        playerSpeedOriginal = playerClass.moveSpeed;
        playerClass.SetMove_Func(Player_Script.MoveDir.Right, playerSpeedOriginal * rushSpeed);

        StartCoroutine(CalcRushTime_Cor());
    }
    void SetTime_Func()
    {
        isTime = true;

        playerClass.SetShield_Func(shieldValueData.recentValue);
        
        StartCoroutine(CalcShieldTime_Cor());
    }
    private void Update()
    {
        if (isMove == false) return;
        
        if (playerClass.GetCollideCheck_Func(collideDistance) == true)
        {
            MoveOver_Func();
        }
    }
    IEnumerator CalcRushTime_Cor()
    {
        float _rushTime = rushMoveTime;

        while (0f < _rushTime && isActive == true)
        {
            yield return new WaitForFixedUpdate();
            _rushTime -= 0.02f;
        }

        MoveOver_Func();
    }
    IEnumerator CalcShieldTime_Cor()
    {
        float _rushTime = shieldTimeData.recentValue;
        
        while(0f < _rushTime && isActive == true)
        {
            yield return new WaitForFixedUpdate();
            _rushTime -= 0.02f;
        }

        TimeOver_Func();
    }
    private void TimeOver_Func()
    {
        isTime = false;

        playerClass.SetShield_Func(0f);

        if(isMove == true)
        {
            MoveOver_Func();
        }

        Deactive_Func();
    }
    private void MoveOver_Func()
    {
        if(isMove == true)
        {
            isMove = false;

            playerClass.SetControlOut_Func(false);

            playerClass.SetMove_Func(Player_Script.MoveDir.None, playerSpeedOriginal);
            playerSpeedOriginal = 0f;
        }
    }
    protected override void Deactive_Func()
    {
        isActive = false;
    }
}