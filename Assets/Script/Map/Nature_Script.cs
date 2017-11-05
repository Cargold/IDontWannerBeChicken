using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nature_Script : MonoBehaviour
{
    public NatureType natureType;
    public NatureWorkType workType;
    public int natureID;
    public float naturePosX;
    public Animation natureAni;
    public GameObject[] devastatedObjArr;
    public GameObject[] woodyObjArr;
    public GameObject controlObj_Woody;
    public GameObject controlObj_Devastated;
    public GameObject controlObj;
    public bool isWork_Woody;
    public bool isWork_Devastated;
    public bool isWorking;
    public int workID;

    public void Init_Func(NatureType _natureType, int _natureID, float _naturePosX)
    {
        natureType = _natureType;
        workType = NatureWorkType.Devastated;
        natureID = _natureID;
        naturePosX = _naturePosX - 7.5f;
        
        int _randID = 0;
        _randID = Random.Range(0, 2);
        devastatedObjArr[_randID].transform.SetParent(controlObj.transform);
        devastatedObjArr[0].SetActive(false);
        devastatedObjArr[1].SetActive(false);
        controlObj_Devastated = devastatedObjArr[_randID];
        controlObj_Devastated.SetActive(true);

        if(natureType == NatureType.Tree)
            _randID = Random.Range(0, 2);
        woodyObjArr[_randID].transform.SetParent(controlObj.transform);
        woodyObjArr[0].SetActive(false);
        woodyObjArr[1].SetActive(false);
        controlObj_Woody = woodyObjArr[_randID];
        controlObj_Woody.SetActive(false);
    }
    
    public void OnWoody_Func()
    {
        isWork_Woody = true;
        
        if (isWorking == false)
        {
            // 아무 연출도 안 하고 있다면...

            isWorking = true;
            
            OnWoodyWork_Func();
        }
    }
    void OnWoodyWork_Func()
    {
        workID++;
        switch (workID)
        {
            case 1:
                // 찌그러지기
                isWork_Woody = false;
                workType = NatureWorkType.Woody;
                controlObj.transform.DOScaleY(0f, 0.25f).OnComplete(OnWoodyWork_Func);
                break;
            case 2:
                // 황폐 이미지 끄고, 울창 이미지 키고
                if(Battle_Manager.Instance.battleType == BattleType.Normal)
                {
                    controlObj_Devastated.SetActive(false);
                    controlObj_Woody.SetActive(true);
                }
                OnWoodyWork_Func();
                break;
            case 3:
                // 이미지 정상 크기, 이미지 바운딩
                controlObj.transform.DOScaleY(1f, 0.25f);
                controlObj.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.8f).OnComplete(OnWoodyWork_Func);
                break;
            case 4:
                // 값 초기화
                // 예약된 황폐화 연출 시작
                workID = 0;
                isWorking = false;
                
                if (isWork_Woody == true && isWork_Devastated == true)
                {
                    isWork_Woody = false;
                    isWork_Devastated = false;
                }
                else if (isWork_Woody == false && isWork_Devastated == true)
                {
                    OnDevastated_Func();
                }
                break;
        }
    }

    public void OnDevastated_Func()
    {
        isWork_Devastated = true;

        if(isWorking == false)
        {
            // 아무 연출도 안 하고 있다면...

            isWorking = true;

            OnDevastatedWork_Func();
        }
    }
    void OnDevastatedWork_Func()
    {
        workID++;
        switch (workID)
        {
            case 1:
                // 이미지 찌그러트리기
                isWork_Devastated = false;
                workType = NatureWorkType.Devastated;
                controlObj.transform.DOScaleY(0f, 0.25f).OnComplete(OnDevastatedWork_Func);
                break;
            case 2:
                // 울창 이미지 끄고, 황폐 이미지 키고
                controlObj_Woody.SetActive(false);
                controlObj_Devastated.SetActive(true);
                OnDevastatedWork_Func();
                break;
            case 3:
                // 이미지 펴기
                controlObj.transform.DOScaleY(1f, 0.25f).OnComplete(OnDevastatedWork_Func);
                break;
            case 4:
                // 값 초기화
                // 예약된 울창화 연출 시작
                workID = 0;
                isWorking = false;
                
                if (isWork_Woody == true && isWork_Devastated == true)
                {
                    isWork_Woody = false;
                    isWork_Devastated = false;
                }
                else if (isWork_Devastated == false && isWork_Woody == true)
                {
                    OnWoody_Func();
                }
                break;
        }
    }
}
