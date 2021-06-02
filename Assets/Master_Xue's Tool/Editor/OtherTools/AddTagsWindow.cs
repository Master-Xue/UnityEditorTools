using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AddTagsWindow : EditorWindow
{
    [MenuItem("Master_Xue's Tools/Tags/Add Tags")]
    public static void ShowWindow()
    {
        //弹出窗口
        EditorWindow.GetWindow(typeof(AddTagsWindow), false, "Add Tags Window");
    }
    //一次最多新加的tag
    string[] tags = new string[10];
    //最多可以选中的已有tag
    bool[] oldTagsBool = new bool[20];
    string[] oldTags;
    int tagsCount;
    public static string showNotify;
    void OnEnable()
    {
        tags = new string[10];
        oldTagsBool = new bool[20];
        oldTags = null;
        tagsCount = 0;
        showNotify = "";
    }
    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("Tags List");
        //获取已有tag
        oldTags = TagsTool.GetTags();
        tagsCount = 0;
        //显示已有tag
        if (oldTags != null && oldTags.Length > 0)
        {
            for (int i = 0; i < oldTags.Length; i++)
            {
                if (!string.IsNullOrEmpty(oldTags[i]) && i < oldTagsBool.Length)
                {
                    tagsCount++;
                    oldTagsBool[i] = EditorGUILayout.Toggle(oldTags[i], oldTagsBool[i]);
                }
            }
        }
        if (tagsCount == 0)
            EditorGUILayout.SelectableLabel("None");
        //删除选中tag
        if (GUILayout.Button("Clear Select Tags"))
        {
            List<string> toDeleteList = new List<string>();
            for (int i = 0; i < oldTagsBool.Length; i++)
            {
                if (oldTagsBool[i])
                {
                    toDeleteList.Add(oldTags[i]);
                }
            }
            showNotify = "";
            TagsTool.DeleteTags(toDeleteList.ToArray());
        }
        //删除所有tag
        if (GUILayout.Button("Clear All Tags"))
        {
            showNotify = "";
            TagsTool.DeleteTags(new string[0], true);
        }
        // -----
        GUILayout.Label("");
        GUILayout.Label("Input New Tags");
        //显示tag输入框
        for (int i = 0; i < tags.Length; i++)
        {
            tags[i] = EditorGUILayout.TextField("Tag Value: ", tags[i]);
        }
        //添加tag
        if (GUILayout.Button("Add Tags"))
        {
            showNotify = "";
            TagsTool.AddTags(tags);
        }
        //添加新tag，清除原有tag
        if (GUILayout.Button("Clear and Add New Tags"))
        {
            showNotify = "";
            TagsTool.AddTags(tags, false);
        }
        // -----
        GUILayout.Label("");
        //关闭弹窗
        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
        GUILayout.Label("");
        EditorGUILayout.TextArea(showNotify, GUILayout.ExpandHeight(true));
        //实时刷新
        this.Repaint();
    }
    void OnDisable()
    {
        tags = null;
        oldTagsBool = null;
        oldTags = null;
        tagsCount = 0;
        showNotify = "";
    }
    void OnLostFocus()
    {
    }
}
// ----- 批量设置 Obj Tag 操作弹窗 -----
public class SetTagsWindow : EditorWindow
{
    [MenuItem("Master_Xue's Tools/Tags/Set Tags")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SetTagsWindow), false, "Set Tags Window");
    }
    static GameObject[] selectObjs;
    GameObject newAddObj;
    List<GameObject> targetObjs = new List<GameObject>();
    bool isAddSelectObjs;
    string tagStr;
    string showNotify;
    void OnEnable()
    {
        newAddObj = null;
        targetObjs.Clear();
        isAddSelectObjs = false;
        tagStr = "Untagged";
        showNotify = "";
        selectObjs = Selection.gameObjects;
    }
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("U Can Select GameObjects By Mouse In Hierachy And Project Then Open This Window");
        GUILayout.Label("");
        GUILayout.Label("Selection List");
        if (!isAddSelectObjs && selectObjs != null && selectObjs.Length > 0)
        {
            targetObjs.AddRange(selectObjs);
            isAddSelectObjs = true;
        }
        if (targetObjs.Count > 0)
        {
            for (int i = 0; i < targetObjs.Count; i++)
            {
                targetObjs[i] = EditorGUILayout.ObjectField(targetObjs[i], typeof(GameObject), true) as GameObject;
            }
        }
        else
            GUILayout.Label("None");
        GUILayout.Label("");
        GUILayout.Label("Add To Selection");
        newAddObj = EditorGUILayout.ObjectField(newAddObj, typeof(GameObject), true) as GameObject;
        if (newAddObj != null && !targetObjs.Contains(newAddObj))
        {
            if (GUILayout.Button("Add Obj"))
            {
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    if (targetObjs[i] == null)
                    {
                        targetObjs[i] = newAddObj;
                        showNotify = "Add Obj To Select List";
                        return;
                    }
                }
                targetObjs.Add(newAddObj);
                showNotify = "Add Obj To Select List";
            }
        }
        GUILayout.Label("");
        GUILayout.Label("Set Tag");
        tagStr = EditorGUILayout.TagField("Tag for Objects : ", tagStr);
        if (GUILayout.Button("Set Tag"))
        {
            if (string.IsNullOrEmpty(tagStr))
            {
                showNotify = "Tag Is Null";
                return;
            }
            for (int i = 0; i < targetObjs.Count; i++)
            {
                targetObjs[i].tag = tagStr;
            }
            showNotify = "Set Tags Success";
        }
        GUILayout.Label("");
        //关闭弹窗
        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
        GUILayout.Label("");
        EditorGUILayout.TextArea(showNotify, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndVertical();
        //实时刷新
        this.Repaint();
    }
    void OnDisable()
    {
        selectObjs = null;
        newAddObj = null;
        targetObjs.Clear();
        isAddSelectObjs = false;
        tagStr = "Untagged";
        showNotify = "";
    }
    void OnLostFocus()
    {
    }
}
// ----- 批量修改 Tag Obj 操作弹窗 -----
public class ChangeTagsWindow : EditorWindow
{
    const string defaultTag = "Untagged";
    [MenuItem("Master_Xue's Tools/Tags/Change Tags")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChangeTagsWindow), false, "Change Tags Window");
    }
    static List<GameObject> allSceneObjs = new List<GameObject>();
    static List<GameObject> allPrefabs = new List<GameObject>();
    string showNotify;
    string searchTag;
    string newSceneTag;
    string newPrefabTag;
    List<GameObject> tagSceneObjs = new List<GameObject>();
    List<GameObject> tagPrefabs = new List<GameObject>();
    void GetAllObjs()
    {
        allSceneObjs.Clear();
        allPrefabs.Clear();
        //获取场景所有物体
        //GameObject[] tmpObjs = Resources.FindObjectsOfTypeAll<GameObject>() as GameObject[];
        GameObject[] tmpObjs = Object.FindObjectsOfType<GameObject>();
        if (tmpObjs != null && tmpObjs.Length > 0)
            allSceneObjs.AddRange(tmpObjs);
        //获取Project所有物体
        //获取Asset文件夹下所有Prefab的GUID
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        string tmpPath;
        for (int i = 0; i < ids.Length; i++)
        {
            //根据GUID获取路径
            tmpPath = AssetDatabase.GUIDToAssetPath(ids[i]);
            if (!string.IsNullOrEmpty(tmpPath))
            {
                //根据路径获取Prefab(GameObject)
                GameObject tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                if (tmpObj != null)
                    allPrefabs.Add(tmpObj);
            }
        }
    }
    void OnEnable()
    {
        GetAllObjs();
        showNotify = "";
        searchTag = defaultTag;
        newSceneTag = defaultTag;
        newPrefabTag = defaultTag;
        tagSceneObjs.Clear();
        tagPrefabs.Clear();
    }

    Vector2 objScrollPos, preScrollPos;
    void OnGUI()
    {
        GUILayout.Label("");
        searchTag = EditorGUILayout.TagField("Target Tag : ", searchTag);
        if (GUILayout.Button("Search Tag's Objs"))
        {
            if (string.IsNullOrEmpty(searchTag))
            {
                showNotify = "Tag's Objs Is None";
                return;
            }
            tagSceneObjs.Clear();
            tagPrefabs.Clear();
            for (int i = 0; i < allSceneObjs.Count; i++)
            {
                if (allSceneObjs[i].CompareTag(searchTag))
                    tagSceneObjs.Add(allSceneObjs[i]);
            }
            for (int i = 0; i < allPrefabs.Count; i++)
            {
                if (allPrefabs[i].CompareTag(searchTag))
                    tagPrefabs.Add(allPrefabs[i]);
            }
            showNotify = "Search Tag's Objs Success";
        }
        GUILayout.Label("");
        GUILayout.Label("Tag's All Scene Objs");
        objScrollPos = GUILayout.BeginScrollView(objScrollPos);
        if (tagSceneObjs.Count > 0)
        {
            //显示标记Tag的Obj
            for (int i = 0; i < tagSceneObjs.Count; i++)
            {
                tagSceneObjs[i] = EditorGUILayout.ObjectField(tagSceneObjs[i], typeof(GameObject), true) as GameObject;
            }
            //修改Tag
            newSceneTag = EditorGUILayout.TagField("Target Tag : ", newSceneTag);
            if (GUILayout.Button("Change To Target Tag"))
            {
                for (int i = 0; i < tagSceneObjs.Count; i++)
                {
                    tagSceneObjs[i].tag = newSceneTag;
                }
                showNotify = "Change All Scene Tag : " + searchTag + " To " + newSceneTag;
            }
            //清除Tag
            if (GUILayout.Button("Cleart Tags"))
            {
                for (int i = 0; i < tagSceneObjs.Count; i++)
                {
                    tagSceneObjs[i].tag = defaultTag;
                }
                showNotify = "Cleart All Scene Obj's Tag : " + searchTag;
            }
        }
        else
            GUILayout.Label("None");
        GUILayout.EndScrollView();

        GUILayout.Label("");
        GUILayout.Label("Tag's All Prefab Objs");

        preScrollPos = GUILayout.BeginScrollView(preScrollPos);
        if (tagPrefabs.Count > 0)
        {
            //显示标记Tag的Obj
            for (int i = 0; i < tagPrefabs.Count; i++)
            {
                tagPrefabs[i] = EditorGUILayout.ObjectField(tagPrefabs[i], typeof(GameObject), true) as GameObject;
            }
            //修改Tag
            newPrefabTag = EditorGUILayout.TagField("Target Tag : ", newPrefabTag);
            if (GUILayout.Button("Change To Target Tag"))
            {
                for (int i = 0; i < tagPrefabs.Count; i++)
                {
                    tagPrefabs[i].tag = newPrefabTag;
                }
                showNotify = "Change All Prefab Tag : " + searchTag + " To " + newPrefabTag;
            }
            //清除Tag
            if (GUILayout.Button("Clear Tags"))
            {
                for (int i = 0; i < tagPrefabs.Count; i++)
                {
                    tagPrefabs[i].tag = defaultTag;
                }
                showNotify = "Clear All Prefab's Tag : " + searchTag;
            }
        }
        else
            GUILayout.Label("None");
        GUILayout.EndScrollView();
        GUILayout.Label("");
        GUILayout.Label("Clear All Obj's Tag");
        if (GUILayout.Button("Clear All Scene Obj's Tag"))
        {
            for (int i = 0; i < allSceneObjs.Count; i++)
            {
                allSceneObjs[i].tag = defaultTag;
            }
            showNotify = "Clear All Scene Obj's Tag";
        }
        if (GUILayout.Button("Clear All Prefab's Tag"))
        {
            for (int i = 0; i < allPrefabs.Count; i++)
            {
                allPrefabs[i].tag = defaultTag;
            }
            showNotify = "Clear All Prefab's Tag";
        }
        GUILayout.Label("");
        //关闭弹窗
        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
        GUILayout.Label("");
        EditorGUILayout.TextArea(showNotify, GUILayout.ExpandHeight(true));
        //实时刷新
        this.Repaint();
    }
    void OnDisable()
    {
        allSceneObjs.Clear();
        allPrefabs.Clear();
        showNotify = "";
        searchTag = defaultTag;
        newSceneTag = defaultTag;
        newPrefabTag = defaultTag;
        tagSceneObjs.Clear();
        tagPrefabs.Clear();
    }
    void OnLostFocus()
    {
    }
}

// ----- 增加、删除 Tag 类 -----
public class TagsTool
{
    //添加tag
    public static void AddTags(string[] toAddtags, bool append = true)
    {
        //是否删除原有tag
        if (!append)
            DeleteAllTags();
        AddTags(toAddtags);
        AssetDatabase.Refresh();
    }
    public static void DeleteTags(string[] toDelTags, bool deleteAll = false)
    {
        if (deleteAll)
            DeleteAllTags();
        else
            DeleteTags(toDelTags);
        AssetDatabase.Refresh();
    }
    public static string[] GetTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "tags")
            {
                List<string> tmpTags = new List<string>();
                SerializedProperty dataPoint;
                for (int i = 0; i < it.arraySize; i++)
                {
                    dataPoint = it.GetArrayElementAtIndex(i);
                    if (!string.IsNullOrEmpty(dataPoint.stringValue))
                    {
                        tmpTags.Add(dataPoint.stringValue);
                    }
                }
                return tmpTags.ToArray();
            }
        }
        return null;
    }
    // -----
    //是否已有tag
    static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            //if (UnityEditorInternal.InternalEditorUtility.tags[i].Equals(tag))
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
            {
                AddTagsWindow.showNotify += "\n is has tags : " + tag;
                return true;
            }
        }
        return false;
    }
    //添加tag
    static void AddTags(string tag)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "tags")
                {
                    //如果有空tag值，替换
                    SerializedProperty dataPoint;
                    for (int i = 0; i < it.arraySize; i++)
                    {
                        dataPoint = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(dataPoint.stringValue))
                        {
                            dataPoint.stringValue = tag;
                            tagManager.ApplyModifiedProperties();
                            AddTagsWindow.showNotify += "\n add tags : " + tag;
                            return;
                        }
                    }
                    //没有空tag值 则新加tag
                    int tmpInt = it.arraySize;
                    it.InsertArrayElementAtIndex(tmpInt);
                    dataPoint = it.GetArrayElementAtIndex(tmpInt);
                    dataPoint.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                    AddTagsWindow.showNotify += "\n add tags : " + tag;
                }
            }
        }
    }
    //添加tag
    static void AddTags(string[] toAddtags)
    {
        if (toAddtags == null || toAddtags.Length == 0)
            return;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "tags")
            {
                bool isAdded;
                for (int i = 0; i < toAddtags.Length; i++)
                {
                    if (string.IsNullOrEmpty(toAddtags[i]) || isHasTag(toAddtags[i]))
                        continue;
                    isAdded = false;
                    //如果有空tag值，替换
                    SerializedProperty dataPoint;
                    for (int j = 0; j < it.arraySize; j++)
                    {
                        dataPoint = it.GetArrayElementAtIndex(j);
                        if (string.IsNullOrEmpty(dataPoint.stringValue))
                        {
                            dataPoint.stringValue = toAddtags[i];
                            tagManager.ApplyModifiedProperties();
                            AddTagsWindow.showNotify += "\n add tags : " + toAddtags[i];
                            isAdded = true;
                            break;
                        }
                    }
                    //没有空tag值 则新加tag
                    if (!isAdded)
                    {
                        int tmpInt = it.arraySize;
                        it.InsertArrayElementAtIndex(tmpInt);
                        dataPoint = it.GetArrayElementAtIndex(tmpInt);
                        dataPoint.stringValue = toAddtags[i];
                        tagManager.ApplyModifiedProperties();
                        AddTagsWindow.showNotify += "\n add tags : " + toAddtags[i];
                    }
                }
            }
        }
    }
    //删除tag
    static void DeleteTags(string[] toDelTags)
    {
        if (toDelTags == null || toDelTags.Length == 0)
            return;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "tags")
            {
                for (int i = 0; i < toDelTags.Length; i++)
                {
                    if (string.IsNullOrEmpty(toDelTags[i]) || !isHasTag(toDelTags[i]))
                        continue;
                    //将原tag置空
                    SerializedProperty dataPoint;
                    for (int j = 0; j < it.arraySize; j++)
                    {
                        dataPoint = it.GetArrayElementAtIndex(j);
                        if (dataPoint.stringValue.Equals(toDelTags[i]))
                        {
                            dataPoint.stringValue = null;
                            tagManager.ApplyModifiedProperties();
                            AddTagsWindow.showNotify += "\n clear tags : " + toDelTags[i];
                        }
                    }
                }
            }
        }
    }
    static void DeleteAllTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "tags")
            {
                //将原tag置空
                SerializedProperty dataPoint;
                for (int i = 0; i < it.arraySize; i++)
                {
                    dataPoint = it.GetArrayElementAtIndex(i);
                    if (!string.IsNullOrEmpty(dataPoint.stringValue))
                    {
                        dataPoint.stringValue = null;
                        tagManager.ApplyModifiedProperties();
                    }
                }
            }
        }
        AddTagsWindow.showNotify += "\n clear all old tags";
    }
    // --- Layer 与 Tag 类似 ---
    static bool isHasLayer(string layer)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
            {
                Debug.Log("is has layer : " + layer);
                return true;
            }
        }
        return false;
    }
    static void AddLayer(string layer)
    {
        if (!isHasLayer(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name.StartsWith("User Layer", System.StringComparison.Ordinal))
                {
                    if (it.type == "string")
                    {
                        if (string.IsNullOrEmpty(it.stringValue))
                        {
                            it.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            Debug.Log("add layer : " + layer);
                            return;
                        }
                    }
                }
            }
        }
    }
}
