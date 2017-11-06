using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManagement_Script : LobbyUI_Parent
{
    public SkillCard_Script[] skillCardClassArr;
    public Transform skillCardGroupTrf;
    public SkillCard_Script selectCardClass;
    public SkillCard_Script[] setCardClassArr;
    public Transform gradeGroupTrf;
    public SkillGrade_Script[] skillGradeArr;

    public SkillInfo_Script skillInfoClass;

    #region Override Group
    protected override void InitUI_Func()
    {
        skillInfoClass.Init_Func(this);

        skillGradeArr = new SkillGrade_Script[5];
        for (int i = 0; i < 5; i++)
        {
            skillGradeArr[i] = gradeGroupTrf.GetChild(i).GetComponent<SkillGrade_Script>();
            skillGradeArr[i].Init_Func(this, i);
        }

        skillCardClassArr = new SkillCard_Script[10];
        for (int i = 0; i < 10; i++)
        {
            PlayerSkill_Data _playerSkillData = Player_Data.Instance.skillDataArr[i];

            skillCardClassArr[i] = skillCardGroupTrf.GetChild(i).GetComponent<SkillCard_Script>();
            skillCardClassArr[i].Init_Func(this, i, _playerSkillData);

            if(_playerSkillData.isUnlock == true)
            {
                int _grade = i / 2;
                skillGradeArr[_grade].UnlockGrade_Func();
            }
        }

        setCardClassArr = new SkillCard_Script[5];
        for (int i = 0; i < 5; i++)
        {
            int _selectCardID = Player_Data.Instance.selectSkillIDArr[i];
            if(0<=_selectCardID)
            {
                skillCardClassArr[_selectCardID].OnButton_Func();
                setCardClassArr[i] = skillCardClassArr[_selectCardID];
            }
        }

        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            
        }
    }

    public override void Exit_Func()
    {
        // Call : Btn Event

        this.gameObject.SetActive(false);
    }
    #endregion

    public void CheckSelectCard_Func(SkillCard_Script _selectCardClass)
    {
        // 이전 선택된 카드, 효과 해제
        if(selectCardClass != null)
            selectCardClass.DeselectCard_Func(selectCardClass);

        // 새로운 선택 카드 기록
        selectCardClass = _selectCardClass;
        
        // 선택 카드 정보 출력
        PrintCardInfo_Func(_selectCardClass);

        // 등급 선택지 표시
        int _grade = _selectCardClass.grade;
        int _upCheck = _selectCardClass.cardID % 2;
        if (_upCheck == 0)
        {
            skillGradeArr[_grade].SelectSkill_Func(true);
        }
        else
        {
            skillGradeArr[_grade].SelectSkill_Func(false);
        }
    }
    public void PrintCardInfo_Func(SkillCard_Script _selectCardClass)
    {
        skillInfoClass.PrintSelectCardInfo_Func(_selectCardClass);
    }

    public void OnFeedingRoom_Func()
    {
        lobbyManager.OnFeedingRoom_Func(999);
    }
}
