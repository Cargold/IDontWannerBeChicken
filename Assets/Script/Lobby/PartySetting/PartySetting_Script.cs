using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySetting_Script : LobbyUI_Parent
{
    public UnitCard_Script[] unitCardClassArr;
    public UnitCard_Script selectCardClass;
    public Transform unitCardGroupTrf;
    GameObject dragCard;

    public PartySlot_Script[] partySlotClassArr;
    public PartySlot_Script contactCardClass;
    public PartySlot_Script disbandCardClass;
    public Transform partySlotGroupTrf;
    public Text playerPopulationText;

    #region Override Group
    protected override void InitUI_Func()
    {
        int _cardNum = unitCardGroupTrf.childCount;

        unitCardClassArr = new UnitCard_Script[_cardNum];

        for (int i = 0; i < _cardNum; i++)
        {
            unitCardClassArr[i] = unitCardGroupTrf.GetChild(i).GetComponent<UnitCard_Script>();
            unitCardClassArr[i].Init_Func(this, UnitCard_Script.CardState.Active, UnitCard_Script.CardPurpose.UnitSlot);
        }

        partySlotClassArr = new PartySlot_Script[5];
        for (int i = 0; i < 5; i++)
        {
            partySlotClassArr[i] = partySlotGroupTrf.GetChild(i).GetComponent<PartySlot_Script>();
            partySlotClassArr[i].Init_Func(this, i, PartySlot_Script.CardState.Empty);
        }
    }

    protected override void EnterUI_Func()
    {

    }

    public override void Exit_Func()
    {

    }
    #endregion

    #region Unit Slot Group
    public void SelectUnit_Func(UnitCard_Script _unitCardClass)
    {
        selectCardClass = _unitCardClass;
    }

    public void DragBegin_Func(UnitCard_Script _unitCardClass)
    {
        if(_unitCardClass.cardPurpose == UnitCard_Script.CardPurpose.UnitSlot)
        {
            dragCard = Instantiate(selectCardClass.gameObject);
            dragCard.transform.position = selectCardClass.transform.position;
            dragCard.transform.parent = this.transform;

            _unitCardClass.cardPurpose = UnitCard_Script.CardPurpose.Drag;
        }
        else if(_unitCardClass.cardPurpose == UnitCard_Script.CardPurpose.PartySlot)
        {
            dragCard = _unitCardClass.gameObject;
        }
    }

    public void Dragging_Func(UnitCard_Script _unitCardClass)
    {
        dragCard.transform.position = Input.mousePosition;
    }

    public void DragEnd_Func(UnitCard_Script _unitCardClass)
    {
        if (_unitCardClass.cardPurpose == UnitCard_Script.CardPurpose.Drag)
        {
            if(contactCardClass != null)
            {
                JoinParty_Func(_unitCardClass);
            }

            Destroy(dragCard);

            _unitCardClass.cardPurpose = UnitCard_Script.CardPurpose.UnitSlot;
        }
        else if(_unitCardClass.cardPurpose == UnitCard_Script.CardPurpose.PartySlot)
        {
            if(disbandCardClass != null)
            {
                if(contactCardClass != null)
                {
                    disbandCardClass.OnEmpty_Func(true);
                    contactCardClass.JoinParty_Func(dragCard.GetComponent<UnitCard_Script>(), true);                }
                else
                {
                    disbandCardClass.OnEmpty_Func();
                }
            }
            else
            {
                _unitCardClass.transform.position = contactCardClass.transform.position;
            }

            dragCard = null;
            contactCardClass = null;
            disbandCardClass = null;
        }
    }
    #endregion
    #region Party Slot Group
    public void DecontactCard_Func()
    {
        if (contactCardClass != null)
            contactCardClass.OnDecontact_Func();

        contactCardClass = null;
    }

    public void ContactCard_Func(PartySlot_Script _partySlotClass)
    {
        if(contactCardClass != null)
            contactCardClass.OnDecontact_Func();

        contactCardClass = _partySlotClass;

        contactCardClass.OnContact_Func();
    }

    void JoinParty_Func(UnitCard_Script _unitCardClass)
    {
        contactCardClass.JoinParty_Func(_unitCardClass);

        contactCardClass = null;
    }
    #endregion
}
