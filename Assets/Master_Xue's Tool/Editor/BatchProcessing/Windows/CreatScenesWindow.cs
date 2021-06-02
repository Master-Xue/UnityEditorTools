using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

/// <summary>
/// 根据贴图创建VR场景的窗口
/// </summary>
public class CreatScenesWindow : EditorWindow
{
    List<string> texturePatheList = new List<string>();

    public static List<GameObject> prefebList = new List<GameObject>();


    public static bool delegateCamera = false;
    WindowTools preTool = new WindowTools();
    WindowTools texTool = new WindowTools();
    public static void Init()
    {
        GetWindow(typeof(CreatScenesWindow), true, "根据贴图创建VR场景");
    }


    private void OnGUI()
    {
        EditorGUILayout.Space();
        texTool.ShowFoldout("贴图文件路径", SelectTexture);

        EditorGUILayout.Space();
        preTool.ShowFoldout("预设体", SelectPrefeb);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        delegateCamera = GUILayout.Toggle(delegateCamera, "删除原场景摄像机");

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("开始创建场景"))
            CreatScenes();

    }

    /// <summary>
    /// 贴图窗口
    /// </summary>
    void SelectTexture()
    {
        GUILayout.Label("场景贴图选项");

        EditorGUILayout.Space();

        texTool.WindowBaseGroup<string>(
            delegate
            {
                for (int i = 0; i < texturePatheList.Count; i++)
                {
                    texturePatheList[i] = EditorGUILayout.TextField(texturePatheList[i]);
                }
            }, "贴图路径", texturePatheList, null);

        GUILayout.Space(10);
        if (GUILayout.Button("刷新列表"))
        {
            if (texturePatheList.Count > 0)
            {
                texturePatheList[texturePatheList.Count - 1] = Data.assetPath;
            }
        }


    }

    /// <summary>
    /// 预设体窗口
    /// </summary>
    void SelectPrefeb()
    {
        GUILayout.Label("场景中生成预设体(第一个用于存放赋予材质球的全景球)");

        preTool.WindowBaseGroup<GameObject>(
            delegate
            {
                for (int i = 0; i < prefebList.Count; i++)
                    prefebList[i] = (GameObject)EditorGUILayout.ObjectField(prefebList[i], typeof(GameObject));
            },
            "预设体", prefebList, null);

    }



    void CreatScenes()
    {
        CreatScenesByTex.Init(texturePatheList);
    }

    /// <summary>
    /// 判断枚举中选择了哪几种
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    //string[] IsSelectEventType(TextureExtension type)
    //{
    //    StringBuilder str = new StringBuilder();
    //    foreach (TextureExtension myEnum in System.Enum.GetValues(typeof(TextureExtension)))
    //        if ((type & myEnum) > 0)
    //            if ((myEnum & type) == myEnum)
    //                str.Insert(str.Length,myEnum.ToString() + "|");
    //    //str.Remove(str.Length - 1, 1);  //移除最后面的一个'|'
    //    return str.ToString().Split('|');
    //}


    /// <summary>
    /// 检查文件夹是否符合需求
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool CheckFloder(int index)
    {
        string path = texturePatheList[index];
        if (texturePatheList.Count <= index || string.IsNullOrEmpty(path))
            return false;
        for (int i = 0; i < texturePatheList.Count; i++)
            if (path == texturePatheList[i] && index != i)
                return false;
        return true;
    }






}
