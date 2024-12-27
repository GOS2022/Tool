using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool
{
    public static class SysmonFunctions
    {
        private static SemaphoreSlim sysmonSemaphore = new SemaphoreSlim(1, 1);
        public static EventHandler<(int, int)> BinaryDownloadProgressEvent; 
        public enum PingResult
        {
            OK,
            NOK
        }

        public static PingResult PingDevice()
        {           
            SysmonPingMessage sysmonPingMessage = new SysmonPingMessage();
            GcpMessageHeader messageHeader = new GcpMessageHeader();
            PingResult result = PingResult.OK;
            byte[] recvBuf;

            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_PING_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_PING_PV;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) != true)
            {
                result = PingResult.NOK;
            }
            else
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) != true)
                {
                    result = PingResult.NOK;
                }
                else
                {
                    sysmonPingMessage.GetFromBytes(recvBuf);

                    if (sysmonPingMessage.MessageResult != SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                    {
                        result = PingResult.NOK;
                    }
                }
            }

            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return result;
        }

        public static List<TaskData> GetAllTaskData()
        {
            TaskDataMessage taskDataMessage = new TaskDataMessage();
            List<TaskData> taskDatas = new List<TaskData>();
            TaskDataGetMessage taskDataGetMessage = new TaskDataGetMessage();            
            GcpMessageHeader messageHeader = new GcpMessageHeader();
            byte[] recvBuf;

            taskDataGetMessage.TaskIndex = 0xFFFF;
            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_TASK_GET_DATA_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_TASK_GET_DATA_PV;
            messageHeader.PayloadSize = TaskDataGetMessage.ExpectedSize;

            sysmonSemaphore.Wait();

            GCP.TransmitMessage(0, messageHeader, taskDataGetMessage.GetBytes(), 0xffff);        

            while (true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    taskDataMessage.GetFromBytes(recvBuf);

                    if (taskDataMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                    {
                        taskDatas.Add(taskDataMessage.TaskData);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            Thread.Sleep(10);
            sysmonSemaphore.Release();
            return taskDatas;
        }

        public static void SynchronizeTime()
        {
            SysTimeMessage sysTimeMessage = new SysTimeMessage();
            sysTimeMessage.SystemTime.Years = (ushort)DateTime.Now.Year;
            sysTimeMessage.SystemTime.Months = (byte)DateTime.Now.Month;
            sysTimeMessage.SystemTime.Days = (byte)DateTime.Now.Day;
            sysTimeMessage.SystemTime.Hours = (byte)DateTime.Now.Hour;
            sysTimeMessage.SystemTime.Minutes = (byte)DateTime.Now.Minute;
            sysTimeMessage.SystemTime.Seconds = (byte)DateTime.Now.Second;
            sysTimeMessage.SystemTime.Milliseconds = (byte)DateTime.Now.Millisecond;
            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (ushort)SysmonMessageId.GOS_SYSMON_MSG_SYSTIME_SET_ID;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = SysTimeMessage.ExpectedSize;

            sysmonSemaphore.Wait();

            if (GCP.TransmitMessage(0, messageHeader, sysTimeMessage.GetBytes(), 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    //softwareInfo.GetFromBytes(recvBuf); TODO response?
                }
                else
                {
                    // Error.
                }
            }
            else
            {
                // Error.
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();
        }

        public static SoftwareInfo GetSoftwareInfo()
        {
            SoftwareInfo softwareInfo = new SoftwareInfo();
            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2000;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    softwareInfo.GetFromBytes(recvBuf);
                }
                else
                {
                    // Error.
                }
            }
            else
            {
                // Error.
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return softwareInfo;
        }

        public static HardwareInfo GetHardwareInfo()
        {
            HardwareInfo hardwareInfo = new HardwareInfo();
            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2001;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    hardwareInfo.GetFromBytes(recvBuf);
                }
                else
                {
                    // Error.
                }
            }
            else
            {
                // Error.
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return hardwareInfo;
        }

        public static SysmonMessageResult ModifyTask(int taskIndex, SysmonTaskModifyType modifyType)
        {
            byte[] recvBuf;

            TaskModifyMessage taskModifyMessage = new TaskModifyMessage();
            taskModifyMessage.TaskIndex = (UInt16)taskIndex;
            taskModifyMessage.TaskModifyType = modifyType;
            TaskModifyResultMessage taskModifyResultMessage = new TaskModifyResultMessage();

            GcpMessageHeader messageHeader = new GcpMessageHeader();
            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_TASK_MODIFY_STATE_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_TASK_MODIFY_STATE_PV;
            messageHeader.PayloadSize = TaskModifyMessage.ExpectedSize;

            sysmonSemaphore.Wait();
            GCP.TransmitMessage(0, messageHeader, taskModifyMessage.GetBytes(), 0xffff);
            GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff);
            sysmonSemaphore.Release();

            taskModifyResultMessage.GetFromBytes(recvBuf);

            return taskModifyResultMessage.MessageResult;
        }

        public static void SendResetRequest()
        {
            GcpMessageHeader messageHeader = new GcpMessageHeader();
            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_RESET_REQ_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_RESET_REQ_PV;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff);
            Thread.Sleep(10);
            sysmonSemaphore.Release();
        }

        public static int GetBinaryNum()
        {
            byte[] recvBuf;
            int binaryNum = 0;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2101;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    binaryNum = ((recvBuf[1] << 8) + recvBuf[0]);
                }
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return binaryNum;
        }

        public static BinaryDescriptorMessage GetBinaryInfo(int index)
        {
            byte[] recvBuf;
            BinaryDescriptorMessage binaryInfo = new BinaryDescriptorMessage();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2102;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 2;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    binaryInfo.GetFromBytes(recvBuf);
                }
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return binaryInfo;
        }

        public static bool SendInstallRequest(int index)
        {
            byte[] recvBuf;
            bool retval = false;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2105;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 2;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { (byte)(index), (byte)((int)index >> 8), }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    int idx = 0;
                    int rxIdx = Helper<UInt16>.GetVariable(recvBuf, ref idx);

                    if (rxIdx == index)
                        retval = true;
                }
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return retval;
        }

        public static bool SendBinary(List<byte> bytes)
        {
            byte[] recvBuf;
            bool res = false;
            int chunkSize = 2048;

            ChunkDescriptor chunkDesc = new ChunkDescriptor();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            sysmonSemaphore.Wait();

            Thread.Sleep(10);

            int chunks = bytes.Count / chunkSize + (bytes.Count % chunkSize == 0 ? 0 : 1);

            for (int chunkCounter = 0; chunkCounter < chunks; chunkCounter++)
            {
                chunkDesc.ChunkIndex = (UInt16)chunkCounter;

                messageHeader.MessageId = 0x2104;
                messageHeader.ProtocolVersion = 1;
                messageHeader.PayloadSize = (UInt16)(chunkDesc.GetBytes().Length + chunkSize);

                List<byte> payload = new List<byte>();
                payload.AddRange(chunkDesc.GetBytes());

                if (bytes.Skip(chunkCounter * chunkSize).ToArray().Length >= chunkSize)
                {
                    payload.AddRange(bytes.Skip(chunkCounter * chunkSize).Take(chunkSize));
                }
                else
                {
                    payload.AddRange(bytes.Skip(chunkCounter * chunkSize).Take(bytes.Skip(chunkCounter * chunkSize).ToArray().Length));
                }

                if (GCP.TransmitMessage(0, messageHeader, payload.ToArray(), 0xffff) == true)
                {
                    if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                    {
                        chunkDesc.GetFromBytes(recvBuf);

                        if (chunkDesc.Result != 1)
                        {
                            res = false;
                            break;
                        }
                        else
                        {
                            res = true;
                            BinaryDownloadProgressEvent?.Invoke(null, (chunkCounter + 1, chunks));
                        }
                    }
                    else
                    {
                        res = false;
                        break;
                    }
                }
                else
                {
                    res = false;
                    break;
                }
            }
            
            sysmonSemaphore.Release();

            return res;
        }

        public static BinaryDownloadRequestResult SendBinaryDownloadRequest(BinaryDescriptorMessage binaryDescriptor)
        {
            byte[] recvBuf;
            BinaryDownloadRequestResult result = BinaryDownloadRequestResult.COMM_ERR;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = 0x2103;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = (UInt16)binaryDescriptor.GetBytes().Length;

            sysmonSemaphore.Wait();

            Thread.Sleep(10);

            if (GCP.TransmitMessage(0, messageHeader, binaryDescriptor.GetBytes(), 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {
                    result = (BinaryDownloadRequestResult)recvBuf[0];
                }
            }

            sysmonSemaphore.Release();

            return result;
        }

        public static float GetCpuLoad()
        {
            byte[] recvBuf;
            float cpuLoad = -1;
            CpuUsageMessage cpuUsageMessage = new CpuUsageMessage();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_CPU_USAGE_GET_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_CPU_USAGE_GET_PV;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    cpuUsageMessage.GetFromBytes(recvBuf);

                    if (cpuUsageMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                    {
                        cpuLoad = cpuUsageMessage.CpuUsagePercent;
                    }
                }
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return cpuLoad;            
        }

        public static string GetSystemRuntime()
        {
            byte[] recvBuf;
            string runtime = "0000:00:00:00";
            SysRuntimeMessage sysRuntimeMessage = new SysRuntimeMessage();
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_SYSRUNTIME_GET_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_SYSRUNTIME_GET_PV;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    sysRuntimeMessage.GetFromBytes(recvBuf);

                    if (sysRuntimeMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                    {
                        runtime = sysRuntimeMessage.SysRunTime.Days.ToString("D2") + ":" +
                            sysRuntimeMessage.SysRunTime.Hours.ToString("D2") + ":" +
                            sysRuntimeMessage.SysRunTime.Minutes.ToString("D2") + ":" +
                            sysRuntimeMessage.SysRunTime.Seconds.ToString("D2") + "." +
                            sysRuntimeMessage.SysRunTime.Milliseconds.ToString("D3");
                    }
                }
            }
            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return runtime;
        }

        public static TaskVariableData GetTaskVariableData(int taskIndex)
        {
            TaskVariableDataMessage taskDataMessage = new TaskVariableDataMessage();
            TaskDataGetMessage taskDataGetMessage = new TaskDataGetMessage();
            TaskVariableData taskVariableData = null;
            GcpMessageHeader messageHeader = new GcpMessageHeader();
            byte[] recvBuf;

            taskDataGetMessage.TaskIndex = (UInt16)taskIndex;
            messageHeader.MessageId = (UInt16)SysmonMessageId.GOS_SYSMON_MSG_TASK_GET_VAR_DATA_ID;
            messageHeader.ProtocolVersion = (UInt16)SysmonMessagePv.GOS_SYSMON_MSG_TASK_GET_VAR_DATA_PV;
            messageHeader.PayloadSize = TaskDataGetMessage.ExpectedSize;

            sysmonSemaphore.Wait();
            
            if (GCP.TransmitMessage(0, messageHeader, taskDataGetMessage.GetBytes(), 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 1000) == true)
                {
                    taskDataMessage.GetFromBytes(recvBuf);

                    if (taskDataMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                    {
                        taskVariableData = taskDataMessage.TaskVariableData;
                    }
                }
            }

            Thread.Sleep(10);
            sysmonSemaphore.Release();

            return taskVariableData;
        }
    }
}
