using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    // Wealth
    [SerializeField]
    private int goldValue;
    [SerializeField]
    private int mineralValue;
    [SerializeField]
    private PlayerWealth_Script playerWealthClass;

    // Party
    public int[] partyUnitIdArr;

    // Unit
    [SerializeField]
    public PlayerUnit_Data[] playerUnitDataArr;

    // Hero
    public int levelHero;
    public List<PlayerFood_Data> heroFoodDataList;

    // Inventory
    public List<PlayerFood_Data> inventoryFoodDataList;

    // Trophy

    // Skill

    // Test
    public PlayerFood_DataTemp[] test_InventoryDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield return InitWealth_Cor();
        yield return InitCharacter_Cor();

        yield break;
    }
    IEnumerator InitWealth_Cor()
    {
        // 골드, 미네랄 데이터 불러오기

        playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

        Test_InventoryFood_Func();

        yield break;
    }
    IEnumerator InitCharacter_Cor()
    {
        // 캐릭터 정보 불러오기

        for (int i = 0; i < playerUnitDataArr.Length; i++)
        {
            //playerUnitDataArr[i].unitClass
        }

        yield break;
    }
    void Test_InventoryFood_Func()
    {
        inventoryFoodDataList = new List<PlayerFood_Data>();

        for (int i = 0; i < 14; i++)
        {
            PlayerFood_Data _playerFoodData = new PlayerFood_Data();
            //_playerFoodData.level = 1;
            _playerFoodData.level = Random.Range(1, 4);
            int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
            //_playerFoodData.haveFoodID = 0;
            _playerFoodData.haveFoodID = Random.Range(0, _foodIDMax);
            //_playerFoodData.foodExp = 0f;
            _playerFoodData.foodExp = Random.Range(0f, 99f);

            inventoryFoodDataList.Add(_playerFoodData);
        }
    }

    #region Party Group
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        partyUnitIdArr[_partySlotId] = _unitId;
    }
    public void DisbandParty_Func(int _partySlotId, int _unitId)
    {
        if(partyUnitIdArr[_partySlotId] == _unitId)
        {
            partyUnitIdArr[_partySlotId] = -1;
        }
        else
        {
            Debug.LogError("Bug : 파티해제하려는 유닛과 기존 파티 유닛이 서로 정보가 다릅니다.");
        }
    }
    #endregion
    #region Wealth Group
    public void AddWealth_Func(WealthType _wealthType, int _value)
    {
        if (_wealthType == WealthType.Gold)
        {
            goldValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        }
        else if (_wealthType == WealthType.Mineral)
        {
            mineralValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
        }
    }
    public bool PayWealth_Func(WealthType _wealthType, int _value, bool _isJustCheck = false)
    {
        if (_wealthType == WealthType.Gold)
        {
            // 지불

            if (_value <= goldValue)
            {
                if (_isJustCheck == false)
                {
                    goldValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
                }

                return true;
            }
            else
            {
                // 재화가 부족함

                return false;
            }
        }
        else if (_wealthType == WealthType.Mineral)
        {
            if (_value <= mineralValue)
            {
                if (_isJustCheck == false)
                {
                    mineralValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
                }

                return true;
            }
            else
            {
                // 재화가 부족함

                return false;
            }
        }
        else
        {
            Debug.LogError("Bug : 재화 종류 선택이 잘못되었습니다.");
            return false;
        }
    }
    public void ActiveWealthUI_Func()
    {
        playerWealthClass.Active_Func();
    }
    public void DeactiveWealthUI_Func()
    {
        playerWealthClass.Deactive_Func();
    }
    #endregion
    #region Food Group
    public void AddFood_Func(Food_Script _foodClass)
    {
        PlayerFood_Data _playerFoodData = new PlayerFood_Data();
        _playerFoodData.SetData_Func(_foodClass);

        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void RemoveFood_Func(Food_Script _foodClass, bool _isInventoryFood, int _haveFoodUnitID = -1)
    {
        if(_isInventoryFood == true)
        {
            // 인벤토리의 음식이 삭제되는 경우

            int _inventoryFoodID = -1;

            for (int i = 0; i < inventoryFoodDataList.Count; i++)
            {
                if (_foodClass == inventoryFoodDataList[i].foodClass)
                {
                    _inventoryFoodID = i;
                    break;
                }
            }

            if (_inventoryFoodID == -1)
                Debug.LogError("Bug : 해당 음식을 인벤토리에 찾을 수 없습니다.");

            Destroy(inventoryFoodDataList[_inventoryFoodID].foodClass.gameObject); // 풀링으로 복귀...
            inventoryFoodDataList.RemoveAt(_inventoryFoodID);
        }
        else
        {
            // 유닛이 가진 음식이 삭제되는 경우

            
        }
    }
    public PlayerFood_Data GetPlayerFoodData_Func(int _inventoryFoodID)
    {
        return inventoryFoodDataList[_inventoryFoodID];
    }
    public int GetInventoryFoodNum_Func()
    {
        return inventoryFoodDataList.Count;
    }
    #endregion
}