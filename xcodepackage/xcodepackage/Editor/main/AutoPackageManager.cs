using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using cn.newvision.sdkporter;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
#define UNITY_EDITOR
public static class AutoPackageManager
{
    /// <summary>
    /// 
    /// </summary>
    private static XcodePackageManager packageManager;

    public static string BuiltProjectPath;
#if UNITY_EDITOR
    [PostProcessBuild(100)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        packageManager = new XcodePackageManager();
        OtherPackageManager.InitOtherPackage(packageManager);
        BuiltProjectPath = pathToBuiltProject;
        if (target != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        EditProjectSetting(pathToBuiltProject);

        EditInfoPlist(pathToBuiltProject);

        EditInfoPlistWithUnity(pathToBuiltProject);

        EditProjectSettingWithUnity(target, pathToBuiltProject);

        EditEmbedFramework(target, pathToBuiltProject);
    }

    private static void EditEmbedFramework(BuildTarget target, string pathToBuiltProject)
    {
        packageManager.EditEmbedFramework();
        string projPath =
            UnityEditor.iOS.Xcode.Custom.PBXProject.GetPBXProjectPath(pathToBuiltProject);
        UnityEditor.iOS.Xcode.Custom.PBXProject proj = new UnityEditor.iOS.Xcode.Custom.PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));
        string targetGuid = proj.TargetGuidByName("Unity-iPhone");
        foreach (var embedFrameworks in packageManager.getEmbedFrameworks)
        {
            foreach (var framework in embedFrameworks)
            {
                string fileGuid = proj.AddFile(framework, "Frameworks/" + framework,
                    UnityEditor.iOS.Xcode.Custom.PBXSourceTree.Build);
                UnityEditor.iOS.Xcode.Custom.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid,
                    fileGuid);
                proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
            }
        }
        
        proj.WriteToFile(projPath);
    }

    private static void AddEmbedFramework(string absoluteFilePath)
    {
        string projPath =
            UnityEditor.iOS.Xcode.Custom.PBXProject.GetPBXProjectPath(AutoPackageManager.BuiltProjectPath);
        UnityEditor.iOS.Xcode.Custom.PBXProject proj = new UnityEditor.iOS.Xcode.Custom.PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));
        string targetGuid = proj.TargetGuidByName("Unity-iPhone");
        string fileGuid = proj.AddFile(absoluteFilePath, "Frameworks/" + absoluteFilePath,
            UnityEditor.iOS.Xcode.Custom.PBXSourceTree.Build);
        UnityEditor.iOS.Xcode.Custom.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid,
            fileGuid);
        proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
        proj.WriteToFile(projPath);
    }


    /// <summary>
    /// 编辑project设置
    /// </summary>
    /// <param name="pathToBuiltProject"></param>
    private static void EditProjectSetting(string pathToBuiltProject)
    {
        string unityEditorAssetPath = Application.dataPath;
        XCProject project = new XCProject(pathToBuiltProject);

        //add libraries to search setting
        PBXDictionary<XCBuildConfiguration> buids = project.buildConfigurations;

        foreach (KeyValuePair<string, XCBuildConfiguration> buildConfig in buids)
        {
            buildConfig.Value.AddLibrarySearchPaths(new PBXList(Path.GetFullPath(pathToBuiltProject) + "/Libraries"),
                false);
        }
        //end

        packageManager.EditProjectSetting(project);

        /*********** get projmods config ********/
        var curScriptFiles = Directory.GetFiles(unityEditorAssetPath, "OtherPackageManager.cs",
            SearchOption.AllDirectories);
        foreach (var scriptFile in curScriptFiles)
        {
            string lastPath = scriptFile.Substring(0, scriptFile.LastIndexOf("/", StringComparison.Ordinal));
            var files = Directory.GetFiles(lastPath + "/configs", "*.projmods",
                SearchOption.AllDirectories);
            foreach (var file in files)
            {
                project.ApplyMod(file);
            }
        }

        project.Save();
    }

    /// <summary>
    /// 用unity自带的打包api设置info.plist
    /// </summary>
    /// <param name="pathToBuiltProject"></param>
    private static void EditInfoPlistWithUnity(string pathToBuiltProject)
    {
        string plistPath = pathToBuiltProject + "/Info.plist";
        UnityEditor.iOS.Xcode.Custom.PlistDocument plist = new UnityEditor.iOS.Xcode.Custom.PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        UnityEditor.iOS.Xcode.Custom.PlistElementDict rootDict = plist.root;
        packageManager.EditInfoPlistWithUnity(rootDict);
        plist.WriteToFile(plistPath);
    }

    /// <summary>
    /// 修改info.plist文件
    /// </summary>
    /// <param name="projPath"></param>
    private static void EditInfoPlist(string pathToBuiltProject)
    {
        string projPath = Path.GetFullPath(pathToBuiltProject);
        XCPlist plist = new XCPlist(projPath);
        packageManager.EditInfoPlist(plist);
        packageManager.EditScriptCode(projPath);
        plist.Save();
    }

    /// <summary>
    /// 用unity自带的api修改xcode设置
    /// </summary>
    /// <param name="target"></param>
    /// <param name="pathToBuiltProject"></param>
    private static void EditProjectSettingWithUnity(BuildTarget target, string pathToBuiltProject)
    {
        string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        packageManager.EditProjectSettingWithUnity(pbxProject);
        pbxProject.WriteToFile(projectPath);
    }
#endif
}