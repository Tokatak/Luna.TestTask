﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDeployer
{
    /// <summary>
    /// Deploys template by path
    /// </summary>
    /// <param name="source">Source path to template</param>
    /// <returns> path deploy root </returns>
    string Deploy(string SourcePath);
}

public class Deployer : IDeployer
{
    private static readonly string[] DeployFormats = {"*.html","*.js"};
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

        //gather deploy files
        List<string> files = new List<string>();
        foreach (string deployFormat in DeployFormats)
        {
            files.AddRange(Directory.GetFiles(SourcePath, deployFormat , SearchOption.AllDirectories));
        }
        
        //copy with replace files
        foreach (string newPath in files)
            File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

        //return path to index
        return DestinationPath;
    }
}
