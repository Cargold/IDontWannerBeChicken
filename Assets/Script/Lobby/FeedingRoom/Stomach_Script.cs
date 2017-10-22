using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    public List<Food_Script> foodClassList;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            _foodClass.IntoStomach_Func(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            _foodClass.IntoStomach_Func(false);

            _foodClass.transform.rotation = Quaternion.identity;

            feedingRoomClass.ReplaceFood_Func(_foodClass, true);
        }
    }
}
