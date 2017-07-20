using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Xiaolei.TraceLib;

namespace Xiaolei.ServicePacker
{
     
    public class MyService : ServiceBase
    {  
 
        public static string MyServiceName = ConfigurationManager.AppSettings["MyServiceName"];

        public static string ExecuteFileName = ConfigurationManager.AppSettings["ExecuteFileName"]; 
        public static string KillProcessName = ConfigurationManager.AppSettings["KillProcessName"];

        public static string StartPara = ConfigurationManager.AppSettings["StartPara"];
        public static string StopPara = ConfigurationManager.AppSettings["StopPara"];

        public static string StartBatFileName = ConfigurationManager.AppSettings["StartBatFileName"];
        public static string StopBatFileName = ConfigurationManager.AppSettings["StopBatFileName"];


        private Enum_ExecuteMode CurrentMode = Enum_ExecuteMode.BatMode;

        public MyService()
        {
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = false;
            base.AutoLog = false;

            TraceHelper.TraceInfo ("MyServiceName:" + MyServiceName);
            this.ServiceName =  MyServiceName;


            if(!string.IsNullOrEmpty(StartBatFileName) && !string.IsNullOrEmpty(StopBatFileName))
            {
                CurrentMode = Enum_ExecuteMode.BatMode;
            }
            else
            {
                if (!string.IsNullOrEmpty(StopPara))
                {
                    CurrentMode = Enum_ExecuteMode.ParaMode;
                }
                else
                {
                    CurrentMode = Enum_ExecuteMode.KillProcessMode;
                } 
            } 
        }

        protected override void OnStart(string[] args)
        {

            TraceHelper.TraceInfo("start service" + CurrentMode.ToString());

            switch (CurrentMode)
            {
                case Enum_ExecuteMode.BatMode:
                    {
                        try
                        {
                            Process pro = new Process();
                            pro.StartInfo.UseShellExecute = true;
                            pro.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                            string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, StartBatFileName);
                            TraceHelper.TraceInfo("fileName:  " + fileName);
                            pro.StartInfo.FileName = fileName; 
                            pro.StartInfo.CreateNoWindow = true;
                            pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            pro.Start(); 
                        }
                        catch (Exception ex)
                        {
                            TraceHelper.TraceInfo("OnStart 异常    " + ex.Message + ex.StackTrace);
                        }
                        break;
                    }
                case Enum_ExecuteMode.ParaMode:
                case Enum_ExecuteMode.KillProcessMode:
                    {
                        try
                        {
                            Process pro = new Process();
                            pro.StartInfo.UseShellExecute = true;
                            pro.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                            string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ExecuteFileName);
                            TraceHelper.TraceInfo("fileName:  " + fileName);
                            pro.StartInfo.FileName = fileName;
                            pro.StartInfo.Arguments = StartPara;
                            pro.StartInfo.CreateNoWindow = true;
                            pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            pro.Start();
                            TraceHelper.TraceInfo("process id:  " + pro.Id);
                        }
                        catch (Exception ex)
                        {
                            TraceHelper.TraceInfo("OnStart 异常" + ex.Message + ex.StackTrace);
                        }

                        break;
                    } 
                default:
                    {
                        break;
                    }
            } 
        }


        protected override void OnStop()
        {

            TraceHelper.TraceInfo("stop service" + CurrentMode.ToString());

            switch (CurrentMode)
            {
                case Enum_ExecuteMode.BatMode:
                    {
                        try
                        {
                            Process pro = new Process();
                            pro.StartInfo.UseShellExecute = true;
                            pro.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                            string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, StopBatFileName);
                            TraceHelper.TraceInfo("fileName:  " + fileName);
                            pro.StartInfo.FileName = fileName;
                            pro.StartInfo.CreateNoWindow = true;
                            pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            pro.Start();

                            TraceHelper.TraceInfo("process id:  " + pro.Id);

                        }
                        catch (Exception ex)
                        {
                            TraceHelper.TraceInfo("OnStart  BatMode 异常" + ex.Message + ex.StackTrace);
                        }
                        break;
                    }
                case Enum_ExecuteMode.ParaMode:
                    {
                        try
                        {
                            Process pro = new Process();
                            pro.StartInfo.UseShellExecute = true;
                            pro.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                            string fileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ExecuteFileName);
                            TraceHelper.TraceInfo("fileName:  " + fileName);
                            pro.StartInfo.FileName = fileName;
                            pro.StartInfo.Arguments = StopPara;
                            pro.StartInfo.CreateNoWindow = true;
                            pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            pro.Start();
                            TraceHelper.TraceInfo("process id:  " + pro.Id);
                        }
                        catch (Exception ex)
                        {
                            TraceHelper.TraceInfo("OnStart 异常" + ex.Message + ex.StackTrace);
                        }

                        break;
                    }
                case Enum_ExecuteMode.KillProcessMode:
                    {
                        List<string> processList = KillProcessName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        KillProcessList(processList);
                        break;
                    }
                default:
                    {
                        break;
                    }
            } 
        }
         
        private static void KillProcessList(List<string> I_ProcessList  )
        { 
            foreach(string curName in I_ProcessList)
            {
                try
                {
                    Process[] proc = Process.GetProcessesByName(curName);

                    TraceHelper.TraceInfo("process name:" + curName + "    count: "+ proc.Length);
                    foreach(Process curProcess in proc)
                    {
                        if (!curProcess.HasExited)
                        {
                            curProcess.Kill();
                        }
                    } 
                }
                catch (Exception ex)
                {
                    TraceHelper.TraceInfo("KillProcessList 异常" + ex.Message + ex.StackTrace);
                }
            } 
        } 
    }


    public enum Enum_ExecuteMode
    {
        /// <summary>通bat文件执行开始服务和结束服务
        /// </summary>
        BatMode =1,

        /// <summary> 通过ExecuteFileName 和 StartPara StopPara来 执行开始服务和结束服务
        /// </summary>
        ParaMode = 2,

        /// <summary> 通过ExecuteFileName 和 StartPara 来执行开始服务， 通过 结束  KillProcessName 进程来 结束服务
        /// </summary>
        KillProcessMode = 3

    }

}
