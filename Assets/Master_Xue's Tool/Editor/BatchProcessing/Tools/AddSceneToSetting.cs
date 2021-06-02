using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AddSceneToSetting : Editor
{

    #region 旧方法
    //public static void SetEditorBuildSettingsScenes(string path)
    //{
    //    List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
    //    if (EditorBuildSettings.scenes.Length > 0)
    //        foreach (var scene in EditorBuildSettings.scenes)
    //            editorBuildSettingsScenes.Add(scene);


    //    FileInfo[] files = DataTools.GetFiles(path);
    //    for (int i = 0; i < files.Length; i++)
    //    {
    //        string scenePath = DataTools.WordPathToLocal(files[i].FullName);

    //        if (CheckSamePath(scenePath, editorBuildSettingsScenes))
    //            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
    //    }

    //    EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    //}
    #endregion


    public static void SetEditorBuildSettingsScenes(List<string> pathList, bool resetScenes)
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        if (!resetScenes)
            if (EditorBuildSettings.scenes.Length > 0)
                foreach (var scene in EditorBuildSettings.scenes)
                    editorBuildSettingsScenes.Add(scene);

        List<string> scenePathList = DataTools.FindObjPathWithType("scene", pathList);
        foreach (var path in scenePathList)
        {
            if (string.IsNullOrEmpty(path))
                continue;
            if (CheckSamePath(path, editorBuildSettingsScenes))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        //添加完成后清空缓存
        editorBuildSettingsScenes.Clear();
    }

    /// <summary>
    /// 检查要添加的场景
    /// 和已添加场景是否重复
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static bool CheckSamePath(string path, List<EditorBuildSettingsScene> editorBuildSettingsScenes)
    {
        foreach (var item in editorBuildSettingsScenes)
            if (path.Equals(item.path))
                return false;
        return true;
    }
}
