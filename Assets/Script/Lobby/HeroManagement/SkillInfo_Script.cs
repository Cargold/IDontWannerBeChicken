using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo_Script : MonoBehaviour
{
    public HeroManagement_Script heroManagementClass;

    public Text nameText;
    public Text descText;

    public void Init_Func(HeroManagement_Script _heroManagementClass)
    {
        heroManagementClass = _heroManagementClass;
    }

    public void PrintSelectCardInfo_Func(SkillCard_Script _skillCardClass)
    {
        PlayerSkill_Data _playerSkillData = Player_Data.Instance.skillDataArr[_skillCardClass.cardID];
        Skill_Parent _skillClass = _playerSkillData.skillParentClass;

        nameText.text = _skillClass.skillName + " Lv." + _playerSkillData.skillLevel;

        int _varNum = _skillClass.skillVarArr.Length;
        float[] _varUpgradeValueArr = new float[_varNum];
        for (int i = 0; i < _varNum; i++)
        {
            _varUpgradeValueArr[i] = _skillClass.skillVarArr[i].upgradeValue;
        }

        string _descByFormatting = "";
        _descByFormatting = string.Format(_skillClass.skillDesc, _varUpgradeValueArr[0]);

        descText.text = _descByFormatting;
    }
}
