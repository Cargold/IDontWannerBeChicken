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

    [SerializeField]
    private GameObject cloudObj;
    [SerializeField]
    private Transform[] cloudGroupTrfArr;
    [SerializeField]
    private float[] flowSpeedArr;

    [SerializeField]
    private Material skyMat;
    [SerializeField]
    private SpriteRenderer skyRend;
    [SerializeField]
    private Sprite[] skySpriteArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        StartCoroutine(InitTree_Cor());
        StartCoroutine(InitMountain_Cor());
        StartCoroutine(InitCloud_Cor());
        StartCoroutine(FlowSky_Cor());

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
            
            float _randValue = Random.Range(0.5f, 1.5f);
            //if (_randValue <= 1.0f)
            //    _randValue *= 2f;
            axisX += _randValue;
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

            float _randValue = Random.Range(1.5f, 4.5f);
            axisX += _randValue;
            natureID++;
            yield return null;
        }
    }
    IEnumerator InitCloud_Cor()
    {
        int _groupNum = cloudGroupTrfArr.Length;

        for (int i = 0; i < _groupNum; i++)
        {
            float _axisX = 0f;

            for (; _axisX < 110f;)
            {
                GameObject _cloudObj = Instantiate(cloudObj);
                _cloudObj.transform.SetParent(cloudGroupTrfArr[i]);
                int _randValue = Random.Range(0, 2);
                _cloudObj.transform.GetChild(_randValue).gameObject.SetActive(true);
                _cloudObj.transform.GetChild(_randValue).GetComponent<SpriteRenderer>().sortingOrder = (i+50) * -1;
                _cloudObj.transform.localScale = Vector3.one - (Vector3.one * i * 0.2f);

                float _randAxisX = Random.Range(5f, 15f);
                float _randAxisY = Random.Range(0f, 0.5f);

                _cloudObj.transform.localPosition = new Vector3(_axisX, i + _randAxisY, 0f);
                _cloudObj.transform.localEulerAngles = Vector3.zero;

                _axisX += _randAxisX;

                yield return null;
            }
        }

        StartCoroutine(FlowCloud_Cor());
    }

    IEnumerator FlowCloud_Cor()
    {
        while (true)
        {
            for (int i = 0; i < cloudGroupTrfArr.Length; i++)
            {
                cloudGroupTrfArr[i].transform.localPosition += Vector3.right * flowSpeedArr[i];

                int _childCount = cloudGroupTrfArr[i].childCount - 1;
                while (100f < cloudGroupTrfArr[i].GetChild(_childCount).position.x)
                {
                    float _randAxisX = Random.Range(1.5f, 4.5f);
                    float _randAxisY = Random.Range(0f, 1.5f);

                    cloudGroupTrfArr[i].GetChild(_childCount).position = new Vector3(_randAxisX, i + 5 + _randAxisY, 0f);
                    cloudGroupTrfArr[i].GetChild(_childCount).SetAsFirstSibling();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator FlowSky_Cor()
    {
        while(true)
        {
            skyMat.mainTextureOffset -= Vector2.right * 0.001f;

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnSky_Func(BattleType _battleType)
    {
        skyRend.sprite = skySpriteArr[(int)_battleType];
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