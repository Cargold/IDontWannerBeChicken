using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PartySetting_Script : LobbyUI_Parent
{
    public Transform unitCardGroupTrf;
    public Vector2 unitCardGap;
    public GameObject unitCardObj;
    public UnitCard_Script[] unitCardClassArr;
    public Vector2[] unitCardPosInitArr;
    public UnitCard_Script selectCardClass;
    public int selectUnitID;

    public bool isTouchOn;
    public PartySlot_Script[] partySlotClassArr;
    public PartySlot_Script contactCardClass;
    public PartySlot_Script disbandCardClass;
    public Transform partySlotGroupTrf;
    public Text playerPopulationText;

    public int populationPoint;

    public Text[] unitInfoTextArr;
    public Animation anim;
    public Image unitImage;
    public Image unitShadowImage;
    private Sprite changeUnitSprite;
    private float unitImagePivotAxisY;
    private Vector2 unitShadowSize;

    private Vector3 touchOffsetPos;
    public bool isActive;

    #region Override Group
    protected override void InitUI_Func()
    {
        InitUnitCardSlot_Func();
        InitPartySlot_Func();
        InitPartymember_Func();

        isTouchOn = false;
        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        isActive = true;

        unitImage.SetNaturalAlphaColor_Func(0f);
        unitShadowImage.SetNaturalAlphaColor_Func(0f);

        unitCardClassArr[0].OnSelect_Func(false);
    }
    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
    #region Init Group
    void InitUnitCardSlot_Func()
    {
        int _cardNum = DataBase_Manager.Instance.unitDataArr.Length;

        unitCardPosInitArr = new Vector2[_cardNum];
        unitCardClassArr = new UnitCard_Script[_cardNum];

        bool _isLoopOut = false;
        for (int axisY = 0, _cardCount = -1; ; axisY++)
        {
            for (int axisX = 0; axisX < 5; axisX++)
            {
                _cardCount++;

                if (_cardNum <= _cardCount)
                {
                    _isLoopOut = true;
                    break;
                }
                else
                {
                    unitCardPosInitArr[_cardCount] = new Vector2
                        (
                            axisX * unitCardGap.x,
                            axisY * -unitCardGap.y
                        );

                    GameObject _unitCardObj = Instantiate(unitCardObj);
                    _unitCardObj.transform.SetParent(unitCardGroupTrf);
                    _unitCardObj.transform.localScale = Vector3.one;

                    UnitCard_Script.CardState _cardState = UnitCard_Script.CardState.Lock;
                    if (Player_Data.Instance.playerUnitDataArr[_cardCount].isHave == true)
                        _cardState = UnitCard_Script.CardState.Active;

                    unitCardClassArr[_cardCount] = _unitCardObj.GetComponent<UnitCard_Script>();
                    unitCardClassArr[_cardCount].Init_Func
                        (this, _cardCount, _cardState);

                    unitCardClassArr[_cardCount].InitPos_Func();
                }
            }

            if (_isLoopOut == true)
                break;
        }
    }
    void InitPartySlot_Func()
    {
        partySlotClassArr = new PartySlot_Script[5];
        for (int i = 0; i < 5; i++)
        {
            partySlotClassArr[i] = partySlotGroupTrf.GetChild(i).GetComponent<PartySlot_Script>();
            partySlotClassArr[i].Init_Func(this, i, PartySlot_Script.SlotState.Empty);
        }
    }
    void InitPartymember_Func()
    {
        for (int i = 0; i < 5; i++)
        {
            int _unitId = Player_Data.Instance.partyUnitIdArr[i];
            if (0 <= _unitId)
            {
                partySlotClassArr[i].JoinParty_Func(unitCardClassArr[_unitId], false);
            }
        }
    }
    #endregion
    #region Card Control Group
    public void SelectUnit_Func(UnitCard_Script _unitCardClass, bool _isTouchOn)
    {
        isTouchOn = _isTouchOn;

        selectCardClass = _unitCardClass;

        selectUnitID = selectCardClass.cardId;

        PrintInfoUI_Func();
        ReadyUnitImage_Func();
    }
    public void DragBegin_Func(UnitCard_Script _unitCardClass)
    {
        selectCardClass = _unitCardClass;

        selectCardClass.transform.SetAsLastSibling();

        touchOffsetPos = Input.mousePosition - selectCardClass.transform.position;
    }
    public void Dragging_Func(UnitCard_Script _unitCardClass)
    {
        Vector3 _dragPos = Input.mousePosition - touchOffsetPos;

        selectCardClass.transform.position = _dragPos;
    }
    public void DragEnd_Func(UnitCard_Script _unitCardClass)
    {
        isTouchOn = false;

        if(contactCardClass != null)
        {
            // 파티 슬롯에 닿은 경우

            bool _isSwap = false;

            if(disbandCardClass != null)
            {
                // 파티카드에서 옮긴 경우(이탈)

                if(contactCardClass.slotState == PartySlot_Script.SlotState.Joined)
                {
                    // 닿아있는 파티 슬롯에 이미 유닛카드가 있는 경우

                    disbandCardClass.JoinParty_Func(contactCardClass.joinUnitData.joinUnitCardClass, true);
                    _isSwap = true;
                }
                else if(contactCardClass.slotState == PartySlot_Script.SlotState.Empty)
                {
                    // 닿아있는 파티 슬롯에 유닛카드가 없는 경우

                    disbandCardClass.OnEmpty_Func();
                }
                
                disbandCardClass = null;
            }
            else
            {
                // 유닛카드 슬롯에서 옮긴 경우

                if (contactCardClass.slotState == PartySlot_Script.SlotState.Joined)
                {
                    // 닿아있는 파티 슬롯에 이미 유닛카드가 있는 경우

                    _isSwap = true;
                    contactCardClass.OnEmpty_Func();
                }
                else if (contactCardClass.slotState == PartySlot_Script.SlotState.Empty)
                {
                    // 닿아있는 파티 슬롯에 유닛카드가 없는 경우

                    _isSwap = true;
                }
            }
            
            contactCardClass.JoinParty_Func(selectCardClass, _isSwap);
            contactCardClass = null;

            selectCardClass = null;
        }
        else
        {
            // 파티 슬롯에 닿지 않은 경우

            if (selectCardClass.cardState == UnitCard_Script.CardState.Active)
            {
                if (disbandCardClass != null)
                {
                    // 파티에서 이탈하는 경우

                    disbandCardClass.OnEmpty_Func();
                    disbandCardClass = null;
                }

                int _cardId = selectCardClass.cardId;
                selectCardClass.InitPos_Func();
                selectCardClass = null;
            }
            else if (selectCardClass.cardState == UnitCard_Script.CardState.Active_Party)
            {
                // 파티카드 슬롯에서 옮긴 경우

                if (disbandCardClass == null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (partySlotClassArr[i].joinUnitData.isDataOn == true)
                        {
                            if(partySlotClassArr[i].joinUnitData.joinUnitCardClass == selectCardClass)
                            {
                                selectCardClass.gameObject.transform.position = partySlotClassArr[i].transform.position;
                            }
                        }
                    }
                }
                else
                {
                    // 파티에서 이탈하는 경우

                    disbandCardClass.OnEmpty_Func();
                    disbandCardClass = null;

                    int _cardId = selectCardClass.cardId;
                    selectCardClass.InitPos_Func();
                    selectCardClass = null;
                }
            }
        }
    }
    #endregion
    #region Unit Card Group
    public void UnlockCard_Func(int _cardID)
    {
        unitCardClassArr[_cardID].SetState_Func(UnitCard_Script.CardState.Active);
    }
    #endregion
    #region Party Slot Group
    public void OnDisbandPartySlot_Func(PartySlot_Script _partySlotClass)
    {
        disbandCardClass = _partySlotClass;
    }
    public void DecontactCard_Func()
    {
        if (contactCardClass != null)
            contactCardClass.OnDecontact_Func();

        contactCardClass = null;
    }
    public void ContactCard_Func(PartySlot_Script _partySlotClass)
    {
        if (isTouchOn == true)
        {
            if (contactCardClass != null)
                contactCardClass.OnDecontact_Func();

            contactCardClass = _partySlotClass;

            contactCardClass.OnContact_Func();
        }
    }
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        Player_Data.Instance.JoinParty_Func(_partySlotId, _unitId);
    }
    #endregion
    #region Feeding Group
    public void OnFeeding_Func()
    {
        // Call : Btn Event

        StopCoroutine("DisplayAni_Cor");
        StartCoroutine("DisplayAni_Cor");
    }
    IEnumerator DisplayAni_Cor()
    {
        while(anim.IsPlaying("Display") == true)
        {
            anim["Display"].time = anim["Display"].length;
            yield return null;
        }

        RotateUnitImage_Func();
        lobbyManager.OnFeedingRoom_Func(selectUnitID);
    }
    void RotateUnitImage_Func()
    {
        Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(selectUnitID);

        unitImage.rectTransform.DORotate(Vector3.up * 180f, 0.5f);
        unitImage.DOColor(Color.black, 0.5f);
        unitImage.rectTransform.DOLocalMove(_unitClass.feedImagePos, 0.5f);
        unitImage.rectTransform.DOScale(_unitClass.feedImageSize, 0.5f);
    }
    
    public void ReturnUI_Func()
    {
        Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(selectUnitID);

        unitImage.transform.DORotate(Vector3.zero, 0.5f);
        unitImage.DOColor(Color.white, 0.5f);
        unitImage.transform.DOLocalMove(Vector3.up * _unitClass.imagePivotAxisY * 100f, 0.5f);
        unitImage.transform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
    }
    #endregion
    #region Print Group
    public void PrintInfoUI_Func()
    {
        Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(selectUnitID);
        
        unitInfoTextArr[0].text = string.Format("{0:N0}", (int)_unitClass.attackValue);
        unitInfoTextArr[1].text = string.Format("{0:N0}", (int)_unitClass.healthPoint_Max);
        unitInfoTextArr[2].text = string.Format("{0:N0}", _unitClass.defenceValue) + "%";
        unitInfoTextArr[3].text = string.Format("{0:N0}", _unitClass.criticalPercent) + "%";
        unitInfoTextArr[4].text = string.Format("{0:N0}", _unitClass.spawnNum_Limit);
        unitInfoTextArr[5].text = string.Format("{0:N0}", _unitClass.spawnInterval) + "s";
        unitInfoTextArr[6].text = "Lv." + Player_Data.Instance.playerUnitDataArr[_unitClass.unitID].unitLevel + " " + _unitClass.charName;
        unitInfoTextArr[7].text = _unitClass.charDesc;
    }
    public void ReadyUnitImage_Func()
    {
        Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(selectUnitID);
        changeUnitSprite = _unitClass.cardSprite;
        unitImagePivotAxisY = _unitClass.imagePivotAxisY * 100f;
        unitShadowSize = _unitClass.shadowSize;

        anim.Play("Display");
    }
    public void PrintUnitImage_Func()
    {
        // Call : Ani Event

        unitImage.transform.localPosition = Vector2.up * unitImagePivotAxisY;

        unitImage.sprite = changeUnitSprite;
        unitImage.SetNativeSize();
        unitImage.SetNaturalAlphaColor_Func(1f);

        unitShadowImage.SetNaturalAlphaColor_Func(1f);
        unitShadowImage.transform.localScale = unitShadowSize;
    }
    #endregion
}
