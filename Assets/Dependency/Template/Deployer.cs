using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface IDeployer
{
    /// <summary>
    /// Deploys template by path, returns full path to index.html
    /// </summary>
    /// <param name="source">Source path to template</param>
    /// <returns> path to index.html </returns>
    string Deploy(string SourcePath);
}

public class Deployer : IDeployer
{
    private string DestFolder;

    public Deployer(string destFolder)
    {
        DestFolder = destFolder;
    }


    private string DestinationPath
    {
        get
        {
            var dataPath = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(dataPath, DestFolder);
        }
    }

    private void Cleanup()
    {
        if (Directory.Exists(DestinationPath))
        {
            Directory.Delete(DestinationPath, true);
        }

        Directory.CreateDirectory(DestinationPath);
    }

    public string Deploy(string SourcePath)
    {
        Cleanup();

        //copy folder structure
        foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
            SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

        //gather code files
        List<string> files = Directory.GetFiles(SourcePath, "*.html", SearchOption.AllDirectories).ToList();
        files.AddRange(Directory.GetFiles(SourcePath, "*.js", SearchOption.AllDirectories));
        
        //copy with replace files
        foreach (string newPath in files)
            File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

        //return path to index
        return Path.Combine(DestinationPath ,@"index.html");
    }
}
