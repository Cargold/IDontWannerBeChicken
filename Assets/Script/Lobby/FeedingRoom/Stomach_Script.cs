using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach_Script : MonoBehaviour
{
    private FeedingRoom_Script feedingRoomClass;
    [SerializeField]
    private StomachInner_Script innerClass;
    [SerializeField]
    private List<Food_Script> feedFoodClassList;
    [SerializeField]
    private Transform foodGroupTrf;
    [SerializeField]
    private Transform bottomPos;
    [SerializeField]
    private int stomachUnitID;
    public bool isActive;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        innerClass.Init_Func(this);
    }
    public void Active_Func(int _selectUnitID)
    {
        isActive = true;
        stomachUnitID = _selectUnitID;

        SpawnFood_Func();
    }
    void SpawnFood_Func()
    {
        PlayerFood_ClassData[] _playerFoodDataArr = null;

        if (stomachUnitID == 999)
            _playerFoodDataArr =
                Player_Data.Instance.heroFoodDataList.ToArray();
        else
            _playerFoodDataArr =
                Player_Data.Instance.playerUnitDataArr[stomachUnitID].GetPlayerFoodDataArr_Func();

        feedFoodClassList = new List<Food_Script>();

        for (int i = 0; i < _playerFoodDataArr.Length; i++)
        {
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_playerFoodDataArr[i].foodID];

            GameObject _foodObj = ObjectPool_Manager.Instance.Get_Func(_foodData.foodName);

            _foodObj.transform.position = _playerFoodDataArr[i].pos;
            _foodObj.transform.eulerAngles = _playerFoodDataArr[i].rot;
            _foodObj.transform.localScale = Vector3.one;
            ReplaceStomach_Func(_foodObj.transform);

            Food_Script _foodClass = _foodObj.GetComponent<Food_Script>();
            _foodClass.Init_Func
                (
                    feedingRoomClass, 
                    FoodState.FeedingByInner, 
                    _playerFoodDataArr[i].level, 
                    _playerFoodDataArr[i].remainExp
                );
            _foodClass.SetState_Func(FoodPlaceState.Stomach);
            feedFoodClassList.Add(_foodClass);

            _playerFoodDataArr[i].SetData_Func(_foodClass);
        }
    }
    public void Deactive_Func()
    {
        isActive = false;

        for (int i = 0; i < feedFoodClassList.Count; i++)
        {
            Player_Data.Instance.SetFoodData_Func(feedFoodClassList[i], false, stomachUnitID);
            ObjectPool_Manager.Instance.Free_Func(feedFoodClassList[i].gameObject);
        }

        stomachUnitID = -999;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Stomach);

            feedingRoomClass.RemoveFoodInInventory_Func(_foodClass);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isActive == false) return;

        if (collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Inventory);

            feedingRoomClass.AddFoodInInventroy_Func(_foodClass);
        }
    }
    public void FeedFoodByInner_Func(Food_Script _foodClass)
    {
        if(_foodClass.foodState == FoodState.Stomach)
        {
            if (feedFoodClassList.Contains(_foodClass) == false)
            {
                _foodClass.FeedingByInner_Func();
                FeedFood_Func(_foodClass);
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
            if (feedFoodClassList.Contains(_foodClass) == false)
            {
                _foodClass.FeedingByChain_Func();
                FeedFood_Func(_foodClass);
            }
        }
    }
    void FeedFood_Func(Food_Script _foodClass)
    {
        feedFoodClassList.Add(_foodClass);
        ReplaceStomach_Func(_foodClass.transform);
        Player_Data.Instance.FeedFood_Func(stomachUnitID, _foodClass);
    }
    public void OutFoodByInner_Func(Food_Script _foodClass)
    {
        if (isActive == false) return;

        if(_foodClass.foodState == FoodState.FeedingByInner)
        {
            if (feedFoodClassList.Contains(_foodClass) == true)
            {
                _foodClass.OutFoodByInner_Func();
                OutFood_Func(_foodClass);
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
    public void OutFoodByStomachRange_Func(Food_Script _foodClass)
    {
        if (isActive == false) return;

        if (0 < (int)_foodClass.foodState)
        {
            if (feedFoodClassList.Contains(_foodClass) == true)
            {
                OutFood_Func(_foodClass);
            }
            else
            {
                Debug.LogError("Bug : 뱃속에 없는 음식입니다.");
                Debug.LogError("음식 이름 : " + _foodClass.foodName);
            }
        }
    }
    void OutFood_Func(Food_Script _foodClass)
    {
        feedFoodClassList.Remove(_foodClass);
        Player_Data.Instance.OutFood_Func(stomachUnitID, _foodClass);
        Player_Data.Instance.AddFood_Func(_foodClass);
    }

    public Food_Script GetFoodRand_Func()
    {
        if (feedFoodClassList.Count == 0)
            return null;
        else
            return feedFoodClassList[Random.Range(0, feedFoodClassList.Count)];
    }

    public void ReplaceStomach_Func(Transform _trf)
    {
        _trf.SetParent(foodGroupTrf);
    }
}
