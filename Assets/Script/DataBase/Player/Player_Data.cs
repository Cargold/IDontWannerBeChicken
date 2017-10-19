using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    [SerializeField]
    private int goldValue;
    [SerializeField]
    private int mineralValue;

    public int[] partyUnitIdArr;
    [SerializeField]
    public PlayerUnit_Data[] playerUnitDataArr;

    [SerializeField]
    public PlayerFood_Data[] playerFoodDataArr;

    [SerializeField]
    private PlayerWealth_Script playerWealthClass;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        SetWealth_Func(WealthType.Gold, goldValue);
        SetWealth_Func(WealthType.Mineral, mineralValue);

        yield break;
    }
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        partyUnitIdArr[_partySlotId] = _unitId;
    }
    public void DisbandParty_Func(int _partySlotId, int _unitId)
    {
        if(partyUnitIdArr[_partySlotId] == _unitId)
        {
            partyUnitIdArr[_partySlotId] = -1;
        }
        else
        {
            Debug.LogError("Bug : 파티해제하려는 유닛과 기존 파티 유닛이 서로 정보가 다릅니다.");
        }
    }

    public bool SetWealth_Func(WealthType _wealthType, int _value)
    {
        if(_wealthType == WealthType.Gold)
        {
            if (_value < 0)
            {
                // 지불

                if (_value <= goldValue)
                {
                    goldValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
                    return true;
                }
                else
                {
                    // 재화가 부족함

                    return false;
                }
            }
            else
            {
                // 획득

                goldValue += _value;
                playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
                return true;
            }
        }
        else if(_wealthType == WealthType.Mineral)
        {
            if (_value < 0)
            {
                // 지불

                if (_value <= mineralValue)
                {
                    mineralValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
                    return true;
                }
                else
                {
                    // 재화가 부족함

                    return false;
                }
            }
            else
            {
                // 획득

                mineralValue += _value;
                playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
                return true;
            }
        }
        else
        {
            Debug.LogError("Bug : 재화 종류 선택이 잘못되었습니다.");
            return false;
        }
    }
}