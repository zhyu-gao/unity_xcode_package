using System.Collections.Generic;
using cn.newvision.sdkporter;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class ShareSDKPackage : SDKPorterBase
{
    public ShareSDKPackage(XcodePackageManager manager)
    {
        this.manager = manager;
        manager.Register(this);
    }

    public override void EditInfoPlist(XCPlist plist)
    {
        string AppKey = @"<key>MOBAppkey</key> <string>" + "appid" + "</string>";
        string AppSecret = @"<key>MOBAppSecret</key> <string>" + "screctkey" + "</string>";
        plist.AddKey(AppKey);
        plist.AddKey(AppSecret);
        string wechatURL = @"  
            <key>CFBundleURLTypes</key>
            <array>
            <dict>
            <key>CFBundleTypeRole</key>
            <string>Editor</string>
            <key>CFBundleURLSchemes</key>
            <array>
            <string>" + "wechatappid" + @"</string>
            </array>
            </dict>
			<dict>
			<key>CFBundleTypeRole</key>
			<string>Editor</string>
			<key>CFBundleURLSchemes</key>
			<array>
			<string>QQ41E81479</string>
			</array>
			</dict>
            </array>";
        plist.AddKey(wechatURL);
        string thirdAllow = @"
        <key>LSApplicationQueriesSchemes</key>
        <array>
        <string>wechat</string>
        <string>weixin</string>
        <string>mqqOpensdkSSoLogin</string>
        <string>mqqopensdkapiV2</string>
        <string>mqqopensdkapiV3</string>
        <string>wtloginmqq2</string>
        <string>mqq</string>
        <string>mqqapi</string>
        <string>mqzoneopensdk</string>
        <string>mqzoneopensdkapi</string>
        <string>mqzoneopensdkapi19</string>
        <string>mqzoneopensdkapiV2</string>
        <string>mqqwpa</string>
        <string>mqzone</string>
        </array>";
        plist.AddKey(thirdAllow);
    }

    public override void EditInfoPlistWithUnity(PlistElementDict rootDict)
    {
        rootDict.SetString("CFBundleDevelopmentRegion", "zh_CN");
    }

    public override void EditProjectSettingWithUnity(PBXProject pbxProject)
    {
    }

    public override void EditProjectSetting(XCProject project)
    {
    }

    public override void EditScriptCode(string projPath)
    {
        XClass UnityAppController = new XClass(projPath + "/Classes/UnityAppController.mm");
        UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"",
            "#import <ShareSDK/ShareSDK.h>\n#import <ShareSDKConnector/ShareSDKConnector.h>\n" +
            "#import \"WXApi.h\"\n");
        UnityAppController.WriteBelow("::printf(\"-> applicationDidFinishLaunching()\\n\");",
            "[ShareSDK registerActivePlatforms:\n@[\n@(SSDKPlatformTypeWechat)]\nonImport:^ (SSDKPlatformType platformType)\n" +
            "{\nswitch (platformType)\n{\ncase SSDKPlatformTypeWechat:\n[ShareSDKConnector connectWeChat:[WXApi class]];\nbreak;" +
            "\t\ndefault:\nbreak;\n}\n}\nonConfiguration:^(SSDKPlatformType platformType, NSMutableDictionary * appInfo) \n" +
            "{\nswitch (platformType)\n{\ncase SSDKPlatformTypeWechat:\n" +
            "[appInfo SSDKSetupWeChatByAppId:@\"" + "wechatappid" + "\"\nappSecret:@\"" +
            "wechatsecrectkey" + "\"];\nbreak;\n" +
            "\n}\n}];");
        UnityAppController.WriteBelow("::printf(\"-> applicationDidEnterBackground()\\n\");\n}",
            "\n - (BOOL)application:(UIApplication *)app\n            openURL:(NSURL *)url\n            " +
            "options:(NSDictionary<NSString *,id> *)options\n{\n    " +
            "return [WXApi handleOpenURL:url delegate:[WXApiManager sharedManager]];\n}");
    }

    public override void EditEmbedFramework()
    {
        
    }
}