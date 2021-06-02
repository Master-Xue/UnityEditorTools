using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePrefebsInScene : Editor {

    public static void DelegateAndAddObject(List<string> sceneList, List<GameObject> addList,List<string> deleList)
    {
        List<string> scenes = DataTools.FindObjPathWithType("scene", sceneList);
        foreach (var scenePath in scenes)
        {
            EditorSceneManager.OpenScene(scenePath);
            Scene scene = SceneManager.GetActiveScene();
            ChangePrefeb(addList, deleList);
            EditorSceneManager.SaveScene(scene);
        }
    }

    static void ChangePrefeb(List<GameObject> addList, List<string> deleList)
    {
        foreach (var deleObj in deleList)
        {
            GameObject dele = GameObject.Find(deleObj);
            DestroyImmediate(dele);
        }
        foreach (var addObj in addList)
        {
            GameObject add = Instantiate(addObj) as GameObject;
            add.name.Replace("(Clone)", "");
        }
    }

}
