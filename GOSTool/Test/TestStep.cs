using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace GOSTool.Test
{
    [XmlInclude(typeof(SetupSerialTestStep))]
    [XmlInclude(typeof(SetupWirelessTestStep))]
    [XmlInclude(typeof(WaitTestStep))]
    [XmlInclude(typeof(PingTestStep))]
    [XmlInclude(typeof(TaskDataTestStep))]
    public class TestStep
    {
        public string Name { get; set; } = "Anonymous test step";
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<dynamic, object> TestStepFunction { get; set; } = null;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<object, bool> PassCriteria { get; set; } = null;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public dynamic Args { get; set; } = null;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public bool Result { get; private set; } = false;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public TimeSpan ExecutionTime { get; private set; } = new TimeSpan();
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<string, object> Print { get; set; } = null;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<string, object> PrintLine { get; set; } = null;
        public TestStep()
        {

        }
        public TestStep(TestStep other)
        {
            Name = other.Name;
            TestStepFunction = other.TestStepFunction;
            PassCriteria = other.PassCriteria;
            Args = other.Args;
            Result = other.Result;
            ExecutionTime = other.ExecutionTime;
            Print = other.Print;
            PrintLine = other.PrintLine;
        }
        /*public static Type[] GetKnownTypes()
        {
            return new[] { typeof(SetupSerialTestStep), typeof(SetupWirelessTestStep), typeof(WaitTestStep), typeof(PingTestStep), typeof(TaskDataTestStep) };
        }*/
        public void Execute ()
        {
            Print?.Invoke("Executing " + Name + "... ");

            DateTime startTime = DateTime.Now;

            Result = PassCriteria(TestStepFunction(Args));

            ExecutionTime = DateTime.Now - startTime;

            if (Result)
            {
                PrintLine?.Invoke("[passed]. Time taken: " + ExecutionTime.TotalMilliseconds + " ms.");
            }
            else
            {
                PrintLine?.Invoke("[failed]. Time taken: " + ExecutionTime.TotalMilliseconds + " ms.");
            }
        }
    }
}
