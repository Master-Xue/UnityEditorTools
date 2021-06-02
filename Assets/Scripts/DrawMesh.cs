using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 五角星绘画工具
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawMesh : MonoBehaviour {

    Mesh mesh;

    public bool sophisticated = false;      //圆滑曲面  
    public bool star = true;                //星形  
    public int line = 5;                    //边数  
    public float[] maxsize = { 0, 0, 5, 0, 0 };   //外角大小  
    public float[] minSize = { 0, 0, 1, 0, 0 };   //内角大小  
    public float high = 1;                  //高度  
    public float low = -1;                  //低度  

    private Vector3[] vs;                   //顶点坐标  
    private int[] ts;                       //顶点序列  
    private Vector2[] newUVs;               //UV贴图  
    private Vector3[] newNormals;           //法线  

    // Update is called once per frame  
    void Update()
    {
        //变量约束  
        line = Mathf.Clamp(line, 2, 300);
        high = Mathf.Clamp(high, low, high);
        low = Mathf.Clamp(low, low, high);
        if (!star)
        {
            for (int i = 0; i < 5; i++)
            {
                minSize[i] = Mathf.Cos(Mathf.PI / line) * maxsize[i];
            }
        }
        for (int i = 1; i < 4; i++)
        {
            maxsize[i] = Mathf.Clamp(maxsize[i], 0, maxsize[i]);
            minSize[i] = Mathf.Clamp(minSize[i], 0, maxsize[i]);
        }
        maxsize[0] = Mathf.Clamp(maxsize[0], 0, maxsize[1]);
        minSize[0] = Mathf.Clamp(minSize[0], 0, minSize[1]);
        maxsize[4] = Mathf.Clamp(maxsize[4], 0, maxsize[3]);
        minSize[4] = Mathf.Clamp(minSize[4], 0, minSize[3]);
        //角度计算  
        float corner = 2 * Mathf.PI / line;
        //确定顶点数量  
        int temp = 5 * 4 * 3 * line;

        vs = new Vector3[temp];
        ts = new int[temp];
        newUVs = new Vector2[temp];
        newNormals = new Vector3[temp];

        int count = 0;
        float h = 0;
        float l = 0;
        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    h = high;
                    l = high;
                    break;
                case 1:
                    h = high;
                    l = 0;
                    break;
                case 2:
                    h = 0;
                    l = low;
                    break;
                case 3:
                    h = low;
                    l = low;
                    break;
                case 4:
                    h = low;
                    l = high;
                    break;
            }
            for (int k = 0; k < line; k++)
            {
                //确定顶点坐标  
                vs[count] = new Vector3(Mathf.Sin(k * corner) * maxsize[i], Mathf.Cos(k * corner) * maxsize[i], h);
                vs[count + 1] = new Vector3(Mathf.Sin((k + 0.5f) * corner) * minSize[(i + 1) % 5], Mathf.Cos((k + 0.5f) * corner) * minSize[(i + 1) % 5], l);
                vs[count + 2] = new Vector3(Mathf.Sin(k * corner) * maxsize[(i + 1) % 5], Mathf.Cos(k * corner) * maxsize[(i + 1) % 5], l);

                vs[count + 3] = new Vector3(Mathf.Sin(k * corner) * maxsize[i], Mathf.Cos(k * corner) * maxsize[i], h);
                vs[count + 4] = new Vector3(Mathf.Sin((k + 0.5f) * corner) * minSize[i], Mathf.Cos((k + 0.5f) * corner) * minSize[i], h);
                vs[count + 5] = new Vector3(Mathf.Sin((k + 0.5f) * corner) * minSize[(i + 1) % 5], Mathf.Cos((k + 0.5f) * corner) * minSize[(i + 1) % 5], l);

                vs[count + 6] = new Vector3(Mathf.Sin(k * corner) * maxsize[i], Mathf.Cos(k * corner) * maxsize[i], h);
                vs[count + 7] = new Vector3(Mathf.Sin(k * corner) * maxsize[(i + 1) % 5], Mathf.Cos(k * corner) * maxsize[(i + 1) % 5], l);
                vs[count + 8] = new Vector3(Mathf.Sin((k - 0.5f) * corner) * minSize[(i + 1) % 5], Mathf.Cos((k - 0.5f) * corner) * minSize[(i + 1) % 5], l);

                vs[count + 9] = new Vector3(Mathf.Sin(k * corner) * maxsize[i], Mathf.Cos(k * corner) * maxsize[i], h);
                vs[count + 10] = new Vector3(Mathf.Sin((k - 0.5f) * corner) * minSize[(i + 1) % 5], Mathf.Cos((k - 0.5f) * corner) * minSize[(i + 1) % 5], l);
                vs[count + 11] = new Vector3(Mathf.Sin((k - 0.5f) * corner) * minSize[i], Mathf.Cos((k - 0.5f) * corner) * minSize[i], h);
                //确定法线  
                if (sophisticated)
                {
                    newNormals[count] = Vector3.Normalize(vs[count] - new Vector3(0, 0, h));
                    newNormals[count + 1] = Vector3.Normalize(vs[count + 1] - new Vector3(0, 0, l));
                    newNormals[count + 2] = Vector3.Normalize(vs[count + 2] - new Vector3(0, 0, l));
                    newNormals[count + 3] = Vector3.Normalize(vs[count + 3] - new Vector3(0, 0, h));
                    newNormals[count + 4] = Vector3.Normalize(vs[count + 4] - new Vector3(0, 0, h));
                    newNormals[count + 5] = Vector3.Normalize(vs[count + 5] - new Vector3(0, 0, l));
                    newNormals[count + 6] = Vector3.Normalize(vs[count + 6] - new Vector3(0, 0, h));
                    newNormals[count + 7] = Vector3.Normalize(vs[count + 7] - new Vector3(0, 0, l));
                    newNormals[count + 8] = Vector3.Normalize(vs[count + 8] - new Vector3(0, 0, l));
                    newNormals[count + 9] = Vector3.Normalize(vs[count + 9] - new Vector3(0, 0, h));
                    newNormals[count + 10] = Vector3.Normalize(vs[count + 10] - new Vector3(0, 0, l));
                    newNormals[count + 11] = Vector3.Normalize(vs[count + 11] - new Vector3(0, 0, h));
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Vector3 newNormal = Vector3.Cross(vs[count + 3 * j + 1] - vs[count + 3 * j + 0], vs[count + 3 * j + 2] - vs[count + 3 * j + 0]);
                        for (int z = 0; z < 3; z++)
                        {
                            newNormals[count + 3 * j + z] = Vector3.Normalize(newNormal);
                        }
                    }
                }

                count += 12;
            }
        }
        //确定顶点序列  
        for (int i = 0; i < ts.Length; i++)
        {
            ts[i] = i;
        }
        //创建网格  
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vs;
        mesh.uv = newUVs;
        mesh.triangles = ts;
        mesh.normals = newNormals;

    }
}
