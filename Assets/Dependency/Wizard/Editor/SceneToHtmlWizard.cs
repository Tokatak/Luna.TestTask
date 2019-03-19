using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dependency.SceneToHtml;
using Dependency.Template;
using TreeJs;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityYaml;
using UnityYaml.Attributes;
using YamlDotNet.Serialization;


public class SceneToHtmlWizard : ScriptableWizard
{
    [Tooltip("Relative to project root")]
    public string BuildDestinationFolder= "Builds";
    
    public string TemplatePath = @"\Dependency\Template\treejs";
    public string TemplateContentPath = @"data\storedScene.json";
    
    [MenuItem("Tools/SceneToHtmlWizard %t")]
    static void CreateWizard()
    {
        DisplayWizard<SceneToHtmlWizard>("Scene Parser", "Parse");
    }

    void OnWizardCreate()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneContent = new SceneReader().Read(scene);
        var rawYamlComponents = new SceneContentParser().Parse(sceneContent);
        var typeMapper = new YamlUnityIdMapper(Constants.COMPONENTS_WRAPPER_ASSEMBLY, Constants.COMPONENTS_NAMESPACE);
        var componentParser = new YamlDotNetComponentParser(typeMapper);
        var components = componentParser.Parse(rawYamlComponents);
     
        //deply template
        var deployer = new Deployer(BuildDestinationFolder);
        string deployPath =  deployer.Deploy(Path.Combine(Application.dataPath +TemplatePath));
        var pathToIndex =  Path.Combine(deployPath ,@"index.html");
        
        var sceneData = new SceneData(components);
        var pathToContent = Path.Combine(deployPath, TemplateContentPath);
        
        var treeJsAdapterMapper = new YamlToTreeJSComponentMapper(Constants.TREEJS_ASSEMBLY,Constants.TREEJS_ADAPTERS_NAMESPAGE);
        new TreeJsAdapter(treeJsAdapterMapper).DeployContent(sceneData, pathToContent);
        
        
        //point explorer to index file
//        EditorUtility.RevealInFinder(pathToIndex);

    }
}


