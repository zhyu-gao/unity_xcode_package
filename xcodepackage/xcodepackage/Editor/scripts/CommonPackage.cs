using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using cn.newvision.sdkporter;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class CommonPackage : SDKPorterBase
{
    public CommonPackage(XcodePackageManager manager)
    {
        this.manager = manager;
        manager.Register(this);
    }

    public override void EditInfoPlist(XCPlist plist)
    {
        string permission = @"  
            <key>NSCameraUsageDescription</key>
            <string>本应用需要您的同意才能使用您的摄像头</string>
            <key>NSLocationWhenInUseUsageDescription</key>
            <string>本应用需要您的同意才能使用您的位置信息</string>
			<key>NSPhotoLibraryUsageDescription</key>
            <string>本应用需要您的同意才能使用您的照片库</string>
			<key>NSMicrophoneUsageDescription</key>
            <string>本应用需要您的同意才能使用您的麦克风</string>
            <key>NSContactsUsageDescription</key>
            <string>本应用需要您的同意才能使用您的联系人</string>
            <key>NSPhotoLibraryAddUsageDescription</key>
            <string>本应用需要您的同意才能添加照片</string>
            ";
        plist.AddKey(permission);
    }

    public override void EditInfoPlistWithUnity(PlistElementDict rootDict)
    {
    }

    public override void EditProjectSettingWithUnity(PBXProject pbxProject)
    {
        //bitcode disable
        string bitTarget = pbxProject.TargetGuidByName("Unity-iPhone");
        pbxProject.SetBuildProperty(bitTarget, "ENABLE_BITCODE", "NO");
    }

    public override void EditProjectSetting(XCProject project)
    {
    }

    public override void EditScriptCode(string projPath)
    {
    }

    public override void EditEmbedFramework()
    {
    }
}