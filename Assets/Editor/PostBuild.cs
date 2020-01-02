using UnityEditor;
using System.IO;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor.Build.Reporting;

class PostBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log(report.summary.outputPath);
        FileInfo info = new FileInfo(report.summary.outputPath);
        DirectoryInfo buildTargetDirectory = info.Directory;
        DirectoryInfo buildAssetsDirectory = buildTargetDirectory.CreateSubdirectory("Assets");
        DirectoryInfo buildModulesDirectory = buildAssetsDirectory.CreateSubdirectory("Modules");

        FileLoader.CopyDirectory(FileLoader.ModulePath,buildModulesDirectory.FullName,true);

        DirectoryInfo buildSettingsDirectory = buildAssetsDirectory.CreateSubdirectory("Settings");

        FileLoader.CopyDirectory(FileLoader.SettingsPath,buildSettingsDirectory.FullName,true);

        Debug.Log("Copied Modules Directory");
    }
}