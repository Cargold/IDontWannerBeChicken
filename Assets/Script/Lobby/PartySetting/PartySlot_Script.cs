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
                    partySettingClass.disbandCardClass = null;
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
                        partySettingClass.disbandCardClass = this;
                    }
                }
            }
            else if(cardState == CardState.Empty)
            {
                partySettingClass.DecontactCard_Func();
            }
        }
    }

    public void OnEmpty_Func(bool _isPartySwapping = false)
    {
        OnDecontact_Func();

        if (joinUnitCardClass != null)
        {
            if(_isPartySwapping == false)
            {
                Destroy(joinUnitCardObj);
            }
            else
            {
                joinUnitCardClass = null;
                joinUnitCardObj = null;
            }
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

    public void JoinParty_Func(UnitCard_Script _unitCardClass, bool _isPartySwapping = false)
    {
        isContactState = false;

        contactCheckTrf.localScale = Vector3.zero;

        if(_isPartySwapping == false)
        {
            joinUnitCardObj = Instantiate(_unitCardClass.gameObject);
            joinUnitCardObj.transform.position = this.transform.position;
            joinUnitCardObj.transform.parent = this.transform.parent;

            joinUnitCardClass = joinUnitCardObj.GetComponent<UnitCard_Script>();
            joinUnitCardClass.Init_Func(partySettingClass, UnitCard_Script.CardState.Active, UnitCard_Script.CardPurpose.PartySlot);
        }
        else
        {
            joinUnitCardObj = _unitCardClass.gameObject;
            joinUnitCardObj.transform.position = this.transform.position;

            joinUnitCardClass = _unitCardClass;
        }

        cardState = CardState.Join;
    }
}
