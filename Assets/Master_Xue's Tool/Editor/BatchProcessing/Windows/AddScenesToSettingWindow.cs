using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 将场景添加到Setting中
/// </summary>
public class AddScenesToSettingWindow : EditorWindow
{
    WindowTools tool = new WindowTools();
    public static void Init()
    {
        GetWindow<AddScenesToSettingWindow>(true, "批量添加场景文件");
    }

    private void OnGUI()
    {
        tool.ShowFoldout("场景文件", SelectTexture);
        EditorGUILayout.Space();
        resetList = GUILayout.Toggle(resetList, "删除原有场景");
        if (GUILayout.Button("开始添加"))
            StartAdd();
    }

    private void StartAdd()
    {
        AddSceneToSetting.SetEditorBuildSettingsScenes(scenePatheList, resetList);
    }

    List<string> scenePatheList = new List<string>();
    bool resetList = false;
    /// <summary>
    /// 场景控制窗口
    /// </summary>
    void SelectTexture()
    {
        tool.WindowBaseGroup<String>(delegate
        {
            for (int i = 0; i < scenePatheList.Count; i++)
                scenePatheList[i] = EditorGUILayout.TextField(scenePatheList[i]);
        }, "scene文件路径", scenePatheList, null);

        GUILayout.Space(10);
        if (GUILayout.Button("刷新列表"))
        {
            if (scenePatheList.Count > 0)
            {
                scenePatheList[scenePatheList.Count - 1] = Data.assetPath;
            }
        }


    }
}
