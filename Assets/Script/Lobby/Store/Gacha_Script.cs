using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gacha_Script : MonoBehaviour
{
    public StoreRoom_Script storeRoomClass;
    public Image boxImage;
    public GachaCard_Script[] gachaCardArr;

    public void Init_Func(StoreRoom_Script _storeRoomClass)
    {
        for (int i = 0; i < gachaCardArr.Length; i++)
        {
            gachaCardArr[i].Init_Func(this);
        }
    }

    public void OnGacha_Func(GachaType _gachaType, Store_Data _storeData)
    {
        switch (_gachaType)
        {
            case GachaType.NormalBox:
                OnGacha_NormalBox_Func(_storeData);
                break;
            case GachaType.SpeicalBox:
                OnGacha_SpecialBox_Func(_storeData);
                break;
        }
    }
    void OnGacha_NormalBox_Func(Store_Data _storeData)
    {

    }
    void OnGacha_SpecialBox_Func(Store_Data _storeData)
    {

    }

    public void CardResizeClear_Func()
    {

    }
}
