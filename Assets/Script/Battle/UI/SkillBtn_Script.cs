using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillBtn_Script : MonoBehaviour
{
    public SkillSystem_Manager skillSystemManager;
    public Skill_Parent playerSkillClassArr;
    public int slotID;
    public bool isActive;
    public bool isSkillOn;
    public Image skillIcon;
    public Image costIcon;
    public Image coolTimeImage;

    public void Init_Func(SkillSystem_Manager _skillSystemManager, int _slotID)
    {
        skillSystemManager = _skillSystemManager;
        
        slotID = _slotID;

        isActive = false;
        isSkillOn = false;
    }

    public void Active_Func(Skill_Parent _playerSkillClassArr)
    {
        if (_playerSkillClassArr != null)
        {
            this.gameObject.SetActive(true);

            isActive = true;

            isSkillOn = true;

            playerSkillClassArr = _playerSkillClassArr;

            skillIcon.sprite = _playerSkillClassArr.skillSprite;
            skillIcon.SetNativeSize();

            int _manaCost = (int)_playerSkillClassArr.manaCost;
            costIcon.sprite = DataBase_Manager.Instance.manaCostSpriteArr[_manaCost];
            costIcon.SetNativeSize();

            coolTimeImage.fillAmount = 0f;
            coolTimeImage.color = skillSystemManager.skillBtnCoolTimeColor;

            StartCoroutine("CheckCoolTime_Cor");
        }
        else
        {
            // 스킬이 없는 경우

            this.gameObject.SetActive(false);
        }
    }
    IEnumerator CheckCoolTime_Cor()
    {
        while(isActive == true)
        {
            if(isSkillOn == false)
            {
                float _time = playerSkillClassArr.coolTime;
                float _calcTime = 0f;

                while (0f < _time)
                {
                    _time -= 0.02f;

                    _calcTime = _time / playerSkillClassArr.coolTime;

                    coolTimeImage.fillAmount = _calcTime;

                    yield return new WaitForFixedUpdate();
                }

                SkillReady_Func();
            }
            else if(isSkillOn == true)
            {
                yield return null;
            }
        }
    }
    void SkillReady_Func()
    {
        isSkillOn = true;
    }
    void SkillUse_Func()
    {
        isSkillOn = false;

        skillSystemManager.UseSkill_Func(slotID);
    }
    void SkillFail_Func()
    {

    }

    public void OnButton_Func()
    {
        if(isSkillOn == false)
        {
            // 스킬 쿨이 준비가 안 됨

            SkillFail_Func();
        }
        else if(isSkillOn == true)
        {
            bool _isManaOn = skillSystemManager.CheckSkillUse_Func(slotID);

            if(_isManaOn == false)
            {
                // 마나 부족

                SkillFail_Func();
            }
            else if (_isManaOn == true)
            {
                // 스킬 발동

                SkillUse_Func();
            }
        }
    }

    public void Deactive_Func()
    {
        isActive = false;

        StopCoroutine("CheckCoolTime_Cor");
    }
}
