using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEditor;

public class CreateXMLWindow : EditorWindow
{
    [MenuItem("Master_Xue's Tools/Create XML")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateXMLWindow), false, "Create XML");
    }
    string fileName, rootName = "故障统计";
    //bool rootAttr = false;
    string ele1, ele2, ele3;
    string ele11, ele12, ele13;
    string ele21, ele22, ele23;
    string ele31, ele32, ele33;

    
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        
        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.UpperCenter;
        GUILayout.Label("创建XML文件到Assets文件夹");

        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("文件名");
        fileName = GUILayout.TextField(fileName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("根节点名称");
        rootName = GUILayout.TextField(rootName);
        GUILayout.EndHorizontal();

        //rootAttr = GUILayout.Toggle(rootAttr, "包含属性值");
        //if (rootAttr)
        //{
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Label("根节点属性");
        //    string rootAttr="";
        //    rootAttr = rootAttr == "" ? "属性值" : GUILayout.TextField(rootAttr);
            
        //    GUILayout.EndHorizontal();
        //}

        if (GUILayout.Button("创建"))
        {
            Debug.Log("crate");
            CreatXMLDoc();
        }

        GUILayout.EndVertical();
    }



    /// <summary>
    /// 定义一个创建XML文档的方法
    /// </summary>
    void CreatXMLDoc()
    {
        Debug.Log("开始写入");
        //2.创建XML文档
        XmlDocument doc = new XmlDocument();
        //3.创建文件头
        XmlDeclaration declar = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //3.1将文件头拼接进文档
        doc.AppendChild(declar);
        //3.2创建根节点
        XmlNode root = doc.CreateNode(XmlNodeType.Element, rootName, null);
        ////3.3创建属性名
        //XmlAttribute bute = doc.CreateAttribute("企业名称");
        ////3.4给属性赋值
        //bute.Value = "北京禾中海外有限责任公司（禾中海外事业集团母公司）";
        ////3.5将创建的属性赋值给root节点
        //root.Attributes.SetNamedItem(bute);
        ////3.6将root节点拼接到doc中
        doc.AppendChild(root);

        ////4.企业总体信息节点
        ////4.1创建企业信息元素节点
        //XmlElement _message = doc.CreateElement("企业信息");
        ////4.2给节点设置属性
        //_message.SetAttribute("官网", "http://hzhwjt.cn/index.html");
        //_message.SetAttribute("企业电话", "010-86399686");
        //_message.SetAttribute("全景720yun链接网址", "https://720yun.com/t/e49jt7tfOa6");
        ////4.3设置标签的值
        //_message.InnerText = "Aria";
        ////4.4将gameObject节点拼接到文档
        //root.AppendChild(_message);

        ////5.备注信息节点
        ////5.1创建备注信息节点
        //XmlElement _remark = doc.CreateElement("备注");
        ////5.2创建position节点

        //XmlElement web = doc.CreateElement("官网");
        //web.InnerText = "http://hzhwjt.cn/index.html";

        //XmlElement tel = doc.CreateElement("企业电话");
        //tel.InnerText = "010-86399686";

        //XmlElement vrWeb = doc.CreateElement("全景720yun链接网址");
        //vrWeb.InnerText = "https://720yun.com/t/e49jt7tfOa6";

        //XmlElement _intro = doc.CreateElement("企业简介");
        //_intro.InnerText = "简介";

        //XmlElement _develop = doc.CreateElement("发展历程");
        //_develop.InnerText = "发展";

        //XmlElement _serve = doc.CreateElement("服务方案");
        //_serve.InnerText = "服务";

        //XmlElement _picText = doc.CreateElement("图片文字");
        //_picText.InnerText = "图片";

        //root.AppendChild(web);
        //root.AppendChild(tel);
        //root.AppendChild(vrWeb);
        //root.AppendChild(_intro);
        //root.AppendChild(_develop);
        //root.AppendChild(_serve);
        //root.AppendChild(_picText);

        //XmlElement earth = doc.CreateElement("地区");
        XmlElement fault1 = doc.CreateElement("电控发动机运转不正常");
        XmlElement defPart = doc.CreateElement("故障零件");
        defPart.InnerText = "流量调节阀";
        XmlElement demOrder = doc.CreateElement("拆除顺序");
        demOrder.InnerText = "1、2、3、4";
        fault1.AppendChild(defPart);
        fault1.AppendChild(demOrder);
        root.AppendChild(fault1);

        //root.AppendChild(earth);

        //root.AppendChild(_remark);


        


        //保存文档至给定的文件夹路径
        doc.Save(Application.dataPath +"/"+ fileName+".xml");
        Debug.Log("创建成功");
    }
}
