using System;
using System.Collections.Generic;
using System.Linq;

namespace GOSTool
{
    public class SvlErsEvent
    {
        public string Description { get; private set; }
        public DateTime TimeStamp { get; private set; } = new DateTime();
        public UInt32 Trigger { get; private set; }
        public List<byte> EventData { get; private set; } = new List<byte>();

        public const UInt16 ExpectedSize = 64 + Time.ExpectedSize + 4 + 8;

        public void GetFromBytes(byte[] bytes)
        {
            if (!(bytes is null) && bytes.Length >= ExpectedSize)
            {
                int idx = 0;
                Description = Helper.GetString(bytes, 64, ref idx);
                TimeStamp = Helper.GetTime(bytes, ref idx);
                Trigger = Helper<UInt32>.GetVariable(bytes, ref idx);
                EventData = bytes.Skip(idx).Take(8).ToList();
            }
        }
    }
}
