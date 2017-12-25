using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem_Manager : MonoBehaviour
{
    public static SaveSystem_Manager Instance;

    public bool isDeleteAll;
    public bool isContinuePlayer;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        if(isDeleteAll == true)
            PlayerPrefs.DeleteAll();

        CheckContinuePlayer_Func();

        yield break;
    }
    void CheckContinuePlayer_Func()
    {
        isContinuePlayer = LoadDataBool_Func(SaveType.Player_IsContinuePlayer);
        Cargold_Library.Log_Func("isContinuePlayer", isContinuePlayer);

        if (isContinuePlayer == false)
        {
            PlayerPrefs.DeleteAll();

            SaveData_Func(SaveType.Player_IsContinuePlayer, true);
        }
    }

    #region TypeRename Group
    public string SetRename_Func(SaveType _saveType, params int[] _idArr)
    {
        string _rename = "";

        switch (_saveType)
        {
            case SaveType.Party_Member_zzzSlotIDzzz_UnitID:
                _rename = "Party_Member_" + _idArr[0] + "_UnitID";
                break;
            case SaveType.Unit_zzzUnitIDzzz_Level:
                _rename = "Unit_" + _idArr[0] + "_Level";
                break;
            case SaveType.Unit_zzzUnitIDzzz_FoodHaveNum:
                _rename = "Unit_" + _idArr[0] + "_FoodHaveNum";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodID:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_FoodID";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodLevel:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_FoodLevel";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodExp:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_FoodExp";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosX:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_PosX";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosY:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_PosY";
                break;
            case SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_RotZ:
                _rename = "Unit_" + _idArr[0] + "_" + _idArr[1] + "_RotZ";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_FoodID:
                _rename = "Hero_" + _idArr[0] + "_FoodID";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_FoodLevel:
                _rename = "Hero_" + _idArr[0] + "_FoodLevel";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_FoodExp:
                _rename = "Hero_" + _idArr[0] + "_FoodExp";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_PosX:
                _rename = "Hero_" + _idArr[0] + "_" + "_PosX";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_PosY:
                _rename = "Hero_" + _idArr[0] + "_" + "_PosY";
                break;
            case SaveType.Hero_zzzFoodHaveIDzzz_RotZ:
                _rename = "Hero_" + _idArr[0] + "_" + "_RotZ";
                break;
            case SaveType.Inventory_zzzFoodHaveIDzzz_FoodID:
                _rename = "Inventory_" + _idArr[0] + "_FoodID";
                break;
            case SaveType.Inventory_zzzFoodHaveIDzzz_FoodLevel:
                _rename = "Inventory_" + _idArr[0] + "_FoodLevel";
                break;
            case SaveType.Inventory_zzzFoodHaveIDzzz_FoodExp:
                _rename = "Inventory_" + _idArr[0] + "_FoodExp";
                break;
            case SaveType.Trophy_zzzTrophyIDzzz_HaveNum:
                _rename = "Trophy_" + _idArr[0] + "_HaveNum";
                break;
            case SaveType.Skill_zzzSkillIDzzz_Level:
                _rename = "Skill_" + _idArr[0] + "_Level";
                break;
            case SaveType.Drink_zzzDrinkIDzzz_HaveNum:
                _rename = "Drink_" + _idArr[0] + "_HaveNum";
                break;
            case SaveType.Store_zzzPackageIDzzz_IsBuyRecord:
                _rename = "Store_" + _idArr[0] + "_IsBuyRecord";
                break;
            case SaveType.Player_zzzTutorialIDzzz_IsClear:
                _rename = "Player_" + _idArr[0] + "_IsClear";
                break;
        }

        return _rename;
    }
    #endregion
    #region Save Group
    public void SaveDataAll_Func()
    {

    }
    public void SaveData_Func(SaveType _saveType, int _value)
    {
        PlayerPrefs.SetInt(_saveType.ToString(), _value);
    }
    public void SaveData_Func(SaveType _saveType, float _value)
    {
        PlayerPrefs.SetFloat(_saveType.ToString(), _value);
    }
    public void SaveData_Func(SaveType _saveType, string _value)
    {
        PlayerPrefs.SetString(_saveType.ToString(), _value);
    }
    public void SaveData_Func(SaveType _saveType, bool _value)
    {
        string _saveValue = "";
        if (_value == true)
            _saveValue = "T";
        else
            _saveValue = "F";

        PlayerPrefs.SetString(_saveType.ToString(), _saveValue);
    }
    public void SaveData_Func(string _saveType, int _value)
    {
        PlayerPrefs.SetInt(_saveType, _value);
    }
    public void SaveData_Func(string _saveType, float _value)
    {
        PlayerPrefs.SetFloat(_saveType, _value);
    }
    public void SaveData_Func(string _saveType, string _value)
    {
        PlayerPrefs.SetString(_saveType, _value);
    }
    public void SaveData_Func(string _saveType, bool _value)
    {
        string _saveValue = "";
        if (_value == true)
            _saveValue = "T";
        else
            _saveValue = "F";

        PlayerPrefs.SetString(_saveType, _saveValue);
    }
    public void SaveData_Func(int _feedID, PlayerFood_ClassData _playerFoodData)
    {
        string _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodType, _feedID);
        SaveData_Func(_loadType, (int)_playerFoodData.foodType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.foodID);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodLevel, _feedID);
        SaveData_Func(_loadType, _playerFoodData.level);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodExp, _feedID);
        SaveData_Func(_loadType, _playerFoodData.remainExp);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_PosX, _feedID);
        SaveData_Func(_loadType, _playerFoodData.pos.x);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_PosY, _feedID);
        SaveData_Func(_loadType, _playerFoodData.pos.y);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_RotZ, _feedID);
        SaveData_Func(_loadType, _playerFoodData.rot.z);
    }
    public void SaveData_Func(int _unitID, int _feedID, PlayerFood_ClassData _playerFoodData)
    {
        string _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodType, _unitID, _feedID);
        SaveData_Func(_loadType, (int)_playerFoodData.foodType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodID, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.foodID);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodLevel, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.level);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodExp, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.remainExp);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosX, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.pos.x);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosY, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.pos.y);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_RotZ, _unitID, _feedID);
        SaveData_Func(_loadType, _playerFoodData.rot.z);
    }
    #endregion
    #region Load Group
    public int LoadDataInt_Func(SaveType _saveType)
    {
        return PlayerPrefs.GetInt(_saveType.ToString());
    }
    public float LoadDataFloat_Func(SaveType _saveType)
    {
        return PlayerPrefs.GetFloat(_saveType.ToString());
    }
    public bool LoadDataBool_Func(SaveType _saveType)
    {
        string _value = PlayerPrefs.GetString(_saveType.ToString());
        
        return _value.ToBool();
    }
    public int LoadDataInt_Func(string _saveType)
    {
        return PlayerPrefs.GetInt(_saveType);
    }
    public float LoadDataFloat_Func(string _saveType)
    {
        return PlayerPrefs.GetFloat(_saveType);
    }
    public bool LoadDataBool_Func(string _saveType)
    {
        string _value = PlayerPrefs.GetString(_saveType);

        return _value.ToBool();
    }
    public SaveFoodDataStr LoadDataUnitFood_Func(int _unitID, int _feedID)
    {
        string _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodType, _unitID, _feedID);
        int _feedFoodTypeID = LoadDataInt_Func(_loadType);
        FoodType _feedFoodType = (FoodType)_feedFoodTypeID;

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodID, _unitID, _feedID);
        int _feedFoodID = LoadDataInt_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodLevel, _unitID, _feedID);
        int _feedFoodLevel = LoadDataInt_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodExp, _unitID, _feedID);
        float _feedFoodExp = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosX, _unitID, _feedID);
        float _feedFoodPosX = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosY, _unitID, _feedID);
        float _feedFoodPosY = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_RotZ, _unitID, _feedID);
        float _feedFoodRotZ = LoadDataFloat_Func(_loadType);
        
        SaveFoodDataStr _returnData = new SaveFoodDataStr();
        _returnData.foodType = _feedFoodType;
        _returnData.foodID = _feedFoodID;
        _returnData.foodLevel = _feedFoodLevel;
        _returnData.foodExp = _feedFoodExp;
        _returnData.foodPos = new Vector2(_feedFoodPosX, _feedFoodPosY);
        _returnData.foodRot = Vector3.forward * _feedFoodRotZ;

        return _returnData;
    }
    public SaveFoodDataStr LoadDataHeroFood_Func(int _feedID)
    {
        string _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodType, _feedID);
        int _feedFoodTypeID = LoadDataInt_Func(_loadType);
        FoodType _feedFoodType = (FoodType)_feedFoodTypeID;

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodID, _feedID);
        int _feedFoodID = LoadDataInt_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodLevel, _feedID);
        int _feedFoodLevel = LoadDataInt_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_FoodExp, _feedID);
        float _feedFoodExp = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_PosX, _feedID);
        float _feedFoodPosX = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_PosY, _feedID);
        float _feedFoodPosY = LoadDataFloat_Func(_loadType);

        _loadType = SetRename_Func(SaveType.Hero_zzzFoodHaveIDzzz_RotZ, _feedID);
        float _feedFoodRotZ = LoadDataFloat_Func(_loadType);
        
        SaveFoodDataStr _returnData = new SaveFoodDataStr();
        _returnData.foodType = _feedFoodType;
        _returnData.foodID = _feedFoodID;
        _returnData.foodLevel = _feedFoodLevel;
        _returnData.foodExp = _feedFoodExp;
        _returnData.foodPos = new Vector2(_feedFoodPosX, _feedFoodPosY);
        _returnData.foodRot = Vector3.forward * _feedFoodRotZ;

        return _returnData;
    }
    #endregion

    public struct SaveFoodDataStr
    {
        public FoodType foodType;
        public int foodID;
        public int foodLevel;
        public float foodExp;
        public Vector2 foodPos;
        public Vector3 foodRot;
    }
}
