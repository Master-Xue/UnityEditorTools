using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatScenesByTex : Editor
{
    static List<string> localPathList = new List<string>();
    public static void Init(List<string> pathes)
    {
        localPathList = DataTools.FindObjPathWithType("texture", pathes);
        foreach (var path in localPathList)
        {
            if (string.IsNullOrEmpty(path))
                continue;
            StartCreat(path);
        }
    }
    static Texture texture;
    static string[] strs;

    /// <summary>
    /// 创建材质球
    /// </summary>
    /// <param name="path"></param>
    private static void StartCreat(string path)
    {
        Debug.Log("开始创建场景：" + path);
        strs = path.Split('/');
        texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
        string matPath = path.Replace(strs[strs.Length-1], "Materials");
        CreatFloder(matPath);//创建材质球文件夹
        Material mat = CreatMat(matPath + "/" + texture.name.Insert(texture.name.Length, ".mat"));//创建材质球
        mat.mainTexture = texture;
        CreatScene(path, mat);
    }

    /// <summary>
    /// 创建场景
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mat"></param>
    private static void CreatScene(string path, Material mat)
    {
        //string[] strs = path.Split('/');

        //将贴图路径转为场景文件路径↓
        string scenePath = "Assets/Scenes";
        for (int i = 2; i < strs.Length-1; i++)
        {
            scenePath.Insert(scenePath.Length, strs[i]);
        }
        string sceneName = PingYinHelper.GetFirstSpell(strs[strs.Length-2]) + "_" + PingYinHelper.ConvertToAllSpell(texture.name);// 中文转拼音（场景文件名=贴图文件上一级文件夹名字首字母 + _ + 贴图文件名）
        
        //scenePath.Insert(scenePath.Length, "/" + sceneName);
        //将贴图路径转为场景文件路径↑

        CreatFloder(scenePath);//创建场景文件夹
        //确定要先打开一个新的场景
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        ApplyScence(mat);//实例化预设体
        Scene scene = SceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(scene, scenePath + "/" + sceneName + ".unity", false);

    }

    /// <summary>
    /// 检查文件夹是否存在，不存在则创建
    /// </summary>
    /// <param name="floderPath">材质球文件夹完整路径</param>
    private static void CreatFloder(string floderPath)
    {
        if (!Directory.Exists(floderPath))
            Directory.CreateDirectory(floderPath);

    }

    /// <summary>
    /// 实例化预设体
    /// </summary>
    /// <param name="matAsset"></param>
    private static void ApplyScence(Material matAsset)
    {
        if (CreatScenesWindow.delegateCamera)
            DestroyImmediate(GameObject.Find("Main Camera"));

        for (int i = 0; i < CreatScenesWindow.prefebList.Count; i++)
        {
            if (CreatScenesWindow.prefebList[i] == null)  return;

            GameObject pre = Instantiate(CreatScenesWindow.prefebList[i]) as GameObject;
            pre.name = pre.name.Replace("(Clone)", "");
            if (i==0)
            {
                MeshRenderer render = pre.GetComponentInChildren<MeshRenderer>();
                if (render != null)
                    render.material = matAsset;
            }
            
        }
        //
        //GameObject prefab = Instantiate(Resources.Load<GameObject>("ScecePrefabs/PictureSphere"), Vector3.zero, Quaternion.identity);
        //prefab.name = "PictureSphere";
        //prefab.GetComponent<Renderer>().material = matAsset;

        //prefab = Instantiate(Resources.Load<GameObject>("ScecePrefabs/PlayerUI"), Vector3.zero, Quaternion.identity);
        //prefab.name = "PlayerUI";


        //GameObject MainCamera = Instantiate(Resources.Load<GameObject>("ScecePrefabs/Main Camera"), Vector3.zero, Quaternion.identity);
        //MainCamera.name = "MainCamera";
    }


    /// <summary>
    /// 创建材质球
    /// </summary>
    /// <param name="matPath">材质球完整路径</param>
    /// <returns></returns>
    private static Material CreatMat(string matPath)
    {
        Debug.Log("//" + matPath);
        Material matAsset = Instantiate(new Material(Shader.Find("Unlit/Texture")));
        if (!File.Exists(matPath))
        {
            AssetDatabase.CreateAsset(matAsset, matPath);
            EditorUtility.SetDirty(matAsset);
        }
        else
            matAsset = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return matAsset;
    }

    #region 旧方法
    //static string fileLocalPath;
    //public static void Init(string texPathes, string[] imageType)
    //{
    //    if (imageType.Length == 1)
    //    {
    //        Debug.Log("请选择贴图文件类型");
    //        return;
    //    }

    //    FileInfo[] files = DataTools.GetFiles(texPathes);
    //    foreach (var file in files)
    //        for (int i = 0; i < imageType.Length - 1; i++)
    //            if (file.Name.EndsWith(imageType[i].ToLower()) || file.Name.EndsWith(imageType[i].ToUpper()))
    //            {
    //                fileLocalPath = DataTools.WordPathToLocal(file.FullName);
    //                StartCreat(file);
    //            }

    //}
    ///*
    // * file.Extension 后缀
    // * file.DirectoryName 上级文件夹路径
    // */

    //private static void StartCreat(FileInfo file)
    //{
    //    Debug.Log("开始创建场景：" + file.FullName);
    //    string matPath = fileLocalPath.Replace(file.Name, "Materials");
    //    CreatFloder(matPath.ToString());//创建材质球文件夹
    //    Material mat = CreatMat(matPath + "/" + file.Name.Replace(file.Extension, ".mat"));//创建材质球
    //    mat.mainTexture = AssetDatabase.LoadAssetAtPath(fileLocalPath, typeof(Texture)) as Texture;
    //    CreatScene(file, mat);
    //}


    ///// <summary>
    ///// 创建场景
    ///// </summary>
    ///// <param name="file"></param>
    ///// <param name="mat"></param>
    //private static void CreatScene(FileInfo file, Material mat)
    //{
    //    string sceneName = PingYinHelper.GetFirstSpell(file.Directory.Name) + "_" + PingYinHelper.ConvertToAllSpell(file.Name.Replace(file.Extension, ""));// 中文转拼音（场景文件名=贴图文件上一级文件夹名字首字母 + _ + 贴图文件名）

    //    //将贴图路径转为场景文件路径↓
    //    string scenePath = "Assets/Scenes";
    //    string[] pathes = fileLocalPath.Split('/');
    //    for (int i = 2; i < pathes.Length - 1; i++)
    //        scenePath.Insert(scenePath.Length, "/" + pathes[i]);

    //    //scenePath.Insert(scenePath.Length, "/" + sceneName);
    //    //将贴图路径转为场景文件路径↑

    //    CreatFloder(scenePath);//创建场景文件夹
    //    //确定要先打开一个新的场景
    //    EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    //    ApplyScence(mat);//实例化预设体
    //    Scene scene = SceneManager.GetActiveScene();
    //    EditorSceneManager.SaveScene(scene, scenePath + "/" + sceneName + ".unity", false);

    //}
    #endregion

}
