using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GOSTool.Test
{
    public class Test
    {
        public string Name { get; set; } = "Anonymous test";
        public List<TestCase> TestCases { get; } = new List<TestCase>();
        [XmlIgnoreAttribute]
        public bool Result { get; private set; } = false;
        [XmlIgnoreAttribute]
        public TimeSpan ExecutionTime { get; private set; } = new TimeSpan();
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<string, object> Print { get; set; } = null;
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public Func<string, object> PrintLine { get; set; } = null;
        public void Execute()
        {
            PrintLine?.Invoke("Executing " + Name + "... ");
            Result = true;

            DateTime startTime = DateTime.Now;

            foreach (var testCase in TestCases)
            {
                testCase.Execute();

                if (!testCase.Result)
                {
                    Result = false;
                }
            }

            ExecutionTime = DateTime.Now - startTime;

            if (Result)
            {
                PrintLine?.Invoke(Name + " [passed]. Time taken: " + ExecutionTime.TotalMilliseconds + " ms.");
            }
            else
            {
                PrintLine?.Invoke(Name + " [failed]. Time taken: " + ExecutionTime.TotalMilliseconds + " ms.");
            }
        }
    }
}
