using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam_Script : Skill_Parent
{
    public Player_Script playerClass;
    public Transform playerTrf;

    [SerializeField]
    private SkillVar collideNumData;
    [SerializeField]
    private SkillVar pushPowerData;
    [SerializeField]
    private float rushSpeed;
    [SerializeField]
    private float rushMoveTime;
    [SerializeField]
    private float rushCollideDistance;
    [SerializeField]
    private float pushPower;
    [SerializeField]
    private float pushHeight;
    [SerializeField]
    private float pushTime;

    private float playerSpeedOriginal;
    private bool isSlam;

    private List<Character_Script> collideCharList;

    public override void Init_Func()
    {
        playerClass = Player_Data.Instance.playerClass;
        playerTrf = Player_Data.Instance.playerClass.transform;

        collideCharList = new List<Character_Script>();
    }
    protected override void BattleEnterChild_Func()
    {
        collideNumData = skillVarArr[0];
        pushPowerData = skillVarArr[1];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        SetMove_Func();
    }
    void SetMove_Func()
    {
        isSlam = true;

        playerClass.SetControlOut_Func(true);

        playerSpeedOriginal = playerClass.moveSpeed;
        playerClass.SetMove_Func(Player_Script.MoveDir.Right, playerSpeedOriginal * rushSpeed);

        StartCoroutine(CalcRushTime_Cor());
    }
    private void FixedUpdate()
    {
        if (isSlam == false) return;

        Character_Script _collideCharClass = playerClass.GetCollideCharClass_Func(rushCollideDistance);
        if(_collideCharClass != null)
        {
            OnCollide_Func(_collideCharClass);
        }
    }
    void OnCollide_Func(Character_Script _collideCharClass)
    {
        Character_Script.KnockBackData knockBackData = new Character_Script.KnockBackData();
        knockBackData.Init_Func(pushPower, pushHeight, pushTime);
        
        collideCharList.Add(_collideCharClass);

        _collideCharClass.KnockBack_Func(knockBackData);

        if(2 < collideCharList.Count)
        {
            SlamOver_Func();
        }
    }

    IEnumerator CalcRushTime_Cor()
    {
        float _rushTime = pushTime;

        while (0f < _rushTime && isSlam == true)
        {
            yield return new WaitForFixedUpdate();
            _rushTime -= 0.02f;
        }

        SlamOver_Func();
    }
    private void SlamOver_Func()
    {
        if (isSlam == true)
        {
            isSlam = false;

            playerClass.SetControlOut_Func(false);

            playerClass.SetMove_Func(Player_Script.MoveDir.None, playerSpeedOriginal);
            playerSpeedOriginal = 0f;

            collideCharList.Clear();
        }

        Deactive_Func();
    }
    protected override void Deactive_Func()
    {
        isActive = false;
    }
}
