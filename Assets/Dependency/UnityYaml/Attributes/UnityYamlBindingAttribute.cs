using System;

namespace UnityYaml.Attributes
{
    [AttributeUsage(AttributeTargets.Class,Inherited = false)]
    public class UnityYamlBindingAttribute : Attribute
    {
        public int ClassID ;

        public UnityYamlBindingAttribute(int classId)
        {
            ClassID = classId;
        }
    }
}