using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GOSTool.Test
{
    [XmlRoot]
    public class TestCase
    {
        public string Name { get; set; } = "Anonymous test case";
        [XmlArray]
        [XmlArrayItem("Steps")]
        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();
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
        public static XmlSerializer CreateCustomSerializer()
        {
            // Create overrides that allow each Brass or Woodwind object 
            // to be read from and written as members of an Instruments
            // collection. 
            // Oddly enough, an override is also needed to allow an      
            // Instrument to be read/written as an Instrument.
            XmlAttributes xAttrs = new XmlAttributes();

            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(SetupSerialTestStep)));     
            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(SetupWirelessTestStep)));     
            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(WaitTestStep)));
            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(PingTestStep)));
            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(TaskDataTestStep)));
            xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(typeof(TestStep)));

            var overrides = new XmlAttributeOverrides(); 
            overrides.Add(typeof(TestCase), "Steps", xAttrs);

            var serializer = new XmlSerializer(typeof(TestCase), overrides);
            return serializer;
        }
        public void Execute ()
        {
            PrintLine?.Invoke("Executing " + Name + "... ");
            
            Result = true;

            DateTime startTime = DateTime.Now;

            for (int i = 0; i < TestSteps.Count; i++)
            {
                TestSteps[i].Execute();

                if (!TestSteps[i].Result)
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
