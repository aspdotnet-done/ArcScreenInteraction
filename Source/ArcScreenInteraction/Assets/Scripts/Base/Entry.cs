using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Entry
{


    private static readonly List<ManagerComponent> managerComponents = new List<ManagerComponent>();
    /// <summary>
    /// 注册管理组件
    /// </summary>
    /// <param name="component"></param>
    internal static void RegisterComponent(ManagerComponent component)
    {
        if (component == null)
        {
            //Debug.LogError("ManagerComponent 为空");
            return;
        }

        Type t = component.GetType();

        foreach (var c in managerComponents)
        {
            if (c.GetType() == t)
            {
                //Debug.LogError($"已存在对应的ManagerComponent{t.FullName}");
                return;
            }
        }
        managerComponents.Add(component);

    }

    public static T GetComponent<T>() where T : ManagerComponent
    {
        return (T)GetComponent(typeof(T));
    }

    /// <summary>
    /// 获取游戏框架组件。
    /// </summary>
    /// <param name="type">要获取的游戏框架组件类型。</param>
    /// <returns>要获取的游戏框架组件。</returns>
    public static ManagerComponent GetComponent(Type type)
    {

        foreach (var c in managerComponents)
        {
            if (c.GetType() == type)
            {
                return c;
            }
        }
        //Debug.LogError("当前没有该ManagerComponent：" + type.FullName);
        return null;
    }

    public static void ClearComponent()
    {
        managerComponents.Clear();
    }



    /// <summary>
    /// 关闭游戏框架。
    /// </summary>
    public static void Shutdown()
    {
        Debug.Log($"关闭应用");
        managerComponents.Clear();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        return;

    }


}