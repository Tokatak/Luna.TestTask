using System;
using UnityYaml.Components;

namespace TreeJs.Adapters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TreeJsInterpreterAttribute: Attribute 
    {
        public  Type Target;

        public TreeJsInterpreterAttribute(Type target)
        {
            Target = target;
        }
    }
    
    
}