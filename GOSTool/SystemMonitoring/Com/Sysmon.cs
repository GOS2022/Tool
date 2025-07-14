using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool.SystemMonitoring.Com
{
    public class SysmonEventArgs : EventArgs
    {
        public SysmonEventArgs(params object[] args)
        {
            Args = args;
        }

        public object[] Args { get; set; }
    }

    public class SysmonStatusUpdateEventArgs : EventArgs
    {
        public SysmonStatusUpdateEventArgs(bool isWireless, string status)
        {
            IsWireless = isWireless;
            Status = status;
        }

        public bool IsWireless { get; set; }

        public string Status { get; set; }
    }

    public class SysmonProgressUpdateEventArgs : EventArgs
    {
        public SysmonProgressUpdateEventArgs(bool isWireless, int percentage)
        {
            IsWireless = isWireless;
            Percentage = percentage;
        }

        public bool IsWireless { get; set; }

        public int Percentage { get; set; }
    }

    public class Sysmon
    {
        private static bool _isWiredInitialized = false;
        private static bool _isWirelessInitialized = false;

        private static SemaphoreSlim _wiredSemaphore = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _wirelessSemaphore = new SemaphoreSlim(1, 1);

        private static TcpClient _client;
        private static NetworkStream _stream;

        private static string _ip = "192.168.100.184";
        public static string Ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                _isWirelessInitialized = false;
            }
        }
        private static int _wirelessPort = 3000;
        public static int WirelessPort 
        {
            get
            {
                return _wirelessPort;
            }
            set
            {
                _wirelessPort = value;
                _isWirelessInitialized = false;
            }
        } 
        public static int SendTmo { get; set; } = 1000;
        public static int ReceiveTmo { get; set; } = 3000;
        private static string _serialPort = "COM1";
        public static string SerialPort 
        { 
            get
            {
                return _serialPort;
            }
            set
            {
                _serialPort = value;
                _isWiredInitialized = false;
            }
        }
        private static int _baud = 115200;
        public static int Baud
        {
            get
            {
                return _baud;
            }
            set
            {
                _baud = value;
                _isWiredInitialized = false;
            }
        }

        public static EventHandler<SysmonStatusUpdateEventArgs> StatusUpdateEvent;
        public static EventHandler<SysmonProgressUpdateEventArgs> ProgressUpdateEvent;
        public static EventHandler<(int, int)> BinaryDownloadProgressEvent;


        public static void InitializeWired()
        {
            Uart.Init(SerialPort, Baud);
            _isWiredInitialized = true;
        }

        public static void DeInitializeWired()
        {
            Uart.DeInit();
            _isWiredInitialized = false;
        }

        public static void InitializeWireless()
        {
            DeInitializeWireless();
            _client = new TcpClient();
            _client.ReceiveTimeout = ReceiveTmo;
            _client.SendTimeout = SendTmo;

            try
            {
                _client.Connect(Ip, WirelessPort);
                _stream = _client.GetStream();
                _isWirelessInitialized = true;
            }
            catch
            {

            }          
        }

        public static void DeInitializeWireless()
        {
            if (!(_client is null))
                _client.Close();

            _isWirelessInitialized = false;
        }

        public static bool SvlSysmon_PingDevice(bool isWireless)
        {
            SysmonPingMessage sysmonPingMessage = new SysmonPingMessage();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Pinging device..."));

            if (isWireless)
            {
                sysmonPingMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_PING_ID, null));
            }
            else
            {
                sysmonPingMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_PING_ID, null, SysmonMessagePv.GOS_SYSMON_MSG_PING_PV));
            }

            if (sysmonPingMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Device found."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Device error."));
            }

            return sysmonPingMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK;
        }

        public static List<TaskData> SvlSysmon_GetAllTaskData(bool isWireless)
        {
            List<TaskData> taskDatas = new List<TaskData>();
            TaskDataGetMessage taskDataGetMessage = new TaskDataGetMessage();
            TaskDataMessage taskDataMessage = new TaskDataMessage();
            taskDataGetMessage.TaskIndex = 0;

            while (true)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting task data [" + taskDataGetMessage.TaskIndex + "]..."));

                if (isWireless)
                {
                    taskDataMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_TASK_GET_DATA_ID, taskDataGetMessage.GetBytes()));
                }
                else
                {
                    taskDataMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_TASK_GET_DATA_ID, taskDataGetMessage.GetBytes(), SysmonMessagePv.GOS_SYSMON_MSG_TASK_GET_DATA_PV));
                }

                if (taskDataMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
                {
                    taskDatas.Add(taskDataMessage.TaskData);
                }
                else
                {
                    break;
                }

                taskDataGetMessage.TaskIndex++;
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Idle."));

            return taskDatas;
        }

        public static TaskVariableData SvlSysmon_GetTaskVariableData(int taskIndex, bool isWireless)
        {
            TaskVariableDataMessage taskDataMessage = new TaskVariableDataMessage();
            TaskDataGetMessage taskDataGetMessage = new TaskDataGetMessage();

            taskDataGetMessage.TaskIndex = (UInt16)taskIndex;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting task variable data of task [" + taskDataGetMessage.TaskIndex + "]..."));

            if (isWireless)
            {
                taskDataMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_TASK_GET_VAR_DATA_ID, taskDataGetMessage.GetBytes()));
            }
            else
            {
                taskDataMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_TASK_GET_VAR_DATA_ID, taskDataGetMessage.GetBytes(), SysmonMessagePv.GOS_SYSMON_MSG_TASK_GET_VAR_DATA_PV));
            }

            if (!(taskDataMessage.TaskVariableData is null))
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Task variable data received successfully."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Communication error."));
            }

            return taskDataMessage.TaskVariableData;
        }

        public static float SvlSysmon_GetCpuLoad(bool isWireless)
        {
            CpuUsageMessage cpuUsageMessage = new CpuUsageMessage();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting CPU load..."));

            if (isWireless)
            {
                cpuUsageMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_CPU_USAGE_GET_ID, null));
            }
            else
            {
                cpuUsageMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_CPU_USAGE_GET_ID, null, SysmonMessagePv.GOS_SYSMON_MSG_CPU_USAGE_GET_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "CPU load received successfully."));

            return cpuUsageMessage.CpuUsagePercent;
        }

        public static string SvlSysmon_GetSystemRuntime(bool isWireless)
        {
            string runtime = "0000:00:00:00";
            SysRuntimeMessage sysRuntimeMessage = new SysRuntimeMessage();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting system runtime..."));

            if (isWireless)
            {
                sysRuntimeMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_SYSRUNTIME_GET_ID, null));
            }
            else
            {
                sysRuntimeMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_SYSRUNTIME_GET_ID, null, SysmonMessagePv.GOS_SYSMON_MSG_SYSRUNTIME_GET_PV));
            }

            if (sysRuntimeMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
            {
                runtime = sysRuntimeMessage.SysRunTime.Days.ToString("D2") + ":" +
                    sysRuntimeMessage.SysRunTime.Hours.ToString("D2") + ":" +
                    sysRuntimeMessage.SysRunTime.Minutes.ToString("D2") + ":" +
                    sysRuntimeMessage.SysRunTime.Seconds.ToString("D2") + "." +
                    sysRuntimeMessage.SysRunTime.Milliseconds.ToString("D3");

                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "System runtime received successfully."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Communication error."));
            }

            return runtime;
        }

        public static void SvlSysmon_SendResetRequest(bool isWireless)
        {
            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Sending reset request..."));

            if (isWireless)
            {
                SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_RESET_REQ_ID, null);
            }
            else
            {
                SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_RESET_REQ_ID, null, SysmonMessagePv.GOS_SYSMON_MSG_RESET_REQ_PV);
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Reset request sent."));
        }

        public static void SvlSysmon_ModifyTask(int taskIndex, SysmonTaskModifyType modifyType, bool isWireless)
        {
            TaskModifyMessage taskModifyMessage = new TaskModifyMessage();
            taskModifyMessage.TaskIndex = (UInt16)taskIndex;
            taskModifyMessage.TaskModifyType = modifyType;
            TaskModifyResultMessage taskModifyResultMessage = new TaskModifyResultMessage();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Modifying task state of task ]" + taskIndex + "]..."));

            if (isWireless)
            {
                taskModifyResultMessage.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SYSMON_MSG_TASK_MODIFY_STATE_ID, taskModifyMessage.GetBytes()));
            }
            else
            {
                taskModifyResultMessage.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SYSMON_MSG_TASK_MODIFY_STATE_ID, taskModifyMessage.GetBytes(), SysmonMessagePv.GOS_SYSMON_MSG_TASK_MODIFY_STATE_PV));
            }

            if (taskModifyResultMessage.MessageResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Task state modification successful."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Communication error."));
            }
        }

        public static PdhSoftwareInfo SvlPdh_GetSoftwareInfo(bool isWireless)
        {
            PdhSoftwareInfo softwareInfo = new PdhSoftwareInfo();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting software information..."));

            if (isWireless)
            {
                softwareInfo.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_REQ, null));
            }
            else
            {
                softwareInfo.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_REQ, null, SysmonMessagePv.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_GET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Software information received successfully."));

            return softwareInfo;
        }

        public static PdhSoftwareInfo SvlPdh_SetSoftwareInfo(PdhSoftwareInfo softwareInfo, bool isWireless)
        {
            PdhSoftwareInfo softwareInfoReadback = new PdhSoftwareInfo();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Setting software information..."));

            if (isWireless)
            {
                softwareInfoReadback.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_SET_REQ, softwareInfo.GetBytes()));
            }
            else
            {
                softwareInfoReadback.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_SET_REQ, softwareInfo.GetBytes(), SysmonMessagePv.SVL_PDH_SYSMON_MSG_SOFTWARE_INFO_SET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Software information set successfully."));

            return softwareInfoReadback;
        }

        public static PdhHardwareInfo SvlPdh_GetHardwareInfo(bool isWireless)
        {
            PdhHardwareInfo hardwareInfo = new PdhHardwareInfo();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting hardware information..."));

            if (isWireless)
            {
                hardwareInfo.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_REQ, null));
            }
            else
            {
                hardwareInfo.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_REQ, null, SysmonMessagePv.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_GET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Hardware information received successfully."));

            return hardwareInfo;
        }

        public static PdhHardwareInfo SvlPdh_SetHardwareInfo(PdhHardwareInfo hardwareInfo, bool isWireless)
        {
            PdhHardwareInfo hardwareInfoReadback = new PdhHardwareInfo();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Setting hardware information..."));

            if (isWireless)
            {
                hardwareInfoReadback.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_SET_REQ, hardwareInfo.GetBytes()));
            }
            else
            {
                hardwareInfoReadback.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_SET_REQ, hardwareInfo.GetBytes(), SysmonMessagePv.SVL_PDH_SYSMON_MSG_HARDWARE_INFO_SET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Hardware information set successfully."));

            return hardwareInfoReadback;
        }

        public static PdhBootloaderConfig SvlPdh_GetBootloaderConfig(bool isWireless)
        {
            PdhBootloaderConfig bootloaderConfig = new PdhBootloaderConfig();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting bootloader configuration..."));

            if (isWireless)
            {
                bootloaderConfig.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_BLD_CONFIG_GET_REQ, null));
            }
            else
            {
                bootloaderConfig.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_BLD_CONFIG_GET_REQ, null, SysmonMessagePv.SVL_PDH_SYSMON_MSG_BLD_CONFIG_GET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Bootloader configuration received successfully."));

            return bootloaderConfig;
        }

        public static PdhBootloaderConfig SvlPdh_SetBootloaderConfig(PdhBootloaderConfig bldConfig, bool isWireless)
        {
            PdhBootloaderConfig bldConfigReadBack = new PdhBootloaderConfig();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Setting bootloader configuration..."));

            if (isWireless)
            {
                bldConfigReadBack.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_BLD_CONFIG_SET_REQ, bldConfig.GetBytes()));
            }
            else
            {
                bldConfigReadBack.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_BLD_CONFIG_SET_REQ, bldConfig.GetBytes(), SysmonMessagePv.SVL_PDH_SYSMON_MSG_BLD_CONFIG_SET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Bootloader configuration set successfully."));

            return bldConfigReadBack;
        }

        public static PdhWifiConfig SvlPdh_GetWifiConfig(bool isWireless)
        {
            PdhWifiConfig wifiConfig = new PdhWifiConfig();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting WiFi configuration..."));

            if (isWireless)
            {
                wifiConfig.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_GET_REQ, null));
            }
            else
            {
                wifiConfig.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_GET_REQ, null, SysmonMessagePv.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_GET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "WiFi configuration received successfully."));

            return wifiConfig;
        }

        public static PdhWifiConfig SvlPdh_SetWifiConfig(PdhWifiConfig wifiConfig, bool isWireless)
        {
            PdhWifiConfig wifiConfigReadBack = new PdhWifiConfig();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Setting WiFi configuration..."));

            if (isWireless)
            {
                wifiConfigReadBack.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_SET_REQ, wifiConfig.GetBytes()));
            }
            else
            {
                wifiConfigReadBack.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_SET_REQ, wifiConfig.GetBytes(), SysmonMessagePv.SVL_PDH_SYSMON_MSG_WIFI_CONFIG_SET_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "WiFi configuration set successfully."));

            return wifiConfigReadBack;
        }

        public static bool SvlErs_ClearEvents(bool isWireless)
        {
            UInt32 eventNum = 255;
            int idx = 0;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Clearing events..."));

            if (isWireless)
            {
                eventNum = Helper<UInt32>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ, null), ref idx);
            }
            else
            {
                eventNum = Helper<UInt32>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ, null, SysmonMessagePv.SVL_ERS_SYSMON_MSG_EVENTS_CLEAR_REQ_PV), ref idx);
            }

            if (eventNum == 0)
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Events cleared successfully."));
            else
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Event clearing failed."));

            return eventNum == 0;
        }

        public static List<SvlErsEvent> SvlErs_GetEvents(bool isWireless)
        {
            List<SvlErsEvent> events = new List<SvlErsEvent>();
            UInt32 eventNum = 0;
            int idx = 0;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting number of ERS events..."));

            if (isWireless)
            {
                eventNum = Helper<UInt32>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_REQ, null), ref idx);
            }
            else
            {
                eventNum = Helper<UInt32>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_REQ, null, SysmonMessagePv.SVL_ERS_SYSMON_MSG_EVENTS_GET_NUM_REQ_PV), ref idx);
            }

            for (int i = 0; i < eventNum; i++)
            {
                SvlErsEvent ev = new SvlErsEvent();

                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting ERS event [" + i + "]..."));

                if (isWireless)
                {
                    ev.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ, Helper<UInt32>.GetBytes((UInt32)i)));
                }
                else
                {
                    ev.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ, Helper<UInt32>.GetBytes((UInt32)i), SysmonMessagePv.SVL_ERS_SYSMON_MSG_EVENTS_GET_REQ_PV));
                }

                events.Add(ev);
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Events received."));

            return events;
        }

        public static int SvlDhs_GetDeviceNum(bool isWireless)
        {
            int devNum = 0;
            int idx = 0;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting number of DHS devices..."));

            if (isWireless)
            {
                devNum = Helper<UInt16>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ, null), ref idx);
            }
            else
            {
                devNum = Helper<UInt16>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ, null, SysmonMessagePv.SVL_DHS_SYSMON_MSG_DEVICE_NUM_REQ_PV), ref idx);
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Number of DHS devices received."));

            return devNum;
        }

        public static DhsDeviceDescriptor SvlDhs_GetDeviceInfo(int index, bool isWireless)
        {
            DhsDeviceDescriptor deviceInfo = new DhsDeviceDescriptor();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting DHS device info [" + index + "]..."));

            if (isWireless)
            {
                deviceInfo.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ, Helper<UInt16>.GetBytes((UInt16)index)));
            }
            else
            {
                deviceInfo.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ, Helper<UInt16>.GetBytes((UInt16)index), SysmonMessagePv.SVL_DHS_SYSMON_MSG_DEVICE_INFO_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "DHS device info received."));

            return deviceInfo;
        }

        public static List<SvlMdiVariable> SvlMdi_GetMonitoringData(bool isWireless)
        {
            List<SvlMdiVariable> variables = new List<SvlMdiVariable>();
            UInt16 numOfVariables = 0;
            int idx = 0;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting number of MDI variables..."));

            if (isWireless)
            {
                numOfVariables = Helper<UInt16>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_REQ, null), ref idx);
            }
            else
            {
                numOfVariables = Helper<UInt16>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_REQ, null, SysmonMessagePv.SVL_MDI_SYSMON_MSG_MONITORING_DATA_NUM_GET_REQ_PV), ref idx);
            }

            for (int i = 0; i < numOfVariables; i++)
            {
                SvlMdiVariable var = new SvlMdiVariable();

                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting MDI variable [" + i + "]..."));

                if (isWireless)
                {
                    var.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ, Helper<UInt16>.GetBytes((UInt16)i)));
                }
                else
                {
                    var.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ, Helper<UInt16>.GetBytes((UInt16)i), SysmonMessagePv.SVL_MDI_SYSMON_MSG_MONITORING_DATA_GET_REQ_PV));
                }

                variables.Add(var);
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Variables received."));

            return variables;
        }

        public static int SvlSdh_GetBinaryNum(bool isWireless)
        {
            int binaryNum = 0;
            int idx = 0;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting number of SDH binaries..."));

            if (isWireless)
            {
                binaryNum = Helper<UInt16>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_NUM_REQ, null), ref idx);
            }
            else
            {
                binaryNum = Helper<UInt16>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_NUM_REQ, null, SysmonMessagePv.SVL_SDH_SYSMON_MSG_BINARY_NUM_REQ_PV), ref idx);
            }

            return binaryNum;
        }

        public static SdhBinaryDescriptorMessage SvlSdh_GetBinaryInfo(int index, bool isWireless)
        {
            SdhBinaryDescriptorMessage binaryInfo = new SdhBinaryDescriptorMessage();

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Getting SDH binary info [" + index + "]..."));

            if (isWireless)
            {
                binaryInfo.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_INFO_REQ, Helper<UInt16>.GetBytes((UInt16)index)));
            }
            else
            {
                binaryInfo.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_INFO_REQ, Helper<UInt16>.GetBytes((UInt16)index), SysmonMessagePv.SVL_SDH_SYSMON_MSG_BINARY_INFO_REQ_PV));
            }

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH binary info received."));

            return binaryInfo;
        }

        public static SdhBinaryDownloadRequestResult SvlSdh_SendBinaryDownloadRequest(SdhBinaryDescriptorMessage binaryDescriptor, bool isWireless)
        {
            SdhBinaryDownloadRequestResult result = SdhBinaryDownloadRequestResult.COMM_ERR;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Sending SDH download request..."));

            if (isWireless)
            {
                result = (SdhBinaryDownloadRequestResult)SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_DOWNLOAD_REQ, binaryDescriptor.GetBytes())[0];
            }
            else
            {
                result = (SdhBinaryDownloadRequestResult)SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_DOWNLOAD_REQ, binaryDescriptor.GetBytes(), SysmonMessagePv.SVL_SDH_SYSMON_MSG_DOWNLOAD_REQ_PV)[0];
            }

            if (result == SdhBinaryDownloadRequestResult.OK)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH download request sent successfully."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH download request failed."));
            }

            return result;
        }

        public static bool SvlSdh_SendBinary(List<byte> bytes, bool isWireless)
        {
            bool res = false;
            int chunkSize = 512;
            int retryCounter = 0;
            bool decreaseCounter = false;

            int chunks = bytes.Count / chunkSize + (bytes.Count % chunkSize == 0 ? 0 : 1);

            for (int chunkCounter = 0; chunkCounter < chunks; chunkCounter++)
            {
                if (decreaseCounter)
                {
                    chunkCounter--;
                    decreaseCounter = false;
                }
                ChunkDescriptor chunkDesc = new ChunkDescriptor();
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Sending SDH binary chunk [" + chunkCounter + "]..."));

                chunkDesc.ChunkIndex = (UInt16)chunkCounter;
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

                if (isWireless)
                {
                    chunkDesc.GetFromBytes(SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_CHUNK_REQ, payload.ToArray()));
                }
                else
                {
                    chunkDesc.GetFromBytes(SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_CHUNK_REQ, payload.ToArray(), SysmonMessagePv.SVL_SDH_SYSMON_MSG_BINARY_CHUNK_REQ_PV));
                }

                if (chunkDesc.Result != 1)
                {
                    retryCounter++;
                    decreaseCounter = true;

                    if (retryCounter > 10)
                    {
                        res = false;
                        StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH binary download failed."));
                        break;
                    }
                }
                else
                {
                    res = true;
                    BinaryDownloadProgressEvent?.Invoke(null, (chunkCounter + 1, chunks));
                    retryCounter = 0;
                    decreaseCounter = false;
                }
            }

            if (res)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH download successful."));
            }

            return res;
        }

        public static bool SvlSdh_SendEraseRequest(int index, bool isWireless)
        {
            bool retval = false;
            int idx = 0;
            int rxIdx = -1;
            List<byte> payload = new List<byte>();
            payload.AddRange(Helper<UInt16>.GetBytes((UInt16)index));
            payload.Add(73);

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Sending SDH erase request..."));

            if (isWireless)
            {
                rxIdx = Helper<UInt16>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ, payload.ToArray()), ref idx);
            }
            else
            {
                rxIdx = Helper<UInt16>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ, payload.ToArray(), SysmonMessagePv.SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ_PV), ref idx);
            }

            if (rxIdx == index)
            {
                retval = true;
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH erase successful."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH erase request failed."));
            }

            return retval;
        }

        public static bool SvlSdh_SendInstallRequest(int index, bool isWireless)
        {
            bool retval = false;
            int idx = 0;
            int rxIdx = -1;
            List<byte> payload = new List<byte>();
            payload.AddRange(Helper<UInt16>.GetBytes((UInt16)index));

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Sending SDH install request..."));

            if (isWireless)
            {
                rxIdx = Helper<UInt16>.GetVariable(SendReceiveMessageWireless(SysmonMessageId.SVL_SDH_SYSMON_MSG_SOFTWARE_INSTALL_REQ, payload.ToArray()), ref idx);
            }
            else
            {
                rxIdx = Helper<UInt16>.GetVariable(SendReceiveMessageWired(SysmonMessageId.SVL_SDH_SYSMON_MSG_SOFTWARE_INSTALL_REQ, payload.ToArray(), SysmonMessagePv.SVL_SDH_SYSMON_MSG_BINARY_ERASE_REQ_PV), ref idx);
            }

            if (rxIdx == index)
            {
                retval = true;
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH install request successful."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "SDH install request failed."));
            }

            Thread.Sleep(50);

            return retval;
        }

        public static void AppRtc_SynchronizeTime(bool isWireless)
        {
            SysTimeMessage sysTimeMessage = new SysTimeMessage();
            sysTimeMessage.SystemTime.Years = (ushort)DateTime.Now.Year;
            sysTimeMessage.SystemTime.Months = (byte)DateTime.Now.Month;
            sysTimeMessage.SystemTime.Days = (byte)DateTime.Now.Day;
            sysTimeMessage.SystemTime.Hours = (byte)DateTime.Now.Hour;
            sysTimeMessage.SystemTime.Minutes = (byte)DateTime.Now.Minute;
            sysTimeMessage.SystemTime.Seconds = (byte)DateTime.Now.Second;
            sysTimeMessage.SystemTime.Milliseconds = (byte)DateTime.Now.Millisecond;
            SysmonMessageResult syncResult;

            StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Synchronizing RTC time..."));

            if (isWireless)
            {
                syncResult = (SysmonMessageResult)SendReceiveMessageWireless(SysmonMessageId.APP_SYSMON_MSG_RTC_SET_REQ, sysTimeMessage.GetBytes())[0];
            }
            else
            {
                syncResult = (SysmonMessageResult)SendReceiveMessageWired(SysmonMessageId.APP_SYSMON_MSG_RTC_SET_REQ, sysTimeMessage.GetBytes(), SysmonMessagePv.APP_SYSMON_MSG_RTC_SET_REQ_PV)[0];
            }

            if (syncResult == SysmonMessageResult.GOS_SYSMON_MSG_RES_OK)
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Time synchronization successful."));
            }
            else
            {
                StatusUpdateEvent?.Invoke(null, new SysmonStatusUpdateEventArgs(isWireless, "Communication error."));
            }
        }

        private static byte[] SendReceiveMessageWired (SysmonMessageId messageId,  byte[] payload, SysmonMessagePv protocolVersion)
        {
            _wiredSemaphore.Wait();

            if (!_isWiredInitialized)
            {
                // Initialize first.
                InitializeWired();
            }

            GcpMessageHeader messageHeader = new GcpMessageHeader();
            byte[] rxBuffer = new byte[1024];

            messageHeader.MessageId = (UInt16)messageId;
            messageHeader.ProtocolVersion = (UInt16)protocolVersion;
            messageHeader.PayloadSize = !(payload is null) ? (UInt16)payload.Length : (UInt16)0;

            if (GCP.TransmitMessage(0, messageHeader, payload, 0xffff) &&
                GCP.ReceiveMessage(0, out messageHeader, out rxBuffer, 0xffff, 5000))
            {
                // Successful.
            }
            else
            {
                //StatusUpdateEvent?.Invoke(null, false, "Communication error.");
                rxBuffer = null;
                _isWiredInitialized = false;
            }

            Thread.Sleep(10);
            _wiredSemaphore.Release();

            return rxBuffer;
        }

        private static byte[] SendReceiveMessageWireless (SysmonMessageId messageId, byte[] payload)
        {
            _wirelessSemaphore.Wait();

            if (!_isWirelessInitialized)
            {
                // Initialize first.
                InitializeWireless();
            }

            int txSize = 4;
            byte[] rxBuffer = new byte[1024];

            if (!(payload is null))
            {
                txSize += payload.Length;
            }

            byte[] txBuffer = new byte[txSize];

            txBuffer[0] = (byte)((UInt16)messageId >> 8);
            txBuffer[1] = (byte)((UInt16)messageId & 0x00ff);
            txBuffer[2] = (byte)((UInt16)(txSize - 4) >> 8);
            txBuffer[3] = (byte)((UInt16)(txSize - 4) & 0x00ff);

            if (!(payload is null))
            {
                Array.Copy(payload, 0, txBuffer, 4, payload.Length);
            }
            
            try
            {
                _stream.Write(txBuffer, 0, txBuffer.Length);
                _stream.Read(rxBuffer, 0, rxBuffer.Length);
            }
            catch
            {
                //StatusUpdateEvent?.Invoke(null, "Communication error.");
                rxBuffer = null;
                _isWirelessInitialized = false;
            }

            _wirelessSemaphore.Release();

            return rxBuffer;
        }
    }
}
