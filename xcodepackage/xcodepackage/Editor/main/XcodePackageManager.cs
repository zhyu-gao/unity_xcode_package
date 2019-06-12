using System.Collections;
using System.Collections.Generic;
using cn.newvision.sdkporter;
using UnityEngine;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class XcodePackageManager
{
    public static List<SDKPorterBase> porterList = new List<SDKPorterBase>();

    private List<List<string>> embedFrameworks = new List<List<string>>();

    public List<List<string>> getEmbedFrameworks
    {
        get
        {
            foreach (var porterBase in porterList)
            {
                embedFrameworks.Add(porterBase.embedFrameworks);
            }

            return embedFrameworks;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sdkPorterBase"></param>
    public void Register(SDKPorterBase sdkPorterBase)
    {
        if (porterList.Contains(sdkPorterBase))
            return;
        porterList.Add(sdkPorterBase);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sdkPorterBase"></param>
    public void UnRegister(SDKPorterBase sdkPorterBase)
    {
        porterList.Remove(sdkPorterBase);
    }

    public void EditInfoPlist(XCPlist plist)
    {
        foreach (var porter in porterList)
        {
            porter.EditInfoPlist(plist);
        }
    }

    public void EditInfoPlistWithUnity(PlistElementDict rootDict)
    {
        foreach (var porter in porterList)
        {
            porter.EditInfoPlistWithUnity(rootDict);
        }
    }

    public void EditProjectSettingWithUnity(PBXProject pbxProject)
    {
        foreach (var porter in porterList)
        {
            porter.EditProjectSettingWithUnity(pbxProject);
        }
    }

    public void EditProjectSetting(XCProject project)
    {
        foreach (var porter in porterList)
        {
            porter.EditProjectSetting(project);
        }
    }

    public void EditScriptCode(string projPath)
    {
        foreach (var porter in porterList)
        {
            porter.EditScriptCode(projPath);
        }
    }

    public void EditEmbedFramework()
    {
        foreach (var porter in porterList)
        {
            porter.EditEmbedFramework(); 
        }
    }
}