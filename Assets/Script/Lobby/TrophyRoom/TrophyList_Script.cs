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
        int _languageID = TranslationSystem_Manager.Instance.languageTypeID;

        trophyRoomClass = _trophyRoomClass;

        Trophy_Data _trophyData = DataBase_Manager.Instance.trophyDataArr[_trophyID];
        trophyImage.sprite = _trophyData.trophySprite;
        trophyImage.SetNativeSize();

        int _trophyNum = Player_Data.Instance.GetTrophyNum_Func(_trophyID);
        trophyNumText.text = TranslationSystem_Manager.Instance.trophyRoomNumDesc + _trophyNum;

        trophyEffectText.text = _trophyData.descArr[_languageID];
    }
}
