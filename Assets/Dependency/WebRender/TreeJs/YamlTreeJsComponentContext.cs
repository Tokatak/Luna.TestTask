using System.Collections.Generic;
using System.Linq;
using Dependency.Template;
using UnityYaml.Components;
using Transform = UnityYaml.Components.Transform;

namespace TreeJs
{
    public class YamlTreeJsComponentContext
    {
        public YamlGameObject Go;
        public Transform Transform;
        public TreeJSObject Container;
        public SceneContentParser.YamlComponent YamlComponent;

        public YamlTreeJsComponentContext(SceneContentParser.YamlComponent yamlComponent,
            Dictionary<int, object> ComponentsDictionary, TreeJSObject container)
        {
            YamlComponent = yamlComponent;
            Container = container;
            Go = yamlComponent.Instance as YamlGameObject;

            var componentIds = Go.m_Component.Select(item => item.component.fileID);
            var components = componentIds.Where(ComponentsDictionary.ContainsKey).Select(x => ComponentsDictionary[x]).ToList();
            Transform = components.First(item => item is Transform) as Transform;
        }
    }
}