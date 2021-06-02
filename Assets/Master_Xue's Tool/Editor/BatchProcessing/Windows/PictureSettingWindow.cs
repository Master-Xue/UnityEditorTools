using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 批量处理贴图
/// </summary>
public class PictureSettingWindow : EditorWindow {
    WindowTools tool = new WindowTools();
    public static void Init()
    {
        GetWindow<PictureSettingWindow>(true,"批量更改图片");
    }

    private void OnGUI()
    {
        SelectTexture();
        SetWindow();

        if (GUILayout.Button("确定"))
            StartSetting();
    }

    private void StartSetting()
    {
        PictureSettings.SetPicture(picPatheList, importerPrefeb);
    }
    
    List<string> picPatheList = new List<string>();
    /// <summary>
    /// 场景控制窗口
    /// </summary>
    void SelectTexture()
    {
        tool.ShowFoldout("贴图路径", delegate
         {
             tool.WindowBaseGroup(delegate
             {
                 for (int i = 0; i < picPatheList.Count; i++)
                     picPatheList[i] = EditorGUILayout.TextField(picPatheList[i]);
             }, "贴图文件路径", picPatheList, null);

             GUILayout.Space(10);
             if (GUILayout.Button("刷新列表"))
             {
                 if (picPatheList.Count > 0)
                 {
                     picPatheList[picPatheList.Count - 1] = Data.assetPath;
                 }
             }

         });

    }

    TextureImporter importerPrefeb;
    Texture texture;
    void SetWindow()
    {
        texture = EditorGUILayout.ObjectField("模板贴图", texture, typeof(Texture), true) as Texture;
        importerPrefeb = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
    }

}
