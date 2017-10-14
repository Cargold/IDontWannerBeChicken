using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard_Script : MonoBehaviour
{
    public PartySetting_Script partySettingClass;

    public enum CardState
    {
        None = -1,
        Lock,
        Active,
        Party,
    }
    public CardState cardState;

    public enum CardPurpose
    {
        None = -1,
        UnitSlot,
        Drag,
        PartySlot,
    }
    public CardPurpose cardPurpose;

    public Image populationImage;
    public Image unitImage;
    public GameObject[] cardStateObjArr;

    public void Init_Func(PartySetting_Script _partySettingClass, CardState _cardState, CardPurpose _cardPurpose)
    {
        partySettingClass = _partySettingClass;

        SetState_Func(_cardState);

        cardPurpose = _cardPurpose;
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
                cardStateObjArr[1].SetActive(false);
                break;
            case CardState.Party:
                cardStateObjArr[1].SetActive(true);
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
}
