using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

/// <summary>
/// 批量替换场景中的预设
/// </summary>
public class ChangePrefebsWindow : EditorWindow
{
    WindowTools sceneTool = new WindowTools();
    WindowTools addTool = new WindowTools();
    WindowTools delTool = new WindowTools();
    public static void Init()
    {
        GetWindow<ChangePrefebsWindow>(true, "更改场景中预设体");
    }
    List<string> sceneList = new List<string>();
    List<string> delegateObjectList = new List<string>();
    List<GameObject> addObjectList = new List<GameObject>();
    //static  Vector2 sceneScrollPos,addScrollPos, deleScrollPos;

    private void OnGUI()
    {
        sceneTool.ShowFoldout("要更改的场景", delegate
        {
            sceneTool.WindowBaseGroup<string>(delegate
            {
                for (int i = 0; i < sceneList.Count; i++)
                    sceneList[i] = EditorGUILayout.TextField(sceneList[i]);
            }, "更改的场景目录", sceneList, null);
        });

        EditorGUILayout.Space();

        delTool.ShowFoldout("要删除的物体名", DelegateController);

        EditorGUILayout.Space();

        addTool.ShowFoldout("要添加的物体", AddController);
        EditorGUILayout.Space();
        if (GUILayout.Button("确定"))
            ChangePrefebsInScene.DelegateAndAddObject(sceneList, addObjectList, delegateObjectList);
    }

    private void AddController()
    {
        addTool.WindowBaseGroup<GameObject>(delegate
        {
            for (int i = 0; i < addObjectList.Count; i++)
                addObjectList[i] = EditorGUILayout.ObjectField(addObjectList[i], typeof(GameObject)) as GameObject;
        }, "添加预设体", addObjectList, null);
    }

    private void DelegateController()
    {
        delTool.WindowBaseGroup<string>(delegate
        {
            for (int i = 0; i < delegateObjectList.Count; i++)
                delegateObjectList[i] = EditorGUILayout.TextField(delegateObjectList[i]);
        }, "移除预设体", delegateObjectList, null);
    }

}
