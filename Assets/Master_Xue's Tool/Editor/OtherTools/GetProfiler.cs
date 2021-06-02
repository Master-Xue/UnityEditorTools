using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.Profiling;

public class GetProfiler : Editor
{

    [MenuItem("Assets/Master_Xue's Tools/获取图片资源占用内存和硬盘大小")]
    public static void GetPictureProfiler()
    {
        Texture target = Selection.activeContext as Texture;
        var type = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
        MethodInfo methodInfo = type.GetMethod("GetStorageMemorySize", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
        string memorySize = EditorUtility.FormatBytes(Profiler.GetRuntimeMemorySizeLong(Selection.activeContext));
        string HDDSize = EditorUtility.FormatBytes((int)methodInfo.Invoke(null, new object[] { target }));
        Debug.Log(string.Format("{0} 内存占用：{1}，硬盘占用：{2}", target.name, memorySize, HDDSize));
    }
}
