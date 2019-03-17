using System;
using System.Collections.Generic;


public interface ISceneContentParser
{
    List<SceneContentParser.RawYamlComponent> Parse(string raw);
}

public class SceneContentParser : ISceneContentParser
{
    public string YAML;
    public string TAG;

    private const string END_LINE = "\n";
    private const string UNITY_SPLIT = "!u!";
    private const string UNITY_COMPONENT_SPLIT = "--- ";

    public List<RawYamlComponent> Parse(string raw)
    {
        var endIndex = raw.IndexOf(END_LINE, StringComparison.Ordinal) + 1;
        YAML = raw.Substring(0, endIndex).Split(' ')[1].TrimEnd(END_LINE.ToCharArray());

        raw = raw.Substring(endIndex, raw.Length - endIndex);

        endIndex = raw.IndexOf(END_LINE, StringComparison.Ordinal) + 1;
        TAG = raw.Substring(0, endIndex).Split(new[] {UNITY_SPLIT}, StringSplitOptions.None)[1]
            .TrimEnd(END_LINE.ToCharArray());

        raw = raw.Substring(endIndex, raw.Length - endIndex);

        string[] splits = raw.Split(new[] {UNITY_COMPONENT_SPLIT + UNITY_SPLIT}, StringSplitOptions.RemoveEmptyEntries);
        List<RawYamlComponent> Components = new List<RawYamlComponent>();
        foreach (var split in splits)
        {
            Components.Add(new RawYamlComponent(split));
        }

        return Components;
    }

    public interface IYamlComponent
    {
        string Raw { get; }
        int ClassId { get; }
        int InstanceId { get; }
        string Header { get; }
        string Body { get; }
        string Name { get; }
    }

    public class YamlComponent : IYamlComponent
    {
        public string Raw => _yamlComponentImplementation.Raw;
        public int ClassId => _yamlComponentImplementation.ClassId;
        public int InstanceId => _yamlComponentImplementation.InstanceId;
        public string Header => _yamlComponentImplementation.Header;
        public string Body => _yamlComponentImplementation.Body;
        public string Name => _yamlComponentImplementation.Name;
        
        public object Instance { get; }

        private IYamlComponent _yamlComponentImplementation;

        public YamlComponent(IYamlComponent yamlComponentImplementation, object instance)
        {
            _yamlComponentImplementation = yamlComponentImplementation;
            Instance = instance;
        }
    }

    public class RawYamlComponent : IYamlComponent
    {
        public string Raw { get; }
        public int ClassId { get; }
        public int InstanceId { get; }
        public string Header { get; }
        public string Body { get; }
        public string Name { get; }

        private const string YAML_COMPONENT_HEADER_SPLIT = " &";

        public RawYamlComponent(string raw)
        {
            Raw = raw;
            Header = raw.Substring(0, raw.IndexOf(END_LINE));

            Body = raw.Substring(raw.IndexOf(END_LINE) + 1, raw.Length - (Header.Length + 1));

            var headerSplits = Header.Split(new[] {YAML_COMPONENT_HEADER_SPLIT}, StringSplitOptions.None);

            ClassId = int.Parse(headerSplits[0]);
            InstanceId = int.Parse(headerSplits[1]);

            Name = Body.Substring(0, Body.IndexOf(END_LINE));

            Body = Body.Substring(Body.IndexOf(END_LINE) + 1, Body.Length - (Name.Length + 1));
        }
    }
}
