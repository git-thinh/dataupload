using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    class IEUtil
    {
        public static void openIE(string url)
        {
            try
            {
                //System.Diagnostics.Process.Start(url);
                
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "iexplore.exe";
                p.StartInfo.Arguments = url;
             
                p.Start();
            }
            catch (Exception ex)
            {
                Log.logger("openIE" + url+"----------"+ex.Message);
            }
        }
        public static void closeAllIEProcess()
        {
            string defaultBrowserName = GetDefaultBrowerName();
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("IEXPLORE");

            foreach (System.Diagnostics.Process proc in procs)
            {
                proc.Kill();
            }
        }


        public static void CleanCookie()
        {
            try
            {

                string[] theFiles = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies), "*", System.IO.SearchOption.AllDirectories);
                foreach (string s in theFiles)
                    FileDelete(s);
            }
            catch(Exception e)
            {
                Log.logger("Delete cookie error" + e.Message);
            }
        }

        static bool FileDelete(string path)
        {
            //first set the File's ReadOnly to 0
            //if EXP, restore its Attributes

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            System.IO.FileAttributes att = 0;
            bool attModified = false;

            try
            {
                //### ATT_GETnSET
                att = file.Attributes;
                file.Attributes &= (~System.IO.FileAttributes.ReadOnly);
                attModified = true;

                file.Delete();
            }
            catch (Exception e)
            {
                if (attModified)
                    file.Attributes = att;

                return false;
            }

            return true;
        }

        public static string GetDefaultBrowerName()
        {
           string mainKey = @"http\shell\open\command";
            string nameKey = @"http\shell\open\ddeexec\Application";

            string strRet = string.Empty;
            try
            {
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(nameKey);
                strRet = regKey.GetValue("").ToString();
            }
            catch
            {
                strRet = "";
            }
            return strRet;
        }
        /// <summary>
        /// 清除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        static void FolderClear(string path)
        {
            System.IO.DirectoryInfo diPath = new System.IO.DirectoryInfo(path);
            if (diPath.Exists)
            {
                foreach (System.IO.FileInfo fiCurrFile in diPath.GetFiles())
                {
                    FileDelete(fiCurrFile.FullName);

                }
                foreach (System.IO.DirectoryInfo diSubFolder in diPath.GetDirectories())
                {
                    FolderClear(diSubFolder.FullName); // Call recursively for all subfolders
                }
            }
        }
        /// <summary>
        /// 执行命令行
        /// </summary>
        /// <param name="cmd"></param>
        static void RunCmd(string cmd)
        {
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "cmd.exe";
            p.Arguments = "/c " + cmd;
            p.WindowStyle = ProcessWindowStyle.Hidden;  // Use a hidden window
            Process.Start(p);
        }
        /// <summary>
        /// 删除临时文件
        /// </summary>
        public static void CleanTempFiles()
        {
            FolderClear(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
        }
    }
}
