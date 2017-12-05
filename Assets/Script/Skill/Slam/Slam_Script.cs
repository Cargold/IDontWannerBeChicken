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
    
    private GameObject effectPivotObj;
    private SlamEffect_Script slamEffectClass;

    public override void Init_Func()
    {
        playerClass = Player_Data.Instance.heroClass;
        playerTrf = Player_Data.Instance.heroClass.transform;

        collideCharList = new List<Character_Script>();

        effectPivotObj = this.transform.GetChild(0).gameObject;

        slamEffectClass = effectPivotObj.GetComponent<SlamEffect_Script>();
        slamEffectClass.Init_Func(this);
    }
    protected override void BattleEnterChild_Func()
    {
        collideNumData = skillVarArr[0];
        pushPowerData = skillVarArr[1];
    }
    public override void UseSkill_Func()
    {
        isActive = true;
        
        slamEffectClass.Active_Func(pushTime + 2f, playerTrf);

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
        if (collideCharList.Contains(_collideCharClass) == true) return;

        collideCharList.Add(_collideCharClass);

        Character_Script.KnockBackData knockBackData = new Character_Script.KnockBackData();
        knockBackData.Init_Func(pushPower, pushHeight, pushTime);

        _collideCharClass.KnockBack_Func(knockBackData);

        if(collideNumData.recentValue < collideCharList.Count)
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
