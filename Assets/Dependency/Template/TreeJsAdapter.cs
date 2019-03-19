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

public class TreeJsAdapter
{
    public void DeployContent(SceneData sceneData, string pathToContent)
    {
        Dictionary<int, YamlComponentContext> GOJstreeContainerDictionary = new Dictionary<int, YamlComponentContext>();
        var ComponentGODictionary = new Dictionary<int, SceneContentParser.YamlComponent>();

        TreeJSScene scene = new TreeJSScene();

        scene.Geometries.Add("Cube", new GeometryCube());
        scene.Metadata.Geometries = 1;

        scene.Materials.Add("Material", new MeshLambertMaterial());
        scene.Metadata.Materials = 1;

        var gameObjects = sceneData.Components.Where(item => item.Instance is YamlGameObject).ToList();
        foreach (var yamlComponent in gameObjects)
        {
            TreeJSObject container = new TreeJSObject();
            var context = new YamlComponentContext(yamlComponent, sceneData.ComponentsDictionary, container);

            GOJstreeContainerDictionary.Add(yamlComponent.InstanceId, context);

            var yamlGamObject = yamlComponent.Instance as YamlGameObject;
            var componentIds = yamlGamObject.m_Component.Select(item => item.component.fileID);
            var components = sceneData.Components.Where(component => componentIds.Contains(component.InstanceId));
            foreach (var component in components)
            {
                Apply(component, context);
                
                ComponentGODictionary.Add(component.InstanceId,yamlComponent);
            }
        }

        //hierarchy
        foreach (var yamlComponent in gameObjects)
        {
            var context = GOJstreeContainerDictionary[yamlComponent.InstanceId];
            if (context.Transform.m_Father.fileID == 0)
            {
                scene.Objects.Add(context.Go.m_Name + (yamlComponent.InstanceId), context.Container);
            }

            foreach (Transform.FileID childTransform in context.Transform.m_Children)
            {
                SceneContentParser.YamlComponent childContainer = ComponentGODictionary[childTransform.fileID]; 
                var child = GOJstreeContainerDictionary[childContainer.InstanceId];
                context.Container.Children.Add(child.Go.m_Name + child.YamlComponent.InstanceId, child.Container);
            }
        }

        var str = JsonConvert.SerializeObject(scene);
        File.WriteAllText(pathToContent, str);
    }

    private void Apply(SceneContentParser.YamlComponent yamlComponent, YamlComponentContext context)
    {
        var component = yamlComponent.Instance;
        if (component is Transform)
        {
            var transform = component as Transform;

            //Note other z direction
            context.Container.Position = new[]{transform.m_LocalPosition.x, transform.m_LocalPosition.y, - transform.m_LocalPosition.z};
            
            var orientation = new Quaternion(
                transform.m_LocalRotation.x,
                transform.m_LocalRotation.y,
                transform.m_LocalRotation.z,
                transform.m_LocalRotation.w);

            var euler = orientation.eulerAngles;
            //Note same direction
            var xRot = Quaternion.AngleAxis(-euler.x,Vector3.right);
            var yRot = Quaternion.AngleAxis(-euler.y, Vector3.up);
            //Note different directions
            var zRot = Quaternion.AngleAxis(-euler.z, Vector3.back);   
                
            var  XYZeuler = (xRot * yRot * zRot).eulerAngles;  
                
            context.Container.Rotation = new[]
            {
                XYZeuler.x * Mathf.Deg2Rad,
                XYZeuler.y * Mathf.Deg2Rad,
                XYZeuler.z * Mathf.Deg2Rad
            };
            
            context.Container.Scale = new[]
                {transform.m_LocalScale.x, transform.m_LocalScale.y, transform.m_LocalScale.z};
        }

        if (component is MeshFilter)
        {
            var obj = new GeometryTreeJsObject {Geometry = "Cube", Material = "Material"};
            context.Container.Children.Add(context.Go.m_Name + (yamlComponent.InstanceId), obj);
        }

        if (component is Camera)
        {
            Debug.Log(component);
            context.Container.Children.Add("MainCamera", new PerspectiveCamera());
            ;
        }
    }

    class YamlComponentContext
    {
        public YamlGameObject Go;
        public Transform Transform;
        public TreeJSObject Container;
        public SceneContentParser.YamlComponent YamlComponent;

        public YamlComponentContext(SceneContentParser.YamlComponent yamlComponent,
            Dictionary<int, object> ComponentsDictionary, TreeJSObject container)
        {
            YamlComponent = yamlComponent;
            Container = container;
            Go = yamlComponent.Instance as YamlGameObject;

            var componentIds = Go.m_Component.Select(item => item.component.fileID);
            var components = componentIds.Select(x => ComponentsDictionary[x]).ToList();
            Transform = components.First(item => item is Transform) as Transform;
        }
    }
}