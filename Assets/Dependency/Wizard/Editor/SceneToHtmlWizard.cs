using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityYaml;
using UnityYaml.Attributes;
using YamlDotNet.Serialization;


public class SceneToHtmlWizard : ScriptableWizard
{
    [MenuItem("Tools/SceneToHtmlWizard %t")]
    static void CreateWizard()
    {
        DisplayWizard<SceneToHtmlWizard>("Scene Parser", "Parse");
    }

    void OnWizardCreate()
    {
        var scene = SceneManager.GetActiveScene();
        var rawSceneContent = new SceneReader().Read(scene);
        var components = new SceneContentParser().Parse(rawSceneContent);

        IYamlUnityIdMapper typeMapper = new YamlUnityIdMapper(Constants.COMPONENTS_WRAPPER_ASSEMBLY, Constants.COMPONENTS_NAMESPACE);
        IComponentParser componentParser = new YamlDotNetComponentParser(typeMapper);
        var parsedComponents = componentParser.Parse(components);

        foreach (object parsedComponent in parsedComponents)
        {
            Debug.Log(parsedComponent);
        }
    }
}


