using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Serialization;

public interface IComponentParser
{
    List<object> Parse(List<SceneContentParser.YamlComponent> components);
}

public class ComponentParser : IComponentParser
{
    IYamlUnityIdMapper typeDictionary { get; }

    public ComponentParser(IYamlUnityIdMapper typeDictionary)
    {
        this.typeDictionary = typeDictionary;
    }

    public List<object> Parse(List<SceneContentParser.YamlComponent> components)
    {
        List<object> result = new List<object>();

        foreach (var serializedComponent in components)
        {
            if (!typeDictionary.ContainsKey(serializedComponent.ClassID))
            {
                Debug.LogWarning($"Missing class wrapper for ClassID {serializedComponent.ClassID}");
                continue;
            }

            var kvp = typeDictionary[serializedComponent.ClassID];

            var input = new StringReader(serializedComponent.Body);

            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            var methods = typeof(Deserializer).GetMethods().ToList();
            var byName = methods.Where(item =>
                item.Name == "Deserialize").ToList();

            var generic = byName.Where(item => item.IsGenericMethodDefinition).ToList();

            var parametred = generic.Where(item => item.GetParameters().Any(p => p.ParameterType == typeof(TextReader)))
                .ToList();

            var method = parametred.FirstOrDefault();

            MethodInfo genericMethod = method.MakeGenericMethod(kvp);
            var obj = genericMethod.Invoke(deserializer, new[] {input});

            result.Add(obj);
        }

        return result;
    }
}
