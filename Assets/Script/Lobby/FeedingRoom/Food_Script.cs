using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Food_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;

    public FoodType foodType;
    public int foodId;
    public string[] nameArr;
    public FoodGrade foodGrade;
    [SerializeField]
    private float gradePenalty;
    public FoodEffect_Main effectMain;
    public FoodEffect_Sub effectSub;
    [SerializeField]
    private float mainEffectValue;
    [SerializeField]
    private float subEffectValue;
    public Image foodImage;
    [SerializeField]
    private Rigidbody2D thisRigid;
    private SpriteRenderer spriteRend;
    [SerializeField]
    private PolygonCollider2D thisCol;

    public int level;
    [SerializeField]
    private float maxExp;
    public float remainExp;
    [SerializeField]
    private float materialExp;
    public Vector3 stomachPos;
    public Vector3 stomachRot;
    
    public bool isDragState;

    public FoodPlaceState foodPlaceState;

    public void SetData_Func(Food_Data _foodData)
    {
        foodId = _foodData.foodId;
        nameArr = _foodData.nameArr;
        foodGrade = _foodData.foodGrade;
        gradePenalty = DataBase_Manager.Instance.foodGradePenaltyValue[(int)foodGrade];
        effectMain = _foodData.effectMain;
        effectSub = _foodData.effectSub;
        mainEffectValue = _foodData.mainEffectValue;
        subEffectValue = _foodData.subEffectValue;
        if (foodImage == null)
            foodImage = this.GetComponent<Image>();
        foodImage.sprite = _foodData.foodSprite;
        foodImage.SetNativeSize();
        foodImage.alphaHitTestMinimumThreshold = 0.5f;

        thisRigid = this.GetComponent<Rigidbody2D>();
        spriteRend = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRend.sprite = _foodData.foodSprite;
        thisCol = this.transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>();

        if(foodType != FoodType.Stone)
        {
            thisCol.isTrigger = true;
        }
        else
        {
            thisRigid.bodyType = RigidbodyType2D.Kinematic;
            thisCol.isTrigger = false;
        }
        
        this.transform.localScale = Vector3.one;
        this.transform.GetChild(0).transform.localScale = Vector3.one * 85f;
    }
    public void Init_Func(FeedingRoom_Script _feedingRoomClass, FoodPlaceState _foodPlaceState, int _placeID, int _level, float _exp = 0f)
    {
        feedingRoomClass = _feedingRoomClass;
        
        SetState_Func(_foodPlaceState);
        
        level = _level;
        
        remainExp = _exp;
        materialExp = GetMaterialExp_Func(_level);
        maxExp = GetMaxExp_Func(_level);
    }
    public void GetExp_Func(float _expValue)
    {
        CalcFoodExpData _calcExpData = GetCalcExpData_Func(_expValue);

        if(0 < _calcExpData.level_UpCount)
        {
            level = _calcExpData.level_Reached;
            maxExp = _calcExpData.exp_ReachedMax;
            materialExp = GetMaterialExp_Func(_calcExpData.level_Reached);

            LevelUp_Func();
        }

        remainExp = _calcExpData.exp_Remain;
    }
    
    void LevelUp_Func()
    {
        // 레벨업 연출
    }
    public void UseMaterialFood_Func()
    {
        // Call : 음식강화 시 재료로 사용됨

        // 제거 연출

        feedingRoomClass.UseMaterialFood_Func(this);
    }

    public CalcFoodExpData GetCalcExpData_Func(float _getExp)
    {
        int level_Calc = level;
        int levelUpCount = 0;
        float maxExp_Calc = maxExp;
        _getExp += remainExp;

        while (maxExp_Calc <= _getExp)
        {
            level_Calc++;
            levelUpCount++;

            _getExp -= maxExp_Calc;

            maxExp_Calc = GetMaxExp_Func(level_Calc);
        }

        CalcFoodExpData _calcExpData;
        _calcExpData.level_UpCount = levelUpCount;
        _calcExpData.level_Reached = level_Calc;
        _calcExpData.exp_ReachedMax = maxExp_Calc;
        _calcExpData.exp_ReachedPer = _getExp / maxExp_Calc;
        _calcExpData.exp_Remain = _getExp;
        _calcExpData.effectValue_Reached = mainEffectValue * level_Calc;
        _calcExpData.effectValue_UpValue = mainEffectValue * levelUpCount;

        return _calcExpData;
    }
    public float GetMaterialExp_Func(int _level = -1)
    {
        if (_level == -1)
            _level = level;

        float _returnValue = _level * 100f * gradePenalty;

        return _returnValue;
    }
    public float GetMaxExp_Func(int _level = -1)
    {
        if (_level == -1)
            _level = level;

        float _returnValue = (_level - 1) * 200f * gradePenalty + 100f;

        return _returnValue;
    }
    public float GetMainEffectValue_Func()
    {
        return mainEffectValue * level;
    }
    public float GetSubEffectValue_Func()
    {
        return subEffectValue;
    }
    public float GetExpPer_Func(int _level = -1)
    {
        if (_level == -1)
            _level = level;

        return remainExp / GetMaxExp_Func();
    }
    public int GetUpgradeCost_Func(int _level = -1)
    {
        if (_level == -1)
            _level = level;

        return 1000;
    }

    #region Event Group
    public void PointDown_Func()
    {
        feedingRoomClass.PointDown_Func(this);
    }
    public void PointUp_Func()
    {
        feedingRoomClass.PointUp_Func(this);
    }
    public void DragBegin_Func()
    {
        feedingRoomClass.DragBegin_Func(this);
    }
    public void Dragging_Func()
    {
        feedingRoomClass.Dragging_Func(this);
    }
    public void DragEnd_Func()
    {
        feedingRoomClass.DragEnd_Func(this);
    }
    #endregion
    #region Stomach Group
    public void SetState_Func(FoodPlaceState _foodPlaceState)
    {
        Debug.Log("Food State : " + _foodPlaceState);

        if (_foodPlaceState == FoodPlaceState.Stomach)
        {
            foodPlaceState = FoodPlaceState.Stomach;

            thisCol.isTrigger = false;

            SetRigid_Func(true);

            foodImage.color = Color.red;
        }
        else if (_foodPlaceState == FoodPlaceState.Dragging_Inven)
        {
            foodPlaceState = FoodPlaceState.Dragging_Inven;

            thisCol.isTrigger = false;

            StopCoroutine("FeedingTimeCheck_Cor");
            StartCoroutine("FeedingTimeCheck_Cor");

            foodImage.color = Color.yellow;
        }
        else if (_foodPlaceState == FoodPlaceState.Inventory)
        {
            if (foodType == FoodType.Stone)
                Debug.LogError("Bug : 모래주머니가 인벤토리로 이동되었습니다.");

            foodPlaceState = FoodPlaceState.Inventory;

            SetDragState_Func(false);

            thisCol.isTrigger = true;

            SetRigid_Func(false);

            this.transform.rotation = Quaternion.identity;

            foodImage.color = Color.green;
        }
    }
    public void SetDragState_Func(bool _isState)
    {
        //if (foodType == FoodType.Stone) return;

        if (_isState == true)
        {
            if (isDragState == false)
            {
                isDragState = true;

                thisRigid.velocity = Vector2.zero;
                thisRigid.angularVelocity = 0f;
                thisRigid.gravityScale = 0f;
            }
        }
        else if(_isState == false)
        {
            isDragState = false;
        }
    }
    public void SetVelocity_Func(Vector3 _forcePos)
    {
        if (foodType == FoodType.Stone) return;

        thisRigid.velocity = (_forcePos - this.transform.position) * 10f;
    }
    public void SetRigid_Func(bool _isOn)
    {
        if (foodType == FoodType.Stone) return;

        if(_isOn == true)
        {
            thisRigid.gravityScale = 500f;
        }
        else
        {
            thisRigid.gravityScale = 0f;

            thisRigid.angularVelocity = 0f;
            thisRigid.velocity = Vector2.zero;
        }    
    }
    private IEnumerator FeedingTimeCheck_Cor()
    {
        float _calcTime = 1f;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if(isDragState == false)
            {
                _calcTime -= 0.02f;

                if(_calcTime < 0f)
                {
                    feedingRoomClass.SetFoodPlaceState_Func(this, FoodPlaceState.Stomach);
                    yield break;
                }
            }

            if (foodPlaceState == FoodPlaceState.Inventory)
            {
                yield break;
            }
        }
    }
    #endregion
}

public struct CalcFoodExpData
{
    public int level_UpCount;
    public int level_Reached;
    public float exp_ReachedMax;
    public float exp_ReachedPer;
    public float exp_Remain;
    public float effectValue_Reached;
    public float effectValue_UpValue;
}