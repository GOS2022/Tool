using System;
using System.Collections.Generic;

namespace GOSTool
{
    public enum SdhBinaryDownloadRequestResult
    {
        COMM_ERR = 0,
        OK = 1,
        DESCRIPTOR_SIZE_ERR = 2,
        FILE_SIZE_ERR = 4
    }
    public class SdhBinaryDescriptorMessage
    {
        public string Name { get; set; }
        public DateTime InstallDate { get; set; } = new DateTime();
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
                int idx = 0;
                Name = Helper.GetString(bytes, 32, ref idx);
                InstallDate = Helper.GetTime(bytes, ref idx);
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
            bytes.AddRange(Helper.GetBytes(Name, 32));
            bytes.AddRange(Helper.GetBytes(InstallDate));
            bytes.AddRange(Helper<UInt32>.GetBytes(Location));
            bytes.AddRange(Helper<UInt32>.GetBytes(StartAddress));
            bytes.AddRange(Helper<UInt32>.GetBytes(Size));
            bytes.AddRange(Helper<UInt32>.GetBytes(Crc));

            return bytes.ToArray();
        }
    }
}
