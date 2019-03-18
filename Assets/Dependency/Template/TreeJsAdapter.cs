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
            var components = sceneData.Components.Where(component => componentIds.Contains(component.InstanceId));
            foreach (var component in components)
            {
                Apply(component, context);
            }
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
         
            //Note other z destination
            context.Container.Position = new[]{transform.m_LocalPosition.x, transform.m_LocalPosition.y, -transform.m_LocalPosition.z};
            context.Container.Rotation = new[] {transform.m_LocalEulerAnglesHint.x, transform.m_LocalEulerAnglesHint.y, transform.m_LocalEulerAnglesHint.z}; 
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
