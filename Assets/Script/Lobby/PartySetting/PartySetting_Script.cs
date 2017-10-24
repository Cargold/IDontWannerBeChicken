using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySetting_Script : LobbyUI_Parent
{
    public Transform unitCardGroupTrf;
    public Vector2 unitCardGap;
    public GameObject unitCardObj;
    public UnitCard_Script[] unitCardClassArr;
    public Vector2[] unitCardPosInitArr;
    public UnitCard_Script selectCardClass;
    public int selectUnitID;

    public PartySlot_Script[] partySlotClassArr;
    public PartySlot_Script contactCardClass;
    public PartySlot_Script disbandCardClass;
    public Transform partySlotGroupTrf;
    public Text playerPopulationText;

    public int populationValue_Max;
    public int populationValue_Recent;

    public Text[] unitInfoTextArr;
    public Image unitImage;

    private Vector3 touchOffsetPos;

    #region Override Group
    protected override void InitUI_Func()
    {
        InitUnitCardSlot_Func();
        InitPartySlot_Func();
        InitPartymember_Func();

        playerPopulationText.text = populationValue_Recent.ToString();

        this.gameObject.SetActive(false);
    }
    void InitUnitCardSlot_Func()
    {
        int _cardNum = DataBase_Manager.Instance.charDataArr.Length;

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

                    unitCardClassArr[_cardCount] = _unitCardObj.GetComponent<UnitCard_Script>();
                    unitCardClassArr[_cardCount].Init_Func
                        (this, _cardCount, UnitCard_Script.CardState.Active);

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
            partySlotClassArr[i].Init_Func(this, i, PartySlot_Script.CardState.Empty);
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

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);
    }

    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
    #region Unit Slot Group
    public void SelectUnit_Func(UnitCard_Script _unitCardClass)
    {
        selectCardClass = _unitCardClass;

        selectUnitID = selectCardClass.cardId;

        PrintInfoUI_Func();
        PrintUnitImage_Func();
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
        if(contactCardClass != null)
        {
            // 파티 슬롯에 닿은 경우

            bool _isSwap = false;

            if(disbandCardClass != null)
            {
                // 파티카드에서 옮긴 경우(이탈)

                if(contactCardClass.cardState == PartySlot_Script.CardState.Join)
                {
                    // 닿아있는 파티 슬롯에 이미 유닛카드가 있는 경우

                    disbandCardClass.JoinParty_Func(contactCardClass.joinUnitCardClass, true);
                    _isSwap = true;
                }
                else if(contactCardClass.cardState == PartySlot_Script.CardState.Empty)
                {
                    // 닿아있는 파티 슬롯에 유닛카드가 없는 경우

                    disbandCardClass.OnEmpty_Func();
                }
                
                disbandCardClass = null;
            }
            else
            {
                // 유닛카드 슬롯에서 옮긴 경우

                if (contactCardClass.cardState == PartySlot_Script.CardState.Join)
                {
                    // 닿아있는 파티 슬롯에 이미 유닛카드가 있는 경우

                    int _contactCardPopulValue = contactCardClass.joinUnitCardClass.populValue;
                    int _selectCardPopulValue = selectCardClass.populValue;
                    int _checkValue = _selectCardPopulValue - _contactCardPopulValue;

                    if(CheckPopulation_Func(_checkValue) == true)
                    {
                        // 새로운 카드 인구수 - 기존 카드 인구수, 남은 인구수와 비교

                        contactCardClass.OnEmpty_Func();
                    }
                }
            }
            
            contactCardClass.JoinParty_Func(selectCardClass, _isSwap);
            contactCardClass = null;

            selectCardClass = null;
        }
        else
        {
            // 파티 슬롯에 닿지 않은 경우

            if(disbandCardClass != null)
            {
                // 파티에서 이탈하는 경우

                disbandCardClass.OnEmpty_Func();
                disbandCardClass = null;
            }

            int _cardId = selectCardClass.cardId;
            selectCardClass.InitPos_Func();
            selectCardClass = null;
        }
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
        if(contactCardClass != null)
            contactCardClass.OnDecontact_Func();

        contactCardClass = _partySlotClass;

        contactCardClass.OnContact_Func();
    }
    public bool CheckPopulation_Func(int _populValue)
    {
        int _calcValue = populationValue_Recent - _populValue;

        if (0 <= _calcValue)
        {
            return true;
        }
        else
        {
            StopCoroutine("FailPopul_Cor");
            StartCoroutine("FailPopul_Cor");

            return false;
        }
    }
    IEnumerator FailPopul_Cor()
    {
        playerPopulationText.color = Color.red;
        playerPopulationText.fontSize = 75;

        yield return new WaitForSeconds(0.5f);

        playerPopulationText.color = Game_Manager.Instance.textColor;
        playerPopulationText.fontSize = 50;
    }
    public void CalcPopulation_Func(int _populValue)
    {
        populationValue_Recent -= _populValue;
        playerPopulationText.text = populationValue_Recent.ToString();
    }
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        Player_Data.Instance.JoinParty_Func(_partySlotId, _unitId);
    }
    #endregion
    #region Feeding Group
    public void OnFeeding_Func()
    {
        RotateUnitImage_Func();
        lobbyManager.OnFeedingRoom_Func(selectUnitID);
    }
    void RotateUnitImage_Func()
    {

    }
    #endregion
    #region Print Group
    public void PrintInfoUI_Func()
    {
        Unit_Script _unitClass = Player_Data.Instance.playerUnitDataArr[selectUnitID].unitClass;

        unitInfoTextArr[0].text = ((int)_unitClass.attackValue).ToString();
        unitInfoTextArr[1].text = ((int)_unitClass.healthPoint_Max).ToString();
        unitInfoTextArr[2].text = _unitClass.defenceValue.ToString() + "%";
        unitInfoTextArr[3].text = _unitClass.criticalPercent.ToString() + "%";
        unitInfoTextArr[4].text = _unitClass.spawnNum.ToString();
        unitInfoTextArr[5].text = _unitClass.spawnInterval.ToString() + "s";
        unitInfoTextArr[6].text = _unitClass.charName;
        unitInfoTextArr[7].text = _unitClass.charDesc;
    }
    public void PrintUnitImage_Func()
    {
        Unit_Script _unitClass = Player_Data.Instance.playerUnitDataArr[selectUnitID].unitClass;

        unitImage.sprite = _unitClass.charSprite;
        unitImage.SetNativeSize();
    }
    #endregion
}
