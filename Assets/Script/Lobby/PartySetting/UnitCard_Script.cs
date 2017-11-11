using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitCard_Script : MonoBehaviour
{
    public int cardId;
    public PartySetting_Script partySettingClass;

    public enum CardState
    {
        None = -1,
        Lock,
        Active,
    }
    public CardState cardState;
    
    public Image unitImage;
    public GameObject[] cardStateObjArr;
    public Text unlockConditionText;

    public void Init_Func(PartySetting_Script _partySettingClass, int _cardId, CardState _cardState)
    {
        partySettingClass = _partySettingClass;

        cardId = _cardId;

        SetState_Func(_cardState);

        Sprite _unitSprite = DataBase_Manager.Instance.unitDataArr[_cardId].cardSprite;
        unitImage.sprite = _unitSprite;
        unitImage.SetNativeSize();

        unitImage.transform.localPosition = DataBase_Manager.Instance.unitDataArr[_cardId].cardPortraitPos;

        unitImage.transform.localScale = DataBase_Manager.Instance.unitDataArr[_cardId].cardImageSize * Vector3.one;
    }
    
    public void SetState_Func(CardState _cardState)
    {
        cardState = _cardState;

        switch (_cardState)
        {
            case CardState.Lock:
                cardStateObjArr[0].SetActive(true);
                int _unlockStageLevel = DataBase_Manager.Instance.GetUnitClass_Func(cardId).unlockLevel;
                unlockConditionText.text = "Stage " + _unlockStageLevel;
                break;
            case CardState.Active:
                cardStateObjArr[0].SetActive(false);
                break;
        }
    }

    public void OnSelect_Func()
    {
        if (cardState == CardState.Active)
            partySettingClass.SelectUnit_Func(this);
        else if (cardState == CardState.Lock)
        {
            this.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 15).OnComplete(ResetScale_Func);
        }
    }
    void ResetScale_Func()
    {
        this.transform.localScale = Vector3.one;
    }

    public void DragBegin_Func()
    {
        if (cardState == CardState.Active)
            partySettingClass.DragBegin_Func(this);
    }

    public void Dragging_Func()
    {
        if (cardState == CardState.Active)
            partySettingClass.Dragging_Func(this);
    }

    public void DragEnd_Func()
    {
        if (cardState == CardState.Active)
            partySettingClass.DragEnd_Func(this);
    }

    public void InitPos_Func()
    {
        Vector2 _initPos = partySettingClass.unitCardPosInitArr[cardId];
        this.transform.localPosition = _initPos;
    }
}
