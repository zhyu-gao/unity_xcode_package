using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPackageManager
{
    public static void InitOtherPackage(XcodePackageManager manager)
    {
        new ShareSDKPackage(manager);
        new CommonPackage(manager);
        new BlueToothPackage(manager);
        new OtherPackage(manager);
        new AdaptiPhoneX(manager);
    }
}