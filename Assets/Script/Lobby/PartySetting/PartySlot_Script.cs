using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot_Script : MonoBehaviour
{
    public int slotId;

    public PartySetting_Script partySettingClass;

    public Transform contactCheckTrf;

    public bool isContactState;

    public enum CardState
    {
        None = -1,
        Empty,
        Join,
    }
    public CardState cardState;
    public UnitCard_Script joinUnitCardClass;
    GameObject joinUnitCardObj;

    public void Init_Func(PartySetting_Script _partySettingClass, int _slotId, CardState _cardState)
    {
        partySettingClass = _partySettingClass;

        slotId = _slotId;

        if (_cardState == CardState.Empty)
        {
            OnEmpty_Func();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "UnitCard")
        {
            partySettingClass.ContactCard_Func(this);
            
            if(joinUnitCardObj != null)
            {
                if(col.gameObject == joinUnitCardObj)
                {
                    partySettingClass.OnDisbandPartySlot_Func(null);
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "UnitCard")
        {
            if (cardState == CardState.Join)
            {
                if (joinUnitCardObj != null)
                {
                    if (col.gameObject == joinUnitCardObj)
                    {
                        partySettingClass.OnDisbandPartySlot_Func(this);
                    }
                }
            }
            else if(cardState == CardState.Empty)
            {
                partySettingClass.DecontactCard_Func();
            }
        }
    }

    public void OnEmpty_Func()
    {
        OnDecontact_Func();

        if (joinUnitCardClass != null)
        {
            int _populValue = joinUnitCardClass.populValue;
            partySettingClass.CalcPopulation_Func(-_populValue);

            joinUnitCardClass.InitPos_Func();
            joinUnitCardClass = null;
            joinUnitCardObj = null;
        }

        cardState = CardState.Empty;
    }

    public void OnDecontact_Func()
    {
        isContactState = false;
        contactCheckTrf.localScale = Vector3.one;
    }

    public void OnContact_Func()
    {
        isContactState = true;
        contactCheckTrf.localScale = Vector3.one * 1.2f;
    }

    public void JoinParty_Func(UnitCard_Script _unitCardClass, bool _isSwap)
    {
        isContactState = false;
        contactCheckTrf.localScale = Vector3.zero;

        int _populValue = 0;
        if (_isSwap == false)
         _populValue = _unitCardClass.populValue;

        if (partySettingClass.CheckPopulation_Func(_populValue) == true || _isSwap == true)
        {
            // 조건에 부합하여 유닛이 파티에 합류하는 경우
            // 조건 1 : 인구수가 충분할 때
            // 조건 2 : 파티슬롯 내 서로 교체할 때

            joinUnitCardObj = _unitCardClass.gameObject;
            joinUnitCardObj.transform.position = this.transform.position;

            joinUnitCardClass = _unitCardClass;

            cardState = CardState.Join;

            if(_isSwap == false)
                partySettingClass.CalcPopulation_Func(_populValue);

            partySettingClass.JoinParty_Func(slotId, joinUnitCardClass.cardId);
        }
        else
        {
            _unitCardClass.InitPos_Func();
        }
    }
}
