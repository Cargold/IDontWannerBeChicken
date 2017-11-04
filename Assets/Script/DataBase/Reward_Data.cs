public struct Reward_Data
{
    public RewardType rewardType;
    public int rewardID;
    public int rewardAmount;

    public void SetData_Func(RewardType _rewardType, int _rewardID, int _rewardAmount)
    {
        rewardType = _rewardType;
        rewardID = _rewardID;
        rewardAmount = _rewardAmount;
    }

    public void SetRewardAmount_Func(int _value)
    {
        rewardAmount = _value;
    }
}