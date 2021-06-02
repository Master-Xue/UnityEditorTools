using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 插件初始控制窗口
/// </summary>
public class MainControllerWindow : EditorWindow
{
    private static EditorWindow window;
    [MenuItem("Master_Xue's Tools/批量处理工具")]
    static void InitController()
    {
        window = GetWindow<MainControllerWindow>(true,"批量处理工具");
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("VR场景创建工具"))
            CreatScenesWindow.Init();
        if (GUILayout.Button("将场景添加到BuildSettings"))
            AddScenesToSettingWindow.Init();
        if (GUILayout.Button("批量处理贴图"))
            PictureSettingWindow.Init();
        if (GUILayout.Button("批量更改预设"))
        {
            ChangePrefebsWindow.Init();
        }
        if (GUILayout.Button("批量处理精灵"))
        {
            SpriteControllerWindow.Init();
        }
        if (GUILayout.Button("场景文件组件设置"))
        {
            ComponentsSettingWindow.Init();
        }
    }

    /// <summary>
    /// 提示
    /// </summary>
    /// <param name="tipText"></param>
    void ShowTip(string tipText)
    {
        window.ShowNotification(new GUIContent(tipText));
    }

}
