using UnityEngine;
using UnityEngine.UI;

public class Food_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;
    
    public int foodId;
    public string foodName;
    public FoodGrade foodGrade;
    [SerializeField]
    private float gradePenalty;
    public FoodEffect_Main effectMain;
    public FoodEffect_Sub effectSub;
    [SerializeField]
    private float mainEffectValue;
    public float subEffectValue;
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
    
    public bool isDragState;

    public FoodPlaceState foodPlaceState;
    public FoodState foodState;

    public Unit_Script feederUnitClass;

    public void SetData_Func(Food_Data _foodData)
    {
        foodId = _foodData.foodId;
        foodName = _foodData.foodName;
        foodGrade = _foodData.foodGrade;
        gradePenalty = Game_Manager.Instance.foodGradePenaltyValue[(int)foodGrade];
        effectMain = _foodData.effectMain;
        effectSub = _foodData.effectSub;
        mainEffectValue = _foodData.mainEffectValue;
        subEffectValue = _foodData.subEffectValue;
        foodImage.sprite = _foodData.foodSprite;
        foodImage.SetNativeSize();
        foodImage.alphaHitTestMinimumThreshold = 0.5f;

        thisRigid = this.GetComponent<Rigidbody2D>();
        spriteRend= this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRend.sprite = _foodData.foodSprite;
        thisCol = this.transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
        thisCol.isTrigger = true;
        this.transform.GetChild(0).transform.localScale = Vector3.one * 85f;
    }
    public void Init_Func(FeedingRoom_Script _feedingRoomClass, FoodState _foodFeedState, int _level, float _exp = 0f)
    {
        feedingRoomClass = _feedingRoomClass;

        foodState = _foodFeedState;
        if (foodState == FoodState.Inventory)
        {
            foodPlaceState = FoodPlaceState.Inventory;
        }
        else
            foodPlaceState = FoodPlaceState.Stomach;

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
    public void Destroy_Func()
    {
        // 제거 연출

        bool _isInventoryFood = false;
        _isInventoryFood = feedingRoomClass.CheckInventoryFood_Func(this);

        if(_isInventoryFood == true)
        {
            feedingRoomClass.RemoveFoodInInventory_Func(this);
        }

        Player_Data.Instance.RemoveFood_Func(this, _isInventoryFood);
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
        if(_foodPlaceState == FoodPlaceState.Stomach)
        {
            foodPlaceState = FoodPlaceState.Stomach;

            thisCol.isTrigger = false;

            thisRigid.gravityScale = 500f;
        }
        else if (_foodPlaceState == FoodPlaceState.Inventory)
        {
            foodPlaceState = FoodPlaceState.Inventory;

            isDragState = false;

            thisCol.isTrigger = true;

            thisRigid.velocity = Vector2.zero;
            thisRigid.angularVelocity = 0f;
            thisRigid.gravityScale = 0f;

            this.transform.rotation = Quaternion.identity;
        }
    }
    public void SetDragState_Func(bool _isState)
    {
        if(_isState == true)
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
        thisRigid.velocity = (_forcePos - this.transform.position) * 10f;
    }
    public void FeedingByInner_Func()
    {
        foodState = FoodState.FeedingByInner;
        foodImage.color = Color.red;
    }
    public void FeedingByChain_Func()
    {
        foodState = FoodState.FeedingByChain;
        foodImage.color = Color.yellow;
    }
    public void OutFood_Func()
    {
        foodState = FoodState.Inventory;
        foodImage.color = Color.white;
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            if(0 < (int)this.foodState)
            {
                Food_Script _foodClass = collision.transform.GetComponent<Food_Script>();

                if (_foodClass.foodState == FoodState.Stomach)
                {
                    feedingRoomClass.SetFeedFoodByChain_Func(_foodClass);
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            if (0 < (int)this.foodState)
            {
                Food_Script _foodClass = collision.transform.GetComponent<Food_Script>();

                if (_foodClass.foodState == FoodState.FeedingByChain)
                {
                    feedingRoomClass.SetFoodOutByChain_Func(_foodClass);
                }
            }
        }
    }
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