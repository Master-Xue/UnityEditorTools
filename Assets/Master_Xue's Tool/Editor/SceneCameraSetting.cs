using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// sceneCamera属性设置
/// </summary>
public class SceneCameraSetting : EditorWindow
{

    private static float nearClipping;

    [MenuItem("Master_Xue's Tools/SceneCamera设置")]
    static void Init()
    {
        SceneCameraSetting window = (SceneCameraSetting)EditorWindow.GetWindow(typeof(SceneCameraSetting));
        window.Show();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Scene Camera near clipping", EditorStyles.boldLabel);
        var lastSceneView = SceneView.lastActiveSceneView;
        if (lastSceneView == null || lastSceneView.camera == null)
        {
            EditorGUILayout.HelpBox("No Scene view found", MessageType.Error);
            return;
        }

        nearClipping = lastSceneView.camera.nearClipPlane;
        EditorGUI.BeginChangeCheck();
        {
            nearClipping = EditorGUILayout.Slider("Near clipping", nearClipping, 0.01f, 20f);
        }
        if (EditorGUI.EndChangeCheck())
        {
            lastSceneView.size = nearClipping * 100;
            lastSceneView.Repaint();
        }

        // Instant focus to a near position without caring about the last pivot
        if (GUILayout.Button("Focus near"))
        {
            lastSceneView.LookAt(lastSceneView.camera.transform.position + lastSceneView.camera.transform.forward * 3f, lastSceneView.rotation, 3f);
        }
    }
}
