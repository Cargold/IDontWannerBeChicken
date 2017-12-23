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
            string _foodObjName = "";
            if (_playerFoodDataArr[i].foodType == FoodType.Normal)
                _foodObjName = DataBase_Manager.Instance.foodDataObjArr[_playerFoodDataArr[i].foodID].gameObject.name;
            else if (_playerFoodDataArr[i].foodType == FoodType.Source)
                _foodObjName = DataBase_Manager.Instance.sauceDataObjArr[_playerFoodDataArr[i].foodID].gameObject.name;
            else if (_playerFoodDataArr[i].foodType == FoodType.Stone)
                _foodObjName = DataBase_Manager.Instance.stoneDataObj.gameObject.name;

            GameObject _foodObj = ObjectPool_Manager.Instance.Get_Func(_foodObjName);
            _foodObj.transform.position = _playerFoodDataArr[i].pos;
            _foodObj.transform.eulerAngles = _playerFoodDataArr[i].rot;
            _foodObj.transform.localScale = Vector3.one;
            ReplaceStomach_Func(_foodObj.transform);

            Food_Script _foodClass = _foodObj.GetComponent<Food_Script>();
            _foodClass.Init_Func
                (
                    feedingRoomClass, 
                    FoodPlaceState.Stomach,
                    i,
                    _playerFoodDataArr[i].level, 
                    _playerFoodDataArr[i].remainExp
                );
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
            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Dragging_Inven);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isActive == false) return;

        if (collision.tag == "Food")
        {
            Food_Script _foodClass = collision.transform.parent.GetComponent<Food_Script>();

            feedingRoomClass.SetFoodPlaceState_Func(_foodClass, FoodPlaceState.Inventory);
        }
    }
    public void FeedFood_Func(Food_Script _foodClass)
    {
        feedFoodClassList.Add(_foodClass);
        _foodClass.placeID = feedFoodClassList.Count;

        ReplaceStomach_Func(_foodClass.transform);
    }
    public void OutFood_Func(Food_Script _foodClass)
    {
        if (isActive == false) return;

        if (_foodClass.foodPlaceState != FoodPlaceState.Stomach)
        {
            if (feedFoodClassList.Contains(_foodClass) == true)
            {
                feedFoodClassList.Remove(_foodClass);
                _foodClass.placeID = -1;
            }
            else
            {
                Debug.LogError("Bug : 뱃속에 없는 음식입니다.");
                Debug.LogError("음식 이름 : " + _foodClass.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
            }
        }
    }

    public Food_Script GetFoodRand_Func()
    {
        if (feedFoodClassList.Count == 0)
            return null;
        else
            return feedFoodClassList[Random.Range(0, feedFoodClassList.Count)];
    }
    public bool GetFoodHave_Func(Food_Script _foodClass)
    {
        return feedFoodClassList.Contains(_foodClass);
    }

    public void ReplaceStomach_Func(Transform _trf)
    {
        _trf.SetParent(foodGroupTrf);
    }
}
