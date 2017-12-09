using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachInner_Script : MonoBehaviour
{
    private Stomach_Script stomachClass;

    public void Init_Func(Stomach_Script _stomachClass)
    {
        stomachClass = _stomachClass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            //stomachClass.FeedFoodByInner_Func(_foodClass);
            // CargoldFeed
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            //stomachClass.OutFoodByInner_Func(_foodClass);
            // CargoldFeed
        }
    }
}
