using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
