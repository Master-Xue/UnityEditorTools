using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 精灵图片处理窗口
/// </summary>
public class SpriteControllerWindow : EditorWindow
{
    WindowTools pathTool = new WindowTools();
    WindowTools tagTool = new WindowTools();

    private bool spritePage = true;
    private int maxSpritesInPack = 0;//图集中最大精灵数量，0表示无限制
    private bool autoSize = true;
    private int maxSpriteSize;//最大尺寸
    private List<string> filePathList = new List<string>();//文件夹路径
    private List<string> tagList = new List<string>();

    EPictureSize pictureSize = EPictureSize._1024;

    EPicturePlatform picturePlatform = EPicturePlatform.Default;


    public static void Init()
    {
        GetWindow<SpriteControllerWindow>(true, "精灵设置");
    }


    private void OnGUI()
    {
        {
            GUILayout.Label("提示：不限制图集数量填写0，默认图片尺寸为读取图片尺寸失败时的尺寸设置，否则依据图片实际大小设置，图片路径和图集名称要一一对应");
            GUILayout.Space(20);

            picturePlatform = (EPicturePlatform)EditorGUILayout.EnumPopup("平台设置", picturePlatform);
            GUILayout.Space(10);

            autoSize = EditorGUILayout.Toggle("自动设置图片尺寸", autoSize);
            if (!autoSize)
            {
                pictureSize = (EPictureSize)EditorGUILayout.EnumPopup("默认图片尺寸", pictureSize);
                maxSpriteSize = (int)pictureSize;
            }
            GUILayout.Space(10);

            SetPath();

            GUILayout.Space(10);
            spritePage = EditorGUILayout.Toggle("打包图集", spritePage);

            if (spritePage)
            {
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Label("图集最大数量，无限制请用0");
                maxSpritesInPack = Convert.ToInt32(GUILayout.TextField(maxSpritesInPack.ToString()));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                SetTag();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("开始设置"))
            {
                SpriteSettings.SetSprites(filePathList, tagList, picturePlatform, maxSpritesInPack, autoSize, maxSpriteSize);
            }

        }
    }

    private void SetPath()
    {
        pathTool.ShowFoldout("图片路径(Assets下完整路径):" + filePathList.Count, delegate
           {
               pathTool.WindowBaseGroup(delegate
               {
                   for (int i = 0; i < filePathList.Count; i++)
                   {
                       filePathList[i] = GUILayout.TextField(filePathList[i]);
                   }
               }, "精灵文件夹路径", filePathList, null
               );

               GUILayout.Space(10);
               if (GUILayout.Button("刷新列表"))
               {
                   if (filePathList.Count > 0)
                   {
                       filePathList[filePathList.Count - 1] = Data.assetPath;
                   }
               }

           });



    }

    private void SetTag()
    {
        tagTool.ShowFoldout("图集名称(和图片路径一一对应):" + tagList.Count, delegate
          {
              tagTool.WindowBaseGroup(delegate
              {
                  for (int i = 0; i < tagList.Count; i++)
                  {
                      tagList[i] = GUILayout.TextField(tagList[i]);
                  }
              }, "图集名称", tagList, null
              );
          });
    }
}
