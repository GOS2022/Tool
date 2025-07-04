using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static GOSTool.SysmonFunctions;

namespace GOSTool
{
    public enum IplMsgId
    {
        SVL_IPC_MSG_ID_SYS_RES = 0x100A,
        SVL_IPC_MSG_ID_CPU_LOAD_REQ = 0x100B,
        SVL_IPC_MSG_ID_CPU_LOAD_RESP = 0x600B,
        SVL_IPC_MSG_ID_TASK_NUM_REQ = 0x100C,
        SVL_IPC_MSG_ID_TASK_NUM_RESP = 0x600C,
        SVL_IPC_MSG_ID_TASK_DATA_REQ = 0x100D,
        SVL_IPC_MSG_ID_TASK_DATA_RESP = 0x600D,
        SVL_IPC_MSG_ID_PING_REQ = 0x100E,
        SVL_IPC_MSG_ID_PING_RESP = 0x600E,
        SVL_IPC_MSG_ID_SYS_RUNTIME_REQ = 0x100F,
        SVL_IPC_MSG_ID_SYS_RUNTIME_RESP = 0x600F,
        SVL_IPC_MSG_ID_SWINFO_REQ = 0x1010,
        SVL_IPC_MSG_ID_SWINFO_RESP = 0x6010,
        SVL_IPC_MSG_ID_TASK_VAR_DATA_REQ = 0x1011,
        SVL_IPC_MSG_ID_TASK_VAR_DATA_RESP = 0x6011,
        SVL_IPC_MSG_ID_TASK_MODIFY_REQ = 0x1012,
        SVL_IPC_MSG_ID_TASK_MODIFY_RESP = 0x6012,
        SVL_IPC_MSG_ID_HWINFO_REQ = 0x1013,
        SVL_IPC_MSG_ID_HWINFO_RESP = 0x6013,
        SVL_IPC_MSG_ID_SYNC_TIME_REQ = 0x1014,
        SVL_IPC_MSG_ID_SYNC_TIME_RESP = 0x6014,

        MSG_BINARY_NUM_REQ_ID = (0x1015),
        MSG_BINARY_NUM_RESP_ID = (0x6015),
        MSG_BINARY_INFO_REQ_ID = (0x1016),
        MSG_BINARY_INFO_RESP_ID = (0x6016),
        MSG_DOWNLOAD_REQ_ID = (0x1017),
        MSG_DOWNLOAD_RESP_ID = (0x6017),
        MSG_BINARY_CHUNK_REQ_ID = (0x1018),
        MSG_BIANRY_CHUNK_RESP_ID = (0x6018),
        MSG_INSTALL_REQ_ID = (0x1019),
        MSG_INSTALL_RESP_ID = (0x6019),
        MSG_ERASE_REQ_ID = (0x1020),
        MSG_ERASE_RESP_ID = (0x6020),
    }

    public enum IplTaskModificationType
    {
        IPL_TASK_MODIFY_SUSPEND = 0, //!< Suspend.
        IPL_TASK_MODIFY_RESUME = 1, //!< Resume.
        IPL_TASK_MODIFY_DELETE = 2, //!< Delete.
        IPL_TASK_MODIFY_BLOCK = 3, //!< Block.
        IPL_TASK_MODIFY_UNBLOCK = 4, //!< Unblock.
        IPL_TASK_MODIFY_WAKEUP = 5  //!< Wake up.
    }

    public class Wireless
    {
        public static EventHandler<string> StatusUpdateEvent;

        public static EventHandler<int> ProgressUpdateEvent;

        private static SemaphoreSlim wirelessSemaphore = new SemaphoreSlim(1, 1);
        public static string Ip { get; set; } = "192.168.100.184";
        public static int Port { get; set; } = 3000;
        private static TcpClient _client;
        private static NetworkStream _stream;

        private static void SetMessageData (ref byte[] msgArray, UInt16 msgId, UInt16 msgLength)
        {
            if (msgArray.Length >= 4)
            {
                msgArray[0] = (byte)(msgId >> 8);
                msgArray[1] = (byte)(msgId & 0x00ff);
                msgArray[2] = (byte)(msgLength >> 8);
                msgArray[3] = (byte)(msgLength & 0x00ff);
            }
        }

        public static void Connect()
        {
            Disconnect();
            _client = new TcpClient();
            _client.ReceiveTimeout = 1000;
            _client.SendTimeout = 1000;

            _client.Connect(Ip, Port);
            _stream = _client.GetStream();
        }

        public static void Disconnect()
        {
            if (!(_client is null))
                _client.Close();
        }

        private static byte[] SendReceive (SysmonMessageId messageId, byte[] payload)
        {
            int txSize = 4;
            byte[] rxBuffer = new byte[1024];

            if (!(payload is null))
            {
                txSize += payload.Length;
            }

            byte[] txBuffer = new byte[txSize];

            SetMessageData(ref txBuffer, (UInt16)messageId, (UInt16)(txSize - 4));

            if (!(payload is null))
            {
                Array.Copy(payload, 0, txBuffer, 4, payload.Length);
            }

            wirelessSemaphore.Wait();

            try
            {
                _stream.Write(txBuffer, 0, txBuffer.Length);
                _stream.Read(rxBuffer, 0, rxBuffer.Length);
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            wirelessSemaphore.Release();

            return rxBuffer;
        }

        public static PingResult PingDevice()
        {
            PingResult result = PingResult.NOK;

            StatusUpdateEvent?.Invoke(null, "Pinging device...");

            byte[] pingResp = SendReceive(SysmonMessageId.GOS_SYSMON_MSG_PING_ID, null);

            if (pingResp[0] == 40u)
            {
                result = PingResult.OK;
                StatusUpdateEvent?.Invoke(null, "Device found.");
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, "Device error.");
            }

            return result;
        }

        public static string GetSystemRuntime()
        {
            string runtime = "0000:00:00:00";
            RunTime runTime = new RunTime();

            StatusUpdateEvent?.Invoke(null, "Getting runtime...");

            byte[] runTimeResp = SendReceive(SysmonMessageId.GOS_SYSMON_MSG_SYSRUNTIME_GET_ID, null);

            try
            {
                runTime.GetFromBytes(runTimeResp);

                runtime = runTime.Days.ToString("D2") + ":" +
                    runTime.Hours.ToString("D2") + ":" +
                    runTime.Minutes.ToString("D2") + ":" +
                    runTime.Seconds.ToString("D2") + "." +
                    runTime.Milliseconds.ToString("D3");

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Error.
            }

            return runtime;
        }

        public static float GetCpuLoad()
        {
            byte[] cpuLoadMsg = new byte[4];

            StatusUpdateEvent?.Invoke(null, "Getting CPU load...");

            byte[] cpuLoadResp = SendReceive(SysmonMessageId.GOS_SYSMON_MSG_CPU_USAGE_GET_ID, null);

            StatusUpdateEvent?.Invoke(null, "Idle.");

            return (float)((UInt16)(cpuLoadResp[1] + (cpuLoadResp[2] << 8)) / 100f);
        }

        public static TaskVariableData GetTaskVariableData(int taskIndex)
        {
            byte[] payload = new byte[] { (byte)(taskIndex & 0x00ff), (byte)(taskIndex >> 8) };
            byte[] taskVarDataResp = SendReceive(SysmonMessageId.GOS_SYSMON_MSG_TASK_GET_VAR_DATA_ID, payload);

            TaskVariableData taskVarData = new TaskVariableData();
            taskVarData.GetFromBytes(taskVarDataResp.Skip(1).ToArray());

            StatusUpdateEvent?.Invoke(null, "Idle.");

            return taskVarData;
        }

        public static List<TaskData> GetAllTaskData()
        {
            List<TaskData> taskDatas = new List<TaskData>();
            byte[] taskDataGetMsg = new byte[6];
            byte[] taskDataGetResp = new byte[256];
            int taskIndex = 0;

            wirelessSemaphore.Wait();

            try
            {
                SetMessageData(ref taskDataGetMsg, (UInt16)SysmonMessageId.GOS_SYSMON_MSG_TASK_GET_DATA_ID, 2);

                while (true)
                {
                    StatusUpdateEvent?.Invoke(null, "Getting task data [" + taskIndex + "]...");

                    taskDataGetMsg[5] = (byte)(taskIndex >> 8);
                    taskDataGetMsg[4] = (byte)(taskIndex & 0x00ff);

                    _stream.Write(taskDataGetMsg, 0, 6);
                    _stream.Read(taskDataGetResp, 0, 256);

                    TaskData taskData = new TaskData();
                    taskData.GetFromBytes(taskDataGetResp.Skip(1).ToArray());

                    if (taskDataGetResp[0] == 40)
                    {
                        taskDatas.Add(taskData);
                        taskIndex++;
                    }
                    else
                    {
                        break;
                    }                    
                }

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            wirelessSemaphore.Release();

            return taskDatas;
        }

        public static void SendResetRequest()
        {
            byte[] sysresMessage = new byte[4];

            SetMessageData(ref sysresMessage, (UInt16)SysmonMessageId.GOS_SYSMON_MSG_RESET_REQ_ID, 2);

            byte[] dummyBuff = new byte[256];

            wirelessSemaphore.Wait();

            try
            {
                StatusUpdateEvent?.Invoke(null, "Resetting device...");

                _stream.Write(sysresMessage, 0, 4);
                _stream.Read(dummyBuff, 0, 256);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            wirelessSemaphore.Release();
        }

        public static PdhHardwareInfo GetHardwareInfo()
        {
            PdhHardwareInfo hardwareInfo = new PdhHardwareInfo();
            byte[] hwInfoMsg = new byte[4];
            byte[] hwInfoResp = new byte[1024];

            wirelessSemaphore.Wait();

            try
            {
                SetMessageData(ref hwInfoMsg, (UInt16)SysmonMessageId.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_REQ, 0);

                StatusUpdateEvent?.Invoke(null, "Reading hardware info...");

                _stream.Write(hwInfoMsg, 0, 4);
                _stream.Read(hwInfoResp, 0, 1024);

                hardwareInfo.GetFromBytes(hwInfoResp);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            wirelessSemaphore.Release();

            return hardwareInfo;
        }

        public static PdhSoftwareInfo GetSoftwareInfo()
        {
            PdhSoftwareInfo softwareInfo = new PdhSoftwareInfo();
            byte[] swInfoMsg = new byte[4];
            byte[] swInfoResp = new byte[1024];

            wirelessSemaphore.Wait();

            try
            {
                SetMessageData(ref swInfoMsg, (UInt16)SysmonMessageId.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_REQ, 0);

                StatusUpdateEvent?.Invoke(null, "Reading software info...");

                _stream.Write(swInfoMsg, 0, 4);
                _stream.Read(swInfoResp, 0, 1024);

                softwareInfo.GetFromBytes(swInfoResp);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }


            wirelessSemaphore.Release();

            return softwareInfo;
        }

        public static void SynchronizeTime ()
        {
            SysTimeMessage sysTimeMessage = new SysTimeMessage();
            sysTimeMessage.SystemTime.Years = (ushort)DateTime.Now.Year;
            sysTimeMessage.SystemTime.Months = (byte)DateTime.Now.Month;
            sysTimeMessage.SystemTime.Days = (byte)DateTime.Now.Day;
            sysTimeMessage.SystemTime.Hours = (byte)DateTime.Now.Hour;
            sysTimeMessage.SystemTime.Minutes = (byte)DateTime.Now.Minute;
            sysTimeMessage.SystemTime.Seconds = (byte)DateTime.Now.Second;
            sysTimeMessage.SystemTime.Milliseconds = (byte)DateTime.Now.Millisecond;

            byte[] payload = new byte[sysTimeMessage.GetBytes().Length];

            StatusUpdateEvent?.Invoke(null, "Synchronizing time...");

            Array.Copy(sysTimeMessage.GetBytes(), 0, payload, 0, sysTimeMessage.GetBytes().Length);

            byte[] syncTimeResp = SendReceive(SysmonMessageId.APP_SYSMON_MSG_RTC_SET_REQ, payload);

            StatusUpdateEvent?.Invoke(null, "Idle.");
        }

        public static bool ModifyTask (int taskIndex, IplTaskModificationType modificationType)
        {
            byte[] modifyMsg = new byte[6];
            byte[] modifyResp = new byte[1024];
            bool result = false;

            modifyMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_MODIFY_REQ) >> 8);
            modifyMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_MODIFY_REQ) & 0x00ff);
            modifyMsg[2] = (byte)taskIndex;
            modifyMsg[3] = (byte)(taskIndex >> 8);
            modifyMsg[4] = (byte)modificationType;
            modifyMsg[5] = 10;

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);
            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Modifying task {" + taskIndex + "} with command " + modificationType.ToString() + "...");

                stream.Write(modifyMsg, 0, 6);
                stream.Read(modifyResp, 0, 256);

                if (modifyResp[5] == 0)
                    result = true;

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return result;
        }

        public static int GetBinaryNum()
        {
            int binaryNum = 0;
            byte[] binaryNumMsg = new byte[2];
            byte[] binaryNumResp = new byte[1024];

            binaryNumMsg[0] = (byte)((int)(IplMsgId.MSG_BINARY_NUM_REQ_ID) >> 8);
            binaryNumMsg[1] = (byte)((int)(IplMsgId.MSG_BINARY_NUM_REQ_ID) & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                stream.Write(binaryNumMsg, 0, 2);
                stream.Read(binaryNumResp, 0, 1024);

                binaryNum = ((binaryNumResp[1] << 8) + binaryNumResp[0]);
            }
            catch
            {
                // Nothing.
            }

            client.Close();

            wirelessSemaphore.Release();

            return binaryNum;
        }

        public static SdhBinaryDescriptorMessage GetBinaryInfo(int index)
        {
            SdhBinaryDescriptorMessage binaryInfo = new SdhBinaryDescriptorMessage();

            byte[] binaryInfoMsg = new byte[4];
            byte[] binaryInfoResp = new byte[1024];

            binaryInfoMsg[0] = (byte)((int)(IplMsgId.MSG_BINARY_INFO_REQ_ID) >> 8);
            binaryInfoMsg[1] = (byte)((int)(IplMsgId.MSG_BINARY_INFO_REQ_ID) & 0x00ff);
            binaryInfoMsg[3] = (byte)(index >> 8);
            binaryInfoMsg[2] = (byte)(index & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                stream.Write(binaryInfoMsg, 0, 4);
                stream.Read(binaryInfoResp, 0, 1024);

                binaryInfo.GetFromBytes(binaryInfoResp);
            }
            catch
            {
                // Nothing.
            }

            client.Close();

            wirelessSemaphore.Release();

            return binaryInfo;
        }

        public static SdhBinaryDownloadRequestResult SendBinaryDownloadRequest(SdhBinaryDescriptorMessage binaryDescriptor)
        {
            SdhBinaryDownloadRequestResult result = SdhBinaryDownloadRequestResult.COMM_ERR;
            byte[] reqMsg = new byte[binaryDescriptor.GetBytes().Length + 2];
            byte[] respMsg = new byte[1024];

            reqMsg[0] = (byte)((int)(IplMsgId.MSG_DOWNLOAD_REQ_ID) >> 8);
            reqMsg[1] = (byte)((int)(IplMsgId.MSG_DOWNLOAD_REQ_ID) & 0x00ff);

            Array.Copy(binaryDescriptor.GetBytes(), 0, reqMsg, 2, binaryDescriptor.GetBytes().Length);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                stream.Write(reqMsg, 0, reqMsg.Length);
                stream.Read(respMsg, 0, 1024);

                result = (SdhBinaryDownloadRequestResult)respMsg[0];
            }
            catch
            {
                // Nothing.
            }

            client.Close();

            wirelessSemaphore.Release();

            return result;
        }

        public static bool SendBinary(List<byte> bytes)
        {
            bool res = false;
            int chunkSize = 1024;// 2048;
            bool repeat = false;
            ChunkDescriptor chunkDesc = new ChunkDescriptor();

            wirelessSemaphore.Wait();

            int chunks = bytes.Count / chunkSize + (bytes.Count % chunkSize == 0 ? 0 : 1);

            for (int chunkCounter = 0; chunkCounter < chunks; chunkCounter++)
            {               
                if (repeat)
                {
                    chunkCounter--;
                }

                chunkDesc.ChunkIndex = (UInt16)chunkCounter;

                byte[] reqMsg = new byte[chunkDesc.GetBytes().Length + chunkSize + 2];
                byte[] respMsg = new byte[1024];

                reqMsg[0] = (byte)((int)(IplMsgId.MSG_BINARY_CHUNK_REQ_ID) >> 8);
                reqMsg[1] = (byte)((int)(IplMsgId.MSG_BINARY_CHUNK_REQ_ID) & 0x00ff);

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

                Array.Copy(payload.ToArray(), 0, reqMsg, 2, payload.Count());

                TcpClient client = new TcpClient();
                client.ReceiveTimeout = 5000;
                client.SendTimeout = 5000;

                try
                {
                    client.Connect(Ip, Port);

                    NetworkStream stream = client.GetStream();

                    stream.Write(reqMsg, 0, reqMsg.Length);
                    stream.Read(respMsg, 0, 1024);

                    chunkDesc.GetFromBytes(respMsg);

                    if (chunkDesc.Result != 1)
                    {
                        repeat = true;
                    }
                    else
                    {
                        res = true;
                        SvlSdh.BinaryDownloadProgressEvent?.Invoke(null, (chunkCounter + 1, chunks));
                        repeat = false;
                    }
                }
                catch
                {
                    // Nothing.
                    res = false;
                    break;
                }

                client.Close();
            }

            wirelessSemaphore.Release();

            Thread.Sleep(1000);

            return res;
        }

        public static bool SendInstallRequest(int index)
        {
            bool retval = false;
            byte[] reqMsg = new byte[4];
            byte[] respMsg = new byte[1024];

            reqMsg[0] = (byte)((int)(IplMsgId.MSG_INSTALL_REQ_ID) >> 8);
            reqMsg[1] = (byte)((int)(IplMsgId.MSG_INSTALL_REQ_ID) & 0x00ff);
            reqMsg[3] = (byte)(index >> 8);
            reqMsg[2] = (byte)(index & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                stream.Write(reqMsg, 0, reqMsg.Length);
                stream.Read(respMsg, 0, 1024);

                int idx = 0;
                int rxIdx = Helper<UInt16>.GetVariable(respMsg, ref idx);

                if (rxIdx == index)
                    retval = true;
            }
            catch
            {
                // Nothing.
            }
            
            client.Close();

            wirelessSemaphore.Release();
            return retval;
        }

        public static bool SendEraseRequest(int index)
        {
            bool retval = false;
            byte[] reqMsg = new byte[5];
            byte[] respMsg = new byte[1024];

            reqMsg[0] = (byte)((int)(IplMsgId.MSG_ERASE_REQ_ID) >> 8);
            reqMsg[1] = (byte)((int)(IplMsgId.MSG_ERASE_REQ_ID) & 0x00ff);
            reqMsg[3] = (byte)(index >> 8);
            reqMsg[2] = (byte)(index & 0x00ff);
            reqMsg[4] = (byte)(73);


            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 15000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                stream.Write(reqMsg, 0, reqMsg.Length);
                stream.Read(respMsg, 0, 1024);

                int idx = 0;
                int rxIdx = Helper<UInt16>.GetVariable(respMsg, ref idx);

                if (rxIdx == index)
                    retval = true;
            }
            catch
            {
                // Nothing.
            }

            client.Close();

            wirelessSemaphore.Release();

            return retval;
        }

        public static int GetDeviceNum()
        {
            int devNum = 0;

            StatusUpdateEvent?.Invoke(null, "Getting number of devices...");

            byte[] devNumResp = SendReceive(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ, null);

            StatusUpdateEvent?.Invoke(null, "Idle.");

            devNum = ((devNumResp[1] << 8) + devNumResp[0]);

            return devNum;
        }

        public static DhsDeviceDescriptor GetDeviceInfo(int devIndex)
        {
            byte[] payload = new byte[] { (byte)(devIndex & 0x00ff), (byte)(devIndex >> 8) };
            byte[] devInfoResp = SendReceive(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ, payload);

            StatusUpdateEvent?.Invoke(null, "Getting device info of device: [" + devIndex + "] ...");

            DhsDeviceDescriptor devInfo = new DhsDeviceDescriptor();
            devInfo.GetFromBytes(devInfoResp.ToArray());

            StatusUpdateEvent?.Invoke(null, "Idle.");

            return devInfo;
        }
    }
}
