using UnityEngine;
using Transform = UnityYaml.Components.Transform;

namespace TreeJs.Adapters
{
    [TreeJsInterpreter(target:typeof(Transform))]
    public class TransformInterpreter : ITreeJsInterpreter
    {
        public void Interpret(SceneContentParser.YamlComponent yamlComponent, YamlTreeJsComponentContext context)
        {
            var component = yamlComponent.Instance;
            if (component is Transform)
            {
                var transform = component as Transform;

                //Note other z direction
                context.Container.Position = new[]
                    {transform.m_LocalPosition.x, transform.m_LocalPosition.y, -transform.m_LocalPosition.z};

                var orientation = new Quaternion(
                    transform.m_LocalRotation.x,
                    transform.m_LocalRotation.y,
                    transform.m_LocalRotation.z,
                    transform.m_LocalRotation.w);

                var euler = orientation.eulerAngles;
                //Note same direction
                var xRot = Quaternion.AngleAxis(-euler.x, Vector3.right);
                var yRot = Quaternion.AngleAxis(-euler.y, Vector3.up);
                //Note different directions
                var zRot = Quaternion.AngleAxis(-euler.z, Vector3.back);

                var XYZeuler = (xRot * yRot * zRot).eulerAngles;

                context.Container.Rotation = new[]
                {
                    XYZeuler.x * Mathf.Deg2Rad,
                    XYZeuler.y * Mathf.Deg2Rad,
                    XYZeuler.z * Mathf.Deg2Rad
                };

                context.Container.Scale = new[]
                    {transform.m_LocalScale.x, transform.m_LocalScale.y, transform.m_LocalScale.z};
            }
        }
    }
}