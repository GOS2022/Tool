//*************************************************************************************************
//
//                            #####             #####             #####
//                          #########         #########         #########
//                         ##                ##       ##       ##
//                        ##                ##         ##        #####
//                        ##     #####      ##         ##           #####
//                         ##       ##       ##       ##                ##
//                          #########         #########         #########
//                            #####             #####             #####
//
//                                      (c) Ahmed Gazar, 2023
//
//*************************************************************************************************
//! @file       SysmonMessages.cs
//! @author     Ahmed Gazar
//! @date       2023-09-22
//! @version    1.0
//!
//! @brief      System monitoring messages.
//! @details    Implements the messages used by the system monitoring service.
//*************************************************************************************************
// History
// ------------------------------------------------------------------------------------------------
// Version    Date          Author          Description
// ------------------------------------------------------------------------------------------------
// 1.0        2023-09-22    Ahmed Gazar     Initial version created
//*************************************************************************************************
//
// Copyright (c) 2023 Ahmed Gazar
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//*************************************************************************************************
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ = 0x3001,
        SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_RESP = 0x3A01,

        SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ = 0x4001,
        SVL_ERS_SYSMON_MSG_EVENTS_GET_RESP = 0x4A01,
        SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ = 0x4002,
        SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_RESP = 0x4A02,

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
                MessageResult = (SysmonMessageResult)(bytes[0]);
                CpuUsage = (UInt16)((bytes[2] << 8) + bytes[1]);
                CpuUsagePercent = (float)CpuUsage / 100f;
            }
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        /*public byte[] GetBytes()
        {
            return new byte[]
            {
                (byte)(MessageResult),
                (byte)((int)CpuUsage << 8),
                (byte)(CpuUsage),
            };
        }*/
    }

    public enum BinaryDownloadRequestResult
    {
        COMM_ERR = 0,
        OK = 1,
        DESCRIPTOR_SIZE_ERR = 2,
        FILE_SIZE_ERR = 4
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

    public class ErsEvent
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
                int idx = 64;

                byte[] descBytes = new byte[64];
                Array.Copy(bytes, 0, descBytes, 0, 64);

                Description = Encoding.ASCII.GetString(descBytes).Substring(0, Encoding.ASCII.GetString(descBytes).IndexOf("\0"));

                Time time = new Time();
                time.GetFromBytes(bytes.Skip(idx).Take(Time.ExpectedSize).ToArray());
                try
                {
                    TimeStamp = DateTime.Parse(time.Years.ToString("D4") + "-" + time.Months.ToString("D2") + "-" + time.Days.ToString("D2") + " " +
                        time.Hours.ToString("D2") + ":" + time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2") + "." + time.Milliseconds.ToString("D3"));
                }
                catch
                {
                    TimeStamp = new DateTime();
                }


                idx += Time.ExpectedSize;

                Trigger = Helper<UInt32>.GetVariable(bytes, ref idx);

                EventData = bytes.Skip(idx).Take(8).ToList();
            }
        }
    }

    public enum MdiVariableType
    {
        Byte,
        Word,
        LWord,
        Float
    }

    public class MdiVariable
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public MdiVariableType Type { get; private set; }

        public const UInt16 ExpectedSize = 21;

        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 16;

                byte[] nameBytes = new byte[16];
                Array.Copy(bytes, 0, nameBytes, 0, 16);

                Name = Encoding.ASCII.GetString(nameBytes).Substring(0, Encoding.ASCII.GetString(nameBytes).IndexOf("\0"));
                Type = (MdiVariableType)Enum.Parse(typeof(MdiVariableType), Helper<byte>.GetVariable(bytes, ref idx).ToString());
                
                switch(Type)
                {
                    case MdiVariableType.Byte:
                    case MdiVariableType.Word:
                    case MdiVariableType.LWord:
                        {
                            Value = Helper<UInt32>.GetVariable(bytes, ref idx).ToString();
                            break;
                        }
                    case MdiVariableType.Float:
                        {
                            Value = BitConverter.ToSingle(bytes, idx).ToString();
                            break;
                        }
                    default: break;
                }
            }
        }
    }

    public class DeviceDescriptor
    {
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
            //if (bytes.Length >= ExpectedSize)
            {
                int idx = 32;

                byte[] nameBytes = new byte[32];
                Array.Copy(bytes, 0, nameBytes, 0, 32);

                Name = Encoding.ASCII.GetString(nameBytes).Substring(0, Encoding.ASCII.GetString(nameBytes).IndexOf("\0"));
                
                byte[] descBytes = new byte[64];
                Array.Copy(bytes, idx, descBytes, 0, 64);
                Description = Encoding.ASCII.GetString(descBytes).Substring(0, Encoding.ASCII.GetString(descBytes).IndexOf("\0"));

                idx += 64;
                DeviceId = Helper<UInt32>.GetVariable(bytes, ref idx);
                idx += 4;
                byte devType = Helper<byte>.GetVariable(bytes, ref idx);
                DeviceType = "Unknown";
                switch(devType)
                {
                    case 0: DeviceType = "Read only"; break;
                    case 1: DeviceType = "Write only"; break;
                    case 2: DeviceType = "Read / Write"; break;
                    case 3: DeviceType = "Virtual"; break;
                }

                Enabled = Helper<bool>.GetVariable(bytes, ref idx);
                idx += 4;
                idx += 4;
                idx += 4*4;
                idx += 4*4;
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

    public class BinaryDescriptorMessage
    {
        public string Name { get; set; }
        public UInt32 Location { get; set; }
        public UInt32 StartAddress { get; set; }
        public UInt32 Size { get; set; }
        public UInt32 Crc { get; set; }

        public const UInt16 ExpectedSize = 16;

        /// <summary>
        /// Gets the class member values from the byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void GetFromBytes(byte[] bytes)
        {
            if (bytes.Length >= ExpectedSize)
            {
                int idx = 32;

                byte[] nameBytes = new byte[32];
                Array.Copy(bytes, 0, nameBytes, 0, 32);

                Name = Encoding.ASCII.GetString(nameBytes).Substring(0, Encoding.ASCII.GetString(nameBytes).IndexOf("\0"));
                Location = Helper<UInt32>.GetVariable(bytes, ref idx);
                StartAddress = Helper<UInt32>.GetVariable(bytes, ref idx);
                Size = Helper<UInt32>.GetVariable(bytes, ref idx);
                Crc = Helper<UInt32>.GetVariable(bytes, ref idx);
            }
        }

        /// <summary>
        /// Returns the message as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            List<byte> nameBytes = new List<byte>(Encoding.ASCII.GetBytes(Name));
            while (nameBytes.Count < 32)
                nameBytes.Add(0);
            
            bytes.AddRange(nameBytes);
            bytes.AddRange(Helper<UInt32>.GetBytes(Location));
            bytes.AddRange(Helper<UInt32>.GetBytes(StartAddress));
            bytes.AddRange(Helper<UInt32>.GetBytes(Size));
            bytes.AddRange(Helper<UInt32>.GetBytes(Crc));

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
            return new byte[]
            {
                (byte)(TaskIndex),
                (byte)((int)TaskIndex >> 8),
            };
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
            Microseconds = (UInt16)((bytes[1] << 8) + bytes[0]);
            Milliseconds = (UInt16)((bytes[3] << 8) + bytes[2]);
            Seconds = bytes[4];
            Minutes = bytes[5];
            Hours = bytes[6];
            Days = (UInt16)((bytes[8] << 8) + bytes[7]);
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
            Milliseconds = (UInt16)((bytes[1] << 8) + bytes[0]);
            Seconds = bytes[2];
            Minutes = bytes[3];
            Hours = bytes[4];
            Days = (UInt16)((bytes[6] << 8) + bytes[5]);
            Months = bytes[7];
            Years = (UInt16)((bytes[9] << 8) + bytes[8]);
        }

        public byte[] GetBytes()
        {
            return new byte[]
            {
                (byte)(Milliseconds),
                (byte)(Milliseconds >> 8),
                Seconds,
                Minutes,
                Hours,
                (byte)(Days),
                (byte)(Days >> 8),
                Months,
                (byte)(Years),
                (byte)(Years >> 8)
            };
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
            TaskState = (bytes[0]);
            TaskPriority = (bytes[1]);
            TaskOriginalPriority = (bytes[2]);
            TaskPrivileges = (UInt16)((bytes[4] << 8) + bytes[3]);

            byte[] taskNameBytes = new byte[32];
            Array.Copy(bytes, 5, taskNameBytes, 0, 32);

            TaskName = Encoding.ASCII.GetString(taskNameBytes).Substring(0, Encoding.ASCII.GetString(taskNameBytes).IndexOf("\0"));
            TaskId = (UInt16)((bytes[38] << 8) + bytes[37]);
            TaskCsCounter = (UInt32)((bytes[42] << 24) + (bytes[41] << 16) + (bytes[40] << 8) + bytes[39]);
            TaskStackSize = (UInt16)((bytes[44] << 8) + bytes[43]);

            byte[] taskRunTimeBytes = new byte[9];
            Array.Copy(bytes, 45, taskRunTimeBytes, 0, 9);

            TaskRuntime = new RunTime();
            TaskRuntime.GetFromBytes(taskRunTimeBytes);

            TaskCpuUsageLimit = (UInt16)((bytes[55] << 8) + bytes[54]);
            TaskCpuUsageMax = (UInt16)((bytes[57] << 8) + bytes[56]);
            TaskCpuUsage = (UInt16)((bytes[59] << 8) + bytes[58]);
            TaskStackMaxUsage = (UInt16)((bytes[61] << 8) + bytes[60]);
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
