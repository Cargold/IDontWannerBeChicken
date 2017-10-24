using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceCol_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    public bool isActive;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;
    }

    public void Active_Func()
    {
        isActive = true;
    }
    public void Deactive_Func()
    {
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive == false) return;

        if(collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.ReplaceFood_Func(_foodClass);
        }
    }
}
