using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaketInner_Script : MonoBehaviour
{
    public Basket_Script basketClass;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Debug.Log("Test, Tri");

            Food_Script _foodClass = collision.GetComponent<Food_Script>();

            basketClass.GetFood_Func(_foodClass);
        }
    }
}
