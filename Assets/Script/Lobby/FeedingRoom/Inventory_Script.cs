using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    public UpgradePlate_Script upgradePlateClass;
    public ReplaceCol_Script replaceColClass;
    [SerializeField]
    private List<Food_Script> inventoryFoodClassList;

    public Transform bagGroupTrf;
    public Transform sortInitPos;
    public Vector2 sortGapPos;
    public int axisXNum;
    
    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        upgradePlateClass.Init_Func(_feedingRoomClass);
        replaceColClass.Init_Func(_feedingRoomClass);
    }

    public void Active_Func(int _selectUnitID)
    {
        SettingInventoryFood_Func();

        if (_selectUnitID == 999)
        {
            // Hero

            SetStomachFood_Func();
        }
        else
        {
            // Unit

            SetStomachFood_Func();
        }

        replaceColClass.Active_Func();
    }
    void SettingInventoryFood_Func()
    {
        int _haveFoodNum = Player_Data.Instance.GetInventoryFoodNum_Func();
        inventoryFoodClassList = new List<Food_Script>();
        for (int i = 0; i < _haveFoodNum; i++)
        {
            PlayerFood_ClassData _playerFoodData = Player_Data.Instance.GetPlayerFoodData_Func(i);
            
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_playerFoodData.foodID];

            GameObject _foodObj = ObjectPool_Manager.Instance.Get_Func(_foodData.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
            Food_Script _foodClass = _foodObj.GetComponent<Food_Script>();
            _foodClass.Init_Func(feedingRoomClass, FoodState.Inventory, _playerFoodData.level, _playerFoodData.remainExp);
            _foodClass.SetState_Func(FoodPlaceState.Inventory);
            inventoryFoodClassList.Add(_foodClass);
            Player_Data.Instance.SetFoodClassInInventory_Func(_foodClass, i);

            _foodObj.transform.SetParent(bagGroupTrf);
        }

        SortInventory_Func();
    }
    void SortInventory_Func()
    {
        Vector2 _sortPos = sortInitPos.localPosition;
        for (int i = 0, count = -1; count < inventoryFoodClassList.Count; i++)
        {
            for (int j = 0; j < axisXNum; j++)
            {
                count++;
                if (inventoryFoodClassList.Count <= count)
                {
                    break;
                }
                _sortPos =
                    new Vector2
                    (
                        sortInitPos.localPosition.x + (sortGapPos.x * j),
                        sortInitPos.localPosition.y + (sortGapPos.y * i)
                    );
                inventoryFoodClassList[count].transform.localPosition = _sortPos;
                inventoryFoodClassList[count].transform.localScale = Vector3.one;
            }
        }
    }
    void SetStomachFood_Func()
    {

    }

    public void Deactive_Func()
    {
        for (int i = 0; i < inventoryFoodClassList.Count; i++)
        {
            Player_Data.Instance.SetFoodData_Func(inventoryFoodClassList[i], true);

            ObjectPool_Manager.Instance.Free_Func(inventoryFoodClassList[i].gameObject);
        }

        inventoryFoodClassList.Clear();

        replaceColClass.Deactive_Func();
    }

    public void SetRegroupTrf_Func(Transform _regroupTrf, bool _isReplacePos = false)
    {
        if (_regroupTrf.gameObject.activeInHierarchy == false) return;

        _regroupTrf.SetParent(bagGroupTrf);

        if(_isReplacePos == true)
            _regroupTrf.localPosition = sortInitPos.localPosition;
    }

    public Food_Script GetFood_Func(int _inventoryID)
    {
        return inventoryFoodClassList[_inventoryID];
    }
    public Food_Script GetFoodRand_Func()
    {
        if (inventoryFoodClassList.Count == 0)
            return null;
        else
            return inventoryFoodClassList[Random.Range(0, inventoryFoodClassList.Count)];
    }

    public bool CheckInventoryFood_Func(Food_Script _foodClass)
    {
        return inventoryFoodClassList.Contains(_foodClass);
    }
    public void AddFood_Func(Food_Script _foodClass)
    {
        if(inventoryFoodClassList.Contains(_foodClass) == false)
        {
            inventoryFoodClassList.Add(_foodClass);
        }
        else
        {
            Debug.LogError("Bug : 이미 가방에 있는 음식이 추가되었습니다.");
            Debug.LogError("Name : " + _foodClass.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
        }
    }
    public void RemoveFood_Func(Food_Script _foodClass)
    {
        if (inventoryFoodClassList.Contains(_foodClass) == true)
        {
            inventoryFoodClassList.Remove(_foodClass);
        }
        else
        {
            Debug.LogError("Bug : 가방에 없는 음식을 제거하였습니다.");
            Debug.LogError("Name : " + _foodClass.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
        }
    }
}
