using Microsoft.Azure.Pipelines.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public class SvlErsEvent
    {
        public string Description { get; private set; }
        public DateTime TimeStamp { get; private set; } = new DateTime();
        public UInt32 Trigger { get; private set; }
        public List<byte> EventData { get; private set; } = new List<byte>();

        public const UInt16 ExpectedSize = 64 + Time.ExpectedSize + 4 + 8;

        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                Description = Helper.GetString(bytes, 64, ref idx);
                TimeStamp = Helper.GetTime(bytes, ref idx);
                Trigger = Helper<UInt32>.GetVariable(bytes, ref idx);
                EventData = bytes.Skip(idx).Take(8).ToList();
            }
        }
    }
    public class SvlErs
    {
        public static bool ClearEvents()
        {
            UInt32 eventNum = 255;

            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ;//0x4102;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    int idx = 0;
                    eventNum = Helper<UInt32>.GetVariable(recvBuf, ref idx);
                }
            }
            Thread.Sleep(10);
            SysmonFunctions.SemaphoreRelease();

            return eventNum == 0;
        }

        public static List<SvlErsEvent> GetEvents()
        {
            List<SvlErsEvent> events = new List<SvlErsEvent>();
            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();
            UInt32 eventNum = 0;

            // First determine the number of events.
            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    int idx = 0;
                    eventNum = Helper<UInt32>.GetVariable(recvBuf, ref idx);
                }
            }
            SysmonFunctions.SemaphoreRelease();

            Thread.Sleep(10);

            SysmonFunctions.SemaphoreWait();
            for (int i = 0; i < eventNum; i++)
            {
                messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ;
                messageHeader.ProtocolVersion = 1;
                messageHeader.PayloadSize = 4;

                if (GCP.TransmitMessage(0, messageHeader, Helper<UInt32>.GetBytes((UInt32)i), 0xffff) == true)
                {
                    if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                    {
                        SvlErsEvent ev = new SvlErsEvent();
                        ev.GetFromBytes(recvBuf);
                        events.Add(ev);
                        Thread.Sleep(20);
                    }
                }
            }
            SysmonFunctions.SemaphoreRelease();

            return events;
        }
    }
}
