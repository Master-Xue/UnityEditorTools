
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Data
{
    /// <summary>
    /// 将文字写入txt文件并保存到assets文件夹下
    /// </summary>
    /// <param name="text">写入文字</param>
    /// <param name="fileName">文件名</param>
    public static void WriteTxt(string text, string fileName)
    {
        string writeUrl = Application.dataPath + "/" + fileName + ".txt";
        StreamWriter sw = new StreamWriter(text, true);
        sw.WriteLine(text);
        sw.Flush();
        sw.Close();
    }

    public static string assetPath = "";

    
}

public enum EPictureSize
{
    _32=32,
    _64=64,
    _128=128,
    _256=256,
    _512=512,
    _1024=1024,
    _2048=2048,
    _4096=4096,
    _8192=8192
}

public enum EPicturePlatform
{
    Default,
    WebGL,
    iPhone,
    Android
    
}

public class DataTools
{
    /// <summary>
    /// 通过文件夹名和类型获得所有需要物体路径
    /// </summary>
    /// <param name="type"></param>
    /// <param name="floderName"></param>
    /// <returns></returns>
    public static List<string> FindObjPathWithType(string type, List<string> floderName)
    {
        List<string> pathList = new List<string>();
        floderName = FloderToPath(floderName);
        //string[] s = type.Split('|');
        //for (int i = 0; i < s.Length; i++)
        //{
        //    string[] guids = AssetDatabase.FindAssets("t:" + s[i], floderName.ToArray());
        //    Debug.Log(s[i] + guids.Length);
        //    foreach (var guid in guids)
        //    {
        //        string path = AssetDatabase.GUIDToAssetPath(guid);
        //        pathList.Add(path);
        //    }
        //}
        string[] guids = AssetDatabase.FindAssets("t:" + type, floderName.ToArray());
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            pathList.Add(path);
        }
        return pathList;
    }

    static List<string> FloderToPath(List<string> floderName)
    {
        List<string> name = new List<string>();
        for (int i = 0; i < floderName.Count; i++)
        {
            if (string.IsNullOrEmpty(floderName[i]))
                continue;

            name.Add(floderName[i].Insert(0, "Assets/"));
        }
        return name;
    }

    #region 旧方法
    ///// <summary>
    ///// 文件夹名改成路径
    ///// </summary>
    ///// <param name="FloderName"></param>
    ///// <returns></returns>
    //public static string FloderToPath(string FloderName)
    //{
    //    return "Assets/" + FloderName;
    //}

    ///// <summary>
    ///// 将全局路径改为相对项目路径
    ///// </summary>
    ///// <param name="wordPath">全局路径</param>
    ///// <returns></returns>
    //public static string WordPathToLocal(string wordPath)
    //{
    //    if (!wordPath.Contains("Assets"))
    //    {
    //        return "Assets";
    //    }
    //    wordPath = wordPath.Replace(@"\", "/");
    //    return wordPath.Substring(wordPath.IndexOf("Assets"));

    //}

    ///// <summary>
    ///// 获取路径下所有文件
    ///// </summary>
    ///// <param name="path"></param>
    ///// <returns></returns>
    //public static FileInfo[] GetFiles(string path)
    //{
    //    DirectoryInfo direction = new DirectoryInfo(path);
    //    FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
    //    List<FileInfo> info = new List<FileInfo>();
    //    foreach (var file in files)
    //    {
    //        if (file.Extension.Equals(".meta"))
    //            continue;
    //        info.Add(file);
    //    }
    //    return info.ToArray();
    //}

    ///// <summary>
    ///// 将FileInfo[]转为object[]
    ///// </summary>
    ///// <param name="files"></param>
    ///// <returns></returns>
    //public static UnityEngine.Object[] ToObject(FileInfo[] files)
    //{
    //    List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
    //    foreach (var item in files)
    //    {
    //        Object obj = AssetDatabase.LoadAssetAtPath(WordPathToLocal(item.FullName), typeof(Object));
    //        objects.Add(obj);
    //    }
    //    return objects.ToArray();
    //}
    #endregion
}

public class WindowTools
{
    public static void ShowFadeGroup(AnimBool anim, System.Action action, string showText)
    {
        anim.target = EditorGUILayout.ToggleLeft(showText, anim.target);
        if (EditorGUILayout.BeginFadeGroup(anim.faded))
        {
            action();
        }
        EditorGUILayout.EndFadeGroup();
    }


    /// <summary>
    /// 管理列表模板
    /// </summary>
    /// <typeparam name="T">列表类型</typeparam>
    /// <param name="action">列表赋值的委托</param>
    /// <param name="showText">主题</param>
    /// <param name="list">控制的列表</param>
    /// <param name="t">列表中数据的空数据</param>
    Vector2 scrollPos;
    internal void WindowBaseGroup<T>(System.Action action, string showText, List<T> list, T t = null) where T : class
    {
        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400), GUILayout.Height(100));
        action();

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        if (GUILayout.Button("添加" + showText))
            list.Add(t);
        EditorGUILayout.Space();
        if (GUILayout.Button("删除" + showText))
        {
            if (list.Count == 0)
                return;
            list.RemoveAt(list.Count - 1);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("清空" + showText))
            list.Clear();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 隐藏列表
    /// </summary>
    bool show;
    public void ShowFoldout(string title, System.Action action)
    {
        show = EditorGUILayout.Foldout(show, title);
        if (show)
            action();
    }
}


//public class GenericsTool<T> where T : UnityEngine.Object
//{
//    /// <summary>
//    /// 将FileInfo[]转为泛型列表
//    /// </summary>
//    /// <param name="files"></param>
//    /// <returns></returns>
//    public static List<T> ToObject(FileInfo[] files)
//    {
//        List<T> objects = new List<T>();
//        foreach (var item in files)
//        {
//            T obj = AssetDatabase.LoadAssetAtPath(DataTools.WordPathToLocal(item.FullName), typeof(T)) as T;
//            if (obj != null)
//                objects.Add(obj);
//        }
//        return objects;
//    }

//}

//public enum TextureExtension
//{
//    jpg = 1,
//    png = 2,
//    tga = 4,
//    bmp = 8
//}
