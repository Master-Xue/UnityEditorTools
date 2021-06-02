using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 将所有脚本写入文档
/// </summary>
public class ScriptOut : Editor {

    static string url,writeUrl;
    static StreamWriter sw;
	[MenuItem("Master_Xue's Tools/将所有脚本写入文档")]
	static void OutToFile () {
        url = Application.dataPath;
        writeUrl = Application.dataPath + "/OutScript.txt";
        sw = new StreamWriter(writeUrl,true);
        WriteScript();
	}

    static void WriteScript()
    {
        if (Directory.Exists(url))
        {
            DirectoryInfo direction = new DirectoryInfo(url);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            Debug.Log("filesCount: " + files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".cs"))
                {
                    Debug.Log(files[i].Name);
                    StreamReader sr = files[i].OpenText();
                    sw.Write(sr.ReadToEnd());
                    sw.Flush();
                    sr.Close();
                }
            }
            Debug.Log("Scuccess");
            sw.Close();
        }
    }

    /**  
  * 读取文本文件  
  * path：读取文件的路径  
  * name：读取文件的名称  
  */
    ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取    
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空    
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取    
            //将每一行的内容存入数组链表容器中    
            arrlist.Add(line);
        }
        //关闭流    
        sr.Close();
        //销毁流    
        sr.Dispose();
        //将数组链表容器返回    
        return arrlist;
    }
}
