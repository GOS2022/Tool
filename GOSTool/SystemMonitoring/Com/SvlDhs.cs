using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public class DhsDeviceDescriptor
    {
        public const int ExpectedSize = 32 + 64 + 4 + 4 + 10 + 16 + 16 + 21;
        public string Name { get; set; }
        public string Description { get; set; }
        public UInt32 DeviceId { get; set; }
        public string DeviceType { get; set; }
        public bool Enabled { get; set; }
        public string DeviceState { get; set; }
        public UInt32 ErrorCode { get; set; }
        public UInt32 ErrorCounter { get; set; }
        public UInt32 ErrorTolerance { get; set; }
        public UInt32 ReadCounter { get; set; }
        public UInt32 WriteCounter { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                Name = Helper.GetString(bytes, 32, ref idx);
                Description = Helper.GetString(bytes, 64, ref idx);
                DeviceId = Helper<UInt32>.GetVariable(bytes, ref idx);
                idx += 4;
                byte devType = Helper<byte>.GetVariable(bytes, ref idx);
                DeviceType = "Unknown";
                switch (devType)
                {
                    case 0: DeviceType = "Read only"; break;
                    case 1: DeviceType = "Write only"; break;
                    case 2: DeviceType = "Read / Write"; break;
                    case 3: DeviceType = "Virtual"; break;
                }

                Enabled = Helper<bool>.GetVariable(bytes, ref idx);
                idx += 4;
                idx += 4;
                idx += 4 * 4;
                idx += 4 * 4;
                byte devState = Helper<byte>.GetVariable(bytes, ref idx);
                DeviceState = "Unknown";
                switch (devState)
                {
                    case 0: DeviceState = "Uninitialized"; break;
                    case 1: DeviceState = "Healthy"; break;
                    case 2: DeviceState = "Error"; break;
                }

                ErrorCode = Helper<UInt32>.GetVariable(bytes, ref idx);
                ErrorCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
                ErrorTolerance = Helper<UInt32>.GetVariable(bytes, ref idx);
                ReadCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
                WriteCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
            }
        }
    }
    public class SvlDhs
    {
        public static int GetDeviceNum()
        {
            byte[] recvBuf;
            int devNum = 0;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    devNum = ((recvBuf[1] << 8) + recvBuf[0]);
                }
            }
            Thread.Sleep(10);
            SysmonFunctions.SemaphoreRelease();

            return devNum;
        }

        public static DhsDeviceDescriptor GetDeviceInfo(int index)
        {
            byte[] recvBuf;
            DhsDeviceDescriptor deviceInfo = new DhsDeviceDescriptor();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 2;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    deviceInfo.GetFromBytes(recvBuf);
                }
            }
            Thread.Sleep(10);
            SysmonFunctions.SemaphoreRelease();

            return deviceInfo;
        }
    }
}
