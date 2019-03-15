using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneReader
{
   string Read(Scene scene);
}

public class SceneReader : ISceneReader
{
  public string Read(Scene scene)
  {
      var path = scene.path;

      var dataPath = Directory.GetParent(Application.dataPath).FullName;
      var fullPath = Path.Combine(dataPath, path);
      Debug.Log($"Scene path {fullPath}");

      string rawSceneContent;
      using (var stream = new StreamReader(fullPath))
      {
          rawSceneContent = stream.ReadToEnd();
          Debug.Log(rawSceneContent);
      }

      return rawSceneContent;
  }
}
