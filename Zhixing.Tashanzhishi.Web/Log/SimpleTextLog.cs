using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Log
{
    /// <summary>
    /// GP_WriteLog 的摘要说明。
    /// </summary>
    public class SimpleTextLog
    {
        /// <summary>
        /// 日志的路径。
        /// </summary>
        private string m_LogPath = "logFile";
        /// <summary>
        /// 日志文件名。
        /// </summary>
        private string m_LogFileName = "";
        /// <summary>
        /// 文件当天的
        /// </summary>
        private int m_FileSequence = 0;
        /// <summary>
        /// 日志文件的大小。
        /// </summary>
        private uint m_FileLen = 127;
        /// <summary>
        /// 日志级别。
        /// </summary>
        private int m_LogLeve = 3;
        /// <summary>
        /// 得到和设置日志文件的大小K。
        /// </summary>
        public uint FileLen
        {
            get
            {
                return this.m_FileLen;
            }
            set
            {
                this.m_FileLen = value;
            }
        }
        /// <summary>
        /// 日志的级别。
        /// </summary>
        public int LogLeve
        {
            get
            {
                return this.m_LogLeve;
            }
            set
            {
                this.m_LogLeve = value;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="FileName">日志文件名</param>
        public SimpleTextLog(string FileName)
        {
            this.m_LogFileName = FileName;
            this.CheckFileSequence();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="FileName">日志文件名</param>
        /// <param name="Path">日志路径</param>
        public SimpleTextLog(string FileName, string Path)
        {
            this.m_LogFileName = FileName;
            this.m_LogPath = Path;
            this.CheckFileSequence();
        }
        /// <summary>
        /// 写日志。
        /// </summary>
        /// <param name="Err">要写得内容</param>
        public void WriteLog(string Err)
        {
            try
            {
                lock (this)
                {
                    string Temp = "";

                    Temp = "Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (this.m_LogLeve > 1)
                    {
                        System.IO.StreamWriter FileWriter = this.CheckFile();
                        FileWriter.WriteLine(Temp);
                        FileWriter.WriteLine("Info:" + Err);
                        FileWriter.WriteLine();
                        FileWriter.Close();
                    }
                    if (this.m_LogLeve > 0)
                    {
                        Console.WriteLine(Temp);
                        Console.WriteLine("Info:" + Err);
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// 写日志。
        /// </summary>
        /// <param name="Err">要写得标题</param>
        /// <param name="Ex">出错的异常</param>
        public void WriteLog(string Err, Exception Ex)
        {
            try
            {
                lock (this)
                {
                    string Temp = "";
                    System.IO.StreamWriter FileWriter = this.CheckFile();
                    Temp = "Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    this.ConsoleWrite(Temp);
                    FileWriter.WriteLine(Temp);
                    this.ConsoleWrite("ERR:" + Err);
                    FileWriter.WriteLine("ERR:" + Err);
                    this.ConsoleWrite("Message:" + Ex.Message);
                    FileWriter.WriteLine("Message:" + Ex.Message);
                    this.ConsoleWrite("Source:" + Ex.Source);
                    FileWriter.WriteLine("Source:" + Ex.Source);
                    this.ConsoleWrite("StackTrace:" + Ex.StackTrace);
                    FileWriter.WriteLine("StackTrace:" + Ex.StackTrace);
                    this.ConsoleWrite("TargetSite:" + Ex.TargetSite);
                    FileWriter.WriteLine("TargetSite:" + Ex.TargetSite);
                    FileWriter.WriteLine();
                    FileWriter.Close();
                }
            }
            catch { }
        }
        /// <summary>
        /// 向控制台写错误。
        /// </summary>
        /// <param name="Err">要写的内容。</param>
        private void ConsoleWrite(string Err)
        {
            if (this.LogLeve > 2)
            {
                Console.WriteLine(Err);
            }
        }
        /// <summary>
        /// 检查文件。
        /// </summary>
        private System.IO.StreamWriter CheckFile()
        {
            System.IO.StreamWriter ReturnValue = null;
            System.IO.FileInfo FileInf = new System.IO.FileInfo(this.m_LogPath + @"\" + this.m_LogFileName + DateTime.Now.ToString("yyyy-MM-dd ") + this.m_FileSequence.ToString() + ".LOG");
            if ((!FileInf.Exists) || (FileInf.Length > (this.m_FileLen * 1024)))
            {
                if (FileInf.Exists)
                {//文件超长。
                    this.m_FileSequence++;
                    FileInf = new System.IO.FileInfo(this.m_LogPath + @"\" + this.m_LogFileName + DateTime.Now.ToString("yyyy-MM-dd ") + this.m_FileSequence.ToString() + ".LOG");
                }
                ReturnValue = FileInf.CreateText();
                ReturnValue.WriteLine("河南点金时代科技有限公司日志文件。");
                ReturnValue.WriteLine("版本：1.1");
                ReturnValue.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ReturnValue.WriteLine();
            }
            else
            {
                ReturnValue = FileInf.AppendText();
            }
            return ReturnValue;
        }
        /// <summary>
        /// 检测当天的文件个数。
        /// </summary>
        private void CheckFileSequence()
        {
            System.IO.DirectoryInfo DirInfo = new System.IO.DirectoryInfo(this.m_LogPath);
            if (!DirInfo.Exists)
            {
                DirInfo.Create();
            }
            string[] FileName = System.IO.Directory.GetFiles(this.m_LogPath, this.m_LogFileName + DateTime.Now.ToString("yyyy-MM-dd ") + "*" + ".LOG");
            if (FileName.Length > 0)
            {
                this.m_FileSequence = FileName.Length - 1;
            }
        }
    }
}
