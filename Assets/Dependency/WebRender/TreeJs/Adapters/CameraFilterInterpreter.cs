using Dependency.Template;
using UnityYaml.Components;

namespace TreeJs.Adapters
{
    [TreeJsInterpreter(target: typeof(Camera))]
    public class CameraFilterInterpreter : ITreeJsInterpreter
    {
        public void Interpret(SceneContentParser.YamlComponent yamlComponent, YamlTreeJsComponentContext context)
        {
            context.Container.Children.Add("MainCamera", new PerspectiveCamera(){Aspect = 1.333f});
        }
    }
}