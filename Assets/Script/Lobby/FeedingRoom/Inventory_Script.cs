using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    public UpgradePlate_Script upgradePlateClass;
    public ReplaceCol_Script replaceColClass;
    [SerializeField]
    private List<Food_Script> foodClassList;

    public Transform bagGroupTrf;
    public Transform sortInitPos;
    public Vector2 sortGapPos;
    public int axisXNum;
    
    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        upgradePlateClass.Init_Func(_feedingRoomClass);
        replaceColClass.Init_Func(_feedingRoomClass);

        int _haveFoodNum = Player_Data.Instance.GetInventoryFoodNum_Func();
        foodClassList = new List<Food_Script>();
        for (int i = 0; i < _haveFoodNum; i++)
        {
            PlayerFood_Data _playerFoodData = Player_Data.Instance.GetPlayerFoodData_Func(i);
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_playerFoodData.haveFoodID];
            GameObject _foodObj = ObjectPoolManager.Instance.Get_Func(_foodData.foodName);
            Food_Script _foodClass = _foodObj.GetComponent<Food_Script>();
            _foodClass.Init_Func(_feedingRoomClass, _playerFoodData.level, _playerFoodData.foodExp);
            foodClassList.Add(_foodClass);
            Player_Data.Instance.inventoryFoodDataList[i].foodClass = _foodClass;

            _foodObj.transform.parent = bagGroupTrf;
            _foodObj.transform.localScale = Vector3.one;
        }

        SortInventory_Func();
    }
    void SortInventory_Func()
    {
        Vector2 _sortPos = sortInitPos.localPosition;
        for (int i = 0, count = -1; count < foodClassList.Count; i++)
        {
            for (int j = 0; j < axisXNum; j++)
            {
                count++;
                if (foodClassList.Count <= count)
                {
                    break;
                }
                _sortPos = 
                    new Vector2
                    (
                        sortInitPos.localPosition.x + (sortGapPos.x * j),
                        sortInitPos.localPosition.y + (sortGapPos.y * i)
                    );
                foodClassList[count].transform.localPosition = _sortPos;
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

    public void SetRegroupTrf_Func(Transform _regroupTrf, bool _isReplacePos = false)
    {
        _regroupTrf.parent = bagGroupTrf;

        if(_isReplacePos == true)
            _regroupTrf.localPosition = sortInitPos.localPosition;
    }

    public Food_Script GetFood_Func(int _inventoryID)
    {
        return foodClassList[_inventoryID];
    }

    public Food_Script GetFoodRand_Func()
    {
        return foodClassList[Random.Range(0, foodClassList.Count)];
    }

    public bool CheckInventoryFood_Func(Food_Script _foodClass)
    {
        for (int i = 0; i < foodClassList.Count; i++)
        {
            if (_foodClass == foodClassList[i])
                return true;
        }

        return false;
    }
    public void RemoveFood_Func(Food_Script _foodClass)
    {
        foodClassList.Remove(_foodClass);
    }
}
