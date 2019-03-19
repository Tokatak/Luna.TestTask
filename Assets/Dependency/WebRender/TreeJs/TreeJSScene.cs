using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dependency.Template
{
    public class TreeJSScene
    {
        [JsonProperty("metadata")] public Metadata Metadata { get; set; } = new Metadata();

        [JsonProperty("urlBaseType")] public string UrlBaseType { get; set; } = "relativeToScene";

        [JsonProperty("objects")]
        public Dictionary<string, object> Objects { get; set; } = new Dictionary<string, object>();

        [JsonProperty("geometries")]
        public Dictionary<string, object> Geometries { get; set; } = new Dictionary<string, object>();

        [JsonProperty("materials")]
        public Dictionary<string, object> Materials { get; set; } = new Dictionary<string, object>();

        [JsonProperty("textures")] public Fogs Textures { get; set; } = new Fogs();

        [JsonProperty("fogs")] public Fogs Fogs { get; set; } = new Fogs();

        [JsonProperty("transform")] public Transform Transform { get; set; } = new Transform();

        [JsonProperty("defaults")] public Defaults Defaults { get; set; } = new Defaults();
    }

    public partial class Metadata
    {
        [JsonProperty("formatVersion")] public double FormatVersion { get; set; } = 3.2;

        [JsonProperty("type")] public string Type { get; set; } = "scene";

        [JsonProperty("generatedBy")] public string GeneratedBy { get; set; } = "SceneExporter";

        [JsonProperty("objects")] public long Objects { get; set; }

        [JsonProperty("geometries")] public long Geometries { get; set; }

        [JsonProperty("materials")] public long Materials { get; set; }

        [JsonProperty("textures")] public long Textures { get; set; }
    }

    public partial class Fogs
    {
    }

    public partial class Transform
    {
        [JsonProperty("position")] public float[] Position { get; set; } = {0f, 0f, 0f};

        [JsonProperty("rotation")] public float[] Rotation { get; set; } = {0f, 0f, 0f};

        [JsonProperty("scale")] public float[] Scale { get; set; } = {1f, 1f, 1f};
    }

    public partial class Defaults
    {
        [JsonProperty("camera")] public string Camera { get; set; } = "";
        [JsonProperty("fog")] public string Fog { get; set; } = "";
    }

    public class GeometryCube
    {
        [JsonProperty("type")] public string Type { get; set; } = "cube";
        [JsonProperty("width")] public long Width { get; set; } = 1;
        [JsonProperty("height")] public long Height { get; set; } = 1;
        [JsonProperty("depth")] public long Depth { get; set; } = 1;
        [JsonProperty("widthSegments")] public long WidthSegments { get; set; } = 1;
        [JsonProperty("heightSegments")] public long HeightSegments { get; set; } = 1;
        [JsonProperty("depthSegments")] public long DepthSegments { get; set; } = 1;
    }

    public partial class MeshLambertMaterial
    {
        [JsonProperty("type")] public string Type { get; set; } = "MeshLambertMaterial";
        [JsonProperty("parameters")] public Parameters Parameters { get; set; } = new Parameters();
    }

    public partial class Parameters
    {
        [JsonProperty("color")] public long Color { get; set; } = 16777215;
        [JsonProperty("ambient")] public long Ambient { get; set; } = 16777215;
        [JsonProperty("emissive")] public long Emissive { get; set; } = 0;
        [JsonProperty("reflectivity")] public long Reflectivity { get; set; } = 1;
        [JsonProperty("transparent")] public bool Transparent { get; set; } = false;
        [JsonProperty("opacity")] public long Opacity { get; set; } = 1;
        [JsonProperty("wireframe")] public bool Wireframe { get; set; } = false;
        [JsonProperty("wireframeLinewidth")] public long WireframeLinewidth { get; set; } = 1;
    }

    public class TreeJSObject
    {
        [JsonProperty("position")] public float[] Position { get; set; } = {0, 0, 0};
        [JsonProperty("rotation")] public float[] Rotation { get; set; } = {0, 0, 0};
        [JsonProperty("scale")] public float[] Scale { get; set; } = {1, 1, 1};
        [JsonProperty("visible")] public bool Visible { get; set; } = true;

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Children { get; set; } = new Dictionary<string, object>();
    }

    public class GeometryTreeJsObject : TreeJSObject
    {
        [JsonProperty("geometry")] public string Geometry { get; set; }
        [JsonProperty("material")] public string Material { get; set; }
    }

    public class PerspectiveCamera
    {
        [JsonProperty("type")] public string Type { get; set; } = "PerspectiveCamera";

        [JsonProperty("fov")] public float Fov { get; set; } = 60f;

        [JsonProperty("aspect")] public float Aspect { get; set; } = 16 / 9f;

        [JsonProperty("near")] public float Near { get; set; } = 0.1f;

        [JsonProperty("far")] public float Far { get; set; } = 1000f;

        [JsonProperty("position")] public float[] Position { get; set; } = {0f, 0f, 0f};

        [JsonProperty("rotation")] public float[] Rotation { get; set; } = {0f, 0f, 0f};
    }

    public class DirectionalLight
    {
        [JsonProperty("type")] public string Type { get; set; } = "DirectionalLight";

        [JsonProperty("color")] public long Color { get; set; } = 16777215;

        [JsonProperty("intensity")] public float Intensity { get; set; } = 1;

        [JsonProperty("direction")] public float[] Direction { get; set; } = new float[] {10, 10, 10};
    }
}