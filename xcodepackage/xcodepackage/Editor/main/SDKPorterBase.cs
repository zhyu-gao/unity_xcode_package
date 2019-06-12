using System.Collections;
using System.Collections.Generic;
using cn.newvision.sdkporter;
using UnityEngine;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public abstract class SDKPorterBase
{
    protected XcodePackageManager manager;

    public List<string> embedFrameworks = new List<string>();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="plist"></param>
    public abstract void EditInfoPlist(XCPlist plist);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rootDict"></param>
    public abstract void EditInfoPlistWithUnity(PlistElementDict rootDict);

    public abstract void EditProjectSettingWithUnity(PBXProject pbxProject);

    public abstract void EditProjectSetting(XCProject project);

    public abstract void EditScriptCode(string projPath);

    public abstract void EditEmbedFramework();

}