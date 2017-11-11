using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCard_Script : MonoBehaviour
{
    public StoreRoom_Script storeRoomClass;
    public int cardID;

    public void Init_Func(StoreRoom_Script _storeRoomClass, int _cardID)
    {
        storeRoomClass = _storeRoomClass;
        cardID = _cardID;
    }

    public void OnButton_Func()
    {
        storeRoomClass.OnListButton_Func(cardID);
    }
}
