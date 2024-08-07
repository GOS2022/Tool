﻿using System;
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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) != true)
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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) == true)
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

        public static BootloaderData GetSoftwareInfo()
        {
            BootloaderData bootloaderData = new BootloaderData();           
            bld_com_data_resp_msg_t respMsg = new bld_com_data_resp_msg_t();
            byte[] recvBuf;
            GcpMessageHeader messageHeader = new GcpMessageHeader();

            messageHeader.MessageId = (UInt16)BootloaderMessageId.BLD_MSG_DATA_REQ_ID;
            messageHeader.ProtocolVersion = 1;
            messageHeader.PayloadSize = 0;

            sysmonSemaphore.Wait();
            if (GCP.TransmitMessage(0, messageHeader, new byte[] { }, 0xffff) == true)
            {
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) == true)
                {
                    respMsg.GetFromBytes(recvBuf);

                    // Driver info.
                    bootloaderData.BootloaderDriverInfo = new VersionInfo();
                    bootloaderData.BootloaderDriverInfo.Author = respMsg.bldData.bldDriverVersion.author;
                    bootloaderData.BootloaderDriverInfo.Major = respMsg.bldData.bldDriverVersion.major;
                    bootloaderData.BootloaderDriverInfo.Minor = respMsg.bldData.bldDriverVersion.minor;
                    bootloaderData.BootloaderDriverInfo.Build = respMsg.bldData.bldDriverVersion.build;
                    try
                    {
                        bootloaderData.BootloaderDriverInfo.Date = DateTime.Parse(respMsg.bldData.bldDriverVersion.date.Years.ToString("D4") + "-" + respMsg.bldData.bldDriverVersion.date.Months.ToString("D2") + "-" + respMsg.bldData.bldDriverVersion.date.Days.ToString("D2"));
                    }
                    catch
                    {
                        bootloaderData.BootloaderDriverInfo.Date = new DateTime();
                    }
                    bootloaderData.BootloaderDriverInfo.Description = respMsg.bldData.bldDriverVersion.description;
                    bootloaderData.BootloaderDriverInfo.Name = respMsg.bldData.bldDriverVersion.name;

                    // Bootloader info.
                    bootloaderData.BootloaderInfo = new VersionInfo();
                    bootloaderData.BootloaderInfo.Author = respMsg.bldData.bldVersion.author;
                    bootloaderData.BootloaderInfo.Major = respMsg.bldData.bldVersion.major;
                    bootloaderData.BootloaderInfo.Minor = respMsg.bldData.bldVersion.minor;
                    bootloaderData.BootloaderInfo.Build = respMsg.bldData.bldVersion.build;
                    try
                    {
                        bootloaderData.BootloaderInfo.Date = DateTime.Parse(respMsg.bldData.bldVersion.date.Years.ToString("D4") + "-" + respMsg.bldData.bldVersion.date.Months.ToString("D2") + "-" + respMsg.bldData.bldVersion.date.Days.ToString("D2"));
                    }
                    catch
                    {
                        bootloaderData.BootloaderInfo.Date = new DateTime();
                    }
                    bootloaderData.BootloaderInfo.Description = respMsg.bldData.bldVersion.description;
                    bootloaderData.BootloaderInfo.Name = respMsg.bldData.bldVersion.name;
                    bootloaderData.Crc = (int)respMsg.bldData.bldCrc;
                    bootloaderData.Size = (int)respMsg.bldData.bldSize;
                    bootloaderData.StartAddress = (int)respMsg.bldData.bldStartAddress;
                    bootloaderData.BootUpdateMode = respMsg.bldData.bootUpdateMode;

                    // Application info.
                    bootloaderData.ApplicationData = new ApplicationData();
                    bootloaderData.ApplicationData.AppVersion = new VersionInfo();
                    bootloaderData.ApplicationData.AppVersion.Author = respMsg.appData.appVersion.author;
                    bootloaderData.ApplicationData.AppVersion.Major = respMsg.appData.appVersion.major;
                    bootloaderData.ApplicationData.AppVersion.Minor = respMsg.appData.appVersion.minor;
                    bootloaderData.ApplicationData.AppVersion.Build = respMsg.appData.appVersion.build;
                    try
                    {
                        bootloaderData.ApplicationData.AppVersion.Date = DateTime.Parse(respMsg.appData.appVersion.date.Years.ToString("D4") + "-" + respMsg.appData.appVersion.date.Months.ToString("D2") + "-" + respMsg.appData.appVersion.date.Days.ToString("D2"));
                    }
                    catch
                    {
                        bootloaderData.ApplicationData.AppVersion.Date = new DateTime();
                    }
                    bootloaderData.ApplicationData.AppVersion.Description = respMsg.appData.appVersion.description;
                    bootloaderData.ApplicationData.AppVersion.Name = respMsg.appData.appVersion.name;

                    // Application driver data.
                    bootloaderData.ApplicationData.DriverVersion = new VersionInfo();
                    bootloaderData.ApplicationData.DriverVersion.Author = respMsg.appData.appDriverVersion.author;
                    bootloaderData.ApplicationData.DriverVersion.Major = respMsg.appData.appDriverVersion.major;
                    bootloaderData.ApplicationData.DriverVersion.Minor = respMsg.appData.appDriverVersion.minor;
                    bootloaderData.ApplicationData.DriverVersion.Build = respMsg.appData.appDriverVersion.build;

                    try
                    {
                        bootloaderData.ApplicationData.DriverVersion.Date = DateTime.Parse(respMsg.appData.appDriverVersion.date.Years.ToString("D4") + "-" + respMsg.appData.appDriverVersion.date.Months.ToString("D2") + "-" + respMsg.appData.appDriverVersion.date.Days.ToString("D2"));
                    }
                    catch
                    {
                        bootloaderData.ApplicationData.DriverVersion.Date = new DateTime();
                    }
                    bootloaderData.ApplicationData.DriverVersion.Description = respMsg.appData.appDriverVersion.description;
                    bootloaderData.ApplicationData.DriverVersion.Name = respMsg.appData.appDriverVersion.name;


                    bootloaderData.ApplicationData.Crc = (int)respMsg.appData.appCrc;
                    bootloaderData.ApplicationData.Size = (int)respMsg.appData.appSize;
                    bootloaderData.ApplicationData.StartAddress = (int)respMsg.appData.appStartAddress;
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

            return bootloaderData;
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
            GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 50);
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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) == true)
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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) == true)
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
                if (GCP.ReceiveMessage(0, out messageHeader, out recvBuf, 1000) == true)
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
