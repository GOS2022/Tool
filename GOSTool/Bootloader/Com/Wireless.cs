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

        public static PingResult PingDevice()
        {
            byte[] pingMsg = new byte[2];
            byte[] pingResp = new byte[16];

            PingResult result = PingResult.NOK;

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                pingMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_PING_REQ) >> 8);
                pingMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_PING_REQ) & 0x00ff);

                StatusUpdateEvent?.Invoke(null, "Pinging device...");

                stream.Write(pingMsg, 0, 2);
                stream.Read(pingResp, 0, 16);

                if (pingResp[0] == 40u)
                {
                    result = PingResult.OK;
                    StatusUpdateEvent?.Invoke(null, "Device found.");
                }
                else
                {
                    StatusUpdateEvent?.Invoke(null, "Device error.");
                }
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

        public static string GetSystemRuntime()
        {
            string runtime = "0000:00:00:00";
            RunTime runTime = new RunTime();

            byte[] runTimeMsg = new byte[2];
            byte[] runTimeResp = new byte[64];

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                runTimeMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYS_RUNTIME_REQ) >> 8);
                runTimeMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYS_RUNTIME_REQ) & 0x00ff);

                StatusUpdateEvent?.Invoke(null, "Getting runtime...");

                stream.Write(runTimeMsg, 0, 2);
                stream.Read(runTimeResp, 0, 64);

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
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return runtime;
        }

        public static float GetCpuLoad()
        {
            byte[] cpuLoadMsg = new byte[2];
            byte[] cpuLoadResp = new byte[16];
            cpuLoadResp[0] = 255;
            cpuLoadResp[1] = 255;

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Getting CPU load...");

                cpuLoadMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_CPU_LOAD_REQ) >> 8);
                cpuLoadMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_CPU_LOAD_REQ) & 0x00ff);

                stream.Write(cpuLoadMsg, 0, 2);
                stream.Read(cpuLoadResp, 0, 2);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return (float)((UInt16)(cpuLoadResp[0] + (cpuLoadResp[1] << 8)) / 100f);
        }

        public static TaskVariableData GetTaskVariableData(int taskIndex)
        {
            TaskVariableData taskVarData = null;

            byte[] taskVarDataMsg = new byte[4];
            byte[] taskVarDataResp = new byte[256];

            taskVarDataMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_VAR_DATA_REQ) >> 8);
            taskVarDataMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_VAR_DATA_REQ) & 0x00ff);
            taskVarDataMsg[2] = (byte)taskIndex;
            taskVarDataMsg[3] = (byte)(taskIndex >> 8);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);
            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Getting variable data of task...");

                stream.Write(taskVarDataMsg, 0, 4);
                stream.Read(taskVarDataResp, 0, 256);

                taskVarData = new TaskVariableData();
                taskVarData.GetFromBytes(taskVarDataResp);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return taskVarData;
        }

        public static List<TaskData> GetAllTaskData()
        {
            List<TaskData> taskDatas = new List<TaskData>();
            byte[] taskNumResp = new byte[16];
            byte[] taskDataResp = new byte[256];

            byte[] taskNumMsg = new byte[2];
            taskNumMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_NUM_REQ) >> 8);
            taskNumMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_NUM_REQ) & 0x00ff);

            byte[] taskDataMsg = new byte[4];
            taskDataMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_DATA_REQ) >> 8);
            taskDataMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_TASK_DATA_REQ) & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                int numberOfTasks = 0;

                StatusUpdateEvent?.Invoke(null, "Getting number of tasks...");
                ProgressUpdateEvent?.Invoke(null, 0);

                stream.Write(taskNumMsg, 0, 2);
                stream.Read(taskNumResp, 0, 2);
                numberOfTasks = (taskNumResp[0] + (taskNumResp[1] << 8));

                for (int i = 0; i < numberOfTasks; i++)
                {
                    taskDataMsg[2] = (byte)i;
                    taskDataMsg[3] = (byte)(i >> 8);

                    StatusUpdateEvent?.Invoke(null, "Receiving task data [" + (i+1) + "] / [" + numberOfTasks + "]...");
                    ProgressUpdateEvent?.Invoke(null, (int)((100f * (i + 1) ) / (float)numberOfTasks));

                    stream.Write(taskDataMsg, 0, 4);
                    stream.Read(taskDataResp, 0, 256);

                    TaskData taskData = new TaskData();
                    taskData.GetFromBytes(taskDataResp);

                    taskDatas.Add(taskData);
                }

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return taskDatas;
        }

        public static void SendResetRequest()
        {
            byte[] sysresMessage = new byte[2];
            sysresMessage[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYS_RES) >> 8);
            sysresMessage[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYS_RES) & 0x00ff);

            byte[] dummyBuff = new byte[16];

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Resetting device...");

                stream.Write(sysresMessage, 0, 2);
                stream.Read(dummyBuff, 0, 16);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();
        }

        public static PdhHardwareInfo GetHardwareInfo()
        {
            PdhHardwareInfo hardwareInfo = new PdhHardwareInfo();
            byte[] hwInfoMsg = new byte[2];
            byte[] hwInfoResp = new byte[1024];

            hwInfoMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_HWINFO_REQ) >> 8);
            hwInfoMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_HWINFO_REQ) & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Reading hardware info...");

                stream.Write(hwInfoMsg, 0, 2);
                stream.Read(hwInfoResp, 0, 1024);

                hardwareInfo.GetFromBytes(hwInfoResp);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

            wirelessSemaphore.Release();

            return hardwareInfo;
        }

        public static PdhSoftwareInfo GetSoftwareInfo()
        {
            PdhSoftwareInfo softwareInfo = new PdhSoftwareInfo();
            byte[] swInfoMsg = new byte[2];
            byte[] swInfoResp = new byte[1024];

            swInfoMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SWINFO_REQ) >> 8);
            swInfoMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SWINFO_REQ) & 0x00ff);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;

            wirelessSemaphore.Wait();

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Reading software info...");

                stream.Write(swInfoMsg, 0, 2);
                stream.Read(swInfoResp, 0, 1024);

                softwareInfo.GetFromBytes(swInfoResp);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();

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
           
            byte[] syncTimeMsg = new byte[sysTimeMessage.GetBytes().Length + 2];
            byte[] syncTimeResp = new byte[1024];

            syncTimeMsg[0] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYNC_TIME_REQ) >> 8);
            syncTimeMsg[1] = (byte)((int)(IplMsgId.SVL_IPC_MSG_ID_SYNC_TIME_REQ) & 0x00ff);

            Array.Copy(sysTimeMessage.GetBytes(), 0, syncTimeMsg, 2, sysTimeMessage.GetBytes().Length);

            TcpClient client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            wirelessSemaphore.Wait();

            Thread.Sleep(50);

            try
            {
                client.Connect(Ip, Port);

                NetworkStream stream = client.GetStream();

                StatusUpdateEvent?.Invoke(null, "Synchronizing time...");

                stream.Write(syncTimeMsg, 0, sysTimeMessage.GetBytes().Length + 2);

                StatusUpdateEvent?.Invoke(null, "Idle.");
            }
            catch
            {
                // Nothing.
                StatusUpdateEvent?.Invoke(null, "Communication error.");
            }

            client.Close();
            wirelessSemaphore.Release();
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
    }
}
