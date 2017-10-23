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
    [SerializeField]
    private Transform foodGroupTrf;

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
        if(_foodClass.foodState == FoodState.Stomach)
        {
            if (foodClassList.Contains(_foodClass) == false)
            {
                _foodClass.FeedingByInner_Func();
                foodClassList.Add(_foodClass);
            }
        }
        else if(_foodClass.foodState == FoodState.FeedingByChain)
        {
            _foodClass.FeedingByInner_Func();
        }
    }
    public void FeedFoodByChain_Func(Food_Script _foodClass)
    {
        if (_foodClass.foodState == FoodState.Stomach)
        {
            if (foodClassList.Contains(_foodClass) == false)
            {
                _foodClass.FeedingByChain_Func();
                foodClassList.Add(_foodClass);
            }
        }
    }
    public void OutFoodByInner_Func(Food_Script _foodClass)
    {
        if(_foodClass.foodState == FoodState.FeedingByInner)
        {
            if (foodClassList.Contains(_foodClass) == true)
            {
                _foodClass.OutFoodByInner_Func();
                foodClassList.Remove(_foodClass);
            }
            else
            {
                Debug.LogError("Bug : 뱃속에 없는 음식입니다.");
                Debug.LogError("음식 이름 : " + _foodClass.foodName);
            }
        }
        else
        {
            Debug.LogError("Bug : 뱃속 상태가 아닙니다.");
            Debug.LogError("음식 이름 : " + _foodClass.foodName);
        }
    }
    public void OutFoodByChain_Func(Food_Script _foodClass)
    {
        if (_foodClass.foodState == FoodState.FeedingByChain)
        {
            if (foodClassList.Contains(_foodClass) == true)
            {
                _foodClass.OutFoodByChain_Func();
                foodClassList.Remove(_foodClass);
            }
            else
            {
                Debug.LogError("Bug : 뱃속에 없는 음식입니다.");
                Debug.LogError("음식 이름 : " + _foodClass.foodName);
            }
        }
        else
        {
            Debug.LogError("Bug : 연쇄 상태가 아닙니다.");
            Debug.LogError("음식 이름 : " + _foodClass.foodName);
        }
    }

    public void ReplaceStomach_Func(Transform _trf)
    {
        _trf.SetParent(foodGroupTrf);
    }
}
