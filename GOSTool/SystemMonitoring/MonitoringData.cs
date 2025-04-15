using System;
using System.Drawing;

namespace GOSTool
{
    public class MonitoringData
    {
        public DateTime TimeStamp { get; set; } = new DateTime();
        public float CpuLoad { get; set; } = 0;
        public Point Location { get; set; } = new Point();
        public MonitoringData()
        {

        }
        public MonitoringData(float cpuLoad)
        {
            CpuLoad = cpuLoad;
            TimeStamp = DateTime.Now;
        }
    }
}
