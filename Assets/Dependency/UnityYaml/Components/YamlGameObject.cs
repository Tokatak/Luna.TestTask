using System.Collections.Generic;
using UnityYaml.Attributes;

namespace UnityYaml.Components
{
    [UnityYamlBinding(classId: 1)]
    public class YamlGameObject
    {
        public List<MComponent> m_Component { get; set; }
        public string m_Name { get; set; }
        public int m_IsActive { get; set; }
    }

    public class Component
    {
        public int fileID { get; set; }
    }

    public class MComponent
    {
        public Component component { get; set; }
    }
}