using System.Collections.Generic;
using System.Linq;
using UnityYaml.Components;

namespace Dependency.SceneToHtml
{
    public class SceneData
    {   
        public List<SceneContentParser.YamlComponent> Components;
        public List<YamlGameObject> GameObjects;
        public Dictionary<int, object> ComponentsDictionary;

        public SceneData(List<SceneContentParser.YamlComponent> components)
        {
            Components = components;
            GameObjects = components.Where(item => item.Instance.GetType() == typeof(YamlGameObject)).Select(item=> item.Instance as YamlGameObject).ToList();
            ComponentsDictionary = components.ToDictionary(item=> item.InstanceId, item=>item.Instance);
        }
    }
}