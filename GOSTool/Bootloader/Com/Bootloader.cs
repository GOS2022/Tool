using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOSTool
{
    /*public class VersionInfo
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public DateTime Date { get; set; } = new DateTime();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }*/

    /*public class ApplicationData
    {
        public int StartAddress { get; set; }
        public int Size { get; set; }
        public int Crc { get; set; }
        public VersionInfo DriverVersion { get; set; } = new VersionInfo();
        public VersionInfo AppVersion { get; set; } = new VersionInfo();
    }*/

    /*public class BootloaderData
    {
        public VersionInfo BootloaderDriverInfo { get; set; } = new VersionInfo();
        public int StartAddress { get; set; }
        public int Size { get; set; }
        public int Crc { get; set; }
        public VersionInfo BootloaderInfo { get; set; } = new VersionInfo();
        public ApplicationData ApplicationData { get; set; } = new ApplicationData();
        public bool BootUpdateMode { get; set; }
    }*/

    public class SoftwareVersionInfo
    {
        public const int ExpectedSize = 160;
        public UInt16 Major { get; set; }
        public UInt16 Minor { get; set; }
        public UInt16 Build { get; set; }
        public DateTime Date { get; set; } = new DateTime();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            Major = Helper<UInt16>.GetVariable(bytes, ref idx);
            Minor = Helper<UInt16>.GetVariable(bytes, ref idx);
            Build = Helper<UInt16>.GetVariable(bytes, ref idx);
            Time time = new Time();
            time.GetFromBytes(bytes.Skip(idx).Take(Time.ExpectedSize).ToArray());
            try
            {
                Date = DateTime.Parse(time.Years.ToString("D4") + "-" + time.Months.ToString("D2") + "-" + time.Days.ToString("D2"));
            }
            catch
            {
                Date = new DateTime();
            }
            
            idx += Time.ExpectedSize;
            Name = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Description = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Author = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Helper<UInt16>.GetBytes(Major));
            bytes.AddRange(Helper<UInt16>.GetBytes(Minor));
            bytes.AddRange(Helper<UInt16>.GetBytes(Build));

            Time time = new Time();
            time.Years = (UInt16)Date.Year;
            time.Months = (byte)Date.Month;
            time.Days = (UInt16)Date.Day;
            bytes.AddRange(time.GetBytes());

            bytes.AddRange(Encoding.ASCII.GetBytes(Name));
            int padding = 48 - Encoding.ASCII.GetBytes(Name).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Description));
            padding = 48 - Encoding.ASCII.GetBytes(Description).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Author));
            padding = 48 - Encoding.ASCII.GetBytes(Author).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            return bytes.ToArray();
        }
    }

    public class OsInfo
    {
        public const int ExpectedSize = 4;
        public UInt16 Major { get; set; }
        public UInt16 Minor { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            Major = Helper<UInt16>.GetVariable(bytes, ref idx);
            Minor = Helper<UInt16>.GetVariable(bytes, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Helper<UInt16>.GetBytes(Major));
            bytes.AddRange(Helper<UInt16>.GetBytes(Minor));

            return bytes.ToArray();
        }
    }

    public class BinaryInfo
    {
        public const int ExpectedSize = 12;
        public UInt32 StartAddress { get; set; }
        public UInt32 Size { get; set; }
        public UInt32 Crc { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            StartAddress = Helper<UInt32>.GetVariable(bytes, ref idx);
            Size = Helper<UInt32>.GetVariable(bytes, ref idx);
            Crc = Helper<UInt32>.GetVariable(bytes, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Helper<UInt32>.GetBytes(StartAddress));
            bytes.AddRange(Helper<UInt32>.GetBytes(Size));
            bytes.AddRange(Helper<UInt32>.GetBytes(Crc));

            return bytes.ToArray();
        }
    }

    public class HardwareInfo
    {
        public string BoardName { get; set; }
        public string Revision { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } = new DateTime();
        public string SerialNumber { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            BoardName = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Revision = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Author = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Description = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Time time = new Time();
            time.GetFromBytes(bytes.Skip(idx).Take(Time.ExpectedSize).ToArray());
            try
            {
                Date = DateTime.Parse(time.Years.ToString("D4") + "-" + time.Months.ToString("D2") + "-" + time.Days.ToString("D2"));
            }
            catch
            {
                Date = new DateTime();
            }
            idx += Time.ExpectedSize;
            SerialNumber = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            
            bytes.AddRange(Encoding.ASCII.GetBytes(BoardName));
            int padding = 48 - Encoding.ASCII.GetBytes(BoardName).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Revision));
            padding = 48 - Encoding.ASCII.GetBytes(Revision).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Author));
            padding = 48 - Encoding.ASCII.GetBytes(Author).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Description));
            padding = 48 - Encoding.ASCII.GetBytes(Description).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            Time time = new Time() { Years = (UInt16)Date.Year, Months = (byte)Date.Month, Days = (UInt16)Date.Day };
            bytes.AddRange(time.GetBytes());

            bytes.AddRange(Encoding.ASCII.GetBytes(SerialNumber));
            padding = 48 - Encoding.ASCII.GetBytes(SerialNumber).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            return bytes.ToArray();
        }
    }

    public class SoftwareInfo
    {
        public SoftwareVersionInfo BldLibVerInfo { get; set; } = new SoftwareVersionInfo();
        public SoftwareVersionInfo BldSwVerInfo { get; set; } = new SoftwareVersionInfo();
        public OsInfo BldOsInfo { get; set; } = new OsInfo();
        public BinaryInfo BldBinaryInfo { get; set; } = new BinaryInfo();
        public SoftwareVersionInfo AppLibVerInfo { get; set; } = new SoftwareVersionInfo();
        public SoftwareVersionInfo AppSwVerInfo { get; set; } = new SoftwareVersionInfo();
        public OsInfo AppOsInfo { get; set; } = new OsInfo();
        public BinaryInfo AppBinaryInfo { get; set; } = new BinaryInfo();

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            BldLibVerInfo.GetFromBytes(bytes.Skip(idx).Take(SoftwareVersionInfo.ExpectedSize).ToArray());
            idx += SoftwareVersionInfo.ExpectedSize;
            BldSwVerInfo.GetFromBytes(bytes.Skip(idx).Take(SoftwareVersionInfo.ExpectedSize).ToArray());
            idx += SoftwareVersionInfo.ExpectedSize;
            BldOsInfo.GetFromBytes(bytes.Skip(idx).Take(OsInfo.ExpectedSize).ToArray());
            idx += OsInfo.ExpectedSize;
            BldBinaryInfo.GetFromBytes(bytes.Skip(idx).Take(BinaryInfo.ExpectedSize).ToArray());
            idx += BinaryInfo.ExpectedSize;

            AppLibVerInfo.GetFromBytes(bytes.Skip(idx).Take(SoftwareVersionInfo.ExpectedSize).ToArray());
            idx += SoftwareVersionInfo.ExpectedSize;
            AppSwVerInfo.GetFromBytes(bytes.Skip(idx).Take(SoftwareVersionInfo.ExpectedSize).ToArray());
            idx += SoftwareVersionInfo.ExpectedSize;
            AppOsInfo.GetFromBytes(bytes.Skip(idx).Take(OsInfo.ExpectedSize).ToArray());
            idx += OsInfo.ExpectedSize;
            AppBinaryInfo.GetFromBytes(bytes.Skip(idx).Take(BinaryInfo.ExpectedSize).ToArray());
            idx += BinaryInfo.ExpectedSize;
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BldLibVerInfo.GetBytes());
            bytes.AddRange(BldSwVerInfo.GetBytes());
            bytes.AddRange(BldOsInfo.GetBytes());
            bytes.AddRange(BldBinaryInfo.GetBytes());

            bytes.AddRange(AppLibVerInfo.GetBytes());
            bytes.AddRange(AppSwVerInfo.GetBytes());
            bytes.AddRange(AppOsInfo.GetBytes());
            bytes.AddRange(AppBinaryInfo.GetBytes());

            return bytes.ToArray();
        }
    }

    public class BootloaderConfig
    {
        public bool InstallRequested { get; set; }
        public byte Reserved { get; set; }
        public UInt16 BinaryIndex { get; set; }
        public bool UpdateMode { get; set; }
        //public bool WirelessUpdate { get; set; }
        //public bool WaitForConnectionOnStartup { get; set; }
        public byte StartupCounter { get; set; }
        //public UInt32 ConnectionTimeout { get; set; }
        public UInt32 RequestTimeout { get; set; }
        public UInt32 InstallTimeout { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            InstallRequested = Helper<bool>.GetVariable(bytes, ref idx);
            Reserved = Helper<byte>.GetVariable(bytes, ref idx);
            BinaryIndex = Helper<UInt16>.GetVariable(bytes, ref idx);
            UpdateMode = Helper<bool>.GetVariable(bytes, ref idx);
            //WirelessUpdate = Helper<bool>.GetVariable(bytes, ref idx);
            //WaitForConnectionOnStartup = Helper<bool>.GetVariable(bytes, ref idx);
            StartupCounter = Helper<byte>.GetVariable(bytes, ref idx);
            //ConnectionTimeout = Helper<UInt32>.GetVariable(bytes, ref idx);
            RequestTimeout = Helper<UInt32>.GetVariable(bytes, ref idx);
            InstallTimeout = Helper<UInt32>.GetVariable(bytes, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Helper<bool>.GetBytes(InstallRequested));
            bytes.AddRange(Helper<byte>.GetBytes(Reserved));
            bytes.AddRange(Helper<UInt16>.GetBytes(BinaryIndex));
            bytes.AddRange(Helper<bool>.GetBytes(UpdateMode));
            //bytes.AddRange(Helper<bool>.GetBytes(WirelessUpdate));
            //bytes.AddRange(Helper<bool>.GetBytes(WaitForConnectionOnStartup));
            bytes.AddRange(Helper<byte>.GetBytes(StartupCounter));
            //bytes.AddRange(Helper<UInt32>.GetBytes(ConnectionTimeout));
            bytes.AddRange(Helper<UInt32>.GetBytes(RequestTimeout));
            bytes.AddRange(Helper<UInt32>.GetBytes(InstallTimeout));

            return bytes.ToArray();
        }
    }

    public class WifiConfig
    {
        public string Ssid { get; set; }
        public string Pwd { get; set; }
        public string IpAddress { get; set; }
        public string GateWay { get; set; }
        public string Subnet { get; set; }
        public UInt16 Port { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            Ssid = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            Pwd = Encoding.ASCII.GetString(bytes.Skip(idx).Take(48).ToArray()).Trim('\0');
            idx += 48;
            IpAddress = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
            GateWay = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
            Subnet = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
            Port = Helper<UInt16>.GetVariable(bytes, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Encoding.ASCII.GetBytes(Ssid));
            int padding = 48 - Encoding.ASCII.GetBytes(Ssid).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            bytes.AddRange(Encoding.ASCII.GetBytes(Pwd));
            padding = 48 - Encoding.ASCII.GetBytes(Pwd).Length;
            for (int i = 0; i < padding; i++)
            {
                bytes.Add(0);
            }

            foreach(string ipElement in IpAddress.Split('.'))
            {
                bytes.Add(byte.Parse(ipElement));
            }

            foreach (string ipElement in GateWay.Split('.'))
            {
                bytes.Add(byte.Parse(ipElement));
            }

            foreach (string ipElement in Subnet.Split('.'))
            {
                bytes.Add(byte.Parse(ipElement));
            }

            bytes.AddRange(Helper<UInt16>.GetBytes(Port));

            return bytes.ToArray();
        }
    }
}
