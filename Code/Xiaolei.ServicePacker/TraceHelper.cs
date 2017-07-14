using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;

namespace Xiaolei.ServicePacker
{
    /// <summary>输出调试信息到debugview和log4net日志中
    /// </summary>
    public class TraceHelper
    {
        #region 静态变量
         
        private static Dictionary<LogType, ILog> EnumLogDic;

        private static readonly int IsTrace;

        private static readonly int IsLog;

        #endregion

        #region 静态购置函数

        static TraceHelper()
        { 
            EnumLogDic = new Dictionary<LogType, ILog>();
            string tempString = ConfigurationManager.AppSettings["LogName"];

            if (tempString != null)
            {
                List<string> logNameList = tempString.Split(',').ToList();

                foreach (LogType item in Enum.GetValues(typeof(LogType)))
                {
                    string curEnumString = AttributeHelper.GetEnumDescribeString<LogType>(item);

                    if (logNameList.Contains(curEnumString))
                    {
                        ILog tempLog = LogManager.GetLogger(curEnumString);
                        EnumLogDic.Add(item, tempLog);
                    } 
                } 
            }

            IsTrace = Convert.ToInt32(ConfigurationManager.AppSettings["IsTrace"]);
            IsLog = Convert.ToInt32(ConfigurationManager.AppSettings["IsLog"]);   
        }

        #endregion

        #region 静态方法
         /// <summary>使用默认的ILog，输出日志
         /// </summary>
         /// <param name="I_TraceInfo"></param>
        public static void TraceInfo(string I_TraceInfo)
        {
            TraceInfo(LogType.Default, I_TraceInfo);
        }

        public static void TraceInfo(LogType I_LogType, string I_TraceInfo)
        {
            if (EnumLogDic.ContainsKey(I_LogType))
            {
                ILog curLogName = EnumLogDic[I_LogType];
                TraceHelper.TraceInfo(curLogName, I_TraceInfo);
            }
        }
         
        public static void TraceInfo(ILog I_CurLog, string I_TraceInfo)
        {
            if (I_CurLog != null)
            {
                if (IsTrace == 1)
                {
                    System.Diagnostics.Trace.WriteLine(I_TraceInfo);
                }

                if (IsLog == 1 && I_CurLog != null)
                {
                    I_CurLog.Debug(I_TraceInfo);
                }
            }
        }

        #endregion
    }


    public enum LogType
    {
        [Describe("DefaultLogger")]
        Default,

        [Describe("TestLogger")]
        Test, 
    }


}
