using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySlot_Script : MonoBehaviour
{
    public int slotId;

    public PartySetting_Script partySettingClass;

    public Transform contactCheckTrf;

    public bool isContactState;

    public enum SlotState
    {
        None = -1,
        Empty,
        Joined,
    }
    public SlotState slotState;

    public class JoinUnitData
    {
        public UnitCard_Script joinUnitCardClass
        {
            get
            {
                return m_JoinUnitCardClass;
            }

        }
        public GameObject joinUnitCardObj
        {
            get
            {
                return m_JoinUnitCardObj;
            }
        }
        public bool isDataOn
        {
            get
            {
                return m_IsDataOn;
            }
        }
        private UnitCard_Script m_JoinUnitCardClass;
        private GameObject m_JoinUnitCardObj;
        private bool m_IsDataOn;

        public void Init_Func(UnitCard_Script _joinUnitCardClass)
        {
            if(_joinUnitCardClass == null)
            {
                m_IsDataOn = false;

                if(m_JoinUnitCardClass != null)
                {
                    m_JoinUnitCardClass.cardState = UnitCard_Script.CardState.Active;
                }
                m_JoinUnitCardClass = null;
                m_JoinUnitCardObj = null;
            }
            else
            {
                m_IsDataOn = true;

                m_JoinUnitCardClass = _joinUnitCardClass;
                _joinUnitCardClass.cardState = UnitCard_Script.CardState.Active_Party;

                m_JoinUnitCardObj = _joinUnitCardClass.gameObject;
            }
        }
    }
    public JoinUnitData joinUnitData;

    public void Init_Func(PartySetting_Script _partySettingClass, int _slotId, SlotState _cardState)
    {
        partySettingClass = _partySettingClass;

        slotId = _slotId;

        joinUnitData = new JoinUnitData();
        joinUnitData.Init_Func(null);

        if (_cardState == SlotState.Empty)
        {
            OnEmpty_Func();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "UnitCard")
        {
            partySettingClass.ContactCard_Func(this);
            
            if(joinUnitData.isDataOn == true)
            {
                if(col.gameObject == joinUnitData.joinUnitCardObj)
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
            if (slotState == SlotState.Joined)
            {
                if (joinUnitData.isDataOn == true)
                {
                    if (col.gameObject == joinUnitData.joinUnitCardObj)
                    {
                        partySettingClass.OnDisbandPartySlot_Func(this);
                    }
                }
            }
            else if(slotState == SlotState.Empty)
            {
                partySettingClass.DecontactCard_Func();
            }
        }
    }

    public void OnEmpty_Func()
    {
        OnDecontact_Func();

        if (joinUnitData.isDataOn == true)
        {
            int _unitID = joinUnitData.joinUnitCardClass.cardId;
            Player_Data.Instance.DisbandParty_Func(slotId, _unitID);

            joinUnitData.joinUnitCardClass.InitPos_Func();
            joinUnitData.Init_Func(null);
        }

        slotState = SlotState.Empty;
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

        joinUnitData.Init_Func(_unitCardClass);
        joinUnitData.joinUnitCardObj.transform.position = this.transform.position;

        slotState = SlotState.Joined;
        
        partySettingClass.JoinParty_Func(slotId, joinUnitData.joinUnitCardClass.cardId);
    }
}
