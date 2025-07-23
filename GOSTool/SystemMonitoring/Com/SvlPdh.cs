using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GOSTool
{
    public class PdhSoftwareVersionInfo
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
            Date = Helper.GetTime(bytes, ref idx);
            Name = Helper.GetString(bytes, 48, ref idx);
            Description = Helper.GetString(bytes, 48, ref idx);
            Author = Helper.GetString(bytes, 48, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Helper<UInt16>.GetBytes(Major));
            bytes.AddRange(Helper<UInt16>.GetBytes(Minor));
            bytes.AddRange(Helper<UInt16>.GetBytes(Build));
            bytes.AddRange(Helper.GetBytes(Date));
            bytes.AddRange(Helper.GetBytes(Name, 48));
            bytes.AddRange(Helper.GetBytes(Description, 48));
            bytes.AddRange(Helper.GetBytes(Author, 48));

            return bytes.ToArray();
        }
    }
    public class PdhOsInfo
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
    public class PdhBinaryInfo
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
    public class PdhHardwareInfo
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
            BoardName = Helper.GetString(bytes, 48, ref idx);
            Revision = Helper.GetString(bytes, 48, ref idx);
            Author = Helper.GetString(bytes, 48, ref idx);
            Description = Helper.GetString(bytes, 48, ref idx);
            Date = Helper.GetTime(bytes, ref idx);
            SerialNumber = Helper.GetString(bytes, 48, ref idx);
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Helper.GetBytes(BoardName, 48));
            bytes.AddRange(Helper.GetBytes(Revision, 48));
            bytes.AddRange(Helper.GetBytes(Author, 48));
            bytes.AddRange(Helper.GetBytes(Description, 48));
            bytes.AddRange(Helper.GetBytes(Date));
            bytes.AddRange(Helper.GetBytes(SerialNumber, 48));
            return bytes.ToArray();
        }
    }

    public class PdhSoftwareInfo
    {
        public PdhSoftwareVersionInfo BldLibVerInfo { get; set; } = new PdhSoftwareVersionInfo();
        public PdhSoftwareVersionInfo BldSwVerInfo { get; set; } = new PdhSoftwareVersionInfo();
        public PdhOsInfo BldOsInfo { get; set; } = new PdhOsInfo();
        public PdhBinaryInfo BldBinaryInfo { get; set; } = new PdhBinaryInfo();
        public PdhSoftwareVersionInfo AppLibVerInfo { get; set; } = new PdhSoftwareVersionInfo();
        public PdhSoftwareVersionInfo AppSwVerInfo { get; set; } = new PdhSoftwareVersionInfo();
        public PdhOsInfo AppOsInfo { get; set; } = new PdhOsInfo();
        public PdhBinaryInfo AppBinaryInfo { get; set; } = new PdhBinaryInfo();

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            try
            {
                BldLibVerInfo.GetFromBytes(bytes.Skip(idx).Take(PdhSoftwareVersionInfo.ExpectedSize).ToArray());
                idx += PdhSoftwareVersionInfo.ExpectedSize;
                BldSwVerInfo.GetFromBytes(bytes.Skip(idx).Take(PdhSoftwareVersionInfo.ExpectedSize).ToArray());
                idx += PdhSoftwareVersionInfo.ExpectedSize;
                BldOsInfo.GetFromBytes(bytes.Skip(idx).Take(PdhOsInfo.ExpectedSize).ToArray());
                idx += PdhOsInfo.ExpectedSize;
                BldBinaryInfo.GetFromBytes(bytes.Skip(idx).Take(PdhBinaryInfo.ExpectedSize).ToArray());
                idx += PdhBinaryInfo.ExpectedSize;

                AppLibVerInfo.GetFromBytes(bytes.Skip(idx).Take(PdhSoftwareVersionInfo.ExpectedSize).ToArray());
                idx += PdhSoftwareVersionInfo.ExpectedSize;
                AppSwVerInfo.GetFromBytes(bytes.Skip(idx).Take(PdhSoftwareVersionInfo.ExpectedSize).ToArray());
                idx += PdhSoftwareVersionInfo.ExpectedSize;
                AppOsInfo.GetFromBytes(bytes.Skip(idx).Take(PdhOsInfo.ExpectedSize).ToArray());
                idx += PdhOsInfo.ExpectedSize;
                AppBinaryInfo.GetFromBytes(bytes.Skip(idx).Take(PdhBinaryInfo.ExpectedSize).ToArray());
                idx += PdhBinaryInfo.ExpectedSize;
            }
            catch
            {
                // TODO
            }
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

    public class PdhBootloaderConfig
    {
        public bool InstallRequested { get; set; }
        public byte Reserved { get; set; }
        public UInt16 BinaryIndex { get; set; }
        public bool UpdateMode { get; set; }
        public byte StartupCounter { get; set; }
        public UInt32 RequestTimeout { get; set; }
        public UInt32 InstallTimeout { get; set; }

        public void GetFromBytes(byte[] bytes)
        {
            int idx = 0;
            InstallRequested = Helper<bool>.GetVariable(bytes, ref idx);
            Reserved = Helper<byte>.GetVariable(bytes, ref idx);
            BinaryIndex = Helper<UInt16>.GetVariable(bytes, ref idx);
            UpdateMode = Helper<bool>.GetVariable(bytes, ref idx);
            StartupCounter = Helper<byte>.GetVariable(bytes, ref idx);
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
            bytes.AddRange(Helper<byte>.GetBytes(StartupCounter));
            bytes.AddRange(Helper<UInt32>.GetBytes(RequestTimeout));
            bytes.AddRange(Helper<UInt32>.GetBytes(InstallTimeout));

            return bytes.ToArray();
        }
    }

    public class PdhWifiConfig
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
            if (!(bytes is null))
            {
                Ssid = Helper.GetString(bytes, 48, ref idx);
                Pwd = Helper.GetString(bytes, 48, ref idx);
                IpAddress = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
                GateWay = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
                Subnet = bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++] + "." + bytes[idx++];
                Port = Helper<UInt16>.GetVariable(bytes, ref idx);
            }
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Helper.GetBytes(Ssid, 48));
            bytes.AddRange(Helper.GetBytes(Pwd, 48));

            foreach (string ipElement in IpAddress.Split('.'))
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
