using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Cargold_Library
{
    #region Casting Group
    public static T ToEnum<T>(this string value)
    {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }

    public static int ToInt(this string value)
    {
        if (value == "") return 0;

        return System.Int32.Parse(value);
    }

    public static float ToFloat(this string value)
    {
        if (value == "") return 0f;

        float returnValue = 0f;

        System.Single.TryParse(value, out returnValue);

        return returnValue;
    }

    public static bool ToBool(this string value)
    {
        switch (value)
        {
            case "TRUE":
            case "True":
            case "T":
            case "1":
                return true;

            default:
                return false;
        }
    }
    #endregion
    #region Ui Group
    public static void SetNaturalAlphaColor_Func(this Image _image, float _alphaValue)
    {
        Color _returnColor = _image.color;

        _image.color = GetNaturalAlphaColor_Func(_returnColor, _alphaValue);
    }

    public static void SetNaturalAlphaColor_Func(this Text _text, float _alphaValue)
    {
        Color _returnColor = _text.color;

        _text.color = GetNaturalAlphaColor_Func(_returnColor, _alphaValue);
    }

    public static Color GetNaturalAlphaColor_Func(this Color _color, float _alphaValue)
    {
        Color _returnColor = new Color
            (
            _color.r,
            _color.g,
            _color.b,
            _alphaValue
            );

        return _returnColor;
    }
    #endregion
    #region Co-routine Group
    public static float GetCalcValue_Func(float _time, float _multipleValue = 0.02f)
    {   
        return _multipleValue / _time;
    }
    public static IEnumerator SetFadeIn_Cor(float _time, Image _target, float _inValue = 1f)
    {
        float _calcValue = GetCalcValue_Func(_time);

        for (float _alphaValue = 0; _alphaValue <= _inValue;)
        {
            _alphaValue += _calcValue;
            _target.SetNaturalAlphaColor_Func(_alphaValue);

            yield return new WaitForFixedUpdate();
        }
    }
    public static IEnumerator SetFadeOut_Cor(float _time, Image _target, float _outValue = 0f)
    {
        float _calcValue = GetCalcValue_Func(_time);

        for (float _alphaValue = 1f; _outValue <= _alphaValue;)
        {
            _alphaValue -= _calcValue;
            _target.SetNaturalAlphaColor_Func(_alphaValue);

            yield return new WaitForFixedUpdate();
        }
    }

    #endregion
    #region iTween Group
    //public static void iTweenPlay_Func(GameObject _moveObj, Vector3 _arrivePos, iTween.EaseType _easeType, float _time)
    //{
    //    iTween.MoveTo
    //        (
    //            _moveObj,
    //                iTween.Hash
    //                    (
    //                        "position", _arrivePos,
    //                        "easetype", _easeType, 
    //                        "time", _time
    //                    )
    //        );
    //}
    //public static void iTweenPlay_Func(Transform _moveTrf, Vector3 _arrivePos, iTween.EaseType _easeType, float _time)
    //{
    //    iTweenPlay_Func(_moveTrf.gameObject, _arrivePos, _easeType, _time);
    //}
    //public static void iTweenPlay_Func(GameObject _moveObj, Transform _arriveTrf, iTween.EaseType _easeType, float _time)
    //{
    //    iTweenPlay_Func(_moveObj, _arriveTrf.position, _easeType, _time);
    //}
    //public static void iTweenScale_Func(GameObject _moveObj, Vector3 _initScale, iTween.EaseType _easeType, float _time)
    //{
    //    _moveObj.transform.localScale = _initScale;

    //    iTween.ScaleTo
    //        (
    //            _moveObj,
    //                iTween.Hash
    //                    (
    //                        "time", _time,
    //                        "easetype", _easeType,
    //                        "scale", Vector3.one
    //                    )
    //        );              
    //}
    #endregion

    #region Log Group
    public enum LogType
    {
        Log,
        Warning,
        Error,
    }
    public static void Log_Func(LogType _logType = LogType.Log)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log("Test");
                break;
            case LogType.Warning:
                break;
            case LogType.Error:
                break;
        }
    }
    public static void Log_Func(string content, LogType _logType = LogType.Log)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log("Test : " + content);
                break;
            case LogType.Warning:
                break;
            case LogType.Error:
                break;
        }
    }
    public static void Log_Func(string name, string content, LogType _logType = LogType.Log)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log("Test, " + name + " : " + content);
                break;
            case LogType.Warning:
                break;
            case LogType.Error:
                break;
        }
    }
    public static void Log_Func(int _id, LogType _logType = LogType.Log)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log("Test : " + _id);
                break;
            case LogType.Warning:
                break;
            case LogType.Error:
                break;
        }
    }
    public static void Log_Func(int _id, string content, LogType _logType = LogType.Log)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log("Test, " + _id + " : " + content);
                break;
            case LogType.Warning:
                break;
            case LogType.Error:
                break;
        }
    }
    #endregion
}
