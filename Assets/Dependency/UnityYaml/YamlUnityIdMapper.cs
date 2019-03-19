using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityYaml.Attributes;

public interface IYamlUnityIdMapper
{
    bool ContainsKey(int key);
    Type this[int key] { get; set; }
}

public class YamlUnityIdMapper : IYamlUnityIdMapper
{
    private Dictionary<int, Type> typeDictionary;
    
    public YamlUnityIdMapper(string MapperAssembly , string MapperNamespace)
    {
        typeDictionary = new Dictionary<int, Type>();
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .SingleOrDefault(x => x.GetName().Name == MapperAssembly);
        if (assembly == null)
        {
            Debug.LogError("Missing gameplay assembly");
            return;
        }

        foreach (Type t in assembly.GetTypes())
        {
            if (t.Namespace == MapperNamespace && t.IsClass)
            {
                var attribute = t.GetCustomAttribute<UnityYamlBindingAttribute>();

                if (attribute == null)
                    continue;

                typeDictionary.Add(attribute.ClassID, t);
            }
        }
    }

    public bool ContainsKey(int key)
    {
        return typeDictionary.ContainsKey(key);
    }

    public Type this[int key]
    {
        get => typeDictionary[key];
        set => typeDictionary[key] = value;
    }
}
