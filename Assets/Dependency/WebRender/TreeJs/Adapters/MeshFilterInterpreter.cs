using Dependency.Template;
using UnityYaml.Components;

namespace TreeJs.Adapters
{
    [TreeJsInterpreter(target: typeof(MeshFilter))]
    public class MeshFilterInterpreter : ITreeJsInterpreter
    {
        public void Interpret(SceneContentParser.YamlComponent yamlComponent, YamlTreeJsComponentContext context)
        {
            var obj = new GeometryTreeJsObject {Geometry = "Cube", Material = "Material"};
            context.Container.Children.Add(context.Go.m_Name + (yamlComponent.InstanceId), obj);
        }
    }
}