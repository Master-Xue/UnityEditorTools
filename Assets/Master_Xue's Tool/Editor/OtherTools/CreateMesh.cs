using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class CreateMesh
{

    [MenuItem("GameObject/Master_Xue's Tools/Create Mesh", priority = 0)]

    static void Create()
    {
        GameObject root = Selection.activeGameObject;
        if (root)
        {
            MeshRenderer[] meshRenderers = root.GetComponentsInChildren<MeshRenderer>();

            GameObject[] gos = new GameObject[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                gos[i] = meshRenderers[i].gameObject;
            }

            string scenePath = EditorSceneManager.GetSceneAt(0).path;
            string meshScenePath = scenePath.Replace(".unity", "_mesh");
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), meshScenePath);

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
            Directory.CreateDirectory(fullPath);
            string assetPath = FileUtil.GetProjectRelativePath(fullPath);

            StaticBatchingUtility.Combine(gos, root);
            for (int i = 0; i < gos.Length; i++)
            {
                Mesh mesh = gos[i].GetComponent<MeshFilter>().sharedMesh;
                string meshPath = AssetDatabase.GetAssetPath(mesh);

                if (string.IsNullOrEmpty(meshPath))
                {
                    string path = Path.Combine(assetPath, Random.Range(int.MinValue, int.MaxValue) + ".asset");
                    AssetDatabase.CreateAsset(mesh, path);
                }
            }
        }
        AssetDatabase.Refresh();
        EditorSceneManager.MarkAllScenesDirty();
    }

    
}
