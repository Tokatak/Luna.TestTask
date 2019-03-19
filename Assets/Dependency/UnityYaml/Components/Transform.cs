using System.Collections.Generic;
using UnityYaml.Attributes;

namespace UnityYaml.Components
{
    [UnityYamlBinding(classId:4)]
    public class Transform: UnityYamlComponent
    {
        public FileID m_GameObject { get; set; }
        public Vector4 m_LocalRotation { get; set; }
        public Vector3 m_LocalPosition { get; set; }
        public Vector3 m_LocalEulerAnglesHint { get; set; }
        public Vector3 m_LocalScale { get; set; }
        
        public List<FileID> m_Children { get; set; }
        public FileID m_Father { get; set; }


        public class Vector4
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
            public float w { get; set; }
        }

        public class Vector3
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class FileID
        {
            public int fileID { get; set; }
        }
    }
}