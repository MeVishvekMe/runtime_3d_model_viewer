using UnityEngine;
using SFB; // Standalone File Browser
using System.IO;
using Siccity.GLTFUtility;
using System.Collections;

public class GLBModelLoader : MonoBehaviour
{
    public Transform spawnPoint;

    public void LoadModel()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Choose .glb or .gltf file", "", new[] { new ExtensionFilter("GLTF/GLB Files", "glb", "gltf") }, false);
        if (paths.Length > 0 && File.Exists(paths[0]))
        {
            StartCoroutine(LoadModelCoroutine(paths[0]));
        }
#endif
    }

    private IEnumerator LoadModelCoroutine(string path)
    {
        ImportSettings importSettings = new ImportSettings();

        GameObject result = null;
        bool done = false;

        Importer.ImportGLBAsync(path, importSettings, (go, animations) => {
            result = go;
            done = true;
        });

        // Wait until the model is done loading
        yield return new WaitUntil(() => done);

        if (result != null)
        {
            result.transform.position = spawnPoint.position;
            Debug.Log("Model loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load model.");
        }
    }

}