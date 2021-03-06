﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FeedingRoom_Script : LobbyUI_Parent
{
    public Inventory_Script inventoryClass;
    public Stomach_Script stomachClass;
    public UpgradePlate_Script upgradePlateClass;

    public Text foodText;
    public Text mainEffectText;
    public Text subEffectText;
    public Image expMainImage;
    public Image expProgressImage;

    public Transform selectPointingTrf;
    public Food_Script selectedFoodClass;
    public Food_Script materialFoodClass;
    public Food_Script replaceFoodClass;
    public bool isUpgradeReady;
    private Vector3 touchOffsetPos;
    public Image upgradeFocusImage;
    public Transform upgradeGroupTrf;

    public int selectUnitID;

    public Animation anim;
    public bool isActive = false;

    public Text upgradeText;
    public Text backText;

    public enum FeedingRoomState
    {
        None = -1,
        InitState,
        SelectFood,
        ChooseUpgrade,
    }
    public FeedingRoomState feedingRoomState;

    [Header("Unit Upgrade")]
    public RectTransform unitLevelUpBtnRTrf;
    public Text charLevelUpText;
    private int charLevelUpCost;

    #region Override Group
    protected override void InitUI_Func()
    {
        inventoryClass.Init_Func(this);
        stomachClass.Init_Func(this);
        upgradePlateClass.Init_Func(this);

        upgradeText.text = TranslationSystem_Manager.Instance.Upgrade;
        backText.text = TranslationSystem_Manager.Instance.Back;

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func(int _referenceID = -1)
    {
        // 999 = Hero, Other = Unit

        this.gameObject.SetActive(true);
        
        Player_Data.Instance.ActiveWealthUI_Func();

        selectUnitID = _referenceID;

        inventoryClass.Active_Func(_referenceID);

        InitSelect_Func();

        PrintCharUpgradeCost_Func();

        anim["FeedingRoom"].speed = 1f;
        anim.Play("FeedingRoom");
    }
    public override void Exit_Func()
    {
        if (anim.isPlaying == true) return;
 
        selectedFoodClass = null;

        if(isActive == true)
        {
            stomachClass.Deactive_Func();
            inventoryClass.Deactive_Func();

            anim["FeedingRoom"].time = anim["FeedingRoom"].length;
            anim["FeedingRoom"].speed = -1f;
            anim.Play("FeedingRoom");

            lobbyManager.OffFeedingRoom_Func(selectUnitID);
        }

        if(lobbyManager.partySettingClass.isActive == true)
        {
            Player_Data.Instance.DeactiveWealthUI_Func();
        }
        else if(lobbyManager.heroManagementClass.isActive == true)
        {
            Player_Data.Instance.ActiveWealthUI_Func();
        }
        else
        {
            Debug.LogError("Bug : 피딩룸 닫은 후 파티세팅 또는 영웅관리 외에 다른 룸으로 가는 예외상황 발생");
        }

        SaveFoodData_Func();
    }
    #endregion
    void InitSelect_Func()
    {
        feedingRoomState = FeedingRoomState.InitState;
        
        foodText.text = "";
        mainEffectText.text = "";
        subEffectText.text = "";

        expMainImage.fillAmount = 0f;
        expProgressImage.fillAmount = 0f;

        selectPointingTrf.localPosition = Vector2.left * 1000f;

        upgradePlateClass.SetInitState_Func();
    }
    void SaveFoodData_Func()
    {
        // Save Stomach
        if(selectUnitID == 999)
        {
            Player_Data.Instance.Hero_SaveFeedData_Func();
        }
        else
        {
            Player_Data.Instance.SaveFeedData_Func(selectUnitID);
        }

        // Save Inventory
        Player_Data.Instance.SaveInvenFoodData_Func();
    }
    #region Food Control Group
    public void PointDown_Func(Food_Script _foodClass)
    {
        // 여긴 쓰지 않는다.
        // 선택한 음식이, 선택음식을 바꾸기 위한 건지 재료음식을 선택한 건지 구분할 수 없음.

        
    }
    public void PointUp_Func(Food_Script _foodClass)
    {
        if (materialFoodClass == _foodClass)
        {
            // 재료 음식을 선택 취소한 경우

            if (isUpgradeReady == false)
            {
                UpgradeEnd_Func();
            }
            else if (isUpgradeReady == true)
            {
                int _upgradeCost = selectedFoodClass.GetUpgradeCost_Func();
                if (Player_Data.Instance.PayWealth_Func(WealthType.Gold, _upgradeCost) == true)
                    Upgrade_Func();
                else
                    UpgradeEnd_Func();
            }
        }
        else if(materialFoodClass == null)
        {
            SelectNewFood_Func(_foodClass);
        }
    }
    void SelectNewFood_Func(Food_Script _foodClass)
    {
        if(selectedFoodClass != null)
        {
            inventoryClass.SetRegroupTrf_Func(selectedFoodClass.transform);
        }

        feedingRoomState = FeedingRoomState.SelectFood;
        selectedFoodClass = _foodClass;
        selectedFoodClass.transform.SetParent(upgradeGroupTrf);
        selectedFoodClass.transform.SetAsLastSibling();

        selectPointingTrf.SetParent(selectedFoodClass.transform);
        selectPointingTrf.localPosition = Vector3.zero;

        upgradePlateClass.SetDragState_Func();

        PrintFoodInfo_Func(_foodClass);

        if (_foodClass.foodType == FoodType.Stone)
        {
            SelectStone_Func(_foodClass);   
        }
    }
    void SelectStone_Func(Food_Script _stoneClass)
    {
        int _stoneNum = stomachClass.GetStoneHaveNum_Func();

        int _removeCost = DataBase_Manager.Instance.stoneRemoveCostArr[3 - _stoneNum];
        upgradePlateClass.SetStoneRemove_Func(_removeCost, _stoneClass);
    }
    
    public void DragBegin_Func(Food_Script _foodClass)
    {
        // 음식을 누르고 드래그를 시작했을 때

        if (selectedFoodClass == null)
        {
            // 선택 중인 음식이 없을 때

            SelectNewFood_Func(_foodClass);
        }
        else if (selectedFoodClass != null && selectedFoodClass != _foodClass)
        {
            // 선택 중인 음식이 있을 때

            if (_foodClass.foodPlaceState == FoodPlaceState.Inventory)
            {
                if (materialFoodClass == null)
                {
                    // 재료음식이 없을 때

                    UpgradeBegin_Func(_foodClass);
                    PrintUpgradeInfo_Func();
                }
                else if (materialFoodClass == _foodClass)
                {
                    // 재료음식을 다시 누를 때

                    UpgradeEnd_Func();
                }
            }
            else
            {

            }
        }
    }

    public void Dragging_Func(Food_Script _foodClass)
    {
        if (_foodClass.foodType == FoodType.Stone) return;

        if(_foodClass.foodPlaceState == FoodPlaceState.Inventory)
        {
            if (selectedFoodClass == _foodClass || materialFoodClass == _foodClass)
            {
                // 선택된 음식 또는 재료 음식을 드래그한 경우

                Vector3 _dragPos = Input.mousePosition - touchOffsetPos;
                _foodClass.transform.position = _dragPos;
            }
        }
        else
        {
            // 바깥 또는 위장 음식

            if(_foodClass.isDragState == false)
                StartCoroutine("DraggingPhysics_Cor", _foodClass);
        }
    }
    IEnumerator DraggingPhysics_Cor(Food_Script _foodClass)
    {
        _foodClass.SetDragState_Func(true);

        while (_foodClass.foodPlaceState != FoodPlaceState.Inventory)
        {
            if(_foodClass.isDragState == true)
            {
                Vector3 _dragPos = Input.mousePosition - touchOffsetPos;

                _foodClass.SetVelocity_Func(_dragPos);
            }
            
            yield return null;
        }

        yield break;
    }
    public void DragEnd_Func(Food_Script _foodClass)
    {
        ReplaceInInventory_Func();

        _foodClass.SetDragState_Func(false);

        if (_foodClass.foodPlaceState == FoodPlaceState.Inventory)
        {
            StopCoroutine("DraggingPhysics_Cor");
        }
        else if(_foodClass.foodPlaceState == FoodPlaceState.Dragging_Inven)
        {
            _foodClass.SetRigid_Func(true);
        }
        else if(_foodClass.foodPlaceState == FoodPlaceState.Stomach)
        {
            _foodClass.SetRigid_Func(true);
        }
    }
    void ReplaceInInventory_Func()
    {
        if(replaceFoodClass != null)
        {
            inventoryClass.SetRegroupTrf_Func(replaceFoodClass.transform, true);

            replaceFoodClass = null;
        }
    }
    #endregion

    void PrintFoodInfo_Func(Food_Script _foodClass)
    {
        foodText.text = _foodClass.nameArr[TranslationSystem_Manager.Instance.languageTypeID] + " Lv." + _foodClass.level;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.None:
                mainEffectText.text = TranslationSystem_Manager.Instance.MaterialFoodDesc;
                break;
            case FoodEffect_Main.AttackPower:
                mainEffectText.text = TranslationSystem_Manager.Instance.Damage + " +" + _foodClass.GetMainEffectValue_Func() + "%";
                break;
            case FoodEffect_Main.HealthPoint:
                mainEffectText.text = TranslationSystem_Manager.Instance.Health + " +" + _foodClass.GetMainEffectValue_Func() + "%";
                break;
            case FoodEffect_Main.Gizzard:
                mainEffectText.text = "Nothing On You, Baby ~";
                break;
        }

        switch (_foodClass.effectSub)
        {
            case FoodEffect_Sub.None:
                subEffectText.text = "";
                break;
            case FoodEffect_Sub.Critical:
                subEffectText.text = TranslationSystem_Manager.Instance.Critical + " +" + _foodClass.GetSubEffectValue_Func() + "%";
                break;
            case FoodEffect_Sub.SpawnInterval:
                subEffectText.text = TranslationSystem_Manager.Instance.SpawnInterval + " -" + _foodClass.GetSubEffectValue_Func() + "%";
                break;
            case FoodEffect_Sub.DecreaseHP:
                subEffectText.text = TranslationSystem_Manager.Instance.Health + " -" + _foodClass.GetSubEffectValue_Func() + "%";
                break;
            case FoodEffect_Sub.DefenceValue:
                subEffectText.text = TranslationSystem_Manager.Instance.Defence + " +" + _foodClass.GetSubEffectValue_Func() + "%";
                break;
            case FoodEffect_Sub.DecreaseAttack:
                subEffectText.text = TranslationSystem_Manager.Instance.Damage + " -" + _foodClass.GetSubEffectValue_Func() + "%";
                break;
        }
        
        upgradePlateClass.SetDragState_Func();

        float _expPer = _foodClass.remainExp / _foodClass.GetMaxExp_Func();
        expMainImage.fillAmount = _expPer;
        expProgressImage.fillAmount = 0f;
    }
    public bool GetFoodHaveInInventroy_Func(Food_Script _foodClass)
    {
        return inventoryClass.GetFoodHave_Func(_foodClass);
    }
    public void ReplaceFood_Func(Food_Script _foodClass, bool _isImmediately = false)
    {
        replaceFoodClass = _foodClass;
        
        if (_isImmediately == true)
        {
            ReplaceInInventory_Func();
        }
    }
    public void RemoveStone_Func(Food_Script _stoneClass)
    {
        Player_Data.Instance.playerUnitDataArr[selectUnitID].RemoveStone_Func(_stoneClass);

        stomachClass.OutFood_Func(_stoneClass);

        upgradePlateClass.SetInitState_Func();
    }

    #region Char Upgrade Group
    void PrintCharUpgradeCost_Func(int _charLevel = -1)
    {
        int _charGrade = 0;
        if (selectUnitID == 999)
        {
            if (_charLevel == -1)
            {
                _charLevel = Player_Data.Instance.heroLevel;
            }

            _charGrade = Player_Data.Instance.heroClass.charGrade;
        }
        else
        {
            if (_charLevel == -1)
            {
                _charLevel = Player_Data.Instance.playerUnitDataArr[selectUnitID].unitLevel;
            }

            _charGrade = DataBase_Manager.Instance.GetUnitClass_Func(selectUnitID).charGrade;
        }

        charLevelUpCost = DataBase_Manager.Instance.GetCharLevelUpCost_Func(_charLevel + _charGrade);
        charLevelUpText.text = string.Format("{0:N0}", charLevelUpCost);
    }
    public void OnCharLevelUp_Func()
    {
        bool _isCorrect = Player_Data.Instance.PayWealth_Func(WealthType.Gold, charLevelUpCost, true);
        if(_isCorrect == true)
        {
            int _charLevel = 0;
            if (selectUnitID == 999)
            {
                Debug.Log("Test, Hero");

                _charLevel = Player_Data.Instance.heroLevel;
                _charLevel++;

                Player_Data.Instance.Hero_SetLevel_Func(_charLevel);
            }
            else
            {
                _charLevel = Player_Data.Instance.playerUnitDataArr[selectUnitID].unitLevel;
                _charLevel++;

                Player_Data.Instance.playerUnitDataArr[selectUnitID].SetLevel_Func(_charLevel);
            }

            Player_Data.Instance.PayWealth_Func(WealthType.Gold, charLevelUpCost);

            PrintCharUpgradeCost_Func(_charLevel);
        }
        else
        {
            charLevelUpText.DOColor(Color.red, 0.2f).OnComplete(ResetLevelUpBtn_Func);
        }
    }
    void ResetLevelUpBtn_Func()
    {
        Color _color = DataBase_Manager.Instance.textColor;
        charLevelUpText.DOColor(_color, 0.2f);
    }
    #endregion
    #region Food Upgrade Group
    private void PrintUpgradeInfo_Func()
    {
        float _expTotal = materialFoodClass.GetMaterialExp_Func();
        CalcFoodExpData _calcFoodExpData = selectedFoodClass.GetCalcExpData_Func(_expTotal);

        if (_calcFoodExpData.exp_ReachedPer <= selectedFoodClass.GetExpPer_Func())
            expProgressImage.fillAmount = 1f;
        else
            expProgressImage.fillAmount = _calcFoodExpData.exp_ReachedPer;

        if (0 < _calcFoodExpData.level_UpCount)
        {
            foodText.text = foodText.text + "  <color=#3ed525>+" + _calcFoodExpData.level_UpCount + "</color>";
            mainEffectText.text = mainEffectText.text + " <color=#3ed525>+" + _calcFoodExpData.effectValue_UpValue + "%</color>";
        }

        int _upgradeCost = selectedFoodClass.GetUpgradeCost_Func();
        if (Player_Data.Instance.PayWealth_Func(WealthType.Gold, _upgradeCost, true) == false)
            upgradePlateClass.SetUpgradeState_Func(_upgradeCost, Color.red);
        else
            upgradePlateClass.SetUpgradeState_Func(_upgradeCost, DataBase_Manager.Instance.textColor);
    }
    private void UpgradeBegin_Func(Food_Script _foodClass)
    {
        isUpgradeReady = false;

        materialFoodClass = _foodClass;

        upgradePlateClass.SetDragState_Func();

        touchOffsetPos = Input.mousePosition - materialFoodClass.transform.position;
        
        upgradeFocusImage.SetNaturalAlphaColor_Func(0.7f);
        upgradeFocusImage.transform.SetAsLastSibling();

        selectedFoodClass.transform.SetParent(upgradeGroupTrf);
        selectedFoodClass.transform.SetAsLastSibling();

        materialFoodClass.transform.SetParent(upgradeGroupTrf);
        materialFoodClass.transform.SetAsLastSibling();
    }
    public  void UpgradeFoodApproach_Func(Food_Script _materialFoodClass)
    {
        if(_materialFoodClass == materialFoodClass)
        {
            isUpgradeReady = true;
        }
    }
    public  void UpgradeFoodAway_Func(Food_Script _exitFoodClass)
    {
        if(_exitFoodClass == materialFoodClass)
        {
            isUpgradeReady = false;
        }
    }
    private void UpgradeEnd_Func()
    {
        isUpgradeReady = false;
        
        if(materialFoodClass != null)
        {
            inventoryClass.SetRegroupTrf_Func(materialFoodClass.transform);

            materialFoodClass = null;
        }
        
        upgradePlateClass.SetDragState_Func();

        upgradeFocusImage.SetNaturalAlphaColor_Func(0f);

        PrintFoodInfo_Func(selectedFoodClass);
    }
    private void Upgrade_Func()
    {
        isUpgradeReady = false;

        Player_Data.Instance.SetCharDataByFood_Func(selectUnitID, selectedFoodClass, false);

        float _materialExp = materialFoodClass.GetMaterialExp_Func();
        float _trophyEffectValue = Player_Data.Instance.GetCalcTrophyEffect_Func(TrophyType.UpgradeExp, true) * 0.01f;
        _materialExp *= 1f + _trophyEffectValue;

        materialFoodClass.UseMaterialFood_Func();
        materialFoodClass = null;

        selectedFoodClass.GetExp_Func(_materialExp);
        PrintFoodInfo_Func(selectedFoodClass);

        upgradeFocusImage.SetNaturalAlphaColor_Func(0f);

        bool _isInventoryFood = GetFoodHaveInInventroy_Func(selectedFoodClass);
        Player_Data.Instance.SetFoodData_Func(selectedFoodClass, _isInventoryFood, selectUnitID);
        Player_Data.Instance.SetCharDataByFood_Func(selectUnitID, selectedFoodClass, true);
    }
    public void UseMaterialFood_Func(Food_Script _foodClass)
    {
        Player_Data.Instance.UseMaterialFood_Func(_foodClass);

        RemoveFoodInInventory_Func(_foodClass);

        ObjectPool_Manager.Instance.Free_Func(_foodClass.gameObject);
    }
    public void RemoveFoodInInventory_Func(Food_Script _foodClass)
    {
        if (inventoryClass.GetFoodHave_Func(_foodClass) == true)
        {
            inventoryClass.RemoveFood_Func(_foodClass);
        }
        else
        {
            Debug.LogError("Bug : 가방에 없는 음식이 제거되었습니다.");
        }
    }
    void RemoveFoodInStomach_Func(Food_Script _foodClass)
    {
        
    }
    #endregion
    public void SetFoodPlaceState_Func(Food_Script _setterFoodClass, FoodPlaceState _foodPlaceState)
    {
        if (_foodPlaceState == FoodPlaceState.Inventory)
        {
            if (_setterFoodClass.foodPlaceState == FoodPlaceState.Stomach)
            {
                // 뱃속에서 음식을 꺼냄
                stomachClass.OutFood_Func(_setterFoodClass);

                // 음식을 인벤토리 상태로...
                _setterFoodClass.SetState_Func(FoodPlaceState.Inventory);

                // 인벤토리로 음식 객체 이동
                ReplaceFood_Func(_setterFoodClass, true);

                // 인벤토리로 데이터 이동
                inventoryClass.AddFood_Func(_setterFoodClass);

                // 플레이어 데이터에서도...
                Player_Data.Instance.AddFoodInInventory_Func(_setterFoodClass);
                Player_Data.Instance.OutFoodInChar_Func(selectUnitID, _setterFoodClass);
            }
            else if (_setterFoodClass.foodPlaceState == FoodPlaceState.Dragging_Inven)
            {
                if(_setterFoodClass.isDragState == false)
                {
                    // 인벤토리로 음식 객체 이동
                    ReplaceFood_Func(_setterFoodClass, true);
                }
                
                // 음식을 인벤토리 상태로...
                _setterFoodClass.SetState_Func(FoodPlaceState.Inventory);
            }
        }
        else if (_foodPlaceState == FoodPlaceState.Dragging_Inven)
        {
            if (_setterFoodClass.foodPlaceState == FoodPlaceState.Inventory)
            {
                _setterFoodClass.SetState_Func(FoodPlaceState.Dragging_Inven);

                stomachClass.ReplaceStomach_Func(_setterFoodClass.transform);
            }
            else
            {
                Debug.LogError("Bug : 음식의 상태 변경이 부적절합니다.");
            }
        }
        else if (_foodPlaceState == FoodPlaceState.Stomach)
        {
            if (_setterFoodClass.foodPlaceState == FoodPlaceState.Dragging_Inven)
            {
                // 가방에서 음식을 꺼냄
                inventoryClass.RemoveFood_Func(_setterFoodClass);

                // 음식을 위장 상태로...
                _setterFoodClass.SetState_Func(FoodPlaceState.Stomach);

                // 위장으로 음식 이동
                stomachClass.FeedFood_Func(_setterFoodClass);

                // 플레이어 데이터 수정
                Player_Data.Instance.FeedFood_Func(selectUnitID, _setterFoodClass);
            }
            else
            {
                Debug.LogError("Bug : 음식의 상태 변경이 부적절합니다.");
                Debug.LogError("기존 : " + _setterFoodClass.foodPlaceState + ", 변경 : " + _foodPlaceState);
            }
        }
    }
    #region Animation Group
    public void AnimationStart_Func()
    {
        // Call : Ani Event

        if (isActive == true)
        {
            isActive = false;

            //inventoryClass.Deactive_Func();

            this.gameObject.SetActive(false);
        }
    }
    public void AnimationFinish_Func()
    {
        // Call : Ani Event

        if (isActive == false)
        {
            isActive = true;

            stomachClass.Active_Func(selectUnitID);
        }
    }
    #endregion
}