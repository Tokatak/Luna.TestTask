using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TreeJs.Adapters;
using UnityEngine;
using UnityYaml.Attributes;

namespace Dependency.Template
{
    public class YamlToTreeJSComponentMapper
    {
        public Dictionary<Type, ITreeJsInterpreter> typeDictionary = new Dictionary<Type, ITreeJsInterpreter>();

        public YamlToTreeJSComponentMapper(string MapperAssembly, string MapperNamespace)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(x => x.GetName().Name == MapperAssembly);

            if (assembly == null)
            {
                Debug.LogError("Missing assembly");
                return;
            }

            foreach (Type t in assembly.GetTypes())
            {
                if (t.Namespace == MapperNamespace && t.IsClass)
                {
                    var attribute = t.GetCustomAttribute<TreeJsInterpreterAttribute>();

                    if (attribute == null)
                        continue;

                    typeDictionary.Add(attribute.Target, (ITreeJsInterpreter) Activator.CreateInstance(t));
                }
            }
        }

        public bool ContainsKey(Type key)
        {
            return typeDictionary.ContainsKey(key);
        }

        public ITreeJsInterpreter this[Type key]
        {
            get => typeDictionary[key];
            set => typeDictionary[key] = value;
        }
    }
}