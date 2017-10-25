using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int populValue;
    public Image populationImage;
    public Image unitImage;
    public GameObject[] cardStateObjArr;

    public void Init_Func(PartySetting_Script _partySettingClass, int _cardId, CardState _cardState)
    {
        partySettingClass = _partySettingClass;

        cardId = _cardId;

        SetState_Func(_cardState);

        populValue = DataBase_Manager.Instance.charDataArr[_cardId].populationValue;

        Sprite _populationSprite = Game_Manager.Instance.populationSpriteArr[populValue];
        populationImage.sprite = _populationSprite;
        populationImage.SetNativeSize();

        Sprite _unitSprite = DataBase_Manager.Instance.charDataArr[_cardId].unitSprite;
        unitImage.sprite = _unitSprite;
        unitImage.SetNativeSize();
    }
    
    public void SetState_Func(CardState _cardState)
    {
        cardState = _cardState;

        switch (_cardState)
        {
            case CardState.Lock:
                cardStateObjArr[0].SetActive(true);
                break;
            case CardState.Active:
                cardStateObjArr[0].SetActive(false);
                break;
        }
    }

    public void OnSelect_Func()
    {
        partySettingClass.SelectUnit_Func(this);
    }

    public void DragBegin_Func()
    {
        partySettingClass.DragBegin_Func(this);
    }

    public void Dragging_Func()
    {
        partySettingClass.Dragging_Func(this);
    }

    public void DragEnd_Func()
    {
        partySettingClass.DragEnd_Func(this);
    }

    public void InitPos_Func()
    {
        Vector2 _initPos = partySettingClass.unitCardPosInitArr[cardId];
        this.transform.localPosition = _initPos;
    }
}
