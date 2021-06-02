using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChinToPinYin : EditorWindow
{

    [MenuItem("Assets/Master_Xue's Tools/汉字转拼音首字母")]
    public static void ToPinYinFirstSpell()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            string str = PingYinHelper.GetFirstSpell(objs[i].name);
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(objs[i]), str);
        }

    }

    [MenuItem("Assets/Master_Xue's Tools/汉字转拼音全拼")]
    public static void ToPinYinAllSpell()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            string str = PingYinHelper.ConvertToAllSpell(objs[i].name);
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(objs[i]), str);
        }
    }
}
