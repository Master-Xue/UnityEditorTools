using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class UpdateDatas : Editor
{

    private static bool setGUIDSAuto = false;

    [MenuItem("Master_Xue's Tools/全局设置/拾取选中文件路径")]
    static void SetGUIDSSelected()
    {
        setGUIDSAuto = !setGUIDSAuto;
    }

    static UpdateDatas()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (setGUIDSAuto)
        {
            string[] guids = Selection.assetGUIDs;
            if (guids != null && guids.Length > 0)
                Data.assetPath = AssetDatabase.GUIDToAssetPath(guids[0]).Replace("Assets/", "");

        }


    }
}
