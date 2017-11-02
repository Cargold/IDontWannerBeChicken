using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment_Manager : MonoBehaviour
{
    public static Enviroment_Manager Instance;
    [SerializeField]
    private GameObject treeObj;
    [SerializeField]
    private Transform treeGroupTrf;
    [SerializeField]
    private List<Nature_Script> treeClassList;
    [SerializeField]
    private int treeID_Check;

    [SerializeField]
    private GameObject mountainObj;
    [SerializeField]
    private Transform mountainGroupTrf;
    [SerializeField]
    private List<Nature_Script> mountainClassList;
    [SerializeField]
    private int mountainID_Check;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        StartCoroutine(InitTree_Cor());
        StartCoroutine(InitMountain_Cor());

        yield break;
    }
    IEnumerator InitTree_Cor()
    {
        treeClassList = new List<Nature_Script>();

        float axisX = 0;
        int natureID = 0;
        while (axisX <= 110f)
        {
            GameObject _treeObj = Instantiate(treeObj);
            _treeObj.transform.SetParent(treeGroupTrf);
            _treeObj.transform.localPosition = new Vector3(axisX, Random.Range(-0.2f, 0f), 0f);

            Nature_Script _natureClass = _treeObj.GetComponent<Nature_Script>();
            _natureClass.Init_Func(NatureType.Tree, natureID, axisX);

            treeClassList.Add(_natureClass);
            
            axisX += Random.Range(0.5f, 1.5f);
            natureID++;
            yield return null;
        }
    }
    IEnumerator InitMountain_Cor()
    {
        mountainClassList = new List<Nature_Script>();

        float axisX = 0;
        int natureID = 0;
        while (axisX <= 110f)
        {
            GameObject _mountainObj = Instantiate(mountainObj);
            _mountainObj.transform.SetParent(mountainGroupTrf);
            _mountainObj.transform.localPosition = new Vector3(axisX, 0f, 0f);

            Nature_Script _natureClass = _mountainObj.GetComponent<Nature_Script>();
            _natureClass.Init_Func(NatureType.Mountain, natureID, axisX);

            mountainClassList.Add(_natureClass);

            axisX += Random.Range(5f, 4.5f);
            natureID++;
            yield return null;
        }
    }

    public void OnWoody_Func(float _playerPosX)
    {
        while (treeClassList[treeID_Check].naturePosX < _playerPosX)
        {
            treeClassList[treeID_Check].OnWoody_Func();
            treeID_Check++;
        }

        while (mountainClassList[mountainID_Check].naturePosX < _playerPosX)
        {
            mountainClassList[mountainID_Check].OnWoody_Func();
            mountainID_Check++;
        }
    }
    public void OnDevastated_Func(float _playerPosX)
    {
        while (_playerPosX < treeClassList[treeID_Check].naturePosX)
        {
            treeClassList[treeID_Check].OnDevastated_Func();
            treeID_Check--;
        }

        while (_playerPosX < mountainClassList[mountainID_Check].naturePosX)
        {
            mountainClassList[mountainID_Check].OnDevastated_Func();
            mountainID_Check--;
        }
    }
    public void NatureReset_Func()
    {
        StartCoroutine(ResetTree_Cor());
        StartCoroutine(ResetMountain_Cor());
    }

    IEnumerator ResetTree_Cor()
    {
        treeID_Check = 0;

        for (int i = 0; i < treeClassList.Count; i++)
        {
            treeClassList[i].OnDevastated_Func();

            yield return null;
        }
    }
    IEnumerator ResetMountain_Cor()
    {
        mountainID_Check = 0;

        for (int i = 0; i < mountainClassList.Count; i++)
        {
            mountainClassList[i].OnDevastated_Func();

            yield return null;
        }
    }
}