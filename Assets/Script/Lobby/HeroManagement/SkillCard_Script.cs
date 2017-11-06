using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillCard_Script : MonoBehaviour
{
    // Card Data
    public HeroManagement_Script heroManagementClass;
    public int cardID;
    public int grade;
    public bool isUnlock;

    // Renderer
    public GameObject selectImageObj;
    public Image manaCostImage;
    public Image skillIconImage;
    public GameObject lockImageObj;
    public Text lockConditionText;

    public void Init_Func(HeroManagement_Script _heroManagementClass, int _cardID, PlayerSkill_Data _playerSkillData)
    {
        heroManagementClass = _heroManagementClass;

        cardID = _cardID;
        grade = _cardID / 2;

        selectImageObj.SetActive(false);

        skillIconImage.sprite = DataBase_Manager.Instance.skillDataArr[_cardID].skillSprite;
        skillIconImage.SetNativeSize();

        int _manaCost = (int)DataBase_Manager.Instance.skillDataArr[_cardID].manaCost;
        manaCostImage.sprite = DataBase_Manager.Instance.manaCostSpriteArr[_manaCost];
        manaCostImage.SetNativeSize();

        isUnlock = _playerSkillData.isUnlock;
        lockImageObj.SetActive(!_playerSkillData.isUnlock);
    }

    public void OnButton_Func()
    {
        if(isUnlock == false)
        {
            // 잠겨진 카드를 선택한 경우

            this.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 15).OnComplete(ResetScale_Func);
            heroManagementClass.PrintCardInfo_Func(this);
        }
        else if(isUnlock == true)
        {
            if(selectImageObj.activeInHierarchy == false)
            {
                heroManagementClass.CheckSelectCard_Func(this);

                selectImageObj.SetActive(true);
            }
            else
            {
                // 이미 선택된 카드를 선택한 경우

                this.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 15).OnComplete(ResetScale_Func);
            }
        }
    }
    void ResetScale_Func()
    {
        this.transform.localScale = Vector3.one;
    }

    public void DeselectCard_Func(SkillCard_Script _skillCardClass)
    {
        if(this == _skillCardClass)
        {
            selectImageObj.SetActive(false);
        }
        else
        {
            Debug.LogError("Bug : 선택된 스킬카드의 정보가 일치하지 않습니다.");
        }
    }
}
