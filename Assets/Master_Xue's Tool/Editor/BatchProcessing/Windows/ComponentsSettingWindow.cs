using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 组件修改
/// </summary>
public class ComponentsSettingWindow : EditorWindow
{
    WindowTools addComponentTool = new WindowTools();
    WindowTools deleComponentTool = new WindowTools();
    WindowTools gameobjectTool = new WindowTools();
    WindowTools objSelectTool = new WindowTools();


    public static void Init()
    {
        GetWindow<ComponentsSettingWindow>(true,"场景中组件修改");
    }

    List<string> addComponentList = new List<string>();
    List<string> deleComponentList = new List<string>();
    List<GameObject> objList = new List<GameObject>();

    List<Component> addObjComponentToFind = new List<Component>();

    static bool add = false, dele = false;

    private void OnGUI()
    {
        EditorGUILayout.Space();

        gameobjectTool.ShowFoldout("添加修改物体", AddGameobject);

        EditorGUILayout.Space();

        objSelectTool.ShowFoldout("物体筛选条件", SelectTerm);

        EditorGUILayout.Space();
        add = GUILayout.Toggle(add, "添加组件");
        if (add)
        {
            addComponentTool.ShowFoldout("添加组件", AddComponent);
        }
        EditorGUILayout.Space();
        dele = GUILayout.Toggle(dele, "删除组件");
        if (dele)
        {
            deleComponentTool.ShowFoldout("删除组件", DeleComponent);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("确定"))
            BeginSetting();

    }

    private void AddGameobject()
    {
        gameobjectTool.WindowBaseGroup(delegate
        {
            for (int i = 0; i < objList.Count; i++)
            {
                objList[i] = (GameObject)EditorGUILayout.ObjectField(objList[i], typeof(GameObject));
            }
        }, "要修改的物体", objList);
    }

    private enum ESelectTerm
    {
        NoTerm,
        HasComponent
    }

    private ESelectTerm term = new ESelectTerm();
    private string ComponentType;
    private void SelectTerm()
    {
        term = (ESelectTerm)EditorGUILayout.EnumPopup("筛选条件", term);
        if (term == ESelectTerm.HasComponent)
        {
            ComponentType = EditorGUILayout.TextField("组件名称", ComponentType);
        }
    }

    private void AddComponent()
    {
        addComponentTool.WindowBaseGroup(delegate
        {
            for (int i = 0; i < addComponentList.Count; i++)
            {
                addComponentList[i] = EditorGUILayout.TextField(addComponentList[i]);
            }
        }, "要添加的组件", addComponentList);
    }

    private void DeleComponent()
    {
        deleComponentTool.WindowBaseGroup(delegate
        {
            for (int i = 0; i < deleComponentList.Count; i++)
            {
                deleComponentList[i] = (string)EditorGUILayout.TextField(deleComponentList[i]);
            }
        }, "要删除的组件", deleComponentList);
    }

    private void BeginSetting()
    {
        foreach (var item in objList)
        {
            if (term == ESelectTerm.HasComponent)
            {
                if (!item.GetComponent(ComponentType))
                {
                    continue;
                }
            }
            if (dele)
            {
                foreach (var deleCom in deleComponentList)
                {
                    if (string.IsNullOrEmpty(deleCom))
                        continue;
                    if (item.GetComponent(deleCom))
                    {
                        DestroyImmediate(item.GetComponent(deleCom));
                    }
                }
            }
            if (add)
            {
                foreach (var addCom in addComponentList)
                {
                    if (string.IsNullOrEmpty(addCom))
                        continue;
                    if (!item.GetComponent(addCom))
                    {
                        Type t = StringToComponent(addCom);
                        const string SourceInfo = "Assets/Master_Xue's Tool/Editor/BatchProcessing/Windows/ComponentsSettingWindow.cs (143,25)";
                        UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(item, SourceInfo, addCom);
                    }
                }
            }

        }
    }

    private Type StringToComponent(string name)
    {
        Type t = Type.GetType(name);
        if (t == null)
            return null;
        return t;
    }
}
