using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManagement_Script : LobbyUI_Parent
{
    public SkillCard_Script[] skillCardClassArr;
    public Transform skillCardGroupTrf;
    public SkillCard_Script selectCardClass;
    public SkillCard_Script[] selectCardClassArr;
    public Transform gradeGroupTrf;
    public SkillGrade_Script[] skillGradeArr;

    public SkillInfo_Script skillInfoClass;

    #region Override Group
    protected override void InitUI_Func()
    {
        skillInfoClass.Init_Func(this);

        // 스킬 등급 별 선택 및 잠금 이미지 초기화
        skillGradeArr = new SkillGrade_Script[5];
        for (int i = 0; i < 5; i++)
        {
            skillGradeArr[i] = gradeGroupTrf.GetChild(i).GetComponent<SkillGrade_Script>();
            skillGradeArr[i].Init_Func(this, i);
        }

        // 10장의 스킬 카드 초기화
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

        selectCardClassArr = new SkillCard_Script[5];
        for (int i = 0; i < 5; i++)
        {
            int _selectCardID = Player_Data.Instance.selectSkillIDArr[i];
            if(0<=_selectCardID)
            {
                selectCardClassArr[i] = skillCardClassArr[_selectCardID];
                selectCardClassArr[i].OnButton_Func();
            }
            else
            {
                int _selectSkillCardID = i * 2;
                if (skillCardClassArr[_selectSkillCardID].isUnlock == true)
                {
                    selectCardClassArr[i] = skillCardClassArr[_selectSkillCardID];
                    selectCardClassArr[i].OnButton_Func();
                }
            }
        }

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            if(selectCardClassArr[i] == null)
            {
                int _selectSkillCardID = i * 2;
                if(skillCardClassArr[_selectSkillCardID].isUnlock == true)
                {
                    selectCardClassArr[i] = skillCardClassArr[_selectSkillCardID];
                    selectCardClassArr[i].OnButton_Func();
                }
            }
        }
    }
    public override void Exit_Func()
    {
        // Call : Btn Event

        this.gameObject.SetActive(false);
    }
    #endregion

    public void SelectCard_Func(SkillCard_Script _selectCardClass)
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
