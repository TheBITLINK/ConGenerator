using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace CGApi
{
    public partial class SpecialFolder : Component
    {
        public FileInfo[] Files
        {
            get
            {
                return dI.GetFiles();
            }
        }

        private FileInfo desktop_ini { get; set; }
        private DirectoryInfo dI { get; set; }
        public SpecialFolder(string Location)
        {
            dI = new DirectoryInfo(Location);
            string FullName = dI.Name;
            if (!FullName.StartsWith("spf_"))
            {
                throw new ArgumentException("Not a valid Special Folder.");
            }
            if (!dI.Exists)
            {
                throw new ArgumentException("Not a valid Special Folder.");
            }
            foreach (FileInfo file in Files)
            {
                if (file.Name == "desktop.ini")
                {
                    desktop_ini = file;
                }
            }
        }

        public SpecialFolder(DirectoryInfo Folder)
        {
            dI = Folder;
            string FullName = dI.Name;
            if (!FullName.StartsWith("spf_"))
            {
                throw new ArgumentException("Not a valid Special Folder.");
            }
            if (!dI.Exists)
            {
                throw new ArgumentException("Not a valid Special Folder.");
            }
        }

        /// <summary>
        /// Gets Special Folder ID.
        /// </summary>
        /// 
        /// <returns>
        /// Special Folder ID.
        /// </returns>
        public int GetID()
        {
            string FullName = dI.Name;
            if (FullName.StartsWith("spf_"))
            {
                string ID = FullName.Remove(0, "spf_".Length);
                return Convert.ToInt32(ID);
            }
            return 0;
        }

        public void Delete()
        {
            dI.Delete();
        }

        public void Rename(string newname)
        {
            FileInfo magic = new FileInfo(dI.FullName + @"\desktop.ini");
            FileAttributes fa = magic.Attributes;
            FileStream fs = new FileStream(magic.FullName, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(@"[" + @".ShellClassInfo" + @"]");
            sw.WriteLine(@"LocalizedResourceName=" + newname);
            sw.Flush();
            sw.Close();
            fs.Close();
            FileInfo mer = new FileInfo(dI.FullName + @"\desktop.ini");
            mer.Attributes = FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.System;
            Common.LPSHFOLDERCUSTOMSETTINGS FolderSettings = new Common.LPSHFOLDERCUSTOMSETTINGS();
            FolderSettings.dwMask = 0x10;
            FolderSettings.pszIconFile = @"X:\Windows\system32\SHELL32.dll";
            FolderSettings.iIconIndex = 3;

            UInt32 FCS_READ = 0x00000001;
            UInt32 FCS_FORCEWRITE = 0x00000002;
            UInt32 FCS_WRITE = FCS_READ | FCS_FORCEWRITE;

            string pszPath = dI.FullName;
            UInt32 HRESULT = Common.SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_WRITE);
            Common.RefreshIconCache();
        }

        /// <summary>
        /// Gets Special Folder Icon.
        /// </summary>
        /// 
        /// <returns>
        /// Special Folder Icon.
        /// </returns>
        public Icon GetIcon()
        {
            try
            {
                string[] lines = File.ReadAllLines(desktop_ini.FullName);
                foreach (string line in lines)
                {
                    if (line.StartsWith("IconResource="))
                    {
                        string iconpath = line.Remove(0, "IconResource=".Length);
                        if (iconpath.EndsWith("SHELL32.dll,3")) 
                        {
                            return cgres.spfld;
                        }
                        Icon icn = new Icon(iconpath);
                        return icn;
                    }
                }
            }
            catch { }
            return cgres.spfld;
        }

        /// <summary>
        /// Gets Special Folder Name.
        /// </summary>
        /// 
        /// <returns>
        /// Special Folder Name.
        /// </returns>
        public string GetName()
        {
            try
            {
                string[] lines = File.ReadAllLines(desktop_ini.FullName);
                foreach (string line in lines)
                {
                    if (line.StartsWith("LocalizedResourceName="))
                    {
                        return line.Remove(0, "LocalizedResourceName=".Length);
                    }
                }
            }
            catch { }
            return dI.Name;
        }

        protected char[] LineSeparator = new char[2]
    {
      '\r',
      '\n'
    };
    }
}
