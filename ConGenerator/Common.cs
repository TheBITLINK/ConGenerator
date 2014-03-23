using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConGenerator
{
    public static class Common
    {
        public static void RunOnUIDespatcher(Action action, Window that)
        {
            if (that.Dispatcher.CheckAccess())
                action();
            else
                that.Dispatcher.BeginInvoke((Delegate)action, (object[])null);
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]

        public static extern IntPtr SendMessageTimeout(

            int windowHandle,

            int Msg,

            int wParam,

            String lParam,

            SendMessageTimeoutFlags flags,

            int timeout,

            out int result);

        [Flags]

        public enum SendMessageTimeoutFlags : uint
        {

            SMTO_NORMAL = 0x0,

            SMTO_BLOCK = 0x1,

            SMTO_ABORTIFHUNG = 0x2,

            SMTO_NOTIMEOUTIFNOTHUNG = 0x8

        }



        public static void RefreshIconCache()
        {

            // get the the original Shell Icon Size registry string value

            RegistryKey k = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);

            Object OriginalIconSize = k.GetValue("Shell Icon Size");



            // set the Shell Icon Size registry string value

            k.SetValue("Shell Icon Size", (Convert.ToInt32(OriginalIconSize) + 1).ToString());

            k.Flush(); k.Close();



            // broadcast WM_SETTINGCHANGE to all window handles

            int res = 0;

            SendMessageTimeout(0xffff, 0x001A, 0, "", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5000, out res);

            //SendMessageTimeout(HWD_BROADCAST,WM_SETTINGCHANGE,0,"",SMTO_ABORTIFHUNG,5 seconds, return result to res)



            // set the Shell Icon Size registry string value to original value

            k = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);

            k.SetValue("Shell Icon Size", OriginalIconSize);

            k.Flush(); k.Close();



            SendMessageTimeout(0xffff, 0x001A, 0, "", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5000, out res);



        }
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern UInt32 SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, UInt32 dwReadWrite);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LPSHFOLDERCUSTOMSETTINGS
        {
            public UInt32 dwSize;
            public UInt32 dwMask;
            public IntPtr pvid;
            public string pszWebViewTemplate;
            public UInt32 cchWebViewTemplate;
            public string pszWebViewTemplateVersion;
            public string pszInfoTip;
            public UInt32 cchInfoTip;
            public IntPtr pclsid;
            public UInt32 dwFlags;
            public string pszIconFile;
            public UInt32 cchIconFile;
            public int iIconIndex;
            public string pszLogo;
            public UInt32 cchLogo;
        }

    }
}
