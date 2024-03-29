﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace T_Rec.Helpers
{
    public static class ExtensionHelper
    {
        public static object FindResource(string resourceKey)
        {
            if (string.IsNullOrEmpty(resourceKey))
                return null;

            return Application.Current.Resources.FirstOrDefault(r => r.Key == resourceKey);
        }

        public static T GetPlatformValue<T>(OnPlatform<T> onplatformObjects) where T : class
        {
            TargetPlatform currentOs = Device.OS;
            T obj = onplatformObjects == null
                                    ? null
                                    : currentOs == TargetPlatform.Android
                                        ? onplatformObjects.Android
                                            : currentOs == TargetPlatform.iOS
                                                ? onplatformObjects.iOS
                                                    : onplatformObjects.WinPhone;
            return obj;

        }

        public static string FindOnPlatFormStringResource(string resourceKey)
        {
            return (string.IsNullOrEmpty(resourceKey)) ? null
                        : GetPlatformValue<string>(FindResource(resourceKey)
                            as OnPlatform<string>);
        }
    }
}
