using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerUnit_Data
{
    public bool isHave;
    public int level;
    public Unit_Script unitClass;

    public List<PlayerFood_Data> playerFoodDataList;

    public IEnumerator Init_Cor()
    {
        // 유닛의 음식 정보 불러오기

        for (int i = 0; i < playerFoodDataList.Count; i++)
        {

        }

        yield break;
    }
}
