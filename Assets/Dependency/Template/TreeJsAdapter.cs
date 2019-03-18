﻿using System.Collections.Generic;
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

public class TreeJsAdapter 
{
    public void DeployContent(SceneData sceneData, string pathToContent)
    {
        Dictionary<int,object> CombonentJstreeObject = new Dictionary<int, object>();
        
        TreeJSScene scene = new TreeJSScene();

        scene.Geometries.Add("Cube",new GeometryCube());
        scene.Metadata.Geometries=1;
        
        scene.Materials.Add("Material",new MeshLambertMaterial());
        scene.Metadata.Materials = 1;

        foreach (var yamlComponent in  sceneData.Components.Where(item => item.Instance is YamlGameObject) )
        {
            TreeJSObject container = new TreeJSObject();
            var context = new YamlComponentContext(yamlComponent,sceneData.ComponentsDictionary,container);
            scene.Objects.Add(context.Go.m_Name + (yamlComponent.InstanceId),container);

            var yamlGamObject = yamlComponent.Instance as YamlGameObject;
            var componentIds = yamlGamObject.m_Component.Select(item=>item.component.fileID);
//            var components = componentIds.Select(x => sceneData.Components.Where(item=>item.InstanceId == x));
            var components = sceneData.Components.Where(component => componentIds.Contains(component.InstanceId));
            foreach (var component in components)
            {
                Apply(component, context);
            }
            
            

//            YamlGameObject instance = go.Instance as YamlGameObject;
//
//            var componentIds = instance.m_Component.Select(item=>item.component.fileID);
//            var components = componentIds.Select(x => sceneData.ComponentsDictionary[x]).ToList();
//            var transform = components.First(item => item is Transform) as Transform;
//            
//            var obj = new Object {Geometry = "Cube", Material = "Material"};
//            var rot = new Quaternion(transform.m_LocalRotation.x,transform.m_LocalRotation.y,transform.m_LocalRotation.z,transform.m_LocalRotation.w);
//            obj.Position = new []{transform.m_LocalPosition.x,transform.m_LocalPosition.y,transform.m_LocalPosition.z};
//            obj.Rotation = new[] {rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z}; 
//            
//            scene.Objects.Add(instance.m_Name + go.InstanceId,obj);
        }  
        
        var str = JsonConvert.SerializeObject(scene);
        File.WriteAllText(pathToContent,str);
    }

    private void Apply(SceneContentParser.YamlComponent yamlComponent, YamlComponentContext context)
    {
        var component = yamlComponent.Instance;
        if(component is Transform)
        {
            var transform = component as Transform;
            var rot = new Quaternion(transform.m_LocalRotation.x,transform.m_LocalRotation.y,transform.m_LocalRotation.z,transform.m_LocalRotation.w);
            context.Container.Position = new[]{transform.m_LocalPosition.x, transform.m_LocalPosition.y, -transform.m_LocalPosition.z};
            context.Container.Rotation = new[] {rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z}; 
        }

        if (component is MeshFilter)
        {
            var obj = new GeometryTreeJsObject {Geometry = "Cube", Material = "Material"};
            context.Container.Children.Add(context.Go.m_Name + (yamlComponent.InstanceId), obj);
        }

        if (component is Camera)
        {
            Debug.Log(component);
            context.Container.Children.Add("MainCamera", new PerspectiveCamera());;
        }
    }

    private void Interpret()
    {
        
    }

  

    class YamlComponentContext
    {
        public YamlGameObject Go;
        public Transform Transform;
        public TreeJSObject Container;
        public SceneContentParser.YamlComponent YamlComponent;
        
        public YamlComponentContext(SceneContentParser.YamlComponent yamlComponent , Dictionary<int, object> ComponentsDictionary, TreeJSObject container)
        {

            YamlComponent = yamlComponent;
            Container = container;
            Go = yamlComponent.Instance as YamlGameObject;
          
            var componentIds = Go.m_Component.Select(item=>item.component.fileID);
            var components = componentIds.Select(x => ComponentsDictionary[x]).ToList();
            var Transform = components.First(item => item is Transform) as Transform;
        }
    }
}