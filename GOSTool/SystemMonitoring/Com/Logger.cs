using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GOSTool.Logger;

namespace GOSTool
{
    public static class Logger
    {
        public enum ComType
        {
            RAW_RX_UART,
            RAW_TX_UART,
            RAW_RX_WIRELESS,
            RAW_TX_WIRELESS
        }
        public static string LogsFolder { get; } = ProjectHandler.WorkspacePath.Value + "/" + ProjectHandler.ProjectName.Value + "/logs";

        public static string LogPath { get; private set; } = LogsFolder + "/com_" + DateTime.Now.ToString("yyyyMMdd_HHssmm") + ".log";
        public static string MeasPath { get; private set; } = LogsFolder + "/meas_" + DateTime.Now.ToString("yyyyMMdd_HHssmm") + ".meas";
        public static void StartNewLog()
        {
            LogPath = LogsFolder + "/com_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
        }
        public static void StartNewMeasurement()
        {
            MeasPath = LogsFolder + "/meas_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".meas";
        }

        public static void LogNewCom(byte[] bytes, ComType comType)
        {
            if (!Directory.Exists(LogsFolder))
            {
                Directory.CreateDirectory(LogsFolder);
            }

            string entry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff\t");
            entry += comType.ToString() + "\t";
            entry += BitConverter.ToString(bytes).Replace("-", " ");
            
            using(StreamWriter sw = File.AppendText(LogPath))
            {
                sw.WriteLine(entry);
            }
        }

        public static void LogNewMeasurement(MonitoringData measrement)
        {
            if (!Directory.Exists(LogsFolder))
            {
                Directory.CreateDirectory(LogsFolder);
            }

            string entry = measrement.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff\t");
            entry += measrement.CpuLoad;

            using (StreamWriter sw = File.AppendText(MeasPath))
            {
                sw.WriteLine(entry);
            }
        }
    }
}
