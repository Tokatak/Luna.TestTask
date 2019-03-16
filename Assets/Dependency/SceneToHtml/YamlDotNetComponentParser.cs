using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Serialization;

public interface IComponentParser
{
    List<SceneContentParser.YamlComponent> Parse(List<SceneContentParser.RawYamlComponent> components);
}

public class YamlDotNetComponentParser : IComponentParser
{
    IYamlUnityIdMapper typeDictionary { get; }

    public YamlDotNetComponentParser(IYamlUnityIdMapper typeDictionary)
    {
        this.typeDictionary = typeDictionary;
    }

    public List<SceneContentParser.YamlComponent> Parse(List<SceneContentParser.RawYamlComponent> components)
    {
        List<SceneContentParser.YamlComponent> result = new List<SceneContentParser.YamlComponent>();

        foreach (var rawComponent in components)
        {
            if (!typeDictionary.ContainsKey(rawComponent.ClassId))
            {
                Debug.LogWarning($"Missing class wrapper for ClassID {rawComponent.ClassId}");
                continue;
            }

            var kvp = typeDictionary[rawComponent.ClassId];

            var input = new StringReader(rawComponent.Body);

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

            result.Add(new SceneContentParser.YamlComponent(rawComponent,obj));
        }

        return result;
    }
}
