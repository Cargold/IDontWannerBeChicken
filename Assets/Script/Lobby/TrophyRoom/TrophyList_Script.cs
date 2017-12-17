using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyList_Script : MonoBehaviour
{
    public TrophyRoom_Script trophyRoomClass;

    public Image trophyImage;
    public Text trophyNumText;
    public Text trophyEffectText;

    public void Init_Func(TrophyRoom_Script _trophyRoomClass, int _trophyID)
    {
        trophyRoomClass = _trophyRoomClass;

        int _languageID = TranslationSystem_Manager.Instance.languageTypeID;

        Trophy_Data _trophyData = DataBase_Manager.Instance.trophyDataArr[_trophyID];
        trophyImage.sprite = _trophyData.trophySprite;
        trophyImage.SetNativeSize();

        int _trophyNum = Player_Data.Instance.GetTrophyNum_Func(_trophyID);
        trophyNumText.text = TranslationSystem_Manager.Instance.trophyRoomNumDesc + _trophyNum;

        trophyEffectText.text = _trophyData.descArr[_languageID];
    }

    public void SetNum_Func(int _trophyID)
    {
        Trophy_Data _trophyData = DataBase_Manager.Instance.trophyDataArr[_trophyID];
        int _languageID = TranslationSystem_Manager.Instance.languageTypeID;

        int _trophyNum = Player_Data.Instance.GetTrophyNum_Func(_trophyID);
        trophyNumText.text = TranslationSystem_Manager.Instance.trophyRoomNumDesc + _trophyNum;

        float _value = Player_Data.Instance.GetCalcTrophyEffect_Func(_trophyID, true);
        trophyEffectText.text = _trophyData.descArr[_languageID] + " +" + (int)_value + "%";
    }
}
