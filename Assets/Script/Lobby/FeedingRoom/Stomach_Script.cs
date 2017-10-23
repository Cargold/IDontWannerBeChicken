using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach_Script : MonoBehaviour
{
    private FeedingRoom_Script feedingRoomClass;
    [SerializeField]
    private StomachInner_Script innerClass;
    [SerializeField]
    private List<Food_Script> foodClassList;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        innerClass.Init_Func(this);

        foodClassList = new List<Food_Script>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Stomach);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Inventory);
        }
    }
    public void FeedFoodByInner_Func(Food_Script _foodClass)
    {
        if (foodClassList.Contains(_foodClass) == false)
        {
            _foodClass.FeedingByInner_Func();
            foodClassList.Add(_foodClass);
        }
    }
    public void FeedFoodByChain_Func(Food_Script _foodClass)
    {
        if (foodClassList.Contains(_foodClass) == false)
        {
            _foodClass.FeedingByChain_Func();
            foodClassList.Add(_foodClass);
        }
    }

    public void OutFood_Func(Food_Script _foodClass)
    {
        if (foodClassList.Contains(_foodClass) == true)
        {
            _foodClass.OutFood_Func();
            foodClassList.Remove(_foodClass);
        }
        else
        {
            Debug.LogError("Bug : 뱃속에 없는 음식입니다.");
            Debug.LogError("음식 이름 : " + _foodClass.foodName);
        }
    }

    public bool CheckFeedingFood_Func(Food_Script _foodClass)
    {
        return foodClassList.Contains(_foodClass);
    }
}
