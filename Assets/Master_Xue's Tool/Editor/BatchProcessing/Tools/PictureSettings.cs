using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class PictureSettings : Editor
{


    public static void SetPicture(List<string> flodList, TextureImporter importPre)
    {
        List<string> picPahtList = DataTools.FindObjPathWithType("texture", flodList);
        foreach (var path in picPahtList)
        {
            if (string.IsNullOrEmpty(path))
                continue;
            Debug.Log(path);
            TextureImporterSettings settingPre = new TextureImporterSettings();
            importPre.ReadTextureSettings(settingPre);
            TextureImporter import = AssetImporter.GetAtPath(path) as TextureImporter;
            import.SetTextureSettings(settingPre);
            import.maxTextureSize = importPre.maxTextureSize;
            import.textureCompression = importPre.textureCompression;
            import.textureType = importPre.textureType;
            import.SaveAndReimport();
        }
    }
}
