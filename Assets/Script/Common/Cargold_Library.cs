﻿using UnityEngine;
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
}
