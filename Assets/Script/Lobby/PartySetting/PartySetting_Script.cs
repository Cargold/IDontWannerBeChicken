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

    public PartySlot_Script[] partySlotClassArr;
    public PartySlot_Script contactCardClass;
    public PartySlot_Script disbandCardClass;
    public Transform partySlotGroupTrf;
    public Text playerPopulationText;
    bool isPopulFailEffectProceed;

    public int populationValue_Max;
    public int populationValue_Recent;

    public Text[] unitInfoTextArr;
    public Image unitInfoImage;

    #region Override Group
    protected override void InitUI_Func()
    {
        InitUnitCardSlot_Func();
        InitPartySlot_Func();

        playerPopulationText.text = populationValue_Recent.ToString();
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
                    _unitCardObj.transform.parent = unitCardGroupTrf;

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

        int _unitId = selectCardClass.cardId;
        Character_Data _charData = DataBase_Manager.Instance.charDataArr[_unitId];

        unitInfoTextArr[0].text = ((int)_charData.attackValue).ToString();
        unitInfoTextArr[1].text = ((int)_charData.healthPoint).ToString();
        unitInfoTextArr[2].text = _charData.defenceValue.ToString() + "%";
        unitInfoTextArr[3].text = _charData.criticalPercent.ToString() + "%";
        unitInfoTextArr[4].text = _charData.spawnNum.ToString();
        unitInfoTextArr[5].text = _charData.spawnInterval.ToString() + "s";
        unitInfoTextArr[6].text = _charData.charName;
        unitInfoTextArr[7].text = _charData.charDesc;

        unitInfoImage.sprite = _charData.charSprite;
        unitInfoImage.SetNativeSize();
    }
    public void DragBegin_Func(UnitCard_Script _unitCardClass)
    {
        selectCardClass = _unitCardClass;

        selectCardClass.transform.SetAsLastSibling();
    }
    public void Dragging_Func(UnitCard_Script _unitCardClass)
    {
        selectCardClass.transform.position = Input.mousePosition;
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
    #endregion
}
