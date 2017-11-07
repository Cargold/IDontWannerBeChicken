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
    private SkillVar rushSpeedData;
    [SerializeField]
    private SkillVar rushMoveTimeData;
    [SerializeField]
    private float playerSpeedOriginal;
    public bool isActive;
    private bool isMove;
    private bool isTime;

    public override void Init_Func()
    {
        playerClass = Player_Data.Instance.playerClass;
        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    public override void BattleEnter_Func()
    {
        base.BattleEnter_Func();

        shieldTimeData = skillVarArr[0];
        shieldValueData = skillVarArr[1];
        rushSpeedData = skillVarArr[2];
        rushMoveTimeData = skillVarArr[3];
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
        playerClass.SetMove_Func(Player_Script.MoveDir.Right, playerSpeedOriginal * rushSpeedData.recentValue);

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
        
        if (playerClass.GetCollideCheck_Func() == true)
        {
            MoveOver_Func();
        }
    }
    IEnumerator CalcRushTime_Cor()
    {
        float _rushTime = rushMoveTimeData.recentValue;

        while (0f < _rushTime)
        {
            yield return new WaitForFixedUpdate();
            _rushTime -= 0.02f;
        }

        MoveOver_Func();
    }
    IEnumerator CalcShieldTime_Cor()
    {
        float _rushTime = shieldTimeData.recentValue;
        
        while(0f < _rushTime)
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

    public void Deactive_Func()
    {
        isActive = false;
    }
}