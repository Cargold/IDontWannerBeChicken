using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroManagement_Script : LobbyUI_Parent
{
    public bool isActive;

    public SkillCard_Script[] skillCardClassArr;
    public Transform skillCardGroupTrf;
    public SkillCard_Script selectCardClass;
    public SkillCard_Script[] selectCardClassArr;
    public Transform gradeGroupTrf;
    public SkillGrade_Script[] skillGradeArr;

    public SkillInfo_Script skillInfoClass;
    public Text skillLevelUpCostTest;

    public Image heroImage;
    public Text[] heroInfoTextArr;

    public Text feedText;
    public Text backText;
    public Text upgradeText;

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

        feedText.text = TranslationSystem_Manager.Instance.Feed;
        backText.text = TranslationSystem_Manager.Instance.Back;
        upgradeText.text = TranslationSystem_Manager.Instance.Upgrade;

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func(int _referenceID = -1)
    {
        this.gameObject.SetActive(true);

        isActive = true;

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

        PrintInfoUI_Func();
    }
    public override void Exit_Func()
    {
        // Call : Btn Event

        isActive = false;

        this.gameObject.SetActive(false);
    }
    #endregion
    #region SkillCard Group
    public void SelectSkillCard_Func(SkillCard_Script _selectCardClass)
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
    public void SkillLevelUp_Func()
    {
        int _skillID = selectCardClass.cardID;
        int _skillLevel = Player_Data.Instance.GetSkillLevel_Func(_skillID);
        if (_skillLevel < 20)
        {
            int skillLevelUpCost = Player_Data.Instance.GetSkillUpCost_Func(_skillID);
            bool _isPay = Player_Data.Instance.PayWealth_Func(WealthType.Mineral, skillLevelUpCost, true);
            if (_isPay == true)
            {
                Player_Data.Instance.PayWealth_Func(WealthType.Mineral, skillLevelUpCost);

                Player_Data.Instance.LevelUpSkill_Func(selectCardClass.cardID);

                PrintCardInfo_Func();
            }
            else if (_isPay == false)
            {
                skillLevelUpCostTest.DOColor(Color.red, 0.2f).OnComplete(ResetLevelUpBtn_Func);
            }
        }
        else
        {

        }
    }
    void ResetLevelUpBtn_Func()
    {
        Color _color = DataBase_Manager.Instance.textColor;
        skillLevelUpCostTest.DOColor(_color, 0.2f);
    }
    #endregion
    #region Print Group
    public void PrintCardInfo_Func(SkillCard_Script _selectCardClass = null)
    {
        if (_selectCardClass == null)
            _selectCardClass = selectCardClass;

        skillInfoClass.PrintSelectCardInfo_Func(_selectCardClass);
        PrintSkillUpCost_Func(_selectCardClass.cardID);
    }
    void PrintSkillUpCost_Func(int _skillDataID)
    {
        int _skillID = selectCardClass.cardID;
        int _skillLevel = Player_Data.Instance.GetSkillLevel_Func(_skillID);
        if (_skillLevel < 20)
        {
            int skillLevelUpCost = Player_Data.Instance.GetSkillUpCost_Func(selectCardClass.cardID);

            skillLevelUpCostTest.text = skillLevelUpCost.ToString();
        }
        else
        {
            skillLevelUpCostTest.text = "MAX";
        }
    }
    public void PrintInfoUI_Func()
    {
        Player_Script _heroClass = Player_Data.Instance.heroClass;

        heroInfoTextArr[0].text = string.Format("{0:N0}", (int)_heroClass.attackValue);
        heroInfoTextArr[1].text = string.Format("{0:N0}", (int)_heroClass.healthPoint_Max);
        heroInfoTextArr[2].text = string.Format("{0:N0}", _heroClass.defenceValue) + "%";
        heroInfoTextArr[3].text = string.Format("{0:N0}", _heroClass.criticalPercent) + "%";
        heroInfoTextArr[4].text = "0";
        heroInfoTextArr[5].text = "0";
        heroInfoTextArr[6].text = "Lv." + Player_Data.Instance.heroLevel + " " + _heroClass.charNameArr[TranslationSystem_Manager.Instance.languageTypeID];
        heroInfoTextArr[7].text = _heroClass.charDescArr[TranslationSystem_Manager.Instance.languageTypeID];
    }
    #endregion
    #region OnFeed Group
    public void OnFeedingRoom_Func()
    {
        // Call : Btn Event

        RotateUnitImage_Func();
        lobbyManager.Enter_Func(LobbyState.FeedingRoom, 999);
        //lobbyManager.OnFeedingRoom_Func(999);
    }
    void RotateUnitImage_Func()
    {
        Player_Script _playerClass = Player_Data.Instance.heroClass;

        heroImage.rectTransform.DORotate(Vector3.up * 180f, 0.5f);
        heroImage.DOColor(Color.black, 0.5f);
        heroImage.rectTransform.DOLocalMove(_playerClass.feedImagePos, 0.5f);
        heroImage.rectTransform.DOScale(_playerClass.feedImageSize, 0.5f);
    }
    public void ReturnUI_Func()
    {
        Player_Script _playerClass = Player_Data.Instance.heroClass;

        heroImage.transform.DORotate(Vector3.zero, 0.5f);
        heroImage.DOColor(Color.white, 0.5f);
        heroImage.transform.DOLocalMove(new Vector3(516f, -75, 0f), 0.5f);
        heroImage.transform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
    }
    #endregion
}
