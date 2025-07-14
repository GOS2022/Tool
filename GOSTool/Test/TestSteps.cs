using GOSTool.SystemMonitoring.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOSTool.Test
{
    public class SetupSerialTestStep : TestStep
    {
        private string _serialPort = "";
        public string SerialPort
        {
            get
            {
                return _serialPort;
            }
            set
            {
                _serialPort = value;
                Args = new object[] { _serialPort, _baud };
            }
        }
        private int _baud;
        public int Baud
        {
            get
            {
                return _baud;
            }
            set
            {
                _baud = value;
                Args = new object[] { _serialPort, _baud };
            }
        }

        public SetupSerialTestStep():base()
        {

        }
        public SetupSerialTestStep(SetupSerialTestStep other) : base(other)
        {
            Baud = other.Baud;
            SerialPort = other.SerialPort;
        }
    }

    public class SetupWirelessTestStep : TestStep
    {
        private string _ip = "192.168.100.184";
        public string Ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                Args = new object[] { _ip, _wirelessPort };
            }
        }
        private int _wirelessPort = 3000;
        public int WirelessPort
        {
            get
            {
                return _wirelessPort;
            }
            set
            {
                _wirelessPort = value;
                Args = new object[] { _ip, _wirelessPort };
            }
        }

        public SetupWirelessTestStep() : base()
        {

        }
        public SetupWirelessTestStep(SetupWirelessTestStep other) : base(other)
        {
            Ip = other.Ip;
            WirelessPort = other.WirelessPort;
        }
    }

    public class WaitTestStep : TestStep
    {
        private int _waitTimeMs = 25;
        public int WaitTimeMs 
        { 
            get
            {
                return _waitTimeMs;
            }
            set
            {
                _waitTimeMs = value;
                Args = new object[] { _waitTimeMs };
            }
        }
        public WaitTestStep():base()
        {

        }
        public WaitTestStep(WaitTestStep other):base(other)
        {
            WaitTimeMs = other.WaitTimeMs;
        }
    }

    public class PingTestStep : TestStep
    {
        private bool _isWireless = false;
        public bool IsWireless
        {
            get
            {
                return _isWireless;
            }
            set
            {
                _isWireless = value;
                Args = new object[] { _isWireless };
            }
        }
        public PingTestStep() : base()
        {

        }
        public PingTestStep(PingTestStep other) : base(other)
        {
            IsWireless = other.IsWireless;
        }
    }
    public class TaskDataTestStep : TestStep
    {
        private bool _isWireless = false;
        public bool IsWireless
        {
            get
            {
                return _isWireless;
            }
            set
            {
                _isWireless = value;
                Args = new object[] { _isWireless, _expectedTaskNumber };
            }
        }

        private int _expectedTaskNumber = 0;
        public int ExpectedTaskNumber
        {
            get
            {
                return _expectedTaskNumber;
            }
            set
            {
                _expectedTaskNumber = value;
                Args = new object[] { _isWireless, _expectedTaskNumber };
            }
        }
        public TaskDataTestStep() : base()
        {

        }
        public TaskDataTestStep(TaskDataTestStep other) : base(other)
        {
            ExpectedTaskNumber = other.ExpectedTaskNumber;
            IsWireless = other.IsWireless;
        }
    }

    public class TestSteps
    {
        public static Func<string, object> Print { get; set; } = null;
        public static Func<string, object> PrintLine { get; set; } = null;
        public static string[] GetTestSteps()
        {
            return new string[]
            {
                "Wired Communication Setup",
                "Wireless Communication Setup",
                "Wait",
                "Ping Device",
                "Task Data Get"
            };
        }

        public static void UpdateTestStep (TestStep loadedTest)
        {
            TestStep generatedStep = GetTestStep(loadedTest.Name);
            //loadedTest.Print = generatedStep.Print;
            //loadedTest.PrintLine = generatedStep.PrintLine;
            //loadedTest.Args = generatedStep.Args;
            loadedTest.TestStepFunction = generatedStep.TestStepFunction;
            loadedTest.PassCriteria = generatedStep.PassCriteria;
        }

        public static TestStep GetTestStep(string name)
        {
            TestStep testStep = new TestStep();

            switch(name)
            {
                case "Ping Device":
                    {
                        testStep = PreparePingTestStep();
                        break;
                    }
                case "Wait":
                    {
                        testStep = PrepareWaitTestStep();
                        break;
                    }
                case "Wired Communication Setup":
                    {
                        testStep = PrepareSerialSetupTestStep();
                        break;
                    }
                case "Wireless Communication Setup":
                    {
                        testStep = PrepareWirelessSetupTestStep();
                        break;
                    }
                case "Task Data Get":
                    {
                        testStep = PrepareTaskDataTestStep();
                        break;
                    }
            }

            return testStep;
        }

        private static TaskDataTestStep PrepareTaskDataTestStep()
        {
            TaskDataTestStep testStep = new TaskDataTestStep();

            testStep.Name = "Task Data Get";
            testStep.Print = Print;
            testStep.PrintLine = PrintLine;
            testStep.Args = new object[] { testStep.IsWireless, testStep.ExpectedTaskNumber };
            testStep.TestStepFunction = (args) => { return Sysmon.SvlSysmon_GetAllTaskData(args[0]).Count; };
            testStep.PassCriteria = (args) => { return (int)args == testStep.ExpectedTaskNumber; };

            return testStep;
        }

        private static WaitTestStep PrepareWaitTestStep()
        {
            WaitTestStep testStep = new WaitTestStep();

            testStep.Name = "Wait";
            testStep.Print = Print;
            testStep.PrintLine = PrintLine;
            testStep.Args = new object[] { testStep.WaitTimeMs };
            testStep.TestStepFunction = (args) => {
                Thread.Sleep(args[0]); 
                return 0;
            };
            testStep.PassCriteria = (args) => { return true; };

            return testStep;
        }

        private static SetupWirelessTestStep PrepareWirelessSetupTestStep()
        {
            SetupWirelessTestStep testStep = new SetupWirelessTestStep();

            testStep.Name = "Wireless Communication Setup";
            testStep.Print = Print;
            testStep.PrintLine = PrintLine;
            testStep.Args = new object[] { testStep.Ip, testStep.WirelessPort };
            testStep.TestStepFunction = (args) => {
                Sysmon.Ip = args[0];
                Sysmon.WirelessPort = args[1];
                Sysmon.InitializeWireless();
                return 0;
            };
            testStep.PassCriteria = (args) => { return true; };

            return testStep;
        }

        private static SetupSerialTestStep PrepareSerialSetupTestStep()
        {
            SetupSerialTestStep testStep = new SetupSerialTestStep();

            testStep.Name = "Wired Communication Setup";
            testStep.Print = Print;
            testStep.PrintLine = PrintLine;
            testStep.Args = new object[] { testStep.SerialPort, testStep.Baud };
            testStep.TestStepFunction = (args) => {
                Sysmon.SerialPort = args[0];
                Sysmon.Baud = args[1];
                Sysmon.InitializeWired();
                return 0;
            };
            testStep.PassCriteria = (args) => { return true; };

            return testStep;
        }

        private static PingTestStep PreparePingTestStep()
        {
            PingTestStep testStep = new PingTestStep();

            testStep.Name = "Ping Device";
            testStep.Print = Print;
            testStep.PrintLine = PrintLine;
            testStep.Args = new object[] { testStep.IsWireless };
            testStep.TestStepFunction = (args) => Sysmon.SvlSysmon_PingDevice(args[0]);
            testStep.PassCriteria = (res) => { return (bool)res == true; };

            return testStep;
        }
    }
}
