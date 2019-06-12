using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cn.newvision.sdkporter;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class BlueToothPackage : SDKPorterBase {

	public BlueToothPackage(XcodePackageManager manager)
	{
		this.manager = manager;
		manager.Register(this);
	}

	public override void EditInfoPlist(XCPlist plist)
	{
	}

	public override void EditInfoPlistWithUnity(PlistElementDict rootDict)
	{
	}

	public override void EditProjectSettingWithUnity(PBXProject pbxProject)
	{
		///swift
		string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
		pbxProject.SetBuildProperty(targetGuid, "SWIFT_VERSION", "Swift5");
		pbxProject.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER",
			"Libraries/Plugins/iOS/Unity-iPhone-Bridging-Header.h");
	}

	public override void EditProjectSetting(XCProject project)
	{
	}

	public override void EditScriptCode(string projPath)
	{
	}

	public override void EditEmbedFramework()
	{
		embedFrameworks.Add(AutoPackageManager.BuiltProjectPath+"/ble/ZIPFoundation.framework");
		embedFrameworks.Add(AutoPackageManager.BuiltProjectPath+"/ble/iOSDFULibrary.framework");
	}
}
