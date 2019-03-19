namespace TreeJs.Adapters
{
    public interface  ITreeJsInterpreter
    {
        void Interpret(SceneContentParser.YamlComponent yamlComponent, YamlTreeJsComponentContext context);
    }
}