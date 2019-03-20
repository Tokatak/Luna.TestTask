using UnityEngine;
using Transform = UnityYaml.Components.Transform;

namespace TreeJs.Adapters
{
    [TreeJsInterpreter(target: typeof(Transform))]
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

                float angle = 0.0F;
                Vector3 axis = Vector3.zero;
                orientation.ToAngleAxis(out angle, out axis);
                axis.x *= -1; //invese x rotation
                axis.y *= -1; // inverse y rotation
                orientation = Quaternion.AngleAxis(angle, axis);

                context.Container.Quaternion = new[]
                {
                    orientation.x,
                    orientation.y,
                    orientation.z,
                    orientation.w
                };

                context.Container.Scale = new[]
                    {transform.m_LocalScale.x, transform.m_LocalScale.y, transform.m_LocalScale.z};
            }
        }
    }
}