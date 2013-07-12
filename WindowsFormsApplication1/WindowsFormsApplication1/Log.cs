using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    class Log
    {
        //public static  bool main = true;
        private static void Write(String msg)
        {
         //   if (main == false) { return; }
            string LogPath = Application.StartupPath + "\\log";
            string filePath = LogPath + "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            string fileName = filePath + "\\" + DateTime.Now.Day.ToString() + ".Log";
            try
            {

                if (!System.IO.Directory.Exists(LogPath))
                {
                    System.IO.Directory.CreateDirectory(LogPath);
                }
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                FileInfo filebase = new FileInfo(fileName);//判斷文件大小
                
                using (FileStream filestream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    StreamWriter writer = new StreamWriter(filestream, Encoding.GetEncoding("Gb2312"));
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                    writer.WriteLine("{0} {1}", DateTime.Now.TimeOfDay, msg);
                    writer.Flush();
                    writer.Close();
                 //   filestream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



     //   private static Mutex mu = new Mutex(false);
        static Object obj = new object();
        public static void logger(string log)
        {
            try
            {
                lock (obj)
                {
                    Write(log);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static Object objdata = new object();
        public static void Datalogger(string log,string name)
        {
            try
            {
                lock (objdata)
                {
                    WriteData(log,name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void FileDelete(string name)
        {
            string LogPath = Application.StartupPath + "";
            string filePath = LogPath + "\\" + DateTime.Now.Year.ToString();
            string fileName = filePath + "\\" + name + DateTime.Now.ToString("yyyy_MM_dd") + ".csv";
            try
            {

                if (!System.IO.Directory.Exists(LogPath))
                {
                    System.IO.Directory.CreateDirectory(LogPath);
                }
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                FileInfo filebase = new FileInfo(fileName);//判斷文件大小
                if (filebase.Exists)
                {
                    filebase.Delete();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static void WriteData(String msg,string name)
        {
            //   if (main == false) { return; }
            string LogPath = Application.StartupPath + "";
            string filePath = LogPath + "\\" + DateTime.Now.Year.ToString()  ;
            string fileName = filePath + "\\" +name+ DateTime.Now.ToString("yyyy_MM_dd") + ".csv";
            try
            {

                if (!System.IO.Directory.Exists(LogPath))
                {
                    System.IO.Directory.CreateDirectory(LogPath);
                }
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                FileInfo filebase = new FileInfo(fileName);//判斷文件大小
              
                using (FileStream filestream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    StreamWriter writer = new StreamWriter(filestream, Encoding.GetEncoding("Gb2312"));
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                    writer.WriteLine("{0}" , msg);
                    writer.Flush();
                    writer.Close();
                    //   filestream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
