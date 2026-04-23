using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public enum LogColor
{
    None = 0,
    Yellow = 1,
    Green = 2,
    Cyan = 3
}

public class DebugCustom
{
    private const string FORMAT_COLOR_CONTENT = "<color=#{0}>{1}</color>";

    #region DEBUG LOG
    private static bool IsEnableLog()
    {
        //#if UNITY_EDITOR
        //        return true;
        //#endif
        //        return GameConfig.Instance.enableDebugLog;

        return true;
    }

    public static void Log(object content, LogColor color = LogColor.None)
    {
        if (IsEnableLog())
        {
            if (color == LogColor.None)
            {
                Debug.Log(content);
            }
            else
            {
                string hexColor = string.Empty;

                //switch (color)
                //{
                //    case LogColor.Yellow: hexColor = GameUtils.GetColorHexCode(Color.yellow); break;
                //    case LogColor.Green: hexColor = GameUtils.GetColorHexCode(Color.green); break;
                //    case LogColor.Cyan: hexColor = GameUtils.GetColorHexCode(Color.cyan); break; 
                //}

                if (string.IsNullOrEmpty(hexColor))
                {
                    Debug.Log(content);
                }
                else
                {
                    string msg = string.Format(FORMAT_COLOR_CONTENT, hexColor, content);
                    Debug.Log(msg);
                }
            }
        }
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (IsEnableLog())
        {
            Debug.Log(string.Format(format, args));
        }
    }

    public static void LogError(object content)
    {
        if (IsEnableLog())
        {
            Debug.LogError(content);
        }
    }

    public static void LogWarning(object content)
    {
        if (IsEnableLog())
        {
            Debug.LogWarning(content);
        }
    }

    public static void LogWarning(object message, Object context)
    {
        if (IsEnableLog())
        {
            Debug.LogWarning(message, context);
        }
    }

    public static void LogWarning(Object context, string format, params object[] args)
    {
        if (IsEnableLog())
        {
            Debug.LogWarning(string.Format(format, args), context);
        }
    }

    public static void ShowLog(object content, object content2)
    {
        if (IsEnableLog())
        {
            Debug.Log("__####___" + content + "__" + content2);
        }
    }

    public static void ShowLog(object content)
    {
        if (IsEnableLog())
        {
            Debug.Log("__####___" + content);
        }
    }

    #endregion

    #region ASSERT
    /// Thown an exception if condition = false
    public static void Assert(bool condition)
    {
        if (!condition)
        {
            throw new UnityException();
        }
    }

    /// Thown an exception if condition = false, show message on console's log
    public static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new UnityException(message);
        }
    }

    /// Thown an exception if condition = false, show message on console's log
    public static void Assert(bool condition, string format, params object[] args)
    {
        if (!condition)
        {
            throw new UnityException(string.Format(format, args));
        }
    }
    #endregion
}