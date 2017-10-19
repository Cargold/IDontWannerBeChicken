using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    public Food_Script[] foodClassArr;

    public Transform sortInitPos;
    public Vector2 sortGapPos;
    public int axisXNum;
    public bool isInit = false;
    
    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        int _haveFoodNum = Player_Data.Instance.playerFoodDataArr.Length;
        foodClassArr = new Food_Script[_haveFoodNum];
        for (int i = 0; i < _haveFoodNum; i++)
        {
            GameObject _foodObj = Instantiate(Game_Manager.Instance.foodObj);
            Food_Script _foodClass = _foodObj.GetComponent<Food_Script>();
            PlayerFood_Data _playerFoodData = Player_Data.Instance.playerFoodDataArr[i];
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_playerFoodData.haveFoodID];
            _foodClass.Init_Func(_feedingRoomClass, _foodData, _playerFoodData.level, _playerFoodData.foodExp);
            foodClassArr[i] = _foodClass;
            Player_Data.Instance.playerFoodDataArr[i].foodClass = _foodClass;

            _foodObj.transform.parent = this.transform;
            _foodObj.transform.localScale = Vector3.one;
        }

        SortInventory_Func();
    }
    void SortInventory_Func()
    {
        Vector2 _sortPos = sortInitPos.localPosition;
        for (int i = 0, count = -1; count < foodClassArr.Length; i++)
        {
            for (int j = 0; j < axisXNum; j++)
            {
                count++;
                if (foodClassArr.Length <= count)
                {
                    break;
                }
                _sortPos = 
                    new Vector2
                    (
                        sortInitPos.localPosition.x + (sortGapPos.x * j),
                        sortInitPos.localPosition.y + (sortGapPos.y * i)
                    );
                foodClassArr[count].transform.localPosition = _sortPos;
            }
        }
    }

    public void Active_Func()
    {
        SortInventory_Func();
    }

    public void Deactive_Func()
    {

    }

    public void SetRegroupTrf_Func(Transform _regroupTrf)
    {
        _regroupTrf.parent = this.transform;
    }

    public Food_Script GetFood_Func(int _inventoryID)
    {
        return foodClassArr[_inventoryID];
    }

    public Food_Script GetFoodRand_Func()
    {
        return foodClassArr[Random.Range(0, foodClassArr.Length)];
    }
}
