using UnityEngine;
using System.Collections;
using cn.newvision.sdkporter;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;
using UnityEditor.iOS.Xcode.Custom;

public class AdaptiPhoneX : SDKPorterBase {

	public AdaptiPhoneX(XcodePackageManager manager)
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
		XClass UnityAppController = new XClass(projPath + "/Classes/Unity/DisplayManager.mm");
        UnityAppController.WriteBelow("[window addSubview: view];"
                                      + "\n            [window makeKeyAndVisible];"
                                      + "\n        }"
                                      + "\n    }", "\n */");
        UnityAppController.WriteBelow("#include \"UnityMetalSupport.h\"", "#import <sys/utsname.h>");
        UnityAppController.WriteBelow("[self createView: useForRendering showRightAway: YES];\n}",
            "- (NSString*)getDeviceVersion"
            + "\n{" +
            "struct utsname systemInfo;"
            + "uname(&systemInfo);"
            + "NSString *deviceVersion = [NSString stringWithCString:systemInfo.machine encoding:NSUTF8StringEncoding];"
            + "NSLog(@\"添加获取手机型号方法 ++ %@\", deviceVersion);"
            + "return deviceVersion;"
            + "\n}"
            + "- (void)updateScreenSize"
            + "\n{"
            + "CGSize layerSize    = _view.layer.bounds.size;"
            + "NSString *deviceVersion = [self getDeviceVersion];"
            + "if ([deviceVersion isEqualToString:@\"iPhone10,3\"] || [deviceVersion isEqualToString:@\"iPhone10,6\"])"
            + "{"
            + "    layerSize = CGSizeMake(744, 375);"
            + "}"
            + "CGFloat scale       = UnityScreenScaleFactor(_screen);"
            + "_screenSize = CGSizeMake(layerSize.width * scale, layerSize.height * scale);"
            + " }"
        );
        UnityAppController.WriteBelow("- (void)createView:(BOOL)useForRendering showRightAway:(BOOL)showRightAway;"
                                      + "\n{"
            , "if(_view == nil)"
              + "{"
              + "NSString *deviceVersion = [self getDeviceVersion];"
              + "if ([deviceVersion isEqualToString:@\"iPhone10,3\"] || [deviceVersion isEqualToString:@\"iPhone10,6\"])"
              + "{"
              + "   CGRect bounds = CGRectMake(34, 0, 744, 375);"
              + "   _window  = [[UIWindow alloc] initWithFrame:bounds];"
              + "}"
              + "else"
              + "{"
              + "   _window = [[UIWindow alloc] initWithFrame: _screen.bounds];"
              + "}"
              + "_window.screen = _screen;"
              + "if(_screen == [UIScreen mainScreen])"
              + "{"
              + "  _view = [[GetAppController() init] unityView];"
              + "   NSAssert([_view isKindOfClass:[UnityView class]], @\"You MUST use UnityView subclass as unity view\");"
              + "}"
              + "else"
              + "{"
              + "   _view = [(useForRendering ? [UnityRenderingView alloc] : [UIView alloc]) initWithFrame: _screen.bounds];"
              + "}"
              + "     _view.contentScaleFactor = UnityScreenScaleFactor(self.screen);"
              + "  [self updateScreenSize];"
              + "if(showRightAway)"
              + "{"
              + "    [_window addSubview:_view];"
              + "   [_window makeKeyAndVisible];"
              + "}"
              + "}" +
              "\n /*");
        
	}

	public override void EditEmbedFramework()
	{
	}
}
