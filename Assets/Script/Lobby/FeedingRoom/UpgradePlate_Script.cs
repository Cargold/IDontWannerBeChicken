using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlate_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _materialFoodClass = collision.GetComponent<Food_Script>();
            feedingRoomClass.UpgradeFoodApproach_Func(_materialFoodClass);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _exitFoodClass = collision.GetComponent<Food_Script>();
            feedingRoomClass.UpgradeFoodAway_Func(_exitFoodClass);
        }
    }
}
