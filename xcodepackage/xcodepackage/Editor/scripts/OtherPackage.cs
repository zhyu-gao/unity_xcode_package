using UnityEngine;
using System.Collections;
using cn.newvision.sdkporter;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class OtherPackage : SDKPorterBase
{
    public OtherPackage(XcodePackageManager manager)
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
    }

    public override void EditProjectSetting(XCProject project)
    {
    }

    public override void EditScriptCode(string projPath)
    {
        
        /****************/
        //暂时不知道具体作用,等待归档
        /****************/
        //读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(projPath + "/Classes/UnityAppController.mm");

        //在指定代码后面增加一行代码
        UnityAppController.WriteBelow("#import <OpenGLES/ES2/glext.h>", "#import \"WXApiManager.h\"");

        //在指定代码中替换一行
        UnityAppController.Replace("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n    return YES;",
            "AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n\treturn [WXApi handleOpenURL:url delegate:[WXApiManager sharedManager]];");

        UnityAppController.WriteBelow("[KeyboardDelegate Initialize];",
            "[WXApi registerApp:@\"wx031f3469ef4d952b\"];\n    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;\n    \n    [WXApi registerAppSupportContentFlag:typeFlag];");
        UnityAppController.WriteBelow("_didResignActive = false;", "[[WXApiManager sharedManager] test];");
    }

    public override void EditEmbedFramework()
    {
    }
}