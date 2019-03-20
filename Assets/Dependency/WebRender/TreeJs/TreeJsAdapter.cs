using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dependency.SceneToHtml;
using Dependency.Template;
using Newtonsoft.Json;
using UnityEngine;
using UnityYaml.Components;
using Camera = UnityYaml.Components.Camera;
using MeshFilter = UnityYaml.Components.MeshFilter;
using Transform = UnityYaml.Components.Transform;

namespace TreeJs
{
    public class TreeJsAdapter
    {
        private readonly YamlToTreeJSComponentMapper _treeJsAdapterMapper;

        public TreeJsAdapter(YamlToTreeJSComponentMapper treeJsAdapterMapper)
        {
            _treeJsAdapterMapper = treeJsAdapterMapper;
        }

        public void DeployContent(SceneData sceneData, string pathToContent)
        {
            var instanceIdContextDictionary = new Dictionary<int, YamlTreeJsComponentContext>();
            var componentContextDictionary = new Dictionary<int, SceneContentParser.YamlComponent>();

            TreeJSScene scene = new TreeJSScene();

            BakedInSceneStuff(scene);

            //Look up for unity GameObjects
            var parsedYamlGameObjects = sceneData.Components.Where(item => item.Instance is YamlGameObject).ToList();
            foreach (var yamlGameObject in parsedYamlGameObjects)
            {
                //new TreeJsContainer - same as GO in unity
                TreeJSObject container = new TreeJSObject();

                //context for components
                var context = new YamlTreeJsComponentContext(yamlGameObject, sceneData.ComponentsDictionary, container);

                //fill up connection between Unity GO id and context
                instanceIdContextDictionary.Add(yamlGameObject.InstanceId, context);

                //Gathering "MonoBehaviors"/Components from Unit GO  (including transform)
                var yamlGamObject = yamlGameObject.Instance as YamlGameObject;
                var componentIds = yamlGamObject.m_Component.Select(item => item.component.fileID);
                var components = sceneData.Components.Where(component => componentIds.Contains(component.InstanceId));

                //Applying components over TreeJsContainer
                foreach (var component in components)
                {
                    var type = component.Instance.GetType();
                    
                    if(_treeJsAdapterMapper.ContainsKey(type))
                        _treeJsAdapterMapper[type].Interpret(component, context);
                    else
                    {
                        Debug.LogWarning($"Missing {type} for TreeJsAdaptermapper");
                    }

                    //fill look up dictionary for backtracing context
                    componentContextDictionary.Add(component.InstanceId, yamlGameObject);
                }
            }

            //gathering heirarchy 
            foreach (var yamlGameObject in parsedYamlGameObjects)
            {
                var context = instanceIdContextDictionary[yamlGameObject.InstanceId];
                //if no parent => add to root
                if (context.Transform.m_Father.fileID == 0)
                {
                    scene.Objects.Add(context.Go.m_Name + (yamlGameObject.InstanceId), context.Container);
                }

                //fill children using look ups
                foreach (Transform.FileID childTransform in context.Transform.m_Children)
                {
                    SceneContentParser.YamlComponent childContainer = componentContextDictionary[childTransform.fileID];
                    var child = instanceIdContextDictionary[childContainer.InstanceId];
                    context.Container.Children.Add(child.Go.m_Name + child.YamlComponent.InstanceId, child.Container);
                }
            }

            var serialized = JsonConvert.SerializeObject(scene);

            //flushing data 
            File.WriteAllText(pathToContent, serialized);
        }

        private static void BakedInSceneStuff(TreeJSScene scene)
        {
            //Basic Geometry
            scene.Geometries.Add("Cube", new GeometryCube());
            scene.Metadata.Geometries = 1;

            //Basic Material
            scene.Materials.Add("Material", new MeshLambertMaterial());
            scene.Metadata.Materials = 1;
        }
    }
}