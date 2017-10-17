using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    public int goldValue;
    public int mineralValue;

    public int[] partyUnitIdArr;
    [SerializeField]
    public PlayerUnit_Data[] playerUnitDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        //partyUnitIdArr = new int[5];
        //for (int i = 0; i < 5; i++)
        //{
        //    partyUnitIdArr[i] = -1;
        //}

        //int _unitNum = DataBase_Manager.Instance.charDataArr.Length;
        //playerUnitDataArr = new PlayerUnit_Data[_unitNum];

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
}