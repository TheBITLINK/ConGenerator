using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CGApi
{
    public static class SpecialFolders
    {
        public static SpecialFolder[] GetSpecialFolderByDirectory(string path)
        {
            string[] Directories = Directory.GetDirectories(path);
            //I'm a noob at arrays, so here's a low-budget array return (:
            int count = 0;
            foreach (string directory in Directories)
            {
                string pr = "";
                if (directory.StartsWith("\\"))
                {
                    pr = Environment.GetEnvironmentVariable("HOMEDRIVE");
                }
                DirectoryInfo df = new DirectoryInfo(pr + directory);
                if (df.Name.StartsWith("spf_"))
                {
                    count++;
                }
            }
            SpecialFolder[] SPFA = new SpecialFolder[count];
            count = 0;
            foreach (string directory in Directories)
            {
                string pr = "";
                if (directory.StartsWith("\\"))
                {
                    pr = Environment.GetEnvironmentVariable("HOMEDRIVE");
                }
                DirectoryInfo df = new DirectoryInfo(pr + directory);
                if (df.Name.StartsWith("spf_"))
                {
                    SPFA[count++] = new SpecialFolder(directory);
                }
            }
            return SPFA;
        }

        public static SpecialFolder[] GetSpecialFolderByDirectory(DirectoryInfo directoyInfo)
        {
            string[] Directories = Directory.GetDirectories(directoyInfo.FullName);
            //This is a copy-paste, so here's the same low-budget array return (:
            int count = 0;
            foreach (string directory in Directories)
            {
                string pr = "";
                if (directory.StartsWith("\\"))
                {
                    pr = Environment.GetEnvironmentVariable("HOMEDRIVE");
                }
                DirectoryInfo df = new DirectoryInfo(pr + directory);
                if (df.Name.StartsWith("spf_"))
                {
                    count++;
                }
            }
            SpecialFolder[] SPFA = new SpecialFolder[count];
            count = 0;
            foreach (string directory in Directories)
            {
                string pr = "";
                if (directory.StartsWith("\\"))
                {
                    pr = Environment.GetEnvironmentVariable("HOMEDRIVE");
                }
                DirectoryInfo df = new DirectoryInfo(pr + directory);
                if (df.Name.StartsWith("spf_"))
                {
                    SPFA[count++] = new SpecialFolder(directory);
                }
            }
            return SPFA;
        }

        public static string GetName(string path)
        {
            DirectoryInfo dI = new DirectoryInfo(path);
            FileInfo desktop_ini = null;
            try
            {
                FileInfo[] Files = dI.GetFiles();
                foreach (FileInfo file in Files)
                {
                    if (file.Name == "desktop.ini")
                    {
                        desktop_ini = file;
                    }
                }
                string[] lines = File.ReadAllLines(desktop_ini.FullName);
                foreach (string line in lines)
                {
                    if (line.StartsWith("LocalizedResourceName="))
                    {
                        string sn = line.Remove(0, "LocalizedResourceName=".Length);
                        if (sn.StartsWith("@")) 
                        {
                            return dI.Name;
                        }
                        return sn;
                    }
                }
            }
            catch { return dI.Name; }
            return dI.Name;
        }

        public static SpecialFolder Create(string parentpath, string alias) 
        {
            DirectoryInfo parentdir = new DirectoryInfo(parentpath);
            int thisid = 0;
            foreach (DirectoryInfo dir in parentdir.GetDirectories())
            {
                if (dir.Name.StartsWith("spf_"))
                {
                    thisid++;
                }
            }
            DirectoryInfo count = new DirectoryInfo(parentdir.FullName + @"\spf_" + thisid.ToString());
            while (count.Exists)
            {
                thisid++;
            }
            DirectoryInfo cont = new DirectoryInfo(parentdir.FullName + @"\spf_" + thisid.ToString());
            cont.Create();
            FileInfo magic = new FileInfo(cont.FullName + @"\desktop.ini");
            FileAttributes fa = magic.Attributes;
            FileStream fs = magic.OpenWrite();
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(@"[" + @".ShellClassInfo" + @"]");
            sw.WriteLine(@"LocalizedResourceName=" + alias);
            sw.Flush();
            sw.Close();
            fs.Close();
            FileInfo mer = new FileInfo(cont.FullName + @"\desktop.ini");
            mer.Attributes = FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.System;
            Common.LPSHFOLDERCUSTOMSETTINGS FolderSettings = new Common.LPSHFOLDERCUSTOMSETTINGS();
            FolderSettings.dwMask = 0x10;
            FolderSettings.pszIconFile = @"X:\Windows\system32\SHELL32.dll";
            FolderSettings.iIconIndex = 3;

            UInt32 FCS_READ = 0x00000001;
            UInt32 FCS_FORCEWRITE = 0x00000002;
            UInt32 FCS_WRITE = FCS_READ | FCS_FORCEWRITE;

            string pszPath = cont.FullName;
            UInt32 HRESULT = Common.SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_WRITE);
            Common.RefreshIconCache();
            return new SpecialFolder(cont.FullName);
        }

        public static string GetName(DirectoryInfo dI)
        {
            FileInfo desktop_ini = null;
            try
            {
                FileInfo[] Files = dI.GetFiles();
                foreach (FileInfo file in Files)
                {
                    if (file.Name == "desktop.ini")
                    {
                        desktop_ini = file;
                    }
                }
                string[] lines = File.ReadAllLines(desktop_ini.FullName);
                foreach (string line in lines)
                {
                    if (line.StartsWith("LocalizedResourceName="))
                    {
                        string sn = line.Remove(0, "LocalizedResourceName=".Length);
                        if (sn.StartsWith("@"))
                        {
                            return dI.Name;
                        }
                        return sn;
                    }
                }
            }
            catch { return dI.Name; }
            return dI.Name;
        }

        public static SpecialFolder GetSpecialFolderByID(string ParentPath, int FolderID) { return null; }
    }
}
