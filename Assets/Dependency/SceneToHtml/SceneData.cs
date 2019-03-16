using System.Collections.Generic;
using System.Linq;
using UnityYaml.Components;

namespace Dependency.SceneToHtml
{
    public class SceneData
    {   
        public List<GameObject> GameObjects;
        public Dictionary<int, object> Components;
        
        public SceneData(List<SceneContentParser.YamlComponent> components)
        {
            GameObjects = components.Where(item => item.Instance.GetType() == typeof(GameObject)).Select(item=> item.Instance as GameObject).ToList();
            Components = components.ToDictionary(item=> item.InstanceId, item=>item.Instance);
        }
    }
}