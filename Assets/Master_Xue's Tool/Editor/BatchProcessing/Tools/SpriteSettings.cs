using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SpriteSettings : EditorWindow
{


    /// <summary>
    /// 检索Texture是否为带Alpha通道
    /// </summary>
    private static string RetrivalTextureIsAlpha = @"[Aa][Ll][Pp][Hh][Aa]";
    /// <summary>
    /// 检索Texture的MaxSize大小
    /// </summary>
    private static string RetrivalTextureMaxSize = @"[&]\d{2,4}";

    public static void SetSprites(List<string> filePathList, List<string> tagList, EPicturePlatform platform, int maxCountInPack, bool autoSize, int maxSize)
    {
        string platformString = platform.ToString();
        int platformMaxTextureSize = maxSize;//图片尺寸
        TextureImporterFormat platformTextureFormat = TextureImporterFormat.DXT5Crunched;
        int CurrentCount = 0;
        for (int i = 0; i < filePathList.Count; i++)
        {
            CurrentCount = 0;
            List<string> picPahtList = DataTools.FindObjPathWithType("texture", filePathList[i]);
            for (int j = 0; j < picPahtList.Count; j++)
            {
                string path = picPahtList[j];
                Debug.Log(path);
                EditorUtility.DisplayProgressBar(string.Format("正在设置精灵图片:{0}/{1}",i+1,filePathList.Count), path, 1.0f / picPahtList.Count * j);
                string tag = "";
                if (tagList != null && tagList.Count > 0)
                    tag = tagList[i];
                if (maxCountInPack != 0)
                {
                    CurrentCount++;
                    int final = Mathf.CeilToInt(CurrentCount / maxCountInPack);
                    tag += "_" + final;
                }

                TextureImporter import = AssetImporter.GetAtPath(path) as TextureImporter;
                TextureImporterPlatformSettings importSetting = new TextureImporterPlatformSettings();
                import.textureType = TextureImporterType.Sprite;//模式为精灵
                import.spritePackingTag = tag;
                import.spriteImportMode = SpriteImportMode.Single;
                import.alphaIsTransparency = true;
                import.mipmapEnabled = false;

                object[] args = new object[2];
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(import, args);
                int tempMaxSize = Mathf.Max((int)args[0], (int)args[1]);
                if (autoSize)
                {
                    if (tempMaxSize < 50)
                        platformMaxTextureSize = 32;
                    else if (tempMaxSize < 100)
                        platformMaxTextureSize = 64;
                    else if (tempMaxSize < 150)
                        platformMaxTextureSize = 128;
                    else if (tempMaxSize < 300)
                        platformMaxTextureSize = 256;
                    else if (tempMaxSize < 600)
                        platformMaxTextureSize = 512;
                    else
                        platformMaxTextureSize = 1024;
                }

                importSetting.name = platformString;
                importSetting.overridden = true;
                importSetting.maxTextureSize = platformMaxTextureSize;
                importSetting.format = platformTextureFormat;
                import.SetPlatformTextureSettings(importSetting);

                AssetDatabase.ImportAsset(path);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private void OnGUI()
    {

    }

}
