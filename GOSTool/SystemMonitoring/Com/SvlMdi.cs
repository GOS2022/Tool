using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public enum SvlMdiVariableType
    {
        Byte,
        Word,
        LWord,
        Float
    }

    public class SvlMdiVariable
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public SvlMdiVariableType Type { get; private set; }

        public const UInt16 ExpectedSize = 21;

        public void GetFromBytes(byte[] bytes)
        {
            if ((bytes != null) && (bytes.Length >= ExpectedSize))
            {
                int idx = 0;
                Name = Helper.GetString(bytes, 16, ref idx);
                Type = (SvlMdiVariableType)Enum.Parse(typeof(SvlMdiVariableType), Helper<byte>.GetVariable(bytes, ref idx).ToString());

                switch (Type)
                {
                    case SvlMdiVariableType.Byte:
                    case SvlMdiVariableType.Word:
                    case SvlMdiVariableType.LWord:
                        {
                            Value = Helper<UInt32>.GetVariable(bytes, ref idx).ToString();
                            break;
                        }
                    case SvlMdiVariableType.Float:
                        {
                            Value = BitConverter.ToSingle(bytes, idx).ToString();
                            break;
                        }
                    default: break;
                }
            }
        }
    }
    public class SvlMdi
    {
        public static List<SvlMdiVariable> GetMonitoringData()
        {
            List<SvlMdiVariable> variables = new List<SvlMdiVariable>();
            byte[] recvBuf;
            UInt16 numOfVariables = 0;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            // First determine the number of monitoring variables.
            messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_REQ;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            SysmonFunctions.SemaphoreWait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    int idx = 0;
                    numOfVariables = Helper<UInt16>.GetVariable(recvBuf, ref idx);
                }
            }
            SysmonFunctions.SemaphoreRelease();

            Thread.Sleep(10);

            SysmonFunctions.SemaphoreWait();
            for (int i = 0; i < numOfVariables; i++)
            {
                messageHeader.MessageId = (UInt16)SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ;
                messageHeader.ProtocolVersion = 1;
                messageHeader.PayloadSize = 2;

                if (GCP.TransmitMessage(0, messageHeader, Helper<UInt16>.GetBytes((UInt16)i), 0xffff) == true)
                {
                    if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                    {
                        SvlMdiVariable var = new SvlMdiVariable();
                        var.GetFromBytes(recvBuf);
                        variables.Add(var);
                        Thread.Sleep(20);
                    }
                }
            }
            SysmonFunctions.SemaphoreRelease();

            return variables;
        }
    }
}
