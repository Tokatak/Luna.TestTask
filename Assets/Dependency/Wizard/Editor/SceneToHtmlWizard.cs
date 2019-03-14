using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToHtmlWizard : ScriptableWizard
{
    [MenuItem("Tools/ToHtmlWizard %t")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SceneToHtmlWizard>("Scene Parser", "Parse");
    }

    void OnWizardCreate()
    {
        var scene = SceneManager.GetActiveScene();
        var path = scene.path;

        var dataPath = Directory.GetParent(Application.dataPath).FullName;
        var fullPath = Path.Combine(dataPath, path);
        Debug.Log($"Scene path {fullPath}");

        using (var stream = new StreamReader(fullPath))
        {
            var rawSceneContent = stream.ReadToEnd();
            Debug.Log(rawSceneContent);
        }
    }
}