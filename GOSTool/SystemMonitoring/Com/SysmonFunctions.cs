using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.Azure.Pipelines.WebApi.PipelinesResources;

namespace GOSTool
{
    public static class SysmonFunctions
    {
        private static SemaphoreSlim sysmonSemaphore = new SemaphoreSlim(1, 1);
        public static void SemaphoreWait()
        {
            sysmonSemaphore.Wait();
        }

        public static void SemaphoreRelease()
        {
            sysmonSemaphore.Release();
        }
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

            messageHeader.MessageId = (UInt16)SysmonMessageId.APP_SYSMON_MSG_RTC_SET_REQ;//(ushort)SysmonMessageId.GOS_SYSMON_MSG_SYSTIME_SET_ID;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = SysTimeMessage.ExpectedSize;

            sysmonSemaphore.Wait();

            if (GCP.TransmitMessage(0, messageHeader, sysTimeMessage.GetBytes(), 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
                {

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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 0xffff, 2000) == true)
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
