using System;
using System.Collections.Generic;


public interface ISceneContentParser
{
    List<SceneContentParser.YamlComponent> Parse(string raw);
}

public class SceneContentParser : ISceneContentParser
{
    public string YAML;
    public string TAG;

    private const string END_LINE = "\n";
    private const string UNITY_SPLIT = "!u!";
    private const string UNITY_COMPONENT_SPLIT = "--- ";

    public List<YamlComponent> Parse(string raw)
    {
        var endIndex = raw.IndexOf(END_LINE, StringComparison.Ordinal) + 1;
        YAML = raw.Substring(0, endIndex).Split(' ')[1].TrimEnd(END_LINE.ToCharArray());

        raw = raw.Substring(endIndex, raw.Length - endIndex);

        endIndex = raw.IndexOf(END_LINE, StringComparison.Ordinal) + 1;
        TAG = raw.Substring(0, endIndex).Split(new[] {UNITY_SPLIT}, StringSplitOptions.None)[1]
            .TrimEnd(END_LINE.ToCharArray());

        raw = raw.Substring(endIndex, raw.Length - endIndex);

        string[] splits = raw.Split(new[] {UNITY_COMPONENT_SPLIT + UNITY_SPLIT}, StringSplitOptions.RemoveEmptyEntries);
        List<YamlComponent> Components = new List<YamlComponent>();
        foreach (var split in splits)
        {
            Components.Add(new YamlComponent(split));
        }

        return Components;
    }

    public class YamlComponent
    {
        public string Raw;

        public int ClassID;
        public int InstanceID;

        public string Header;
        public string Body;

        public string Name;

        private const string YAML_COMPONENT_HEADER_SPLIT = " &";

        public YamlComponent(string raw)
        {
            Raw = raw;
            Header = raw.Substring(0, raw.IndexOf(END_LINE));

            Body = raw.Substring(raw.IndexOf(END_LINE) + 1, raw.Length - (Header.Length + 1));

            var headerSplits = Header.Split(new[] {YAML_COMPONENT_HEADER_SPLIT}, StringSplitOptions.None);

            ClassID = int.Parse(headerSplits[0]);
            InstanceID = int.Parse(headerSplits[1]);

            Name = Body.Substring(0, Body.IndexOf(END_LINE));

            Body = Body.Substring(Body.IndexOf(END_LINE) + 1, Body.Length - (Name.Length + 1));
        }
    }
}
