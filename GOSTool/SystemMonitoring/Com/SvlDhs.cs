using System;

namespace GOSTool
{
    public class DhsDeviceDescriptor
    {
        public const int ExpectedSize = 32 + 64 + 4 + 4 + 10 + 16 + 16 + 21;
        public string Name { get; set; }
        public string Description { get; set; }
        public UInt32 DeviceId { get; set; }
        public string DeviceType { get; set; }
        public bool Enabled { get; set; }
        public string RecoveryType { get; set; }
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
            if (!(bytes is null) && (bytes.Length >= ExpectedSize))
            {
                int idx = 0;
                Name = Helper.GetString(bytes, 32, ref idx);
                Description = Helper.GetString(bytes, 64, ref idx);
                DeviceId = Helper<UInt32>.GetVariable(bytes, ref idx);
                idx += 4;
                byte devType = Helper<byte>.GetVariable(bytes, ref idx);
                DeviceType = "Unknown";
                switch (devType)
                {
                    case 0: DeviceType = "Read only"; break;
                    case 1: DeviceType = "Write only"; break;
                    case 2: DeviceType = "Read / Write"; break;
                    case 3: DeviceType = "Virtual"; break;
                }

                Enabled = Helper<bool>.GetVariable(bytes, ref idx);
                idx += 4; // Initializer
                idx += 4; // Error handler

                byte recoveryType = Helper<byte>.GetVariable(bytes, ref idx);
                RecoveryType = "Unknown";
                switch (recoveryType)
                {
                    case 0: RecoveryType = "None"; break;
                    case 1: RecoveryType = "On single error"; break;
                    case 2: RecoveryType = "On limit"; break;
                }

                idx += 4 * 4; // Read functions
                idx += 4 * 4; // Write functions
                byte devState = Helper<byte>.GetVariable(bytes, ref idx);
                DeviceState = "Unknown";
                switch (devState)
                {
                    case 0: DeviceState = "Uninitialized"; break;
                    case 1: DeviceState = "Healthy"; break;
                    case 2: DeviceState = "Warning"; break;
                    case 3: DeviceState = "Error"; break;
                    case 4: DeviceState = "Not present"; break;
                }

                ErrorCode = Helper<UInt32>.GetVariable(bytes, ref idx);
                ErrorCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
                ErrorTolerance = Helper<UInt32>.GetVariable(bytes, ref idx);
                ReadCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
                WriteCounter = Helper<UInt32>.GetVariable(bytes, ref idx);
            }
        }
    }
}
