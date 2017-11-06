using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGrade_Script : MonoBehaviour
{
    public HeroManagement_Script heroManagementClass;
    public int gradeID;
    public Image[] selectImageArr;
    public Image lockImage;

    public void Init_Func(HeroManagement_Script _heroManagementClass, int _gradeID)
    {
        heroManagementClass = _heroManagementClass;

        gradeID = _gradeID;
    }
    public void UnlockGrade_Func()
    {
        lockImage.gameObject.SetActive(false);
    }
    public void SelectSkill_Func(bool _isUpSkill)
    {
        if(_isUpSkill == true)
        {
            selectImageArr[0].gameObject.SetActive(true);
            selectImageArr[1].gameObject.SetActive(false);
        }
        else if(_isUpSkill == false)
        {
            selectImageArr[0].gameObject.SetActive(false);
            selectImageArr[1].gameObject.SetActive(true);
        }

        Player_Data.Instance.SelectSkill_Func(gradeID, _isUpSkill);
    }
}
