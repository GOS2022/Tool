using System;
using System.Collections.Generic;
using System.Text;

namespace GOSTool
{
    /// <summary>
    /// System monitoring message IDs.
    /// </summary>
    public enum SysmonMessageId
    {
        GOS_SYSMON_MSG_UNKNOWN_ID = 0,        //!< Unknown message ID.
        GOS_SYSMON_MSG_PING_ID = 0x0001,   //!< Ping message ID.
        GOS_SYSMON_MSG_PING_RESP_ID = 0x0A01,   //!< Ping response message ID.
        GOS_SYSMON_MSG_CPU_USAGE_GET_ID = 0x0002,   //!< CPU usage get message ID.
        GOS_SYSMON_MSG_CPU_USAGE_GET_RESP_ID = 0x0A02,   //!< CPU usage get response message ID.
        GOS_SYSMON_MSG_TASK_GET_DATA_ID = 0x0003,   //!< Task data get message ID.
        GOS_SYSMON_MSG_TASK_GET_DATA_RESP_ID = 0x0A03,   //!< Task data get response message ID.
        GOS_SYSMON_MSG_TASK_GET_VAR_DATA_ID = 0x0004,   //!< Task variable data get message ID.
        GOS_SYSMON_MSG_TASK_GET_VAR_DATA_RESP_ID = 0x0A04,   //!< Task variable data get response message ID.
        GOS_SYSMON_MSG_TASK_MODIFY_STATE_ID = 0x0005,   //!< Task modify state message ID.
        GOS_SYSMON_MSG_TASK_MODIFY_STATE_RESP_ID = 0x0A05,   //!< Task modify state response ID.
        GOS_SYSMON_MSG_SYSRUNTIME_GET_ID = 0x0006,   //!< System runtime get message ID.
        GOS_SYSMON_MSG_SYSRUNTIME_GET_RESP_ID = 0x0A06,   //!< System runtime get response message ID.
        GOS_SYSMON_MSG_SYSTIME_SET_ID = 0x0007,   //!< System time set message ID.
        GOS_SYSMON_MSG_SYSTIME_SET_RESP_ID = 0x0A07,   //!< System time set response ID.
        GOS_SYSMON_MSG_RESET_REQ_ID = 0x0FFF,   //!< System reset request ID.

        SVL_SDH_SYSMON_MSG_BINARY_NUM_REQ = 0x1001,
        SVL_SDH_SYSMON_MSG_BINARY_NUM_RESP = 0x1A01,
        SVL_SDH_SYSMON_MSG_BINARY_INFO_REQ = 0x1002,
        SVL_SDH_SYSMON_MSG_BINARY_INFO_RESP = 0x1A02,
        SVL_SDH_SYSMON_MSG_DOWNLOAD_REQ = 0x1003,
        SVL_SDH_SYSMON_MSG_DOWNLOAD_RESP = 0x1A03,
        SVL_SDH_SYSMON_MSG_BINARY_CHUNK_REQ = 0x1004,
        SVL_SDH_SYSMON_MSG_BINARY_CHUNK_RESP = 0x1A04,
        SVL_SDH_SYSMON_MSG_SOFTWARE_INSTALL_REQ = 0x1005,
        SVL_SDH_SYSMON_MSG_SOFTWARE_INSTALL_RESP = 0x1A05,
        SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ = 0x1006,
        SVL_SDH_SYSMON_MSG_BINARY_ERASE_RESP = 0x1A06,
        
        SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_REQ = 0x2001,
        SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_RESP = 0x2A01,
        SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_REQ = 0x2002,
        SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_RESP = 0x2A02,
        SVL_PDH_SYSMON_MSG_WIFI_CONFIG_GET_REQ = 0x2003,
        SVL_PDH_SYSMON_MSG_WIFI_CONFIG_GET_RESP = 0x2A03,
        SVL_PDH_SYSMON_MSG_BLD_CONFIG_GET_REQ = 0x2004,
        SVL_PDH_SYSMON_MSG_BLD_CONFIG_GET_RESP = 0x2A04,
        SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_SET_REQ = 0x2005,
        SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_SET_RESP = 0x2A05,
        SVL_PDH_SYSMON_MSG_HARDWARE_INFO_SET_REQ = 0x2006,
        SVL_PDH_SYSMON_MSG_HARDWARE_INFO_SET_RESP = 0x2A06,
        SVL_PDH_SYSMON_MSG_WIFI_CONFIG_SET_REQ = 0x2007,
        SVL_PDH_SYSMON_MSG_WIFI_CONFIG_SET_RESP = 0x2A07,
        SVL_PDH_SYSMON_MSG_BLD_CONFIG_SET_REQ = 0x2008,
        SVL_PDH_SYSMON_MSG_BLD_CONFIG_SET_RESP = 0x2A08,

        SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_REQ = 0x3001,
        SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_RESP = 0x3A01,
        SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ = 0x3002,
        SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_RESP = 0x3A02,

        SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_REQ = 0x4001,
        SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_RESP = 0x4A01,
        SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ = 0x4002,
        SVL_ERS_SYSMON_MSG_EVENTS_GET_RESP = 0x4A02,
        SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ = 0x4003,
        SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_RESP = 0x4A03,

        SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ = 0x5001,
        SVL_DHS_SYSMON_MSG_DEVICE_NUM_RESP = 0x5A01,
        SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ = 0x5002,
        SVL_DHS_SYSMON_MSG_DEVICE_INFO_RESP = 0x5A02,

        APP_SYSMON_MSG_RTC_SET_REQ = 0x8001,
        APP_SYSMON_MSG_RTC_SET_RESP = 0x8A01
    }

    /// <summary>
    /// System monitoring message protocol versions.
    /// </summary>
    public enum SysmonMessagePv
    {
        GOS_SYSMON_MSG_UNKNOWN_PV = 1,
        GOS_SYSMON_MSG_PING_PV = 1,
        GOS_SYSMON_MSG_PING_ACK_PV = 1,
        GOS_SYSMON_MSG_CPU_USAGE_GET_PV = 1,
        GOS_SYSMON_MSG_CPU_USAGE_GET_RESP_PV = 1,
        GOS_SYSMON_MSG_TASK_GET_DATA_PV = 1,
        GOS_SYSMON_MSG_TASK_GET_DATA_RESP_PV = 1,
        GOS_SYSMON_MSG_TASK_GET_VAR_DATA_PV = 1,
        GOS_SYSMON_MSG_TASK_GET_VAR_DATA_RESP_PV = 1,
        GOS_SYSMON_MSG_TASK_MODIFY_STATE_PV = 1,
        GOS_SYSMON_MSG_TASK_MODIFY_STATE_RESP_PV = 1,
        GOS_SYSMON_MSG_SYSRUNTIME_GET_PV = 1,
        GOS_SYSMON_MSG_SYSRUNTIME_GET_RESP_PV = 1,
        GOS_SYSMON_MSG_SYSTIME_SET_PV = 1,
        GOS_SYSMON_MSG_SYSTIME_SET_RESP_PV = 1,
        GOS_SYSMON_MSG_RESET_REQ_PV = 1,
    }

    /// <summary>
    /// System monitoring message results.
    /// </summary>
    public enum SysmonMessageResult
    {
        GOS_SYSMON_MSG_RES_OK = 40,
        GOS_SYSMON_MSG_RES_ERROR = 99,
        GOS_SYSMON_MSG_INV_PV = 35,
        GOS_SYSMON_MSG_INV_PAYLOAD_CRC = 28
    }

    /// <summary>
    /// Task modification types.
    /// </summary>
    public enum SysmonTaskModifyType
    {
        GOS_SYSMON_TASK_MOD_TYPE_SUSPEND = 12,
        GOS_SYSMON_TASK_MOD_TYPE_RESUME = 34,
        GOS_SYSMON_TASK_MOD_TYPE_DELETE = 49,
        GOS_SYSMON_TASK_MOD_TYPE_BLOCK = 52,
        GOS_SYSMON_TASK_MOD_TYPE_UNBLOCK = 63,
        GOS_SYSMON_TASK_MOD_TYPE_WAKEUP = 74
    }

    /// <summary>
    /// Ping message class.
    /// </summary>
    public class SysmonPingMessage
    {
        public SysmonMessageResult MessageResult { get; set; }

        public const UInt16 ExpectedSize = 1;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            MessageResult = (SysmonMessageResult)(bytes[0]);
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return new byte[]
            {
                (byte)(MessageResult)
            };
        }
    }

    /// <summary>
    /// CPU usage message class.
    /// </summary>
    public class CpuUsageMessage
    {
        public SysmonMessageResult MessageResult { get; set; }

        public UInt16 CpuUsage { get; set; }

        public float CpuUsagePercent { get; set; }

        public const UInt16 ExpectedSize = 3;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                MessageResult = (SysmonMessageResult)Helper<byte>.GetVariable(bytes, ref idx);
                CpuUsage = Helper<UInt16>.GetVariable(bytes, ref idx);
                CpuUsagePercent = (float)CpuUsage / 100f;
            }
        }
    }

    public class ChunkDescriptor
    {
        public UInt16 ChunkIndex { get; set; }
        public byte Result { get; set; }

        public const UInt16 ExpectedSize = 3;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                ChunkIndex = Helper<UInt16>.GetVariable(bytes, ref idx);
                Result = Helper<byte>.GetVariable(bytes, ref idx);
            }
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Helper<UInt16>.GetBytes(ChunkIndex));
            bytes.AddRange(Helper<byte>.GetBytes(Result));

            return bytes.ToArray();
        }
    }

    /// <summary>
    /// Task data get message class.
    /// </summary>
    public class TaskDataGetMessage
    {
        public UInt16 TaskIndex { get; set; }

        public const UInt16 ExpectedSize = 2;

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return Helper<UInt16>.GetBytes(TaskIndex);
        }
    }

    /// <summary>
    /// Task data message class.
    /// </summary>
    public class TaskDataMessage
    {
        public SysmonMessageResult MessageResult { get; set; }
        public TaskData TaskData { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                MessageResult = (SysmonMessageResult)(bytes[0]);
                byte[] taskDataByes = new byte[bytes.Length - 1];
                Array.Copy(bytes, 1, taskDataByes, 0, bytes.Length - 1);
                TaskData = new TaskData();
                TaskData.GetFromBytes(taskDataByes);
            }
            else
            {
                MessageResult = SysmonMessageResult.GOS_SYSMON_MSG_RES_ERROR;
            }
        }
    }

    /// <summary>
    /// Task variable data message class.
    /// </summary>
    public class TaskVariableDataMessage
    {
        public SysmonMessageResult MessageResult { get; set; }
        public TaskVariableData TaskVariableData { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                MessageResult = (SysmonMessageResult)(bytes[0]);
                byte[] taskDataByes = new byte[bytes.Length - 1];
                Array.Copy(bytes, 1, taskDataByes, 0, bytes.Length - 1);
                TaskVariableData = new TaskVariableData();
                TaskVariableData.GetFromBytes(taskDataByes);
            }
        }
    }

    /// <summary>
    /// Runtime class.
    /// </summary>
    public class RunTime
    {
        public UInt16 Microseconds { get; set; }
        public UInt16 Milliseconds { get; set; }
        public byte Seconds { get; set; }
        public byte Minutes { get; set; }
        public byte Hours { get; set; }
        public UInt16 Days { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            Microseconds = Helper<UInt16>.GetVariable(bytes, ref idx);
            Milliseconds = Helper<UInt16>.GetVariable(bytes, ref idx);
            Seconds = Helper<byte>.GetVariable(bytes, ref idx);
            Minutes = Helper<byte>.GetVariable(bytes, ref idx);
            Hours = Helper<byte>.GetVariable(bytes, ref idx);
            Days = Helper<UInt16>.GetVariable(bytes, ref idx);
        }
    }

    /// <summary>
    /// Time class.
    /// </summary>
    public class Time
    {
        public UInt16 Milliseconds { get; set; }
        public byte Seconds { get; set; }
        public byte Minutes { get; set; }
        public byte Hours { get; set; }
        public UInt16 Days { get; set; }
        public byte Months { get; set; }
        public UInt16 Years { get; set; }

        public const int ExpectedSize = 2 + 1 + 1 + 1 + 2 + 1 + 2;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            Milliseconds = Helper<UInt16>.GetVariable(bytes, ref idx);
            Seconds = Helper<byte>.GetVariable(bytes, ref idx);
            Minutes = Helper<byte>.GetVariable(bytes, ref idx);
            Hours = Helper<byte>.GetVariable(bytes, ref idx);
            Days = Helper<UInt16>.GetVariable(bytes, ref idx);
            Months = Helper<byte>.GetVariable(bytes,ref idx);
            Years = Helper<UInt16>.GetVariable(bytes,ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Helper<UInt16>.GetBytes(Milliseconds));
            bytes.AddRange(Helper<byte>.GetBytes(Seconds));
            bytes.AddRange(Helper<byte>.GetBytes(Minutes));
            bytes.AddRange(Helper<byte>.GetBytes(Hours));
            bytes.AddRange(Helper<UInt16>.GetBytes(Days));
            bytes.AddRange(Helper<byte>.GetBytes(Months));
            bytes.AddRange(Helper<UInt16>.GetBytes(Years));
            return bytes.ToArray();
        }
    }

    /// <summary>
    /// Task data class.
    /// </summary>
    public class TaskData
    {
        public byte TaskState { get; set; }
        public byte TaskPriority { get; set; }
        public byte TaskOriginalPriority { get; set; }
        public UInt16 TaskPrivileges { get; set; }
        public string TaskName { get; set; }
        public UInt16 TaskId { get; set; }
        public UInt32 TaskCsCounter { get; set; }
        public UInt16 TaskStackSize { get; set; }
        public RunTime TaskRuntime { get; set; }
        public UInt16 TaskCpuUsageLimit { get; set; }
        public UInt16 TaskCpuUsageMax { get; set; }
        public UInt16 TaskCpuUsage { get; set; }
        public UInt16 TaskStackMaxUsage { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            TaskState = Helper<byte>.GetVariable(bytes, ref idx);
            TaskPriority = Helper<byte>.GetVariable(bytes, ref idx);
            TaskOriginalPriority = Helper<byte>.GetVariable(bytes, ref idx);
            TaskPrivileges = Helper<UInt16>.GetVariable(bytes, ref idx);
            TaskName = Helper.GetString(bytes, 32, ref idx);
            TaskId = Helper<UInt16>.GetVariable(bytes, ref idx);
            TaskCsCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
            TaskStackSize = Helper<UInt16>.GetVariable(bytes, ref idx);

            byte[] taskRunTimeBytes = new byte[9];
            Array.Copy(bytes, idx, taskRunTimeBytes, 0, 9);

            TaskRuntime = new RunTime();
            TaskRuntime.GetFromBytes(taskRunTimeBytes);

            idx += taskRunTimeBytes.Length;

            TaskCpuUsageLimit = Helper<UInt16>.GetVariable(bytes, ref idx);
            TaskCpuUsageMax = Helper<UInt16>.GetVariable(bytes, ref idx);
            TaskCpuUsage = Helper<UInt16>.GetVariable(bytes, ref idx);
            TaskStackMaxUsage = Helper<UInt16>.GetVariable(bytes, ref idx);
        }
    }

    /// <summary>
    /// Task variable data class.
    /// </summary>
    public class TaskVariableData
    {
        public byte TaskState { get; set; }
        public byte TaskPriority { get; set; }
        public UInt32 TaskCsCounter { get; set; }
        public RunTime TaskRuntime { get; set; }
        public UInt16 TaskCpuUsageMax { get; set; }
        public UInt16 TaskCpuUsage { get; set; }
        public UInt16 TaskStackMaxUsage { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= 21)
            {
                TaskState = (bytes[0]);
                TaskPriority = (bytes[1]);
                TaskCsCounter = (UInt32)((bytes[5] << 24) + (bytes[4] << 16) + (bytes[3] << 8) + bytes[2]);

                byte[] taskRunTimeBytes = new byte[9];
                Array.Copy(bytes, 6, taskRunTimeBytes, 0, 9);

                TaskRuntime = new RunTime();
                TaskRuntime.GetFromBytes(taskRunTimeBytes);

                TaskCpuUsageMax = (UInt16)((bytes[16] << 8) + bytes[15]);
                TaskCpuUsage = (UInt16)((bytes[18] << 8) + bytes[17]);
                TaskStackMaxUsage = (UInt16)((bytes[20] << 8) + bytes[19]);
            }
        }
    }

    /// <summary>
    /// Task modify message class.
    /// </summary>
    public class TaskModifyMessage
    {
        public UInt16 TaskIndex { get; set; }
        public SysmonTaskModifyType TaskModifyType { get; set; }
        public UInt32 Param { get; set; }

        public const UInt16 ExpectedSize = 2 + 1 + 4;

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return new byte[]
            {
                (byte)(TaskIndex),
                (byte)((int)TaskIndex >> 8),
                (byte)(TaskModifyType),
                (byte)(Param >> 24),
                (byte)(Param >> 16),
                (byte)(Param >> 8),
                (byte)(Param)
            };
        }
    }

    /// <summary>
    /// Task modify message result class.
    /// </summary>
    public class TaskModifyResultMessage
    {
        public SysmonMessageResult MessageResult { get; set; }

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                MessageResult = (SysmonMessageResult)(bytes[0]);
            }
        }
    }

    public class SysRuntimeMessage
    {
        public SysmonMessageResult MessageResult { get; set; }

        public RunTime SysRunTime { get; set; }

        public const UInt16 ExpectedSize = 10;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                MessageResult = (SysmonMessageResult)(bytes[0]);
                byte[] sysruntimeBytes = new byte[9];
                Array.Copy(bytes, 1, sysruntimeBytes, 0, 9);

                SysRunTime = new RunTime();
                SysRunTime.GetFromBytes(sysruntimeBytes);
            }
        }
    }

    public class SysTimeMessage
    {
        public SysmonMessageResult MessageResult { get; set; }
        public Time SystemTime { get; set; } = new Time();

        public const UInt16 ExpectedSize = 10;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            MessageResult = (SysmonMessageResult)(bytes[0]);
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return SystemTime.GetBytes();
        }
    }
}
