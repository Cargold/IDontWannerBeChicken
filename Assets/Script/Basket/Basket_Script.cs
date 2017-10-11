using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket_Script : MonoBehaviour
{
    public List<Food_Script> foodClassLIst;

    private void Awake()
    {
        foodClassLIst = new List<Food_Script>();
    }

    public void GetFood_Func(Food_Script _foodClass)
    {
        foodClassLIst.Add(_foodClass);
    }

    public void OutFood_Func(Food_Script _foodClass)
    {
        foodClassLIst.Remove(_foodClass);
    }
}
